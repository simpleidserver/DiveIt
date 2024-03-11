# Part 1 - Generate verifiable credentials from scratch

Depuis quelques temps, la commission européenne travaille sur un projet de portefeuille électronique qui sera disponible à tous les citoyens européens, ce projet se nomme [ebsi](https://ec.europa.eu/digital-building-blocks/sites/display/EBSI/EBSI+Verifiable+Credentials).
L'idée, est d'offrir aux institutions publiques la possibilité de distribuer des certificats aux citoyens qui seront stockés uniquement sur leurs appareils mobiles comme : Certificat COVID, la carte d'identité, le permis de conduire, les prescriptions médicales etc...

Ce projet ambitieux a des avantages :

* Seul le citoyen européen possédera ses données.
* Supprimer la centralisation des données.
* Plus sécurisée, car la technologie de blockchain est utilisée pour stocker la clef publique de chaque citoyen.

Dans cette article, nous allons expliquer comment vous pouvez créer une REST.API, capable de produire ces certificats.

TODO