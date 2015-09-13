import os
import codecs
import markdown

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

def generate(md_path):
    md_file = codecs.open(md_path, 'r', encoding='utf-8')
    md_text = md_file.read()
    md_file.close()

    md = markdown.Markdown(**MARKDOWN_KWARGS)
    html_body = md.convert(md_text)
    html_text = HTML_TEMPLATE.format(
        title=get_meta(md, 'title'), css=get_meta(md, 'css'), body=html_body)

    html_path = os.path.normpath(os.path.splitext(md_path)[0] + '.html')
    html_file = codecs.open(html_path, 'w', encoding='utf-8', errors='xmlcharrefreplace')
    html_file.write(html_text)
    html_file.close()

def generate_all(dir_path):
    for rootpath, dirs, files in os.walk(dir_path):
        for file in files:
            if file.endswith('.md'):
                generate(os.path.join(rootpath, file))

if __name__ == '__main__':
    generate_all(os.getcwdu())
