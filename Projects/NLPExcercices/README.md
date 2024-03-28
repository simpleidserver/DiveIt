# NLP - Identifier les mots clefs tendances en .NET

Comme tout blogueur, chaque semaine, je souhaite pouvoir identifier les mots clefs populaires, relatifs à l'univers .NET, pour m'aider à produire mes futures articles.

Afin de m'aider dans cette tâche, je développe une application console en `python`, qui récupère et analyse les articles avec le hashtag #dotnet sur le site [dev.to](https://dev.to/).

L'objectif de l'article est de comprendre le fonctionnement du domaine Natural Language Processing (NLP), au travers un cas d'usage concret.

Le processus d'extration des mots clefs est constitué des étapes suivantes :
1. Récupération des articles du site [dev.to].
2. Transformation du texte en vecteurs.
3. Extraction des mots clefs.
4. Identifications des topics.
5. Exploitation des données.

Sachant que nous ne souhaitons pas traiter un gros volume de données, la performance n'a pas été un facteur déterminant dans le choix des algorithmes et des librairies python.
Cependant, la pertinance des mots clefs extraits des articles est plus importante.

## 1. Récupération des articles

Après avoir analysé le trafic HTTP du site [dev.to](https://dev.to/), j'ai pu identifier une API REST. Elle expose une opération qui retourne les 60 derniers articles qui ont le hashtag #dotnet.

```
https://dev.to/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet
```

Sachant cela, nous pouvons créer un script python, qui appel l'opération.

```
import requests

article_result = requests.get("https://dev.to/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet")
```

Une collection d'éléments est retournée par cette opération.
Certaines propriétés de l'élément sont intéressantes, et peuvent être exploitées pour évaluer la popularité d'un article, par exemples :
* public_reactions_count : Nombre de réactions.
* comments_count : Nombre de commentaires.

Le contenu de l'article n'est pas retourné par l'opération HTTP.
Mais il est possible de le récupérer en utilisant la propriété `path`.

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

L'article est récupéré sous sa forme HTML, mais son contenu est difficilement exploitable.
C'est là que rentre en jeu la librairie `BeautifulSoup`. Elle est utilisée pour :

1. Nettoyer le superflu.
1.1 Enlever les balises `iframe`, `script`.
1.2 Enlever les bouts de code écrits dans différents langages.
2. Supprimer les émoticones.
3. Extraire le contenu.

Ainsi la fonction `read_article` peut être réécrite de cette façon :

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

Le code complet pour extraire le contenu des articles, se trouve [ici](LINK).

Maintenant que les articles sont prêts à être exploités, nous pouvons transformer le texte en vecteurs.

## 2. Transformation du texte en vecteurs

Le processus de vectorisation consiste à transformer une chaîne de caractères en un vecteur de nombres numériques.

Il existe plusieurs algorithmes de transformation, dont voici quelques uns.
Si vous souhaitez une liste complète des algorithmes possibles, je vous invite à lire cet [article](https://neptune.ai/blog/vectorization-techniques-in-nlp-guide).

| Algorithme | Description |
| ---------- | ----------- |
| Bag of Words (CountVectorizer) | Transforms a collection of text documents into a numerical matrix of word or token counts. |
| TF-IDF | Measure of originality of a word by comparing the number of times a word appears in a document with the number of documents the word appears in |
| Word2Vec | Uses the power of a simple Neural Network to generate word embeddings |
| Sentence Transformers | Provides a variety of pre-trained models that can convert sentences into meaningful numerical representations. |

Dans notre programme, nous avons choisi le [Sentence Transformer](https://www.sbert.net/) comme processus de vectorization.

Depuis l'apparitition en 2017 du premier transformeur, introduit par l'excellent article scientifique [Attention is all you need](https://arxiv.org/abs/1706.03762), il a été globalement acté par la communauté NLP, que les transformeurs pré-entrainés sont le moyen le plus efficace de vectorization, et permettent d'effectuer les tâches suivantes de manière efficiente :

* **Semantic Textual Similarity (STS)** : Comparison of sentence pairs. We may want to identify patterns in datasets, but this is most often used for benchmarking.
* **Semantic search** : information retrieval (IR) using semantic meaning.
* **Clustering** : We can cluster our sentences, useful for topic modeling.

Le programme python utilise la librairie [KeyBert](https://github.com/MaartenGr/KeyBERT), elle est configurée pour utiliser le modèle [all-mpnet-base-v2](https://huggingface.co/sentence-transformers/all-mpnet-base-v2).

Voici le code utilisé pour vectorizer les articles. 
Le paramètre `keyphrase_ngram_range` est égale à `(1,2)`, car nous voulons extraire les mots clefs de un ou deux tokens.

```
file = "articles.csv"
df = pd.read_csv(file)
df["Content"] = df["Title"] + " " + df["Article"]
# 1. Extract embedding
kw_model = KeyBERT(model='all-mpnet-base-v2')
doc_embeddings, word_embeddings = kw_model.extract_embeddings(df["Content"], keyphrase_ngram_range=(1,2))
```

Maintenant que les articles sont transformés en vecteurs, nous pouvons extraire les mots clefs.

## 3. Extraction des mots clefs

Il existe plusieurs librairie python pour extraire les mots clefs. 

Si vous souhaitez une liste plus détaillée des librairies, je vous invite à lire [l'article](https://www.analyticsvidhya.com/blog/2022/03/keyword-extraction-methods-from-documents-in-nlp/).

| Librairie | Description |
| ---------- | ----------- |
| KeyBert | Utilise un transformateur déjà entraîné pour transformer les phrases en un ensemble de vecteurs. Il utilise ensuite le calcul des similarités cosinus, pour déterminer si le vecteur du token a une forte corrélation avec le vecteur du document. |
| YAKE | Utilise une approche statistique pour sélectionner les mots clefs les plus pertinents |
| Rake | Identifie les mots clefs en se basant sur leur occurence. |

La librairie `KeyBERT` est la plus coûteuse en ressource, mais elle est plus précise et obtient de meilleurs résultats, car elle utilise le résultat du calcul des similarités cosinus entre les vecteurs pour choisir les mots clefs.

Cet algorithme a l'avantage de tenir compte du contexte d'utilisation du mot clef, contrairement aux autres algorithmes.

Voici le code python pour extraire les mots clefs :

```
content_keywords = kw_model.extract_keywords(df["Content"], doc_embeddings=doc_embeddings, word_embeddings=word_embeddings, keyphrase_ngram_range=(1,2))
keywords = []
for lst in content_keywords:
    keywords.append([component[0] for component in lst])
df["Keywords"] = keywords
```

Maintenant que les mots clefs des articles sont extraits, nous pouvons regrouper les articles par topic.

## 4. Identifications des topics.

Pour identifier les topics, il est nécessaire de réduire le nombre de dimension du `doc_embeddings` à 2, pour cela la librairie [UMAP](https://umap-learn.readthedocs.io/en/latest/) est utilisée.

```
# 3. Reduce the doc embeddings
reduced_embeddings = umap.UMAP(n_components=2, n_neighbors=5, min_dist=0.02).fit_transform(doc_embeddings)
```

Vous trouverez plus de détails sur les paramètres (`n_components`, `n_neighbors`, `min_dist`) sur le site officiel [https://umap-learn.readthedocs.io/en/latest/parameters.html](https://umap-learn.readthedocs.io/en/latest/parameters.html).

Ensuite, l'algorithme de clustering [HDBSCAN](https://hdbscan.readthedocs.io/en/latest/how_hdbscan_works.html) est utilisée pour identifier les topics des articles.

```
# 4. Cluster the articles
clusterer = hdbscan.HDBSCAN(min_cluster_size=3)
labels = clusterer.fit_predict(reduced_embeddings)
df["Label"] = [str(label) for label in labels]
```

Maintenant que nous disposons de toutes les informations, nous pouvons exploiter les données afin d'identifier les mots clefs les plus populaires.

## 5. Exploitation des données

MOTS CLEFS LES PLUS TENDANCES SELON LA POPULARITE DE L ARTICLE PRENDRE LES 10 premiers.
* DATE
* (KW1 + R / KW2 + R / KW3 + R)



TOPICS AVEC LE PLUS D INTERET


# Conclusion

VERSION FUTURE, NOUS POUVONS 


# Ressources

https://neptune.ai/blog/vectorization-techniques-in-nlp-guide, Vectorization Techniques in NLP

https://www.pinecone.io/learn/series/nlp/sentence-embeddings/, Sentence Transformers: Meanings in Disguise

https://www.analyticsvidhya.com/blog/2022/03/keyword-extraction-methods-from-documents-in-nlp/, Keyword Extraction Methods from Documents in NLP

https://hdbscan.readthedocs.io/en/latest/how_hdbscan_works.html, HDBSCAN