import requests
import pandas as pd
import multiprocessing
import nltk
import numpy as np
from nltk.stem import WordNetLemmatizer
from bs4 import BeautifulSoup

from sklearn.feature_extraction.text import CountVectorizer

stopwords = nltk.corpus.stopwords.words('english')
lemmatizer = WordNetLemmatizer()

class LemmaCountVectorizer(CountVectorizer):
    def build_analyzer(self):
        analyzer = super(LemmaCountVectorizer, self).build_analyzer()
        return lambda doc: (lemmatizer.lemmatize(w) for w in analyzer(doc))

def read_article(link):
    article_result = requests.get(link['base_url'])
    article_soup = BeautifulSoup(article_result.text, features='html.parser')
    content = article_soup.find(id='article-body')
    return { 'article_content': content.get_text(), 'id': link['id'] }

def download_articles_from_dev_to():
    base_url = 'https://dev.to'
    result = requests.get(base_url + '/search/feed_content?per_page=1&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet')
    json = result.json()
    df = pd.json_normalize(json, 'result')
    df = df[['id', 'title', 'user_id', 'public_reactions_count', 'comments_count', 'published_at_int', 'reading_time', 'path']]
    df.columns = ['Id', 'Title', 'UserId', 'ReactionsCount', 'CommentsCount', 'Published', 'ReadingTime', 'Path']
    df.loc[:,'Article'] = [''] * len(df)
    links = list(map(lambda p,i: { 'base_url' : base_url + p, 'id' : i }, df['Path'], df.index.values))
    with multiprocessing.Pool() as pool:
        results = pool.map(read_article, links)
    for link in results:
        df[link['id'], 'Article'] = link['article_content']
    return df

def clean_article(article):
    article_tokens = nltk.word_tokenize(article)
    article_tokens_cleaned = [lemmatizer.lemmatize(word) for word in article_tokens if word.lower() not in stopwords]

# 'https://dev.to/lovelacecoding/modern-c-development-record-types-101-55bj'
def bag_of_word(article):
    vectoriser = LemmaCountVectorizer(stop_words='english')
    transform = vectoriser.fit_transform(article)
    features = vectoriser.get_feature_names_out()
    count_vec = np.asarray(transform.sum(axis=0)).ravel()
    zipped = list(zip(features, count_vec))
    x, y = (list(x) for x in zip(*sorted(zipped, key=lambda x: x[1], reverse=True)))
    return { 'x': x, 'y': y}

def extract_key_words(article):
    tokens = nltk.word_tokenize(article)


def filter_articles_on_comments_and_reactions(df):
    df = df.sort_values(by='ReactionsCount', ascending=False)

if __name__ == '__main__':
    df = download_articles_from_dev_to()
    print(df)