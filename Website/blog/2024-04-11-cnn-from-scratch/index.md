# Implement Convolutional Neural Network from scratch

## Neural network architecture

Un reseau neuronal est composé de plusieurs couches de neurones interconnectés. Les couches sont organisées en trois types :

1. Input layer : The input layer consists of neurons representing the features of the input data. Each neuron corresponds to a feature, and its value represents the feature’s value.

2. Hidden layers : Entre la couche d'entrée et la couche de sortie, il peut y avoir une ou plusieurs couches cachées. Chaque neurone d'une couche cachée, reçoit des données de la couche précédente, applique un `weighted sum`, ajoute un `bias term` et passe le résultat à une fonction d'activation.

3. Output layer : produit le résultat de la prédiction. Le nombre de neurones dépend de la nature de la prédiction, par exemple pour une classification binaire, il peut y avoir un neurone par classe, avec la probablité d'appartenir à cette classe.

Les paramètres pour chaque neurone :

* **Connection Weights** : mesure le poids entre deux neurones. Chaque connection, d'une neurone A vers un neurone B, possède un poids.
* **Bias terms** : Chaque neurone possède un `bias term`. 
* **Activation function** : Chaque neurone applique une fonction d'activation sur le `weighted sum` et le bias. Quelques exemples de fonction d'activation : RELU.

### Forward propagation

Algorithme de forward :
1. Les données en entrée sont assignées aux neurones.

2. Chaque neurone d'une couche cachée exécute ces opérations :

2.1. Weighted Sum (Z): Z = Σ(Wij * Xj) + bi, where Wij is the weight, Xj is the output of the j-th neuron in the previous layer, and bi is the bias of the current neuron.

2.2. Activation (A): A = σ(Z), where σ is the activation function.

TODO

### Backward propagation

TODO

## Convolutional Layer

Réseau neuronal constitué de plusieurs couches convolutives, elle se base sur l'opération mathématique de `convolution`.

Une couche de `convolution` possède un ensemble de filtres, dont chaque filtre est représenté sous la forme d'une matrice en deux dimensions.

### Algorithme de convolution

L'algorithme de convolution est constitué des étapes suivantes :
1. Lire et extraire les couleurs RGB de l'image.
2. Transformer les trois matrice RGB, en une matrice de nuance de gris de taille (iw * ih).
3. Créer une matrice `kernel`, appelée aussi la fenêtre de `convolution` de taille (kw * kh). Par défault, la matrice aura une taille de (3x3).
4. Créer une matrice de sortie de taille ((iw - kw + 1) * (ih - kh + 1)). Le calcul de la taille sera différent avec l'introduction des deux paramètres `padding` et `stride`.
5. Récupérer toutes les fenêtres de l'image en entrée. Le centre de la matrice `kernel` est utilisé comme un pointeur, l'algorithme bouge le pointeur sur chaque colonne et chaque ligne de l'image. La zone concernée par le kernel, appelée aussi fenêtre, est alors stockée dans une liste.

[INPUT_WINDOW]

* Pour chaque fenêtre de l'image, appliquer le filtre et stocker le résultat dans la matrice de sortie.

[WORKFLOW]

L'algorithme est convultion est écrite sous cette forme mathématique :

[MATH] https://web.pdx.edu/~jduh/courses/Archive/geog481w07/Students/Ludwig_ImageConvolution.pdf

### Paramètre de padding

En appliquant plusieurs couches de `convolution` successives, il existe un risque de perdre des pixels sur les bords de l'image.

La solution la plus couramment utilisée, consiste à définir le paramètre `padding`, il indique le nombre de pixels qui seront ajoutés sur les bords de l'image.

[IMAGE]

Dans ce tutoriel, le paramètre de `padding` est défini par la variable `p`.

En tenant compte de ce paramètre, la taille de la matrice de sortie est obtenue par cette formule :

```
(iw - kw + pw + 1) * (ih - kh + ph + 1)
```

Dans beaucoup de cas, la valeur du padding est égale à `(kw-1,kh-1)`, pour que l'image en sortie ait la même taille que celle de l'image en entrée.

### Paramètre stride

Pour chaque pixel de l'image, une fenêtre de convolution est appliquée.
Cette opération peut être coûteuse en ressource, surtout lorsque l'algorithme est appliqué sur une image de grande taille.

Pour répondre à cette problématique, le paramètre `stride` a été introduit. Il définit le nombre d'éléments devant être ignorés sur la largeur et la hauteur de l'image. Par défault ce paramètre est égale à (1,1).

Dans ce tutoriel, le paramètre `stride` est défini par les variables `sw` et `sh`.

La formule pour calculer la taille de l'image en sortie, est mise à jour comme suit :

```
((iw - kw + pw + sw) / sw, (ih - kh + ph + sh) / sh)
```

### Filtres

Il existe plusieurs types de filtre, la liste complète se trouve sur [wikipedia](https://en.wikipedia.org/wiki/Kernel_(image_processing)).

| Filtre   | 
| -------- |
| Identity | 
| Ridge or edge detection |

### Forward

TODO

### Backward

TODO

## Pooling layer

L'algorithme réduit la taille de la matrice d'entrée, en appliquant une des opérations `max`, `min` ou `average` sur chaque fenêtre. 
L'objectif est double :

1. Réduire la dimension.
2. Résumer les données présentes dans une région, de cette façon, les futures opérations sont exécutées sur un résumé au lieux de données précises.

### Forward propagation

L'algorithme est constitué des étapes suivantes :

1. Créer une matrice de sortie de taille ((iw / pw), (ih / ph)).

2. Décomposer la matrice en entrée en plusieurs fenêtres de taille (pw, ph).

### Backward propagation

Etant donné que le `Pooling layer` ne possède aucun `weights`, il faut trouver le gradient de l'erreur pour chaque élément de la matrice d'entrée : $\frac{\delta E}{\delta x}$.

1. Créer une matrice de sortie, de la même taille que celle qui a été reçue en dernier.

2. Décomposer la matrice en plusieurs fenêtres de taille (pw, ph).

3. Pour chaque fenêtre, si l'élément n'est pas la maximum alors mettre 0, sinon prendre le gradient de l'erreur.

[IMAGE]

3. Pour chaque fenêtre appliquer une des opérations : `max`, `min`, `average`.

## Softmax

Nous utilisons comme dernière couche, une couche dense d'activation de type `softmax`.

La fonction `softmax` transforme des données arbitraires, en probabilités. Elle est utilisée pour calculer la probabilité d'appartenance à une catégorie.

[AJOUTER LE CALCUL].

### Forward propagation

TODO

### Backward propagation

TODO

## Resources

| Link |
| ---- |
| https://victorzhou.com/blog/intro-to-cnns-part-1/, CNNs, Part 1: An Introduction to Convolutional Neural Networks |
| https://observablehq.com/@lemonnish/cross-correlation-of-2-matrices, Cross-correlation of 2 matrices |
| https://www.ibm.com/topics/convolutional-neural-networks, What are convolutional neural networks? |
| https://developer.nvidia.com/discover/convolution#:~:text=The%20convolution%20algorithm%20is%20often,convolution%20operation%20is%20called%20deconvolution., Convolution |
| https://cezannec.github.io/Convolutional_Neural_Networks/, Convolutional Neural Networks |
| https://web.pdx.edu/~jduh/courses/Archive/geog481w07/Students/Ludwig_ImageConvolution.pdf |
| https://d2l.ai/chapter_convolutional-neural-networks/conv-layer.html#fig-correlation, Convolutions for image |
| https://python.plainenglish.io/building-a-handwritten-alphabets-classifier-using-convolutional-neural-networks-2f84a47eb3ec, Building a Handwritten Alphabets Classifier using Convolutional Neural Networks |
| https://towardsdatascience.com/a-comprehensive-guide-to-convolutional-neural-networks-the-eli5-way-3bd2b1164a53, A Comprehensive Guide to Convolutional Neural Networks — the ELI5 way |
| https://towardsdatascience.com/math-neural-network-from-scratch-in-python-d6da9f29ce65, Neural Network from scratch in Python |
| https://victorzhou.com/blog/softmax/, Softmax |