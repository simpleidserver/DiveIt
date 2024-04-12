# Implement Convolutional Neural Network from scratch

## Convolutional Layer

Réseau neuronal constitué de plusieurs couches convolutives, elle se base sur l'opération mathématique de `convolution`.

Une couche de `convolution` possède un ensemble de filtres, dont chaque filtre est représenté sous la forme d'une matrice en deux dimensions.

## Algorithme de convolution

L'algorithme de convolution est constitué des étapes suivantes :
* Lire et extraire les couleurs RGB de l'image.
* Transformer les trois matrices, en une matrice de nuance de gris de taille (iw,ih).
* Créer une matrice `kernel`, appelée aussi la fenêtre de `convolution` de taille (3,3) / (kw,kh).
* Créer une matrice de sortie de taille (iw - kw + 1, ih - kh + 1).
* Récupérer toutes les fenêtres de l'image.
* Pour chaque fenêtre de l'image, appliquer le filtre et stocker le résultat dans la matrice de sortie.

### Paramètre de padding

En appliquant plusieurs couches de `convolution` successives sur une image, il y a des risques de perdre des pixels.

Une solution pour résoudre ce problème, consiste à ajouter des pixels autour de l'image, augmentant ainsi la taille de l'image en sortie.

Ainsi, en ajoutant un padding de valeur (pw, ph), la matrice de sortie aura cette taille :

```
(iw - kw + pw + 1, ih - kh + ph + 1)
```

Dans beaucoup de cas, le padding aura cette taille (kw - 1, kh - 1), la taille de l'image en sortie aura ainsi la même taille que celle de l'image en entrée.

Différents filtres peuvent être exécutés (https://en.wikipedia.org/wiki/Kernel_(image_processing)) :
* Identity
* Ridge or edge detection
* etc...

## Paramètre stride

TODO

La plus part des librairies CNN, utilise l'algorithme de  `cross-correlation` plutôt que de `covolution`.

**TODO : comprendre les algorithmes de covolution !!**

## Resources

https://victorzhou.com/blog/intro-to-cnns-part-1/, CNNs, Part 1: An Introduction to Convolutional Neural Networks

https://observablehq.com/@lemonnish/cross-correlation-of-2-matrices, Cross-correlation of 2 matrices

https://www.ibm.com/topics/convolutional-neural-networks, What are convolutional neural networks?

https://developer.nvidia.com/discover/convolution#:~:text=The%20convolution%20algorithm%20is%20often,convolution%20operation%20is%20called%20deconvolution., Convolution

https://cezannec.github.io/Convolutional_Neural_Networks/, Convolutional Neural Networks

https://web.pdx.edu/~jduh/courses/Archive/geog481w07/Students/Ludwig_ImageConvolution.pdf

https://d2l.ai/chapter_convolutional-neural-networks/conv-layer.html#fig-correlation, Convolutions for image