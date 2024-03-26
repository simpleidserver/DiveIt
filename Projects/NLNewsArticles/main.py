import article_extractor
import extract_keywords
import cluster
import article_preparation
import pandas as pd
import gensim.downloader as api

from sklearn.cluster import MiniBatchKMeans
from sklearn.metrics import silhouette_samples, silhouette_score
from sklearn.decomposition import PCA
from nltk import word_tokenize
from gensim.models.word2vec import Word2Vec

def extract_clusters():
    # 1. extract articles.
    df = article_extractor.extract_articles_from_dev_to('60')
    tokenized_docs = df["Article"].map(lambda x: article_preparation.clean_text(x, word_tokenize))
    print('articles are extracted')
    # articles = df['Article'].values
    # tokenized_docs = df["Article"].map(lambda x: article_preparation.get_corpus([x]))
    # corpus_articles = list([article_preparation.get_corpus([article]) for article in articles])

    # 2.1 Train the algorithm
    # model = Word2Vec(tokenized_docs, vector_size=100, workers=1)

    # 2.2 Load a trained model.
    trained_model = api.load('glove-twitter-25')
    print('model is loaded')

    # 3. vectorize the model.
    vectorized_docs = article_preparation.vectorize(tokenized_docs, trained_model)

    # 4. cluster
    clustering, cluster_labels = cluster.mbkmeans_clusters(X=vectorized_docs, k=5, mb=100)
    df_clusters = pd.DataFrame({
        "text": df["Article"],
        "url": df["Path"],
        "tokens": [" ".join(text) for text in tokenized_docs],
        "cluster": cluster_labels
    })

    # Dislay representative tokens per cluster.
    for i in range(5):
        tokens_per_cluster = ""
        most_representative = trained_model.most_similar(positive=[clustering.cluster_centers_[i]], topn=5)
        for t in most_representative:
            tokens_per_cluster += f"{t[0]} "
        print(f"Cluster {i}: {tokens_per_cluster}")

    print(df_clusters["url"])
    print(df_clusters["cluster"])

def extract_key_words():
    url = 'https://dev.to/lovelacecoding/modern-c-development-record-types-101-55bj'
    article = article_extractor.read_article_content(url)
    # cosine similarity
    bert_keywords = extract_keywords.extract_keywords_with_bert(article)
    # features extraction
    yake_keywords = extract_keywords.extract_keywords_with_yake(article)
    print(bert_keywords)
    print(yake_keywords)

if __name__ == '__main__':
    extract_clusters()