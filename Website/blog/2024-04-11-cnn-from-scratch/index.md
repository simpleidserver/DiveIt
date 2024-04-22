# Implement Convolutional Neural Network from scratch

import Architecture from './images/architecture.jpg'
import BackPropagation from './images/backpropagation.jpg'
import CnnArchitecture from './imagges/cnn-architecture.jpg'
import NeuronConvolutional from './images/neuron-conv.png'



Dans cet article, nous allons couvrir divers sujets théoriques, dans l'objectif d'implémenter un réseau neuronal en C#, capable de reconnaître un chiffre présente sur une image.

L'article se découpe en deux parties :
1. Présenter les grands principes d'un réseau neuronal.
2. Expliquer l'architecture d'un réseau de type Convolutional.
3. Code source c#.

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

Le réseau neuronal adapté à la reconnaissance d'image est de type Convolutional.

L'architecture typique d'un réseau CNN est constituée de quatre couches.

* Input layer : Extraires les données d'une image sous l'un des deux formats :

** Trois matrices RGB de deux dimensions.
** Une matrice avec les nuances de gris de deux dimensions.

* First hidden layer : Convolutional layer.
* Second hidden layer : Max pooling layer.
* Output layer : softmax layer.

<div style={{textAlign: 'center'}}>
    <img src={CnnArchitecture} height="300" />
</div>

### 1. Convolutional Layer

Avant de décrire la structure de cette couche, il est important de comprendre le concept de matrice de convolution.

Une matrice de convolution, appelée aussi kernel, est une matrice de deux dimensions. 
Appliquée sur une image, elle permet d'obtenir divers effets, dont voici un exemple ci-dessous.
Pour une liste complète des filtres, référez vous au site [wikipedia](https://en.wikipedia.org/wiki/Kernel_(image_processing)).


| Opération              | Kernel             | Result                                       |
| ---------------------- | ------------------ | -------------------------------------------- |
| Détection des contours | [ -1 -1 -1 ]       | ![Edge detection](images/edge-detection.png) |
|                        | [ -1  8 -1 ]       |                                              |
|                        | [ -1 -1 -1 ]       |                                              |

En supposant que l'image a été extraite en une matrice de nuance des gris de taille $$(imw * imh)$$.
L'algorithme de convolution est constituée des étapes suivantes :

1. Créer une matrice de convolution / kernel de taille $$(kw * kh)$$. Par défaut, la matrice aura une taille de 3x3.
2. Créer une matrice de sortie de taille $$(imw - kw + 1) * (imh - kh + 1)$$.
3. Récupérer toutes les fenêtres de l'image en entrée. Le centre de la matrice `kernel` est utilisé comme un pointeur, l'algorithme bouge le pointeur sur chaque colonne et chaque ligne de l'image. La zone concernée par le kernel, appelée aussi fenêtre, est alors stockée dans une liste windows.
4. Pour chaque élément de la liste windows. 
4.1. Multiplier la valeur par le kernel.
4.2. Calculer la moyenne et stocker le résultat dans la matrice de sortie. 

Pour résumer, voici l'équation mathématique pour calculer chaque élément de la matrice de sortie.

$$
V=  \frac{\sum_{m}^{imgsize}\sum_{k}^{ksize}kernel(kx;ky) * img(mx,my)}{F}
$$

* $$kernel(kx;ky)$$ : the coefficient of the convolution kernel at position kx,ky.
* $$img(mx,my)$$ : the data of the pixel that corresponds to img(mx,my).
* F : Sum of the coefficients of the kernel.

En appliquant cet algorithme plusieurs fois, il existe un risque de perdre des pixels sur les bords de l'image.
Pour répondre à cette problématique, le paramètre `padding` a été introduit.

#### Padding parameter

Ce paramètre indique le nombre de pixels devant être ajoutés sur les bords de l'image.

En supposant que le paramètre de padding est défini par la variable `p`, la taille de la matrice de sortie sera calculée comme suit :

$$
(imw - kw + pw + 1) * (imh - kh + ph + 1)
$$

Dans beaucoup de cas, la valeur du padding est égale à $(kw-1,kh-1)$, afin que l'image en sortie ait la même taille que celle de l'image en entrée.

#### Stride parameter

Pour chaque pixel de l'image, une fenêtre de convolution est appliquée.
Cette opération peut être coûteuse en ressource, surtout lorsque l'algorithme est appliqué sur une image de grande taille.

Pour diminuer le nombre d'itérations, le paramètre `stride` a été introduit. 
Il définit le nombre d'éléments devant être ignorés sur la largeur et la hauteur de l'image. 
Par défault ce paramètre est égale à `(1,1)`.

En supposant que le paramètre stride est défini par la variable `s`, la taille de la matrice de sortie sera calculée comme suit :

$$
((imw - kw + pw + sw) / sw) * ((imh - kh + ph + sh) / sh)
$$

#### Forward propagation

Une couche de convolution est constituée de 1 ou plusieurs neurones, et chaque neurone possède une fenêtre de convolution distincte.

Durant la phase de `forward propagation`, chaque neurone exécute l'algorithme de convolution sur une fenêtre de l'image.

Voici dans les grandes lignes, les étapes de l'algorithme de forward propagation.

1. Récupérer toutes les fenêtres de l'image en entrée.
2. Pour chaque fenêtre d'entrée, exécuter l'algorithme de convolution de chaque neurone et stocker le résultat dans une liste.
3. Stocker la liste dans la matrice de sortie.

Voici l'architecture d'un neurone d'une couche de convolution.

<div style={{textAlign: 'center'}}>
    <img src={NeuronConvolutional} height="300" />
</div>

#### Backward propagation

Sachant que dans l'article, la couche de convolution ne possède pas de paramètre biais et de fonction d'activation.
L'algorithme de `backward propagation` revient à calculer cette formule :

$$
\frac{\delta L}{\delta filter(x,y))} = \sum_{i}^{}\sum_{j}^{}\frac{\delta L}{\delta conv(i,j)}\frac{\delta conv(i,j)}{\delta filter(x,y)}
$$

### 2. Max Pooling layer

Cette couche est constituée d'aucun neurone et ne possède pas de paramètres `bias` ou `weights`.
Elle réduit la taille de la matrice d'entrée, en appliquant l'opération `max` sur chaque fenêtre de la matrice d'entrée.

Son objectif est double :

1. Réduire la dimension.
2. Pour chaque région, trouver le maximum.

#### Forward propagation

L'algorithme de forward propagation est constituée des étapes suivantes :

1. Définir la taille de la fenêtre de pooling $$(pw * ph)$$.

2. Créer une matrice de sortie de taille $$(iw / pw) * (ih / ph)$$.

3. Décomposer la matrice d'entrée en plusieurs fenêtres de taille $$(pw * ph)$$.

4. Pour chaque fenêtre de la matrice, trouvez le maximum et assignez cette valeur à la cellule de la matrice de sortie.

#### Backward propagation

Etant donné que la couche de Pooling ne possède aucun paramètre d'apprentissage, weights ou bias.

L'algorithme de backward propagation se contente de reconstruire une matrice qui a la même taille que la dernière matrice reçue par la couche.
Voici les grandes étapes de l'algorithme :

1. Créer une matrice de sortie, de la même taille que la matrice en entrée.

2. Décomposer la matrice en plusieurs fenêtres de taille $$(pw * ph)$$.

3. Pour chaque cellule de chaque fenêtre, si l'élément n'est pas le maximum alors mettre 0, sinon prendre le gradient de l'erreur.

### 3. Softmax layer

La couche dense d'activation `softmax` est composé de un ou plusieurs neurones, et possède les paramètres d'apprentissages `weights` et `bias`.
Le nombre de neurones est égale au nombre de classes à prédire.

La fonction softmax est utilisée pour calculer les probabilités d'appartenance à chaque classe.

Voici l'architecture d'un neurone softmax.


#### Forward propagation

L'algorithme de forward propagation est constituée des étapes suivantes :

1. Définir le nombre de classe dans une variable `n`.
2. Pour chaque classe, créer un neurone et initialiser son paramètre `weight`.
3. Initialiser le paramètre `bias` de la couche.
4. Pour chaque neurone, multiplier la matrice d'entrée par son weight, et stocker le résultat dans une variable $Weights = \sum_{i=1}^{n}xi *weighti$.
5. Calculer la somme avec le bias et stocker le résultat dans une variable $SumWeighted = bias + Weights$.
6. Pour finir, exécuter la fonction d'activation sur chaque classe $softmax(ci) = \frac{exp(ci)}{\sum_{i}^{n}exp(i)}$.

#### Backward propagation

La couche de softmax, possède deux paramètres d'apprentissage devant être mise à jour, et une variable `learningRate` qui pondère l'importance des dérivées partielles pour mettre à jour ces paramètres.

L'algorithme doit être capable de calculer les dérivées partielles, pour ces deux paramètres.

$$
\frac{\delta Loss}{\delta weight} = \frac{\delta Loss}{\delta out} * \frac{\delta out}{\delta net} * \frac{\delta net}{\delta weight}
\frac{\delta Loss}{\delta bias} = \frac{\delta Loss}{\delta out} * \frac{\delta out}{\delta bias}
$$

La dérivée $$\frac{\delta Loss}{\delta out}$$ est assez simple à calculer.

Si la classe prédite est différente de celle attendue :

$$
\frac{\delta Loss}{\delta out(i)}=0
$$

Si la classe prédite est la même que celle attendue : 

$$
\frac{\delta Loss}{\delta out(i)}=\frac{-1}{pi}
$$

La dérivée $$\frac{\delta net}{\delta weight}$$ est égale à  :

$$
net = bias + \sum_{i=1}^{n}x(i)*weight(i)

net = input
$$

La dérivée $$\frac{\delta out}{\delta bias}$$ est égale à 1.

La dernière dérivée est $$\frac{\delta out}{\delta net}$$ est plus complexe à calculer, si vous souhaitez une démonstration complète je vous invite à lire l'article publié sur [medium](https://medium.com/@shine160700/softmax-function-and-the-maths-behind-it-12422d07c78a).

Une fois toutes ces dérivées partielles calculées, les paramètres pourront être mises à jour comme suit :

$$
weight = weight - learningRate * \frac{\delta Loss}{\delta weight}
bias = bias - learningRate * \frac{\delta Loss}{\delta bias}
$$

## Implémentation c#

Le code source du projet se trouve ici.

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