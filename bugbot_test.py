"""Throwaway file to check that Cursor Bugbot reviews PRs. Do not merge."""


def add_tag(tag, tags=[]):
    tags.append(tag)
    return tags


def last_item(items):
    return items[len(items)]


def read_config(path):
    try:
        return open(path).read()
    except:
        return None
