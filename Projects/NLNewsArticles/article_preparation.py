import nltk
import numpy as np
import re
import string
from nltk.stem import WordNetLemmatizer
from sklearn.feature_extraction.text import CountVectorizer

stopwords = nltk.corpus.stopwords.words('english')
lemmatizer = WordNetLemmatizer()

class LemmaCountVectorizer(CountVectorizer):
    def build_analyzer(self):
        analyzer = super(LemmaCountVectorizer, self).build_analyzer()
        return lambda doc: (lemmatizer.lemmatize(w) for w in analyzer(doc))

def extract_and_clean_tokens(article):
    article_tokens = nltk.word_tokenize(article)
    article_tokens_cleaned = [lemmatizer.lemmatize(word) for word in article_tokens if word.lower() not in stopwords]

def get_corpus(article):
    vectoriser = CountVectorizer(stop_words='english')
    transform = vectoriser.fit_transform(article)
    return vectoriser.get_feature_names_out()

def extract_bag_of_words(article):
    vectoriser = LemmaCountVectorizer(stop_words='english')
    transform = vectoriser.fit_transform(article)
    features = vectoriser.get_feature_names_out()
    count_vec = np.asarray(transform.sum(axis=0)).ravel()
    zipped = list(zip(features, count_vec))
    x, y = (list(x) for x in zip(*sorted(zipped, key=lambda x: x[1], reverse=True)))
    return { 'x': x, 'y': y}

def clean_text(text, tokenizer):
    text = str(text).lower()  # Lowercase words
    text = re.sub(r"\[(.*?)\]", "", text)  # Remove [+XYZ chars] in content
    text = re.sub(r"\s+", " ", text)  # Remove multiple spaces in content
    text = re.sub(r"\w+…|…", "", text)  # Remove ellipsis (and last word)
    text = re.sub(r"(?<=\w)-(?=\w)", " ", text)  # Replace dash between words
    text = re.sub(f"[{re.escape(string.punctuation)}]", "", text)
    tokens = tokenizer(text)  # Get tokens from text
    tokens = [t for t in tokens if not t in stopwords]  # Remove stopwords
    tokens = ["" if t.isdigit() else t for t in tokens]  # Remove digits
    tokens = [t for t in tokens if len(t) > 1]  # Remove short tokens
    return tokens

def vectorize(list_of_docs, wv):
    features = []
    for tokens in list_of_docs:
        # zero_vector = np.zeros(model.vector_size)
        vectors = []
        for token in tokens:
            if token in wv:
                try:
                    vectors.append(wv[token])
                except KeyError:
                    continue
        if vectors:
            vectors = np.asarray(vectors)
            avg_vec = vectors.mean(axis=0)
            features.append(avg_vec)
    return features