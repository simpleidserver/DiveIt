import re
import pandas as pd
from nltk.stem import WordNetLemmatizer
from nltk.corpus import stopwords
from nltk import word_tokenize
import re

lemmatizer = WordNetLemmatizer()
stop_words = stopwords.words("english")

def extract_tokens_from_article(article):
    article_cleaned = re.sub(r"https?://\S+|www\.\S+", "", article)
    article_cleaned = re.sub(
        r"[^a-zA-Z\s+]+", " ", article_cleaned
    ).lower()
    article_cleaned = re.sub(
        "programming", "", article_cleaned
    )
    tokens = word_tokenize(article_cleaned)
    tokens = [
        token
        for token in tokens
        if token not in stop_words
    ]
    tokens = [
        lemmatizer.lemmatize(token)
        for token in tokens
    ]
    return tokens

def extract_tokens(df):
    df.loc[:, 'ConcatArticleTitle'] = [''] * len(df)
    articles = df["Article"]
    titleTokens = list(map(lambda p: " ".join(extract_tokens_from_article(p)), df['Title']))
    articleTokens = list(map(lambda p: " ".join(extract_tokens_from_article(p)), df['Article']))
    df["TitleTokens"] = titleTokens
    df["ArticleTokens"] = articleTokens
    return df

def prepare_articles():
    file = "../extract_articles/articles.csv"
    df = pd.read_csv(file)
    df = df.drop(df.columns[[0]], axis=1)
    df = extract_tokens(df)
    df.to_csv("cleaned_articles.csv", sep=',', encoding='utf-8')