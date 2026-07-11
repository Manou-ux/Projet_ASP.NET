# GESTION DES SALLES ET DES EMPLOIS DU TEMPS DE L'EMIT

## 🚀 Présentation
Cette application ASP.NET Core MVC est conçue pour la gestion complète de l'EMIT :
- emplois du temps
- réservations de salles
- gestion des utilisateurs (scolarité, enseignants, élèves)
- disponibilité des enseignants
- suivi des clubs et membres

L'objectif principal est de proposer une solution métier cohérente pour la scolarité, les enseignants et les élèves, avec une interface adaptée à chaque rôle.

## 🧱 Stack technique
- `ASP.NET Core MVC` avec C# et Razor Views
- `Entity Framework Core 10` pour le mapping objet-relationnel
- `PostgreSQL` comme base de données
- `Npgsql.EntityFrameworkCore.PostgreSQL` pour le provider PostgreSQL
- `BCrypt.Net-Next` pour le hashage sécurisé des mots de passe
- Authentification par cookies ASP.NET Core
- `dotnet CLI` pour compilation et exécution

## 🧩 Fonctionnalités principales
### Gestion des emplois du temps
- création et consultation des emplois du temps
- affichage personnalisé selon l'utilisateur
- vue hebdomadaire pour les enseignants et élèves
- emploi du temps global accessible à la scolarité
- Gestion maintenu par la Scolarité

### Gestion des salles et des réservations
- gestion des salles disponibles
- réservation de salles par utilisateurs
- lien entre réservation, salle, utilisateur et club
- affinement des disponibilités

### Gestion des utilisateurs et des rôles
- rôle `scolarite` : administration complète
- rôle `enseignant` : emploi du temps, disponibilités, réservations
- rôle `eleve` : emploi du temps personnel, dashboard, club(s)
- possibilité de gérer les élèves, enseignants et personnel de scolarité

### Gestion des clubs et des membres
- création/édition/suppression de clubs
- ajout multiple d'utilisateurs à un club
- chaque utilisateur peut appartenir à plusieurs clubs
- gouvernance des clubs via des responsables

### Disponibilités des enseignants
- saisie des créneaux disponibles
- visualisation des disponibilités
- intégration dans l'interface d'emploi du temps

### Dashboard et interface utilisateur
- pages adaptées selon le rôle connecté
- menu dynamique dans `_Layout.cshtml`
- accès simplifié aux modules clés : emplois du temps, réservations, salles, classes, groupes, matières, clubs

## 📌 Modules visibles dans la navigation
Selon les rôles, le menu expose :
- `Tous les EDT`, `Classes`, `Salles`, `Matières`, `Groupes`, `Réservations`, `Clubs`, `Disponibilités` pour la scolarité
- `Dashboard`, `Mon emploi du temps`, `Mes disponibilités`, `Réservations` pour les enseignants
- `Dashboard`, `Mon emploi du temps`, `Clubs` pour les élèves
- gestion des comptes et accès depuis le profil utilisateur

## 📁 Architecture du projet
- `Program.cs` : configuration, services, authentification, DbContext PostgreSQL
- `Controllers/` : interactions utilisateurs et logique métier
- `Models/` : entités, relations et annotations de base de données
- `Views/` : interface Razor pour chaque module
- `Data/` : contexte EF Core et configurations spécifiques
- `Migrations/` : historique des changements de base de données
- `wwwroot/` : assets front-end (CSS, JS)

## 🔧 Installation et démarrage
À partir du dossier racine du projet :

```powershell
dotnet restore
dotnet build
dotnet run
```

L'application démarre ensuite et le site est accessible sur l'URL indiquée dans la sortie console.

## 🧩 Configuration de la base de données
La chaîne de connexion PostgreSQL est définie dans `appsettings.json` :

```json
"ConnectionStrings": {
  "MaConnexion": "Host=localhost;Database=GESTION_SALLES_ET_EMPLOIS_DU_TEMPS_EMIT;Username=postgres;Password=1234"
}
```

Pour appliquer les migrations actuelles :

```powershell
dotnet ef database update --context MonDbContext
```

## 🧠 Logique et relationnel
Le projet repose sur un modèle de données centré sur l'utilisateur et les ressources de l'établissement.

### Rôles et entités principales
- `Utilisateur` représente un compte générique : `eleve`, `enseignant`, `scolarite`
- `Eleve`, `Enseignant`, `Scolarite` contiennent les données spécifiques à chaque rôle
- `EmploiDuTemps` contient les créneaux planifiés pour des classes ou des groupes
- `Salle` et `ReservationSalle` pilotent l'occupation des locaux
- `DisponibiliteEnseignant` sert à gérer les plages horaires disponibles des enseignants
- `Club` et `MembreClub` gèrent l’appartenance des utilisateurs aux clubs

### Interaction entre les modules
- les utilisateurs se connectent et voient un menu adapté à leur rôle
- la scolarité peut créer des emplois du temps, gérer les salles et planifier les réservations
- les enseignants saisissent leurs disponibilités et consultent leur emploi du temps
- les élèves consultent leur emploi du temps et participent aux clubs
- les réservations de salle sont liées à une salle, un utilisateur et éventuellement à un club

### Diagramme relationnel algorithmique
```mermaid
erDiagram
    UTILISATEUR {
        int id_utilisateur PK
        string Email
        string MotDePasse
        string Role
        bool Actif
    }
    ELEVE {
        int id_eleve PK
        int id_utilisateur FK
        string PrenomEleve
        string NomEleve
    }
    ENSEIGNANT {
        int id_enseignant PK
        int id_utilisateur FK
        string PrenomEnseignant
        string NomEnseignant
    }
    SCOLARITE {
        int id_scolarite PK
        int id_utilisateur FK
        string PrenomScolarite
        string NomScolarite
    }
    SALLE {
        int id_salle PK
        string NomSalle
        string type
        int Capacite
    }
    CLASSE {
        int id_classe PK
        string nom_classe
        string Niveau
    }
    GROUPE {
        int id_groupe PK
        int id_classe FK
        string NomGroupe
    }
    EMPLOI_DU_TEMPS {
        int id_emploi PK
        int? id_classe FK
        int? id_groupe FK
        string semestre
        string statut
    }
    RESERVATION_SALLE {
        int id_reservation PK
        int id_utilisateur FK
        int id_salle FK
        int id_club FK
        DateTime date_reservation
    }
    DISPONIBILITE_ENSEIGNANT {
        int id_dispo PK
        int id_enseignant FK
        string jour
        string heure_debut
        string heure_fin
    }
    CLUB {
        int id_club PK
        int id_responsable FK
        string NomClub
    }
    MEMBRE_CLUB {
        int id_utilisateur FK
        int id_club FK
        string role_membre
        DateTime date_adhesion
    }

    UTILISATEUR ||--o{ ELEVE : "détient"
    UTILISATEUR ||--o{ ENSEIGNANT : "détient"
    UTILISATEUR ||--o{ SCOLARITE : "détient"
    UTILISATEUR ||--o{ RESERVATION_SALLE : "réserve"
    SALLE ||--o{ RESERVATION_SALLE : "est réservée"
    CLUB ||--o{ RESERVATION_SALLE : "peut être liée"
    UTILISATEUR ||--o{ MEMBRE_CLUB : "est membre de"
    CLUB ||--o{ MEMBRE_CLUB : "contient"
    UTILISATEUR ||--o{ CLUB : "peut être responsable de"
    CLASSE ||--o{ GROUPE : "contient"
    CLASSE ||--o{ EMPLOI_DU_TEMPS : "est planifiée pour"
    GROUPE ||--o{ EMPLOI_DU_TEMPS : "est planifiée pour"
    ENSEIGNANT ||--o{ DISPONIBILITE_ENSEIGNANT : "a"
```

## 📌 Points forts à mettre en avant
- gestion métier complète pour l’EMIT, pas seulement les clubs
- expérience utilisateur selon rôle (scolarité / enseignant / élève)
- centralisation des emplois du temps et des réservations de salles
- système multi-utilisateurs et multi-clubs
- interface Bootstrap moderne et navigation adaptative

## 🎯 À savoir pour GitHub
Ce README est conçu pour une présentation claire et complète du projet sur GitHub.
- valeur métier : gestion des plannings, des salles et des utilisateurs
- architecture : ASP.NET Core MVC + PostgreSQL
- boucle fonctionnelle : authentification, rôle, réservation, emploi du temps, disponibilité
- app prête à être déployée ou démo pour un établissement scolaire

---
