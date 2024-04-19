# Implement Convolutional Neural Network from scratch

import Architecture from './images/architecture.jpg'
import BackPropagation from './images/backpropagation.jpg'

Dans cet article, nous allons expliquer comment écrire de a à z en C#, un réseau neuronal de type Convolutional (CNN), capable de reconnaître le chiffre présent dans une image.

Si vous êtes familié avec le concept de réseau neuronal, je vous invite à ignorer ce chapitre et passer au suivant.

## Neural Network Architecture

Un reseau neuronal est composé de plusieurs couches interconnectés les unes aux autres, via les neurones qui les constituent.

Il existe trois types de couche.

<div style={{textAlign: 'center'}}>
    <img src={Architecture} height="300" />
</div>

### 1. Input Layer

The input layer consists of neurons representing the features of the input data. Each neuron corresponds to a feature, and its value represents the feature’s value.

### 2. Hidden Layer

Entre la couche d'entrée et celle de sortie, il peut exister plusieurs couches cachées.

De manière générale, chaque neurone d'une couche cachée est connectée à tous les neurones de la couche précédente, on dit que la couche est `fully connected` ou de type `dense layer`.

Une couche cachée possède deux paramètres :

* **weights** : Chaque connection existante entre deux neurones possède une `weight`, ce paramètre a une influence sur la quantité d'information transmise entre deux neurones.

* **bias** : Constante assignée à une couche.

Ces deux paramètres interviennent dans la formule de calcul du `WeightedSum`.

$$
SumWeighted = bias + \sum_{i=1}^{n}xi *weighti
$$

Ces deux paramètres sont ajustés au cours du processus d'apprentissage, durant l'étape de `back propagation`.

Une fonction d'activation peut ensuite être appliquée sur le paramètre $$SumWeighted$$ calculé.
De manière générale, le même type de fonction est choisi sur toutes les couches cachées. Le choix dépendra de la nature du réseau neuronal, par exemple un réseau de type CNN ou MLP utilise ReLU.

Il existe plusieurs types de fonction dont voici la liste.

$$
output = f(SumWeighted)
$$

| Activation                         | Network type |
| ---------------------------------- | ------------ |
| Rectified Linear Activation (ReLU) | MLP,CNN      |
| Logistic (Sigmoid)                 | RNN          |
| Hyperbolic Tangent (Tanh)          | RNN          |

:::danger

Il existe des couches où les paramètres `weights`, `bias` et la fonction d'activation ne sont pas utilisées.

:::

### 3. Output Layer

La dernière couche du réseau neuronal, est la couche de sortie.
Elle possède la même structure qu'une couche cachée, car elle est composée de plusieurs neurones et peut posséder les paramètres `weights` et `bias`.

Cette dernière reçoit les informations de la dernière couche cachée, et effectue des calculs pour prédire ou classifier les données reçues.

Selon la nature du problème, que le réseau neuronal tente de résoudre, vous pouvez choisir parmi l'une de ces couches :

| Problème                               | Algorithme          |
| -------------------------------------- | ------------------- |
| Classification de plus de deux classes | Softmax             |
| Classification binaire                 | Sigmoid             |
| Régression                             | Régression linéaire |

### Algorithme d'entraînement d'un réseau neuronal

Maintenant que vous avez une vue d'ensemble des éléments qui constituent un réseau neuronal.

Nous allons expliquer les différentes étapes pour entraîner un réseau neuronal, dont voici les grandes lignes.

1. Initialiser les paramètres de toutes les couches : `weights` ou `bias`.
2. Exécuter les points suivants `N` fois.
2.1 Pour chaque couche exécuter l'étape de `forward propagation`.
2.2 Calculer l'erreur.
2.3 Pour chaque couche exécuter l'étape de `backward propagation`.

#### 1. Initialisation des paramètres

Les paramètres d'apprentissage des couches, comme `weights` et `bias` doivent être initialisées avec des valeurs aléatoires.

Selon la méthode [Xavier Initialization](https://cs230.stanford.edu/section/4/), cette étape est plus importante que l'on pense, car si les valeurs initiales sont trop grandes ou trop petites, alors l'entraînement du réseau neuronal devient inefficace. 

Selon la nature de la couche, il ne sera peut être pas nécessaire d'initialiser ces paramètres.

#### 2. Forward propagation

L'étape de `Forward propagation` est exécutée lorsqu'une couche reçoit les données d'une autre. Elle est constituée des étapes suivantes :

1. Pour chaque neurone calculer le `sum weighted` : $sm = \sum_{i=1}^{n}xi *wi$.

1.1. $wi$ est la valeur du weight que le neurone possède avec le ième neurone.

1.2. $xi$ est la valeur du ième neurone.

2. Faire le somme du `sum-weighted` avec le paramètre `bias` :  $t = b + \sum_{i=1}^{n}xi *wi$.

3. Appeler la fonction d'activation : $f(b + \sum_{i=1}^{n}xi *wi)$.

#### 3. Calculer l'erreur

Lorsque les données sont reçues de la dernière couche, le résultat peut être comparé avec celui attendu.
La différence donne l'erreur que le réseau a fait durant sa prédiction.

Il existe différentes methodes pour calculer l'erreur, encore une fois le choix dépendra de la nature du problème.

| Problème                               | Fonction de calcul de la perte |
| -------------------------------------- | ------------------------------ |
| Classification de plus de deux classes | Cross Entropy Loss             |
| Classification sur deux classes        | Logistic loss                  |

#### 4. Backpropagation

L'erreur calculée par la fonction de perte est ensuite propagée sur les différentes couches.

C'est dans le processus de `Backpropagation` que l'apprentissage commence, l'objectif est de réduire le coût de la fonction de perte en ajustant les paramètres `weights` et `bias` des différentes couches.

Le niveau d'ajustement des variables `weights` et `bias` est calculé par des gradients. Ils permettent de comprendre comment une variable comme le `weight` peut influer sur le résultat `E total`.

$$
\frac{\delta Etotal}{\delta weight}

\frac{\delta Etotal}{\delta bias}
$$

<div style={{textAlign: 'center'}}>
    <img src={BackPropagation} height="300" />
</div>

## Convolutional Neural Network (CNN)

Le réseau neuronal adapté pour la reconnaissance d'image est de type Convolutional.

L'architecture typique d'un réseau CNN est constituée de quatre couches.

* Input layer : les données sont exposées sous deux formats différents.
** Extraire l'image en trois matrices (RGB) de deux dimensions.
** Extraire les nuances de gris de l'image en une matrice de deux dimensions.
* First hidden layer : Convolutional layer.
* Second hidden layer : Max pooling layer.
* Output layer : softmax layer.

[ARCHITECTURE]
### 1. Convolutional Layer

Réseau neuronal constitué de plusieurs couches convolutives, elle se base sur l'opération mathématique de `convolution`.

Une couche de `convolution` possède un ensemble de filtres, dont chaque filtre est représenté sous la forme d'une matrice en deux dimensions.

#### Algorithme de convolution

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

#### Paramètre de padding

En appliquant plusieurs couches de `convolution` successives, il existe un risque de perdre des pixels sur les bords de l'image.

La solution la plus couramment utilisée, consiste à définir le paramètre `padding`, il indique le nombre de pixels qui seront ajoutés sur les bords de l'image.

[IMAGE]

Dans ce tutoriel, le paramètre de `padding` est défini par la variable `p`.

En tenant compte de ce paramètre, la taille de la matrice de sortie est obtenue par cette formule :

```
(iw - kw + pw + 1) * (ih - kh + ph + 1)
```

Dans beaucoup de cas, la valeur du padding est égale à `(kw-1,kh-1)`, pour que l'image en sortie ait la même taille que celle de l'image en entrée.

#### Paramètre stride

Pour chaque pixel de l'image, une fenêtre de convolution est appliquée.
Cette opération peut être coûteuse en ressource, surtout lorsque l'algorithme est appliqué sur une image de grande taille.

Pour répondre à cette problématique, le paramètre `stride` a été introduit. Il définit le nombre d'éléments devant être ignorés sur la largeur et la hauteur de l'image. Par défault ce paramètre est égale à (1,1).

Dans ce tutoriel, le paramètre `stride` est défini par les variables `sw` et `sh`.

La formule pour calculer la taille de l'image en sortie, est mise à jour comme suit :

```
((iw - kw + pw + sw) / sw, (ih - kh + ph + sh) / sh)
```

#### Filtres

Il existe plusieurs types de filtre, la liste complète se trouve sur [wikipedia](https://en.wikipedia.org/wiki/Kernel_(image_processing)).

| Filtre   |
| -------- |
| Identity |
| Ridge or edge detection |

EXAMPLE DE INPUT LAYER POUR CNN : L'input layer peut avoir 3 canneaux pour les couleurs RGB ou un seul pour les nuances de gris.

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
| https://www.sciencedirect.com/topics/computer-science/convolutional-layer, Convolutional Layer | 
| https://medium.com/@nerdjock/deep-learning-course-lesson-5-forward-and-backward-propagation-ec8e4e6a8b92 |