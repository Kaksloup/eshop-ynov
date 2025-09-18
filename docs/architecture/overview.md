# 🏗️ Vue d'Ensemble Architecture - eShop

## 📖 Introduction

La solution eShop implémente une architecture microservices moderne avec .NET 9, suivant les principes Domain-Driven Design (DDD), CQRS, et Event-Driven Architecture.

## 🎯 Objectifs Architecturaux

### 🎪 Objectifs Métiers
- **Scalabilité** : Support de milliers d'utilisateurs simultanés
- **Disponibilité** : 99.9% d'uptime
- **Performance** : Temps de réponse < 200ms
- **Flexibilité** : Évolution rapide des fonctionnalités

### 🔧 Objectifs Techniques
- **Maintenabilité** : Code modulaire et testable
- **Observabilité** : Logging, métriques, tracing complets
- **Sécurité** : Protection des données et API
- **Déployabilité** : CI/CD automatisé

## 🏛️ Architecture Globale

```mermaid
C4Context
    title Architecture Contextuelle - eShop

    Person(customer, "Customer", "Utilisateur final de l'application e-commerce")
    Person(admin, "Admin", "Administrateur du système")

    System_Boundary(eshop, "eShop System") {
        System(webapp, "Web Application", "Interface utilisateur React/Blazor")
        System(gateway, "API Gateway", "Point d'entrée unique - YARP")
        
        System_Boundary(services, "Microservices") {
            System(catalog, "Catalog Service", "Gestion du catalogue produits")
            System(basket, "Basket Service", "Gestion des paniers")
            System(ordering, "Ordering Service", "Traitement des commandes")
            System(discount, "Discount Service", "Calcul des remises")
        }
    }

    System_Ext(payment, "Payment System", "Système de paiement externe")
    System_Ext(shipping, "Shipping System", "Système de livraison")
    System_Ext(inventory, "Inventory System", "Système de stock externe")

    Rel(customer, webapp, "Utilise")
    Rel(admin, webapp, "Administre")
    Rel(webapp, gateway, "API calls", "HTTPS")
    Rel(gateway, catalog, "Routes")
    Rel(gateway, basket, "Routes")
    Rel(gateway, ordering, "Routes")
    Rel(basket, discount, "gRPC")
    Rel(ordering, payment, "API calls")
    Rel(ordering, shipping, "API calls")
    Rel(catalog, inventory, "Sync")
```