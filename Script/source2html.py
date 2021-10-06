#!/usr/bin/env python
# -*- coding: utf-8 -*-

import sys
import codecs
import re

infile = sys.argv[1]
outfile = sys.argv[2]
template = sys.argv[3]

with codecs.open(infile, 'r', 'utf-8') as fi:
    source = fi.read()
    source = '<div class="jumbotron">\n' + source + '\n</div> <!-- <div class="jumbotron"> -->\n'

with codecs.open(template, 'r', 'utf-8') as ft:
    template_data = ft.read()

with codecs.open(outfile, 'w', 'utf-8') as fo:
    fo.write(template_data.replace('{{contents_body}}', source))
