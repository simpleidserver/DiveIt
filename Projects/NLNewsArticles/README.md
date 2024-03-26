Introduction à NLP

# Préparer le contenu de l'article

Maintenant que le contenu de l'article a été récupéré, nous allons exploiter son contenu.
1. La première étape, est la `Tokenization` d'un texte. Récupérer chaque mot, et stocker le résultat dans une liste.
2. Les mots vides (`stop words`), comme `the` ou `to` doivent être supprimés de cette liste.

# Appliquer le processus de Stemming

Le processus de `Stemming` consiste à diminuer le nombre de variations du même mot.
En réduisant chaque mot vers leur forme de base par exemple  : `running` et `runs` deviennent `run`.
Sans rentrer dans les détails, le processus se contente de supprimer les `affixes` (prefixes, suffixes) des mots.

Cette approche a plusieurs désavantages :
* Nous perdons la notion du temps, le passé comme le futur.
* Supprime les dernières lettres d'un mot, pouvant entraîner un changement dans la signication du mot par exemple `Caring` devient `Car`.

Cette approche est rapide et appropriée pour un large jeux de données.

# Appliquer le processus de Lemmatization

Le processus de `Lemmatization` est similaire au processus de `Stemming`.
Il réduit le mot vers sa forme de base, appelée `lemma`.
L'algorithme fait une analyse linguistique, en ajoutant des libéllés à chaque mot (nom, verbe, adjectif), et dérive chaque mot
vers leur meilleure forme racine.

# D'autres approches

Dans le domaine NLP, il existe des algorithmes pour classifier les articles, découvrir les topics ou mesurer la distance entre deux textes. ...
Ces algorithmes travaillent sur des matrices binaires.
Pour travailler avec eux, il est nécessaire de transformer le contenu d'un article en une matrice binaire.

L'approche la plus connue est `Bag-Of-Word`. Elle récupère les mots de toutes les phrases, et créé pour chaque phrase une vecteur, où chaque cellule représente l'occurence d'un mot.

# Extraire les mots clefs

Il existe plusieurs méthodes pour extraire les mots clefs d'un article comme `KeyBert`, `YAK`, `RAKE` ou encore `TF-IDF`.

**KeyBert** : Utilise la similarité `cosine`. Chercher des phrases dont la signification se rapproche de la signification globale du document.
Pour plus d'informations, se référer à la documentation : https://github.com/MaartenGr/KeyBERT

**Yake** : Approche non supervisée, pour extraire les mots clefs en utilisant les `Text Features`.

# Identifier cluster de documents

L'approche proposée par le site https://dylancastillo.co/nlp-snippets-cluster-documents-using-word2vec/, pour classifier un ensemble d'articles :

1. Nettoyer et extraire les tokens des articles.
2.1 Générer et entrainer un modèle Word2Vec. Transforme des mots en vecteurs numériques, réseaux de neurones à deux couches entraînés pour reconstruire le contexte linguistique d'un mort.
2.2 Charger un modèle Word2Vec déjà entraîné.
3. Utiliser le modèle Word2Vec pour générer un ensemble de vecteur pour chaque mot.
4. Pour créer un cluster de document, utiliser l'algorithme `Mini-Batches K`.

Implémenter l'approche proposée par https://towardsdatascience.com/extract-trending-stories-in-news-29f0e7d3316a

https://medium.com/ft-product-technology/predicting-ft-trending-topics-7eda85ece727

https://www.kaggle.com/code/arthurtok/spooky-nlp-and-topic-modelling-tutorial

https://nlp.stanford.edu/IR-book/html/htmledition/stemming-and-lemmatization-1.html, Stemming and lemmatization

https://monkeylearn.com/keyword-extraction/, Keyword extraction

https://github.com/abmami/Fine-tuning-CamemBERT-for-Keyword-Extraction, Fine-tuning CamemBERT for Keyword Extraction

https://machinelearningmastery.com/develop-word-embeddings-python-gensim/?ref=dylancastillo.co, How to Develop Word Embeddings in Python with Gensim

https://www.nltk.org/book/ch06.html