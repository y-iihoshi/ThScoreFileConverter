import os
import markdown

MARKDOWN_KWARGS = {
    'output_format': 'xhtml1',
    'extensions': [
        'markdown.extensions.attr_list',
        'markdown.extensions.toc'
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

def generate(md_path):
    html_path = os.path.normpath(os.path.splitext(md_path)[0] + '.html')
    markdown.markdownFromFile(input=md_path, output=html_path, **MARKDOWN_KWARGS)

def generate_all(dir_path):
    for rootpath, dirs, files in os.walk(dir_path):
        for file in files:
            if file.endswith('.md'):
                generate(os.path.join(rootpath, file))

if __name__ == '__main__':
    generate_all(os.getcwdu())
