# JobOffersFetcher

Ce projet permet de récupérer des offres d'emploi depuis l'API France Travail, de les stocker dans une base de données SQLite, puis de générer un rapport synthétique des offres en ligne de commande.  
- https://francetravail.io/data/api/offres-emploi/documentation#/api-reference/operations/recupererListeOffre

---

## Fonctionnalités

- Récupération asynchrone des offres d'emploi pour plusieurs villes (Paris, Rennes, Bordeaux).
- Sauvegarde des offres dans une base SQLite locale.
- Génération d'un rapport affichant les statistiques des offres par type de contrat, entreprise et pays en ligne de commande.
- Configuration sécurisée du token API via `appsettings.json`.

---

## Prérequis

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installé

---

## Installation et lancement

- Ajouter le Bearer Token dans le fichier appsettings.json
- ./run.sh
- ./run.sh test

## Temps passé sur le projet

- 4h15

---

## TO DO 

### Architecture & Structure

- Créer des interfaces comme IJobFetcher, IReportGenerator, injectées pour favoriser les tests et l’extensibilité.

### Qualité & Maintenance

- Plus de tests de manière générale
- Plus de vérification des données d'API
- Logs plutôt que du Console.WriteLine

### Fonctionnalités techniques

- Pagination automatique -> range=0-9
- Params dynamique (liste des villes, Bearer Token, ...) / Configuration
- Export du rapport (.CSV, .HTML, ...)
- Rajout de data

### Experience Utilisateur

- Interface 
- Logging Level (Silencieux, verbeux)

### Déploiement et automatisation

- Docker
- Pipeline CI/CD

### Robustesse

- Retry avec un ptit delai

### Sécurité

- Azure Key Vault
- AWS Secrets Manager