# NLP - Identifier les mots clefs tendances en .NET

Comme tout blogueur, chaque semaine, je souhaite pouvoir identifier les mots clefs populaires, relatifs à l'univers .NET, pour m'aider à produire mes futures articles.

Afin de m'aider dans cette tâche, je vais développer une application console en `python`, qui va récupérer et analyser les articles avec le hashtag #dotnet sur le site [dev.to](https://dev.to/).

L'objectif de cet article est de comprendre le fonctionnement du domaine Natural Language Processing (NLP), au travers un cas d'usage concret.

Le processus d'extration des mots clefs se décompose de cette façon :
1. Récupérer les articles du site [dev.to].
2. Extraire les mots clefs.
3. Récupérer les mots clefs les plus populaires.

## Récupérer les articles

Après avoir analysé le traffic HTTP du site [dev.to], j'ai pu identifier une API REST. Elle expose une opération qui retourne les 60 derniers articles qui ont le hashtag #dotnet.

```
https://dev.to/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet
```

Sachant cela, nous pouvons créer un script python pour appeler l'opération.

```
import requests

article_result = requests.get("https://dev.to/search/feed_content?per_page=60&page=0&sort_by=published_at&sort_direction=desc&tag_names%5B%5D=dotnet&search_fields=dotnet")
```

Une collection d'éléments est retournée par cette opération. Certaines propriétés de l'élément sont intéressantes, et peuvent être exploitées pour évaluer la popularité d'un article, par exemples :
* public_reactions_count : Nombre de réactions.
* comments_count : Nombre de commentaires.

Le contenu de l'article n'est pas retourné par l'opération, mais il peut être récupéré en utilisant la propriété `path`.

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

L'article est récupéré sous forme HTML et est difficilement exploitable.
C'est là que la librairie `BeautifulSoup` rentre en jeu, et est utilisée pour :
1. Nettoyer le superflu comme les balises `iframe`, `script`, les bouts de code écrits dans différents langages.
2. Supprimer les émoticones.
3. Extraire le contenu.

La fonction read_article peut être réécrite de cette façon :

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

Vous trouverez le code complet de l'extraction des articles [ici]().

Maintenant que les articles sont prêts à être exploités, les mots clefs peuvent être extraits.

## Extraire les mots clefs

Il existe plusieurs algorithmes pour extraire les mots clefs :

| Algorithme | Description |
| ---------- | ----------- |
| KeyBert | Utilise un transformeur déjà entraîné sur huggingface pour extraire les documents embeddings avec BERT. Il utilise le cosine similarity des vecteurs, pour trouver les mots similaires au document |
| [Yake](https://github.com/LIAAD/yake)       | Se base sur une approche statistique pour sélectionner les mots clefs les plus pertinents |
| Rake | Identifier les mots clefs en se basant sur leur occurence |

L'algorithme `KeyBERT` est la plus coûteuse en ressource, mais elle est plus précise et obtient de meilleurs résultats, car elle utilise le cosine similarity entre les vecteurs pour choisir les mots clefs.

Voici l'algorithme pour extraire les mots clefs :

```
import pandas as pd
from keybert import KeyBERT

file = "articles.csv"
df = pd.read_csv(file)
title_keywords = df["Title"].apply(kw_model.extract_keywords, keyphrase_ngram_range=(1,3), top_n=2)
article_keywords = df["Article"].apply(kw_model.extract_keywords, keyphrase_ngram_range=(1,3), top_n=2)
```

Les paramètres suivants sont passées à la fonction `extract_keywords`

| Paramètre             | Valeur | Motivation                                       |
| --------------------- | ------ | ------------------------------------------------ |
| keyphrase_ngram_range | (1,2)  | Extraire les mots clefs avec un ou deux tokens   |
| top_n                 | 2      | Extraire les deux mots clefs les plus pertinents |

Maintenant que les mots clefs des articles sont extraits, nous pouvons extraire les mots clefs les plus tendances.

## Récupérer les mots clefs les plus populaires.

Nous assumons que la popularité d'un mot clef est dépendante de celle de l'article où il se trouve.
Dans la première partie, nous avons remarqué que les propriétés ``, `` retournées par l'opération de l'API peuvent être exploitées, pour calculer la populartié d'un mot clef.

Voici la formule utilisée pour calculer la popularité d'un mot clef :

```
P(i) = SUM(A) * SUM(W) + SUM(R) * W
```

* SUM(A) : Nombre total d'articles qui possède le mot clef.
* SUM(R) : Nombre total de réactions.
* W : Pondération sur l'importance de la réaction dans le calcul, par défault cette valeur est égale à 0.4.

IL FAUT MERGE TITLE ET CONTENU POUR EXTRAIRE LES MOTS CLEFS

# Ressources

Implémenter l'approche proposée par https://towardsdatascience.com/extract-trending-stories-in-news-29f0e7d3316a

https://medium.com/ft-product-technology/predicting-ft-trending-topics-7eda85ece727

https://www.kaggle.com/code/arthurtok/spooky-nlp-and-topic-modelling-tutorial

https://nlp.stanford.edu/IR-book/html/htmledition/stemming-and-lemmatization-1.html, Stemming and lemmatization

https://monkeylearn.com/keyword-extraction/, Keyword extraction

https://github.com/abmami/Fine-tuning-CamemBERT-for-Keyword-Extraction, Fine-tuning CamemBERT for Keyword Extraction

https://machinelearningmastery.com/develop-word-embeddings-python-gensim/?ref=dylancastillo.co, How to Develop Word Embeddings in Python with Gensim

https://www.nltk.org/book/ch06.html

https://www.nlplanet.org/course-practical-nlp/02-practical-nlp-first-tasks/12-clustering-articles, Clustering Newspaper Articles