import yake
from keybert import KeyBERT

def extract_keywords_with_bert(article):
    kw_model = KeyBERT()
    keywords = kw_model.extract_keywords(article)
    return keywords

def extract_keywords_with_yake(article):
    kw = yake.KeywordExtractor(lan='en', n=3)
    keywords = kw.extract_keywords(article)
    return keywords