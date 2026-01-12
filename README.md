# CashManagement API  
API de gestion financière développée en **ASP.NET Core 10**, conçue pour gérer les comptes, transactions, soldes journaliers et prévisions financières.

Ce projet met en œuvre une architecture propre, modulaire et maintenable basée sur :
- Repository Pattern  
- Unit of Work  
- Services métier  
- DTOs  
- Controllers RESTful  
- Entity Framework Core  
- SQL Server  

## Fonctionnalités principales

### - Gestion des comptes
- Création de comptes
- Consultation des comptes
- Suppression

### - Gestion des transactions
- Ajout de transactions (IN / OUT)
- Consultation par compte
- Suppression
- Validation automatique des données

### - Daily Balances (soldes journaliers)
- Calcul automatique du solde d’ouverture
- Calcul des flux entrants (inflows)
- Calcul des flux sortants (outflows)
- Calcul du solde de clôture
- Stockage en base pour analyse historique

### - Forecast (prévisions financières)
- Génération des soldes journaliers sur une période
- Analyse des flux financiers
- Préparation des données pour graphiques
- Projection du solde futur
- Maintien de la cohérence de l’historique

## 🏗️ Architecture

cash-management-api/
│
├── src/
│   └── CashManagement.Api/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── AccountsController.cs
│       │   ├── TransactionsController.cs
│       │   └── ForecastsController.cs
│       │
│       ├── Data/
│       │   ├── CashDbContext.cs
│       │   ├── UnitOfWork/
│       │   │   ├── IUnitOfWork.cs
│       │   │   └── UnitOfWork.cs
│       │   │
│       │   ├── Repositories/
│       │   │   ├── Interfaces/
│       │   │   │   ├── IAccountRepository.cs
│       │   │   │   ├── ITransactionRepository.cs
│       │   │   │   └── IForecastRepository.cs
│       │   │   │
│       │   │   ├── AccountRepository.cs
│       │   │   ├── TransactionRepository.cs
│       │   │   └── ForecastRepository.cs
│       │
│       ├── Services/
│       │   ├── Interfaces/
│       │   │   ├── IAccountService.cs
│       │   │   ├── ITransactionService.cs
│       │   │   └── IForecastService.cs
│       │   │
│       │   ├── AccountService.cs
│       │   ├── TransactionService.cs
│       │   └── ForecastService.cs
│       │
│       ├── Models/
│       │   ├── Entities/
│       │   │   ├── Account.cs
│       │   │   ├── Transaction.cs
│       │   │   └── DailyBalance.cs
│       │   │
│       │   └── DTOs/
│       │       ├── AccountDto.cs
│       │       ├── TransactionDto.cs
│       │       └── CreateTransactionDto.cs
│       ├── Program.cs
│       └── appsettings.json

## Logique Forecast
### - Le module Forecast calcule :

 - OpeningBalance = solde de la veille
 - Inflows = somme des transactions IN du jour
 - Outflows = somme des transactions OUT du jour
 - ClosingBalance = OpeningBalance + Inflows − Outflows

### - Il garantit :

 - Cohérence historique
 - Données prêtes pour graphiques
 - Projection future

## Tests via Swagger
Swagger est activé par défaut.
Accède à : https://localhost:7150/swagger

