#!/usr/bin/env python
# -*- coding: utf-8 -*-

import glob
import codecs
import sys
import re

def get_id(src):
	id_src=src.split('/')[-1]
	ids=id_src.split('_')
	ret=ids[0]

	if not re.fullmatch('[a-zA-Z]+\d+', ids[0]):
		return None

	for id in ids[1:]:
		if re.fullmatch('\d+', id):
			ret += '_'+id
		else:
			break
	return ret

def get_outline(src):
	id=get_id(src)
	if id:
		return id.replace('_', '.')+'. '
	else:
		return None

def scan_tree(line_pattern, pre_level, dataset, index):
	ret = []
	while index < len(dataset):
		l = dataset[index]
		index += 1
		m=line_pattern.search(l)
		if m:
			level = len(m.group(1).strip())
			label = m.group(2).strip()
			if pre_level == level:
				ret.append(label)
			elif level < pre_level:
				return ret, index-1
			elif level == pre_level + 1:
				child, index = scan_tree(line_pattern, level, dataset, index-1)
				ret.append(child)
			else:
				print('Error[{}]: level={} -> {}, {}'.format(index, pre_level, level, label))
	return ret, index
    				
def print_tree(tree):
	print_trees(0, tree)

def print_trees(level, tree):
	if type(tree)==str:
		print('{}{}'.format('#'*level, tree))
	else:
		for d in tree:
			print_trees(level+1, d)

def length_of_mindmap(mindmap):
	if type(mindmap) == str:
		return 1
	else:
		return sum([length_of_mindmap(d) for d in mindmap])

def output_submindmap_page(f, index, id, level, mindmap):
	threshold = 16
	if type(mindmap) == str:
		f.write('{}<li>{}</li>\n'.format(' ' * level, mindmap))
	elif len(mindmap) == 1:
		f.write('{}<ul><li>{}</li></ul>\n'.format(' ' * level, mindmap[0]))
	else:
		f.write('{}<ul>\n'.format(' ' * level))
		i=1
		while i < len(mindmap):
			if type(mindmap[i]) == str:
				f.write('{}<li>{}</li>\n'.format(' ' * level, mindmap[i-1]))
				if i == len(mindmap) - 1:
					f.write('{}<li>{}</li>\n'.format(' ' * level, mindmap[i]))
			elif length_of_mindmap(mindmap[i]) > threshold:
				next_index, next_link = generate_submindmap_page(mindmap[i-1], index+1, level+1, mindmap[i], svgs_path)
				f.write('{}<li><a href="{}">{}</a></li>\n'.format(' ' * (level+1), next_link, mindmap[i-1]))
				index = next_index + 1
				i += 1
			else:
				f.write('{}<li><div onclick="obj=document.getElementById(\'open_id_{}\').style; obj.display=(obj.display==\'none\')?\'block\':\'none\';">\n'.format(' ' * level, id))
				f.write('{}<a style="cursor:pointer;">+{}</a></div>'.format(' ' * level, mindmap[i-1]))

#				f.write('{}<li class="parent" onclick="func1(this)">+{}\n'.format(' ' * level, mindmap[i-1]))
				f.write('{}<div id="open_id_{}" style="display:none;clear:both;">'.format(' ' * level, id))
				id, index = output_submindmap_page(f, index, id+1, level+1, mindmap[i])
				f.write('{}</div></li>\n'.format(' ' * level))
				i += 1
			i += 1
		f.write('{}</ul>\n'.format(' ' * level))
	return id, index

def generate_submindmap_page(title, index, level, mindmap, svgs_path):
	head='<!DOCTYPE html><html lang="ja"><head>  <meta charset="UTF-8"><meta name="viewport" content="width=device-width, initial-scale=1.0"><title>{}</title>'.format(title)
	style='</head>'\
		'<body><h1>{}</h1>'.format(title)

	tail='</body></html>'
	length=length_of_mindmap(mindmap)
	link='./mindmap_{}.html'.format(index)
	path='{}/mindmap_{}.html'.format(svgs_path, index)
	id = 0
	with codecs.open(path, 'w', 'utf-8') as f:
		f.write(head)
		f.write(style)
		id, index = output_submindmap_page(f, index, 0, 1, mindmap)
		f.write(tail)
	return index, link

		

def generate_mindmap_page(input_path, svgs_path):
	path_pattern = re.compile("^.*/([^/]+)/([^/]+).pu$")
	line_pattern = re.compile("^(\*+)\s+(.*)$")
	pus = [d.replace('\\', '/') for d in glob.glob(input_path + '/**/*.pu', recursive=True)]
	dic={}
	mindmap = []
	for pu in pus:
		pu=pu.replace('\\', '/')
		title=None
		m=path_pattern.search(pu)
		if not m:
			continue
		category=m.group(1)
		dataname=m.group(2)
		with codecs.open(pu, 'r', 'utf-8') as f:
			datas = f.readlines()
			is_mindmap = (len([d for d in datas if '@startmindmap' in d]) > 0)
			if is_mindmap:
				mp, index = scan_tree(line_pattern, 1, datas, 0)
				mindmap.extend(mp)
	generate_submindmap_page('mindmap', 0, 1, mindmap, svgs_path)
	return mindmap

    							

def generate_svg_page(input_path, svgs_path):
	path_pattern = re.compile("^.*/([^/]+)/([^/]+).pu$")
	title_pattern = re.compile("^\s*title\s*(.*)$")
	pus = [d.replace('\\', '/') for d in glob.glob(input_path + '/**/*.pu', recursive=True)]
	dic={}
	for pu in pus:
		pu=pu.replace('\\', '/')
		title=None
		m=path_pattern.search(pu)
		if not m:
			continue
		category=m.group(1)
		dataname=m.group(2)
		with codecs.open(pu, 'r', 'utf-8') as f:
			datas = f.readlines()
			is_mindmap = (len([d for d in datas if '@startmindmap' in d]) > 0)
			if is_mindmap:
				continue # マインドマップであればここでの処理対象から外す
			for l in datas:
				m=title_pattern.search(l)
				prefix=get_outline(dataname)
				if m and prefix:
					title=prefix + m.group(1).strip()
				elif m:
					title=m.group(1).strip()
		if not title:
			title=dataname
		if not category in dic:
			dic[category]=[]
		dic[category].append((title, dataname))

	# modify svg files. 
	svg_title_pattern = re.compile("^(.*<text[^>]*?)>(.*)(</text>.*)$")
	link_dic = {}
	for category in dic:
		for svg in dic[category]:
			title=svg[0]
			dataname=svg[1]
			link='./{}.svg'.format(dataname)
			id=get_id(link)
			link_dic[id]=link + '.linked.svg'


	svgs = [d.replace('\\', '/') for d in glob.glob(svgs_path + '/**/*.svg', recursive=True)]
	for svg in svgs:
		my_id=get_id(svg)
		svg_data = codecs.open(svg, 'r', 'utf-8').read().replace('\r\n','\n').replace('\r','\n').replace('<text', '\n<text').split('\n')
		with codecs.open(svg+'.linked.svg', 'w', 'utf-8') as f:
			for d in svg_data:
				m=svg_title_pattern.search(d)
				if not m:
					f.write(d+'\n')
				else:
					pre=m.group(1)
					text=m.group(2)
					post=m.group(3)
					id=get_id(text)
					if id and id!=my_id and id in link_dic:
						link=link_dic[id]
						f.write('{} stroke="#00f"><a xlink:href="{}" target="_blank">{}</a>{}\n'.format(pre, link, text, post))
					else:
						f.write(d+'\n')
	return dic


def output_html(output_path, dic, mindmap):
	print('<html><header></header><body>')
	if len(mindmap) > 0:
		print('<a href="{}/mindmap_0.html">マインドマップ</a>\n'.format(output_path))
	for category in dic:
		print('<ul>{}'.format(category))
		items = []
		for svg in dic[category]:
			title=svg[0]
			dataname=svg[1]
			body='<li><a href="{}/{}.svg.linked.svg">{}</a></li>'.format(output_path, dataname, title)
			items.append((title, body))
		items = sorted(items, key=lambda x:x[0])
		for item in items:
			print(item[1])
		print('</ul>')
	print('</body></html>')

if __name__ == '__main__':
	argv = sys.argv
	if len(argv) != 4:
		print('usage: .py root_path_to_pu_dir path_to_svg_dir_in_repogitory path_to_svg_dir_in_github_page')
		exit()

	# construct svg list. 
	input_path = argv[1]
	svgs_path = argv[2]
	output_path = argv[3]

	mindmap = generate_mindmap_page(input_path, svgs_path)
#	print_tree(mindmap)

	svg_dic = generate_svg_page(input_path, svgs_path)
	output_html(output_path, svg_dic, mindmap)
