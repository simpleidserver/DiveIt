---
slug: openid-federation
title: OPENID federation
authors: [geekdiver]
tags: [OPENID, FEDERATION]
enableComments: true
---

import Banner from './images/banner.jpg'
import GiscusComponent from '@site/src/components/GiscusComponent';

<div style={{textAlign: 'center'}}>
    <img src={Banner} />
</div>

## Introduction

The OpenID Federation specification was officially published by the OpenID Connect Working Group on May 31, 2024.

The objective of this new specification is to establish a trust relationship between the OpenID server and the relying parties. 
Therefore, the manual provisioning of the relying parties via a web portal or a dedicated REST API will no longer be needed.

There are several advantages to using OpenID Federation:

* Less human administration.
* Relying parties can manage their properties, such as the redirect_uri.
* Easily establish a trust relationship between the OpenID server and the relying party.

The OpenID Federation is already required by other technologies, such as the issuance of [Verifiable Credentials](https://openid.github.io/OpenID4VP/openid-4-verifiable-presentations-wg-draft.html#section-11.2).
In this context, the OpenID Federation is used to establish a trust relationship between an electronic wallet/verifier and a credential issuer.
Suppose both applications adhere to a known trust scheme, for example, `BSc chemistry degree`.
In that case, the electronic wallet will be able to call its federation API to determine if the credential issuer is indeed a member of the federation/trust scheme that it claims to be.

For more information about the interactions between the electronic wallet and the credential issuer, you can refer to the [official documentation](https://openid.github.io/OpenID4VP/openid-4-verifiable-presentations-wg-draft.html).

This specification takes all its concepts from the Public Key Infrastructure (PKI), but there are some differences between the two:

* The Public Key Infrastructure uses certificates, and the Certificate Authority is installed in the Trusted Root Certificate Authorities certificate store. It contains the root certificates of all CAs that Windows trusts.

* The OpenID Federation uses Entity Statements. Each entity involved in the trust chain has a REST API that exposes some operations described in the [specification](https://openid.net/specs/openid-federation-1_0.html).

Before proceeding further, we will explain the Public Key Infrastructure.

## Chain of trust in Public Key Infrastructure (PKI)

The purpose of a PKI is to facilitate the secure electronic transfer of information for a range of network activities such as e-commerce, internet banking, and confidential email.

PKI uses cryptographic public keys that are connected to a digital certificate, which authenticates the device or user sending the digital communication. Digital certificates are issued by a trusted source, a certificate authority (CA), and act as a type of digital passport to ensure that the sender is who they say they are.

The client who receives a digital certificate, for example, a browser visiting a secure website, validates if the issuer of this certificate exists in its list of trusted root certificates. If there is no match, the client tries to resolve the chain of trust by finding the trusted root certificate authority that has signed the issuing CA certificate.

The chain of trust is an important concept because it proves that the certificate comes from a trusted source. The usage of a certificate store is sufficient to resolve a chain of trust.

There are three basic types of entities that comprise a valid chain of trust:

* **Root CA certificate** : The Root CA certificate is a self-signed X.509 certificate. This certificate acts as a trust anchor, used by all the relying parties as the starting point for path validation. The Root CA private key is used to sign the Intermediate CA certificates.

* **Intermediate CA certificate** : The Intermediate CA certificate sits between the Root CA certificate and the end entity certificate. The Intermediate CA certificate signs the end entity certificates.

* **End-Entity certificate** : The End-Entity certificate is the server certificate that is issued to the website domain.

![Public Key Infrastructure](./images/pki.png)

This chain of trust is also present in the OpenID Federation specification.

## Chain of trust in OPENID federation

The chain of trust in the OpenID Federation is made up of more than two Entity Statements.

An Entity Statement is a signed JSON Web Token (JWT). The subject of the JWT is the entity itself. The issuer of the JWT is the party that issued the Entity Statement. All entities in a federation publish an Entity Statement about themselves called an Entity Configuration.

Entities whose statements build a trust chain are categorized as:

* **Trust anchor** : An entity that represents a trusted third party.

* **Leaf** : In an OpenID Connect identity federation, a relying party or a protected resource.

* **Intermediate** : Neither a leaf entity nor a trust anchor.

![Openid federation trust chain](./images/openidfederation.png)

The resolution of the trust chain is more complex than that present in the Public Key Infrastructure.

Consider the following entities:

* **Relying party** : http://localhost:7001

* **Trust anchor** : http://localhost:7000

The algorithm used to fetch the trust chain consists of the following actions:

1. Retrieve the entity configuration from the endpoint `http://localhost:7001/.well-known/openid-federation`.

2. Store the JSON Web Token into the trust chain.

3. Parse the JSON Web Token and retrieve the list of `authority_hints` from the payload.

4. For each record in the `authority_hints`, execute the following actions :

   4.1. Retrieve the entity configuration from the `authority_hint` (`<authority_hint>/.well-known/openid-federation`).

   4.2. Parse the JSON Web Token and extract the `federation_fetch_endpoint`.

   4.3. Fetch the entity configuration of the relying party `http://localhost:7001` (`<authority_hint>/<federation_fetch_endpoint>?sub=http://localhost:7000`) and store the result into the trust chain.

5. The last entity configuration coming from the `/.well-known/openid-federation` is the trust anchor and must be stored into the trust chain.

In the end, the trust chain must contain three records.

## Difference between PKI and OPENID federation

The structure of the trust chain between both technologies is similar and consists of the same components. 
The difference lies in the terminology of the entities used and their nature. 
In PKI, an entity is a certificate; however, in OpenID Federation, an entity is represented as an Entity Statement.

| PKI                         | Openid federation |
| --------------------------- | ----------------- |
| Root CA certificate         | Trust anchor      |
| Intermediate CA certificate | Intermediate      |
| End-entity certificate      | Leaf              |

The trust chain algorithm proposed by OpenID Federation is more complex than the one used by PKI. 
In OpenID Federation, a set of HTTP requests is executed to retrieve a list of Entity Statements, whereas in PKI, only the certificate store is used.

Now that you understand the differences between PKI and OpenID Federation, we will explain how a relying party can register itself against an OpenID Identity Server.

## Client registration

There are two approaches to establish trust between a relying party and an identity provider: automatic registration and explicit registration.

### Automatic registration

Automatic registration enables a relying party to make authentication requests without a prior registration step with the identity provider. Once the authorization request is received by the identity provider, it will use the client identifier to resolve the chain of trust and check its validity. For more information about this type of registration, refer to the [documentation](https://openid.net/specs/openid-federation-1_0.html#section-12.1).
If the chain of trust is valid, then the client will be registered.

### Explicit registration

The relying party establishes its client registration with the identity provider by means of a dedicated registration request, similar to the [Openid Registration](https://openid.net/specs/openid-connect-registration-1_0.html). However, instead of its metadata, the relying party submits its Entity Configuration or an entire trust chain. When the explicit registration is completed, the relying party can proceed to make regular OpenID authentication requests to the identity provider.

The expiration time of the client corresponds to the minimum of the expiration time of the trust chain. 
When the client expires, the identity provider tries to refresh the trust chain and update the metadata of the client accordingly.

In the next chapter, we will describe how to implement the OpenID Federation with .NET Core.

## Demo

If you want to run a demo of OPENID federation on your local machine, I invite you to read this [guide](https://simpleidserver.com/docs/tutorial/openidfederation#demo) from SimpleIdServer. You'll create a trust chain between a relying party and an identity server.

## Resources

* https://simpleidserver.com/docs/tutorial/openidfederation#demo, Openid federation

* https://openid.net/specs/openid-federation-1_0.html, OPENID federation 1.0 - draft 36

<GiscusComponent />