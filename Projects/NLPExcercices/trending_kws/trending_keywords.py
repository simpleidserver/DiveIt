import pandas as pd
import numpy as np
import json
from collections import Counter
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.feature_extraction.text import CountVectorizer
from keybert import KeyBERT

def aggregate_keywords(df, keywords, article, multiplier, column_name):
    vectorizer = CountVectorizer(ngram_range=(1,2))
    x = vectorizer.fit_transform([article[column_name]])
    freqs = zip(vectorizer.get_feature_names_out(), np.asarray(x.sum(axis=0)).ravel())
    freqs_lst = list(freqs)
    for component in keywords:
        keyword_name = component[0]
        score = component[1] * multiplier
        existing_keyword = df.loc[df['Keyword'] == keyword_name]
        kw_freq = 1
        freqs = list(filter(lambda x: x[0] == keyword_name, freqs_lst))
        if(len(freqs) == 1):
            kw_freq =  freqs[0][1]
        if(len(existing_keyword) == 0):
            kid = df.shape[0]
            new_row = {
                'Keyword': keyword_name, 
                'ArticleIds': str(article['Id']), 
                'ArticleUrls': article['Path'], 
                'Nb': kw_freq , 
                'Ponderation': score, 
                'ReactionsCount': article['ReactionsCount'], 
                'CommentsCount': article['CommentsCount'],
                'NbArticles': 1
            }
            df.loc[kid] = new_row
        else:
            article_ids = existing_keyword['ArticleIds'].to_numpy()[0].split(" ")
            if(str(article['Id']) not in article_ids):
                df.loc[df['Keyword'] == keyword_name, 'NbArticles'] = int(existing_keyword['NbArticles']) + 1
                df.loc[df['Keyword'] == keyword_name, 'ArticleIds'] = df.loc[df['Keyword'] == keyword_name, 'ArticleIds'] + " " + str(article['Id'])
                df.loc[df['Keyword'] == keyword_name, 'ArticleUrls'] = df.loc[df['Keyword'] == keyword_name, 'ArticleUrls'] + " " + str(article['Path'])
                df.loc[df['Keyword'] == keyword_name, 'ReactionsCount'] = int(existing_keyword['ReactionsCount']) + article['ReactionsCount']
                df.loc[df['Keyword'] == keyword_name, 'CommentsCount'] = int(existing_keyword['CommentsCount']) + article['CommentsCount']
            df.loc[df['Keyword'] == keyword_name, 'Ponderation'] = int(existing_keyword['Ponderation']) + score

def extract_keywords(all_titles, all_articles, articles_df):
    df = pd.DataFrame(columns=['Keyword','ArticleIds', 'ArticleUrls', 'NbArticles', 'Nb', 'Ponderation', 'ReactionsCount', 'CommentsCount'])
    for i in range(articles_df.shape[0]):
        article = articles_df.iloc[i]
        title_keywords = all_titles[i]
        article_keywords = all_articles[i]
        aggregate_keywords(df, title_keywords, article, 1.4, 'Title')
        aggregate_keywords(df, article_keywords, article, 1, 'Article')
    return df

def extract_trending_keywords_keyBERT():
    file = "articles.csv"
    df = pd.read_csv(file)
    kw_model = KeyBERT()
    title_keywords = df["Title"].apply(kw_model.extract_keywords, keyphrase_ngram_range=(1,2), use_mmr=False, top_n=1)
    article_keywords = df["Article"].apply(kw_model.extract_keywords, keyphrase_ngram_range=(1,2), use_mmr=False, top_n=1)
    trending_keywords = extract_keywords(title_keywords, article_keywords, df)
    trending_keywords.to_csv("extracted_trending_keywords.csv", sep=',', encoding='utf-8')

def analyse_result():
    file = "extracted_trending_keywords.csv"
    df = pd.read_csv(file)
    df.loc[:, 'ComputedPonderation'] = [''] * len(df)
    # df["PonderationAmongAllArticles"] = df.apply(lambda row: row["Ponderation"] * row["NbArticles"], axis=1)
    # df["PonderationAmongAllReactions"] = df.apply(lambda row: row["Ponderation"] * row["ReactionsCount"], axis=1)
    weight_reactions = 0.4
    df["ComputedPonderation"] = df.apply(lambda row: (row["Ponderation"] * row["NbArticles"]) + (row["ReactionsCount"] * weight_reactions), axis=1)
    sorted_df = df.sort_values(by=['ComputedPonderation'], ascending=False)[:30]
    print(sorted_df[['Keyword', 'NbArticles', 'ReactionsCount', 'ComputedPonderation']])