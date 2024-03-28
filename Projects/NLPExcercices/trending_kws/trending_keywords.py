import pandas as pd
import numpy as np
import json
import hdbscan

from collections import Counter
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.feature_extraction.text import CountVectorizer
from keybert import KeyBERT
from sentence_transformers import SentenceTransformer

def extract_trending_keywords():
    file = "articles.csv"
    df = pd.read_csv(file)
    df["Content"] = df["Title"] + " " + df["Article"]
    # 1. Extract embedding
    kw_model = KeyBERT(model='all-mpnet-base-v2')
    doc_embeddings, word_embeddings = kw_model.extract_embeddings(df["Content"], keyphrase_ngram_range=(1,2))

    # 2. Extract the keywords.
    content_keywords = kw_model.extract_keywords(df["Content"], doc_embeddings=doc_embeddings, word_embeddings=word_embeddings, keyphrase_ngram_range=(1,2))
    keywords = []
    for lst in content_keywords:
        keywords.append([component[0] for component in lst])
    df["Keywords"] = keywords

    # 3. Reduce the doc embeddings
    reduced_embeddings = umap.UMAP(n_components=2, n_neighbors=5, min_dist=0.02).fit_transform(doc_embeddings)
    
    # 4. Cluster the articles
    clusterer = hdbscan.HDBSCAN(min_cluster_size=3)
    labels = clusterer.fit_predict(reduced_embeddings)
    df["Label"] = [str(label) for label in labels]

    # 5. Get trending keywords
    sorted_df = df.sort_values(by=['ReactionsCount'], ascending=False)
    print(sorted[["ReactionsCount", "Label", "Keywords"]][:30])

    # 6. Get topics with the most reactions
    grp = df.groupby(by=["Label"])
    count = grp.size().to_frame(name='counts')
    res = count.join(grp.agg({'ReactionsCount':'sum'}).rename(columns={'ReactionsCount': 'TotalReactions'}))
    print(res.sort_values(by=['TotalReactions'], ascending=False))