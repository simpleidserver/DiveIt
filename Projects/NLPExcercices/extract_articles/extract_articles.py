import requests
import pandas as pd
import multiprocessing
from bs4 import BeautifulSoup
import re

def read_article(url):
    article_content = read_article_content(url['base_url'])
    return { 'article_content': article_content, 'id': url['id'] }

def read_article_content(url):
    article_result = requests.get(url)
    article_soup = BeautifulSoup(article_result.text, features='html.parser')
    [x.extract() for x in article_soup.findAll(True, {"class": ["highlight"]})]
    [x.extract() for x in article_soup.findAll("script")]
    [x.extract() for x in article_soup.findAll("iframe")]
    content = article_soup.find(id='article-body')
    text = content.get_text()
    emoji_pattern = re.compile("["
            u"\U0001F600-\U0001F64F"  # emoticons
            u"\U0001F300-\U0001F5FF"  # symbols & pictographs
            u"\U0001F680-\U0001F6FF"  # transport & map symbols
            u"\U0001F1E0-\U0001F1FF"  # flags (iOS)
    "]+", flags=re.UNICODE)
    return emoji_pattern.sub(r'', text)

def extract_articles_from_dev_to(nb_articles=60):
    base_url = 'https://dev.to'
    result = requests.get(base_url + '/search/feed_content?per_page='+ nb_articles +'&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet')
    json = result.json()
    df = pd.json_normalize(json, 'result')
    df = df[['id', 'title', 'user_id', 'public_reactions_count', 'comments_count', 'published_at_int', 'reading_time', 'path']]
    df.columns = ['Id', 'Title', 'UserId', 'ReactionsCount', 'CommentsCount', 'Published', 'ReadingTime', 'Path']
    df.loc[:,'Article'] = [''] * len(df)
    links = list(map(lambda p,i: { 'base_url' : base_url + p, 'id' : i }, df['Path'], df.index.values))
    with multiprocessing.Pool() as pool:
        results = pool.map(read_article, links)
    for link in results:
        df.at[link['id'], 'Article'] = link['article_content']
    return df

def extract_articles_and_save():
    df = extract_articles_from_dev_to("60")
    df.to_csv("articles.csv", sep=',', encoding='utf-8')
