import prepare_articles
import pandas as pd
import numpy as np
import json
from collections import Counter
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.feature_extraction.text import CountVectorizer
from keybert import KeyBERT

def aggregate_keywords(df, keywords, article, multiplier, column_name):
    vectorizer = CountVectorizer()
    x = vectorizer.fit_transform([article[column_name]])
    freqs = zip(vectorizer.get_feature_names_out(), np.asarray(x.sum(axis=0)).ravel())
    freqs_lst = list(freqs)
    for component in keywords:
        keyword_name = component[0]
        score = component[1] * multiplier
        existing_keyword = df.loc[df['Keyword'] == keyword_name]
        kw_freq = list(filter(lambda x: x[0] == keyword_name, freqs_lst))[0][1]
        if(len(existing_keyword) == 0):
            kid = df.shape[0]
            new_row = {
                'Keyword': keyword_name, 
                'ArticleIds': str(article['Id']), 
                'ArticleUrls': article['Path'], 
                'Nb': kw_freq , 
                'Ponderation': score, 
                'ReactionsCount': article['ReactionsCount'], 
                'CommentsCount': article['CommentsCount']
            }
            df.loc[kid] = new_row
        else:
            df.loc[df['Keyword'] == keyword_name, 'ArticleIds'] = df.loc[df['Keyword'] == keyword_name, 'ArticleIds'] + " " + str(article['Id'])
            df.loc[df['Keyword'] == keyword_name, 'ArticleUrls'] = df.loc[df['Keyword'] == keyword_name, 'ArticleUrls'] + " " + str(article['Path'])
            df.loc[df['Keyword'] == keyword_name, 'Ponderation'] = int(existing_keyword['Ponderation']) + score
            df.loc[df['Keyword'] == keyword_name, 'ReactionsCount'] = int(existing_keyword['ReactionsCount']) + article['ReactionsCount']
            df.loc[df['Keyword'] == keyword_name, 'CommentsCount'] = int(existing_keyword['CommentsCount']) + article['CommentsCount']

def extract_keywords(all_titles, all_articles, articles_df):
    df = pd.DataFrame(columns=['Keyword','ArticleIds', 'ArticleUrls', 'Nb', 'Ponderation', 'ReactionsCount', 'CommentsCount'])
    for i in range(articles_df.shape[0]):
        article = articles_df.iloc[i]
        title_keywords = all_titles[i]
        article_keywords = all_articles[i]
        aggregate_keywords(df, title_keywords, article, 1.4, 'TitleTokens')
        aggregate_keywords(df, article_keywords, article, 1, 'ArticleTokens')
    return df

def extract_trending_keywords_keyBERT():
    file = "cleaned_articles.csv"
    df = pd.read_csv(file)
    kw_model = KeyBERT()
    title_keywords = df["TitleTokens"].apply(kw_model.extract_keywords)
    article_keywords = df["ArticleTokens"].apply(kw_model.extract_keywords)
    trending_keywords = extract_keywords(title_keywords, article_keywords, df)
    trending_keywords.to_csv("extracted_trending_keywords.csv", sep=',', encoding='utf-8')

def analyse_result():
    file = "extracted_trending_keywords.csv"
    df = pd.read_csv(file)
    sorted_df = df.sort_values(by=['ReactionsCount', 'Nb', 'Ponderation'], ascending=False)[:5]
    nb_articles = list(map(lambda x: len(x.split(" ")), df['ArticleIds']))
    df.loc[:, 'NbArticles'] = [''] * len(df)
    df.loc[:, 'PonderationAmongAllArticles'] = [''] * len(df)
    df.loc[:, 'PonderationAmongAllReactions'] = [''] * len(df)
    df.loc[:, 'AllPonderations'] = [''] * len(df)
    df["NbArticles"] = nb_articles
    df["PonderationAmongAllArticles"] = df.apply(lambda row: row["Ponderation"] * row["NbArticles"], axis=1)
    df["PonderationAmongAllReactions"] = df.apply(lambda row: row["Ponderation"] * row["ReactionsCount"], axis=1)
    df["AllPonderations"] = df.apply(lambda row: row["PonderationAmongAllReactions"] + row["PonderationAmongAllArticles"], axis=1)
    print(sorted_df[['Keyword', 'ArticleUrls','Nb','Ponderation','ReactionsCount','CommentsCount']])

    # create a cluster of the KEYWORD & ARTICLES

    # fréquence d'utilisation du mot clef.
    # succès du mot clef 

    # évaluer les mots clefs, mettre une pondération sur le titre.
    # pour chaque article nous disposons d'un mot clef, ainsi que d'une fréquence.
    # calculer son cosine et faire un cluster.
    
    # text = []
    # for lst in evaluated_keywords:
    #     for component in lst:
    #         text.append(str(component[0]))
    # common_words = pd.DataFrame(Counter(text).most_common(20))
    # print(common_words)

# analyser le résultat : nous avons la liste des mots clefs tendances.
# pouvoir faire un cluster des tendances ???