---
slug: stochastic-gradient-from-scratch
title: Part 1 - Stochastic Gradient Descent (SGD) from scratch
authors: [geekdiver]
tags: [ML, Staticstics, .NET]
---

import Banner from './images/banner.jpg'
import GiscusComponent from '@site/src/components/GiscusComponent';

<div style={{textAlign: 'center'}}>
    <img src={Banner} />
</div>

## Introduction

In this article, we will implement the `Stochastic Gradient Descent Regression` algorithm from scratch in C#. 
This algorithm is well-known and widely used in the field of Machine Learning. It will be utilized for performing Linear Regression on both simple and complex forms.

If you are not familiar with the concept of a Regression function, please read the following chapter. 
Otherwise, feel free to skip directly to the [Gradient Descent](#gradient-descent) chapter.

## Regression Versus Classification problems

Variables of a function can be characterized as either `quantitative` or `qualitative` (also known as categorical).
Quantitative variables take on numerical values. Examples include a person's age, height, or income.
In contrast, qualitative variables take on values in one of K different `classes`, or `categories`.
Examples of qualitative variables include a person's gender (male or female), the brand of product purchased (branch A, B or C).

We tend to refer to problems with a quantitative response as `regression` problem, while those involving a qualitative response are often referred to as `classification` problems.

`Least squares linear regression` is used with a quantitative response, whereas `Logistic regression` is used with a qualitative (two-class, or binary) response.

## Gradient Descent

A `Gradient Descent` function is an iterative algorithm designed to find the minimum of a loss function. This algorithm can be applied to any `differentiable` function.

Here are the steps of the algorithm: 

```
1. Initialize the vector `w` for all parameters. These parameters are computed by the algorithm.
2. Define the number of iterations `epoch`.
3. Define the learning rate `η`. It determines the step size at each iteration while moving toward a minimum of a loss function.
4. for i = 1 .... epoch do

   4.1 $w = w - η ∇ Qi(w)$
```

| Parameter | Desription |
| --------- | ---------- |
| $Qi(w)$   | Loss function |
| $∇$       | Gradient of the loss function |

We have chosen to apply this algorithm to both `Simple` and `Complex` forms of `Linear expressions`.

### Gradient Descent on Simple Linear Expression

A simple linear function has this form.

$$
y = itp + w*xi
$$

The best loss function to choose for evaluating the performance of the Gradient model is the `Mean Squared Error (MSE)`.

$$
MSE = \frac{1}{N} \sum_{i}^{n} (yi - f(xi))^{2}
$$

$$
MSE = \frac{1}{N} \sum_{i}^{n} (yi - (itp + w*xi))^{2}
$$

`f(xi)` is the prediction that the function `f` provides for the ith observation.

To calculate the gradient of the $MSE$ / $Qi(itp,w)$ function, it is necessary to determine the partial derivatives with respect to the parameters $\frac{\delta}{\delta itp}$ and $\frac{\delta}{\delta w}$ . The chain rule is used to compute them.

For the function `f(g(x))`, its derivative is `f'(g(x)) * g'(x)`.

Here are the details of the partial derivatives calculations:

$$
\frac{\delta}{\delta itp} = \frac{1}{N}(\sum_{i}^{n} (yi - (itp + w*xi))^{2})* (yi - (itp + w*xi))
$$

$$
\frac{\delta}{\delta itp} = \frac{1}{N}(\sum_{i}^{n} 2*(yi - (itp + w*xi)))* -itp
$$

$$
\frac{\delta}{\delta itp} = \frac{1}{N}(\sum_{i}^{n} -2*(yi - (itp + w*xi)))
$$

$$
\frac{\delta}{\delta w} = \frac{1}{N}(\sum_{i}^{n} (yi - (itp + w*xi))^{2})* (yi - (itp + w*xi))
$$

$$
\frac{\delta}{\delta w} = \frac{1}{N}(\sum_{i}^{n} 2*(yi - (itp + w*xi)))*- w*xi
$$

$$
\frac{\delta}{\delta w} = \frac{1}{N}(\sum_{i}^{n} -2*xi*(yi - (itp + w*xi)))
$$

Here is the updated gradient algorithm to minimize the loss function of a simple linear function.

1. Initialize the vector `x` with values.
2. Initialize the vector `y` with values.
3. Initialize the variable `w` for `weight`.
4. Initialize the variable `ipt` for `intercept`.
5. Assign random values to these two variables.
6. Define the number of iterations `epoch`.
7. Define the learning rate `η`.
8. for i = 1 .... epoch do

   8.1 $w = w - \eta * (\frac{1}{N} \sum_{i}^{n} -2 * xi * (yi - (itp + w * xi)))$
   
   8.2 $itp = itp - \eta * (\frac{1}{N} \sum_{i}^{n} -2 * (yi - (itp + w * xi)))$

A simple-shaped function may not be sufficient for you, as you have more than 2 parameters to calculate. 

In this situation, you need to use the Gradient Descent algorithm to calculate all the parameters of your complex linear function.

### Gradient Decent on complex linear expression

A complex linear function has this form:

$$
y = itp + w*xi1 + w*xi2 + w*xi3 + ... + w*xi4
$$

Here, the values of the variables `xi` can be represented in the form of a matrix of size (O, V), where `O` represents the number of observations, and `V` represents the number of variables. The variable `w` can be represented as a vector of size `V`.

The algorithm is very similar to that applied for a simple linear function but contains a few differences.

1. Initialize the vector `x` with values.
2. Initialize the vector `y` with values.
3. Initialize the variable `w` for `weight`.
4. Initialize the variable `ipt` for `intercept`.
5. Assign random values to these two variables.
6. Define the number of iterations `epoch`.
7. Define the learning rate `η`.
8. for i = 1 .... epoch do

   8.1 $w = w - \eta * ( \frac{1}{N} * \sum_{i}^{n} -2 * wT * xi * (yi - (itp + w * xi)))$

   8.2 $itp = itp - \eta * ( \frac{1}{N} * \sum_{i}^{n} 2 * itp * (yi - (itp + w*xi)))$

At step 8.1, the matrix w must be transposed. For more information on this operation, refer to this site [https://en.wikipedia.org/wiki/Transpose](https://en.wikipedia.org/wiki/Transpose).

At first glance, this algorithm works well on small-sized matrices but is not suitable for large matrices due to a risk of memory loss. 
To address this issue, the Stochastic Gradient Descent algorithm has been introduced.

## Stochastic Gradient Descent

The idea of the algorithm is to take a random dataset from the variables `x` and `y` and apply the Gradient Descent algorithm to it. 
This algorithm diverges slightly from the previous one:

1. Initialize the vector `x` with values.
2. Initialize the vector `y` with values.
3. Initialize the variable `w` for `weight`.
4. Initialize the variable `ipt` for `intercept`.
5. Assign random values to these two variables.
6. Define the number of iterations `epoch`.
7. Define the learning rate `η`.
8. for i = 1 .... epoch do

   8.1 Randomly shuffle samples in the training set and store the result in the `tx` and `ty` variables.

   8.1 $w = w - \eta * ( \frac{1}{N} * \sum_{i}^{n} -2 * wT * txi * (tyi - (itp + w * txi)))$

   8.2 $itp = itp - \eta * ( \frac{1}{N} * \sum_{i}^{n} 2 * itp * (tyi - (itp + w*txi)))$

Now that you have an overview of the Gradient Descent algorithm, we will develop a naive implementation in C# of the Stochastic Gradient Descent algorithm.

## C# implementation

To be able to write the Stochastic Gradient Descent algorithm, we use two classes, `CPUMathUtils` and `MathUtils`. 
These classes facilitate matrix and vector operations, and you can find their source code on GitHub : [https://github.com/simpleidserver/DiveIt/Projects/SGD](https://github.com/simpleidserver/DiveIt/Projects/SGD).

Create a class `GradientDescentRegression` with a static function `Compute`.

| Parameter | Description |
| --------- | ----------- |
| epoch     | Number of iterations |
| x         | matrix where each cell corresponds to the value of the parameter |
| y         | vector where each value corresponds to the value of an observation |
| isStochastic | Enable or disable the stochastic algorithm. If true, then the dataset used to execute the GD algorithm will be randomly selected. |
| batchSize    | Number of observations that the algorithm will randomly select on each iteration. This parameter is used only if `isStochastic` is true. |

```
public static void Compute(
    double learningRate,
    int epoch,
    double[,] x,
    double[] y,
    bool isStochastic = false,
    int batchSize = 0)
```

Initialize the variables, such as `intercept` and an empty vector for `w`.

```
int nbSamples = x.GetLength(0);
var nbWeight = x.GetLength(1);
var w = MathUtils.InitializeVector(nbWeight);
double itp = 0;
var lst = new List<double>();
```

Create a loop where the number of iterations is equal to `epoch`.

```
for (var step = 0; step < epoch; step++)
{

}
```

In this loop, add the logic to randomly draw a dataset of size `batchSize` if `isStochastic` is true.

```
var tmpX = x;
var tmpY = y;
if(isStochastic)
{
    var shuffle = MathUtils.Shuffle(x, y, nbSamples, nbWeight, batchSize);
    tmpX = shuffle.a;
    tmpY = shuffle.b;
}
```

Calculate the new value of `y` (itp + w*txi)

```
var newY = CPUMathUtils.Add(
  CPUMathUtils.Multiply(tmpX, w),
  itp);          
```

Calculate the lose factor (losefactor = tyi - (itp + w*txi))

```
var loseFactor = CPUMathUtils.Substract(tmpY, newY);
```

Update the `w` vector (w = w - η * (1/N Σ(i/n) -2 * wT*txi * (loseFactor)))

```
var tmp = CPUMathUtils.Multiply(
    CPUMathUtils.Divide(
        CPUMathUtils.Multiply(
            CPUMathUtils.Multiply(
                CPUMathUtils.Transpose(tmpX),
                loseFactor
            ),
            -2
        ),
        nbSamples
    ),
    learningRate);
// 4. Update weight
w = CPUMathUtils.Substract(w, tmp);
```

Update the `intercept` (itp = itp - η * (1/N Σ(i/n) 2 * itp * (loseFactor)))

```
itp -= learningRate * (CPUMathUtils.Multiply(loseFactor, -2).Sum() / nbSamples);
```

## Conclusion

**Congratulations! 🥳**

If you have reached the end of the article, it may mean that you have successfully implemented the Stochastic Gradient Descent algorithm for a complex linear function. 
This algorithm is not intended for use in a library as it is not optimized and cannot work on non-convex functions. 
Therefore, I suggest using a library such as [sklearn](https://scikit-learn.org/stable/) for linear regression.

In future articles, we will explain how to improve the performance of the algorithm.

## Source code

You will find the source code at this link: [https://github.com/simpleidserver/DiveIt/Projects/SGD](https://github.com/simpleidserver/DiveIt/Projects/SGD).

## Resources

| Link |
| ---- |
| https://en.wikipedia.org/wiki/Stochastic_gradient_descent, Stochastic gradient descent |
| https://sebastianraschka.com/faq/docs/gradient-optimization.html, What are gradient descent and stochastic gradient descent? |
| https://towardsdatascience.com/stochastic-gradient-descent-explained-in-real-life-predicting-your-pizzas-cooking-time-b7639d5e6a32, Stochastic Gradient Descent explained in real life |
| https://optimization.cbe.cornell.edu/index.php?title=Stochastic_gradient_descent, Stochastic gradient descent |
| https://docs.nvidia.com/deeplearning/performance/dl-performance-matrix-multiplication/index.html, Matrix Multiplication Background User's Guide |

<GiscusComponent />