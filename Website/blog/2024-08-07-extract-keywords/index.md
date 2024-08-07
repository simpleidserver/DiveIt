---
slug: part1-id-kws
title: Part 1 - NLP - Identifying Trending Keywords in .NET
authors: [geekdiver]
tags: [NLP, PYTHON, KEYWORDS]
enableComments: true
---

import Banner from './images/banner.jpg'
import GiscusComponent from '@site/src/components/GiscusComponent';

<div style={{textAlign: 'center'}}>
    <img src={Banner} />
</div>

## Introduction

Like any blogger, every week, I aim to identify popular keywords related to the .NET universe to assist me in crafting my future articles.

To aid me in this task, I am developing a console application in `python` that retrieves and analyzes articles with the hashtag **#dotnet** on the [dev.to](https://dev.to/) website.

The objective of the article is to comprehend the workings of the Natural Language Processing (NLP) domain through a concrete use case.

The process of keyword extraction comprises the following steps:
1. Retrieval of articles from the [dev.to](https://dev.to/) website.
2. Text transformation into vectors.
3. Extraction of keywords.
4. Identification of topics.
5. Data exploration.

Considering that we do not intend to process a large volume of data, performance has not been a determining factor in the choice of algorithms and Python libraries. 
However, the relevance of the keywords extracted from the articles is of greater importance.

## 1. Retrieving articles

After analyzing the HTTP traffic of the [dev.to](https://dev.to/) website, I was able to identify a REST API. It exposes an operation that returns the 60 latest articles tagged with #dotnet.

```
https://dev.to/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet
```

Knowing this, we can create a Python script that calls the operation.

```
import requests

article_result = requests.get("https://dev.to/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet")
```

A collection of elements is returned by this operation. Some properties of the element are noteworthy and can be exploited to evaluate the popularity of an article, for example:

* **public_reactions_counts** : Number of reactions.
* **comments_count** : Number of comments.

The content of the article is not returned by the HTTP operation.
But it is possible to retrieve it using the `path` property.

```
import requests
import pandas as pd
import multiprocessing

def read_article(url):
    article_content = requests.get(url['base_url'])
    return { 'article_content': article_content, 'id': url['id'] }

base_url = 'https://dev.to'
result = requests.get(base_url + '/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet')
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
```

The article is retrieved in its HTML form, but its content is not easily exploitable. 
This is where the [BeautifulSoup](https://www.crummy.com/software/BeautifulSoup/bs4/doc/) library comes into play. It is used to:

1. Clean up the unnecessary.

   1.2. Remove `iframe`, `script` tags.

   1.3 Remove code snippets written in different languages.

2. Remove emoticons.
3. Extract the content.

Thus, the `read_article` function can be rewritten in this way:

```
def read_article(url):    
    article_result = requests.get(url['base_url'])
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
    article_content = emoji_pattern.sub(r'', text)
    return { 'article_content': article_content, 'id': url['id'] }
```

The complete code for extracting the content of the articles can be found [here](https://github.com/simpleidserver/DiveIt/tree/main/Projects/NLPExcercices/trending_kws/extract_articles.py).

Now that the articles are ready to be exploited, we can transform the text into vectors.

## 2. Text to Vector Transformation

The vectorization process involves converting a string of characters into a vector of numerical values.

There are several transformation algorithms, here are a few: If you would like a complete list of possible algorithms, I invite you to read this [article](https://neptune.ai/blog/vectorization-techniques-in-nlp-guide).

| Algorithme | Description |
| ---------- | ----------- |
| Bag of Words (CountVectorizer) | Transforms a collection of text documents into a numerical matrix of word or token counts. |
| TF-IDF | Measure of originality of a word by comparing the number of times a word appears in a document with the number of documents the word appears in |
| Word2Vec | Uses the power of a simple Neural Network to generate word embeddings |
| Sentence Transformers | Provides a variety of pre-trained models that can convert sentences into meaningful numerical representations. |

In our program, we have chosen the [Sentence Transformer](https://www.sbert.net/) as the vectorization process.

Since the emergence in 2017 of the first transformer, introduced by the excellent scientific article [Attention is all you need](https://arxiv.org/abs/1706.03762), it has been generally accepted by the NLP community that pre-trained transformers are the most effective means of vectorization and enable the following tasks to be performed efficiently:

* **Semantic Textual Similarity (STS)** : Comparison of sentence pairs. We may want to identify patterns in datasets, but this is most often used for benchmarking.
* **Semantic search** : information retrieval (IR) using semantic meaning.
* **Clustering** : We can cluster our sentences, useful for topic modeling.

The Python program uses the [KeyBert](https://github.com/MaartenGr/KeyBERT) library, configured to utilize the [all-mpnet-base-v2](https://huggingface.co/sentence-transformers/all-mpnet-base-v2) model.

Here is the code used to vectorize the articles.
The parameter `keyphrase_ngram_range` is set to (1,2) as we aim to extract keywords consisting of one or two tokens.

```
file = "articles.csv"
df = pd.read_csv(file)
df["Content"] = df["Title"] + " " + df["Article"]
# 1. Extract embedding
kw_model = KeyBERT(model='all-mpnet-base-v2')
doc_embeddings, word_embeddings = kw_model.extract_embeddings(df["Content"], keyphrase_ngram_range=(1,2))
```

Now that the articles are transformed into vectors, we can extract the keywords.

## 3. Keyword Extraction

There are several Python libraries for extracting keywords.

For a more detailed list of libraries, I invite you to read the [article](https://www.analyticsvidhya.com/blog/2022/03/keyword-extraction-methods-from-documents-in-nlp/).

| Librairie | Description |
| ---------- | ----------- |
| KeyBert | Utilizes a pre-trained transformer to transform sentences into a set of vectors. It then uses cosine similarity calculation to determine if the token vector has a strong correlation with the document vector. |
| YAKE | Utilizes a statistical approach to select the most relevant keywords. |
| Rake | Identifies keywords based on their occurrence.. |

The KeyBERT library is the most resource-intensive, but it is more accurate and yields better results because it utilizes the cosine similarity calculation between vectors to choose keywords.

This algorithm has the advantage of considering the context of keyword usage, unlike other algorithms.

Here is the Python code to extract keywords:

```
content_keywords = kw_model.extract_keywords(df["Content"], doc_embeddings=doc_embeddings, word_embeddings=word_embeddings, keyphrase_ngram_range=(1,2))
keywords = []
for lst in content_keywords:
    keywords.append([component[0] for component in lst])
df["Keywords"] = keywords
```

Now that the keywords from the articles are extracted, we can group the articles by topic.

## 4. Topic Identification

To identify the topics, it is necessary to reduce the dimensionality of `doc_embeddings` to 2. For this purpose, the UMAP library is used.

```
# 3. Reduce the doc embeddings
reduced_embeddings = umap.UMAP(n_components=2, n_neighbors=5, min_dist=0.02).fit_transform(doc_embeddings)
```

You can find more details about the parameters (`n_components`, `n_neighbors`, `min_dist`) on the official [website](https://umap-learn.readthedocs.io/en/latest/parameters.html).

Next, the clustering algorithm [HDBSCAN](https://hdbscan.readthedocs.io/en/latest/how_hdbscan_works.html) is used to identify the topics of the articles.

```
# 4. Cluster the articles
clusterer = hdbscan.HDBSCAN(min_cluster_size=3)
labels = clusterer.fit_predict(reduced_embeddings)
df["Label"] = [str(label) for label in labels]
```

Now that we have all the information, we can analyze the data to identify the most popular keywords.

## 5. Data exploration.

The 60 latest articles were retrieved on March 27, 2024.

To retrieve the most popular keywords, which were sorted in descending order based on the `ReactionsCount` column, the following Python script was executed:

```
# 5. Get trending keywords
sorted_df = df.sort_values(by=['ReactionsCount'], ascending=False)
print(sorted[["ReactionsCount", "Label", "Keywords"]][:30])
```

As a result, the most popular keywords are:

* azure openai
* primary constructors
* loop execution
* iterate
* coding assistant

By grouping the articles by topic and calculating the sum of reactions, we observe that the topic which received the most reactions is the one discussing challenges in .NET.

```
# 6. Get topics with the most reactions
grp = df.groupby(by=["Label"])
count = grp.size().to_frame(name='counts')
res = count.join(grp.agg({'ReactionsCount':'sum'}).rename(columns={'ReactionsCount': 'TotalReactions'}))
print(res.sort_values(by=['TotalReactions'], ascending=False))
```

## Conclusion

The source code of the Python application can be found [here](https://github.com/simpleidserver/DiveIt/tree/main/Projects/NLPExcercices/trending_kws).

The results obtained by the application are satisfactory and accurate. They enable the rapid identification of the most popular keywords from the latest 60 articles.

This application is not easily usable by a blogger and requires some improvements, which will be the subject of future articles.

## Resources


| Link |
| ---- |
| https://neptune.ai/blog/vectorization-techniques-in-nlp-guide, Vectorization Techniques in NLP |
| https://www.pinecone.io/learn/series/nlp/sentence-embeddings/, Sentence Transformers: Meanings in Disguise |
| https://www.analyticsvidhya.com/blog/2022/03/keyword-extraction-methods-from-documents-in-nlp/, Keyword Extraction Methods from Documents in NLP |
| https://hdbscan.readthedocs.io/en/latest/how_hdbscan_works.html, HDBSCAN |

<GiscusComponent />