import os
import codecs
import markdown
import argparse

MARKDOWN_KWARGS = {
    'output_format': 'xhtml1',
    'extensions': [
        'markdown.extensions.attr_list',
        'markdown.extensions.meta',
        'markdown.extensions.toc',
    ],
    'extension_configs': {
        'markdown.extensions.toc': {
            # 'marker': '[TOC]',
            # 'title': None,
            # 'anchorlink': False,
            # 'permalink': False,
            # 'baselevel': 1,
            # 'slugify': 'markdown.extensions.headerid.slugify',
        },
    },
}

HTML_TEMPLATE = u'''<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="ja">
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css"/>
    <title>{title}</title>
    <style type="text/css">
<!--
{css}
-->
    </style>
  </head>
  <body>

{body}

  </body>
</html>
'''

def get_meta(md, key, sep='\n'):
    return sep.join(md.Meta.get(key, [])) if hasattr(md, 'Meta') else ''

def generate(md_path, output_dir_path):
    md_file = codecs.open(md_path, 'r', encoding='utf-8')
    md_text = md_file.read()
    md_file.close()

    md = markdown.Markdown(**MARKDOWN_KWARGS)
    html_body = md.convert(md_text)
    html_text = HTML_TEMPLATE.format(
        title=get_meta(md, 'title'), css=get_meta(md, 'css'), body=html_body)

    html_name = os.path.splitext(os.path.basename(md_path))[0] + '.html'
    html_path = os.path.join(output_dir_path, html_name)
    html_file = codecs.open(html_path, 'w', encoding='utf-8', errors='xmlcharrefreplace')
    html_file.write(html_text)
    html_file.close()

def generate_all(input_dir_path, output_dir_path):
    for rootpath, dirs, files in os.walk(input_dir_path):
        for file in files:
            if file.endswith('.md'):
                output_rootpath = rootpath.replace(input_dir_path, output_dir_path)
                if not os.path.isdir(output_rootpath):
                    os.mkdir(output_rootpath)
                generate(os.path.join(rootpath, file), output_rootpath)

def parse_arguments():
    parser = argparse.ArgumentParser()
    parser.add_argument('--input-dir', default=os.getcwdu())
    parser.add_argument('--output-dir', default=os.getcwdu())
    return parser.parse_args()

if __name__ == '__main__':
    args = parse_arguments()
    generate_all(args.input_dir, args.output_dir)
