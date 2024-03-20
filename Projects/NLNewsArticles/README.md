Les propriétés d'un article sur dev.to :

title, reading_time, user, user_id, public_reactions_count, comments_count

class_name
cloudinary_video_url
comments_count
id
path
public_reactions_count
readable_publish_date
reading_time
title
user_id
public_reaction_categories
video_duration_string
published_at_int
tag_list
flare_tag
user

Il faut pouvoir extraire les thèmes des articles, tous les KEYWORDS.

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

# Observation

Article : 'https://dev.to/lovelacecoding/modern-c-development-record-types-101-55bj'

Pour cette article, nous observons que le mot le plus utilisé est `record`.

# Extraire les mots clefs

Article : 'https://dev.to/lovelacecoding/modern-c-development-record-types-101-55bj'

Algorithme : KeyBert
Résultat : ('immutability', 0.4226), ('immutable', 0.3791)

Algorithme : YAK
Résultat : 'mode Exit fullscreen', 0.007454046417715711), ('Exit fullscreen mode', 0.007454046417715711)

Références :

https://medium.com/ft-product-technology/predicting-ft-trending-topics-7eda85ece727

https://www.kaggle.com/code/arthurtok/spooky-nlp-and-topic-modelling-tutorial

https://nlp.stanford.edu/IR-book/html/htmledition/stemming-and-lemmatization-1.html, Stemming and lemmatization

https://monkeylearn.com/keyword-extraction/, Keyword extraction

https://github.com/abmami/Fine-tuning-CamemBERT-for-Keyword-Extraction, Fine-tuning CamemBERT for Keyword Extraction