import pandas as pd
import umap
import hdbscan
import plotly.express as px
import re
import pandas as pd
import re

from sklearn.cluster import KMeans
from sklearn.feature_extraction.text import TfidfVectorizer
from nltk.stem import WordNetLemmatizer
from nltk.corpus import stopwords
from nltk import word_tokenize

from sentence_transformers import SentenceTransformer
from keybert import KeyBERT

lemmatizer = WordNetLemmatizer()
stop_words = stopwords.words("english")

def get_cluster_name(df, cluster):
    df_subset = df[df["label"] == cluster].reset_index()
    texts_concat = ". ".join(df_subset["Article"].values)
    kw_model = KeyBERT()
    keywords_and_scores = kw_model.extract_keywords(texts_concat, keyphrase_ngram_range=(1,2), top_n=3)
    keywords = [t[0] for t in keywords_and_scores]
    return " - ".join(keywords)

def read_articles():
    file = "articles.csv"
    df = pd.read_csv(file)
    df = df.drop(df.columns[[0]], axis=1)
    return df

def prepare_article(article):
    article_cleaned = re.sub(r"https?://\S+|www\.\S+", "", article)
    article_cleaned = re.sub(
        r"[^a-zA-Z\s+]+", " ", article_cleaned
    ).lower()
    article_cleaned = re.sub(
        "(programming|net|dotnet)", "", article_cleaned
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

def display_cluster(df):
    df["Article_short"] = df["Article"].str[:100]
    df_no_outliers = df[df["label"] != "-1"]
    all_clusters = df_no_outliers["label"].unique()
    d_cluster_name_mapping = {}
    for cluster in all_clusters:
        if cluster == "-1":
            d_cluster_name_mapping[cluster] = "outliers"
        else:
            d_cluster_name_mapping[cluster] = get_cluster_name(df_no_outliers, cluster)

    df_no_outliers["label"] = df_no_outliers["label"].apply(lambda label: d_cluster_name_mapping[label])
    hover_data = {
        "Article_short": True,
        "x": False,
        "y": False
    }

    fig = px.scatter(df_no_outliers, 
        x="x",
        y="y",
        template="plotly_dark", 
        title="Embeddings", 
        color="label",
        hover_data=hover_data)
    fig.show()

def prepare_articles(df):
    cleanedArticles = list(map(lambda p: " ".join(prepare_article(p)), df['Article']))
    df["CleanedArticle"] = cleanedArticles

def cluster_tfid_kmeans():
    # If needed you must set the LOKY_MAX_CPU_COUNT environment variable.
    # Read articles.
    df = read_articles()

    # Prepare articles.
    prepare_articles(df)
    articles = df["CleanedArticle"]
    print(articles)

    # Vectorize.
    vectorizer = TfidfVectorizer()
    transformed = vectorizer.fit_transform(articles)
    
    # Reduce the number of dimensions
    reduced_embeddings = umap.UMAP(n_components=2, n_neighbors=5, min_dist=0.02, metric='hellinger').fit_transform(transformed)
    df["x"] = reduced_embeddings[:, 0]
    df["y"] = reduced_embeddings[:, 1]

    # Cluster.
    km = KMeans(n_clusters=5)
    km.fit(reduced_embeddings)
    labels = km.predict(reduced_embeddings)
    df["label"] = [str(label) for label in labels]

    # Display.
    display_cluster(df)

def cluster_transformer_hdbscan():
    # Read articles.
    df = read_articles()

    # Prepare articles.
    prepare_articles(df)
    articles = df["CleanedArticle"]

    # Vectorize.
    embedder = SentenceTransformer('all-mpnet-base-v2')
    corpus_embeddings = embedder.encode(articles.values)

    # Reduce the number of dimensions.
    reduced_embeddings = umap.UMAP(n_components=2, n_neighbors=5, min_dist=0.02).fit_transform(corpus_embeddings)
    df["x"] = reduced_embeddings[:, 0]
    df["y"] = reduced_embeddings[:, 1]

    # Cluster.
    clusterer = hdbscan.HDBSCAN(min_cluster_size=3)
    labels = clusterer.fit_predict(reduced_embeddings)
    df["label"] = [str(label) for label in labels]

    # Display.
    display_cluster(df)