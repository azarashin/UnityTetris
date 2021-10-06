#!/usr/bin/env python
# -*- coding: utf-8 -*-

import sys
import markdown
import codecs
import re

infile = sys.argv[1]
outfile = sys.argv[2]
template = sys.argv[3]
template_menu = sys.argv[4]

md = markdown.Markdown(extensions=['tables', 'extra', 'admonition', 'sane_lists', 'toc'])

with codecs.open(infile, 'r', 'utf-8') as fi:
    source = fi.read().split('\n')

data = ''
for s in source:
    line = s
    for i in range(6, 0, -1):
        ptfrom = ' ' * (i*2) + '-'
        ptto = ' ' * (i*4) + '-'
        if line[:i*2+1] == ptfrom:
            line = ptto + line[i*2+1:]
    data += line + '\n'

with codecs.open(template, 'r', 'utf-8') as ft:
    template_data = ft.read()

with codecs.open(template_menu, 'r', 'utf-8') as ft:
    template_menu_data = ft.read()

menu_list = ''
menu_body = md.convert(data)

repattern = re.compile(r'<h(.) id="(.*)">(.*)</h.>')

source = menu_body.split('\n')

max_hlevel = 3
min_hlevel = 2
numbers = [0] * max_hlevel

for s in source:
    ret_match = repattern.match(s)
    if ret_match:
        hlevel = int(ret_match.group(1))
        id = ret_match.group(2)
        text = ret_match.group(3)

        if hlevel > max_hlevel or hlevel < min_hlevel:
            continue

        numbers[hlevel-min_hlevel] += 1
        numbers[hlevel-min_hlevel+1] = 0
        num_title = ''
        for n in numbers:
            if n == 0:
                break
            num_title += '{}.'.format(n)
        menu_list += '<li><a href="#{}">{} {}</a></li>\n'.format(id, num_title, text)

    

with codecs.open(outfile, 'w', 'utf-8') as fo:
    fo.write(
        template_data.replace('{{contents_body}}', 
        template_menu_data.replace('{{menu_list}}', menu_list)
            .replace('{{menu_body}}', menu_body)
        #md.convert(data)
        ))
