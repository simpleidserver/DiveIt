import pandas as pd
import umap
import hdbscan
import plotly.express as px

from sentence_transformers import SentenceTransformer
from keybert import KeyBERT

def filter_keywords(keywords, n_keep=5):
    new_keywords = []
    for candidate_keyword in keywords:
      is_ok = True
      for compare_keyword in keywords:
        if candidate_keyword == compare_keyword:
          continue
        if compare_keyword in candidate_keyword:
          is_ok = False
          break
      if is_ok:
        new_keywords.append(candidate_keyword)
        if len(new_keywords) >= n_keep:
          break
    return new_keywords

def get_cluster_name(df, cluster):
    df_subset = df[df["label"] == cluster].reset_index()
    texts_concat = ". ".join(df_subset["Article"].values)
    kw_model = KeyBERT()
    keywords_and_scores = kw_model.extract_keywords(texts_concat)
    keywords = [t[0] for t in keywords_and_scores]
    keywords_filtered = filter_keywords(keywords)
    return " - ".join(keywords_filtered)

def display_reduced(df):
    hover_data = {
        "Article_short": True,
        "x": False,
        "y": False
    }
    fig = px.scatter(df, x="x", y="y", template="plotly_dark", title="Embeddings", hover_data=hover_data)
    fig.show()

def cluster():
    # read articles.
    file = "../extract_articles/articles.csv"
    df = pd.read_csv(file)
    df = df.drop(df.columns[[0]], axis=1)
    articles = df["Article"]

    # Article embeddings
    embedder = SentenceTransformer('all-mpnet-base-v2')
    corpus_embeddings = embedder.encode(df["Article"].values)

    reduced_embeddings = umap.UMAP(n_components=2, n_neighbors=5, min_dist=0.02).fit_transform(corpus_embeddings)
    df["x"] = reduced_embeddings[:, 0]
    df["y"] = reduced_embeddings[:, 1]
    df["Article_short"] = df["Article"].str[:100]

    clusterer = hdbscan.HDBSCAN(min_cluster_size=3)
    labels = clusterer.fit_predict(reduced_embeddings)
    df["label"] = [str(label) for label in labels]

    df_no_outliers = df[df["label"] != "-1"]
    all_clusters = df_no_outliers["label"].unique()
    print(all_clusters)
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