# Part 2 - Verifiable credentials - Cross device flow

Cet article est la suite de [Generate verifiable credentials from scratch].
Nous y avions expliqué comment développer une application REST.API capable de produire des `verifiable credentials`. 
Nous supposons que vous avez à votre disposition une API REST capable de produire des `verifiable credentials`,  ainsi que d'un portefeuille électronique avec au moins un `verifiable credential`.

Nous allons expliquer, comment utiliser le portefeuille électronique pour s'authentifier auprès d'un site développé en ASP.NET CORE.

Le groupe de travail [OpenID for verifiable Credentials](https://openid.net/sg/openid4vc/), a déjà répondu à cette problématique, et propose une nouvelle spécification [OpenID for verifiable Presentations](OID4VP), que nous allons parcourir.

## Verifiable Presentation

Selon le site [W3C](https://www.w3.org/TR/vc-data-model/#terminology), le terme `Presentation` représente un groupe de `Verifiable credentials`, qui peut être partagé par le portefeuille électronique vers n'importe quel site.

La sécurité d'une `Presentation` est très importante, car le site / `verifier` qui réceptionne les `presentations`, doit être capable de vérifier qu'il n'y a pas eu de changements sur les données, et que les `presentations` viennent du bon utilisateur. 

Par conséquent, avant d'envoyer la présentation, le portefeuille électronique doit prouver qu'il est en possession de la clef privée.
Voici dans les grandes lignes, l'algorithme pour construire une `verifiable presentation`.

[DIAGRAM]

1. Le portefeuille construit une présentation avec un ou plusieurs `verifiable credentials`, qui ont été obtenus de différents `issuers` et qui peuvent avoir des formats différents.
2. Utilise la clef privée de son `distributed identity document` pour signer la `presentation`.
3. Ajoute la preuve de possession de la clef privée dans la `presentation`, nous obtenons alors une `verifiable presentations` qui peut être transmise au site.

Les algorithmes utilisés pour créer la preuve de possession, sont les mêmes que ceux utilisés pour protéger les `credentials`, voici quelques exemples :

| Algorithm            | Url                                 |
| -------------------- | ----------------------------------- |
| JsonWebSignature2020 |  https://www.w3.org/TR/vc-jws-2020/ |
| Ed25519Signature2020 | https://w3c.github.io/vc-di-eddsa/  |

Nous allons maintenant expliquer comment un site vérifie le `verifiable presentation` et parcourir la spécification [OpenID for verifiable presentations].

## OPENID - Verifiable Presentation

Etant donné que le `verifier` et le `portefeuille électronique` ne sont pas hébergés sur le même appareil, le scénario approprié est [Cross Device Flow](https://openid.net/specs/openid-4-verifiable-presentations-1_0.html#name-cross-device-flow).

Le processus d'authentification est simple et n'est pas d'une grande complexité. Il est constitué des étapes suivantes :

[WORKFLOW]

1. L'utilisateur choisit avec quel credential il va s'authentifier, par exemple avec son permis de conduire.

2. Le `verifier` construit une requête d'autorisation, où il indique les `credentials` avec leur méthode d'encryption, qui sont requis pour authentifier l'utilisateur.

3. La requête est présentée sous la forme d'un QR Code.

4. L'utilisateur ouvre son application mobile et scan le QR code qui est présenté. 

   4.1. L'application mobile construit une `presentation` contenant les `credentials` demandés.
   
   4.2. La `presentation` est signée avec la clef privée du document d'identié distribuée de l'utilisateur.
   
   4.3 L'application mobile exécute une requête HTTP POST sur le `verifier` en passant le `presentation verifiable`.

Il existe trois façons de retourner le `presentation verifiable`. 
Nous avons choisi la façon directe `vp_token`. Elle consiste à exécuter une requête HTTP POST vers le verifier en passant le `presentation verifiable` dans le paramètre `vp_token`.

Les autres façons, telles que `vp_token id_token` et `code`, requièrent que l'application mobile puisse agir comme un [serveur d'identité indépendant](https://openid.github.io/SIOPv2/openid-connect-self-issued-v2-wg-draft.html). L'explication du fonctionnement de ce type d'application peut être complexe, et fera l'object d'un autre article.

Maintenant que vous avez une vue globale du processus, nous allons sécuriser un site ASP.NET CORE avec des verifiable presentations

https://openid.net/specs/openid-4-verifiable-presentations-1_0.html#section-6

## Implémentation

Nous allons ici mettre en place un serveur d'identité, OPENID, capable de ... ?

# Resources

https://openid.net/specs/openid-4-verifiable-presentations-1_0.html

https://www.w3.org/TR/vc-data-model/#dfn-verifiable-presentations, Verifiable presentations W3C

https://www.w3.org/TR/vc-data-model/#presentations-0