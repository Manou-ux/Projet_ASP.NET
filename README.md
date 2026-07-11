# Projet GESTION_S_E

## 🚀 Présentation
`GESTION_S_E` est une application web ASP.NET Core MVC destinée à gérer des clubs, des utilisateurs (élèves, enseignants, scolarité), des réservations de salles, des emplois du temps et des disponibilités.

L'application supporte notamment :
- gestion des clubs et de leurs responsables
- gestion multi-membres de clubs
- connexion/authentification via cookies
- gestion d'utilisateurs avec rôles différents
- réservations de salles et emplois du temps
- affichage conditionnel des informations selon le rôle

## 🧱 Stack technique
- `ASP.NET Core MVC` avec C# et Razor Views
- `Entity Framework Core 10` pour l'accès aux données
- `PostgreSQL` comme base de données
- `Npgsql.EntityFrameworkCore.PostgreSQL` pour le provider PostgreSQL
- `BCrypt.Net-Next` pour le hash des mots de passe
- `ASP.NET Core Authentication` via cookies
- `Visual Studio / dotnet CLI` pour le développement et l'exécution

## 📁 Structure du projet
- `Program.cs` : configuration de l'application, routage, auth et DbContext
- `Controllers/` : logique métier et actions MVC
- `Models/` : entités métiers et mapping vers la base
- `Views/` : pages Razor utilisées par les contrôleurs
- `Data/` : `DbContext` et configuration EF Core
- `Migrations/` : historique des migrations de base de données
- `wwwroot/` : ressources statiques (CSS, JS, images)

## 🔧 Installation
1. Ouvrir un terminal dans `C:\Users\Manou\Desktop\Projet_ASP.NET\GESTION_S_E\GESTION_S_E`
2. Restaurer les paquets :
   ```powershell
   dotnet restore
   ```
3. Construire le projet :
   ```powershell
   dotnet build
   ```
4. Lancer l'application :
   ```powershell
   dotnet run
   ```

## 🧩 Configuration de la base de données
Le projet utilise la chaîne de connexion PostgreSQL définie dans `appsettings.json` :

```json
"ConnectionStrings": {
  "MaConnexion": "Host=localhost;Database=GESTION_SALLES_ET_EMPLOIS_DU_TEMPS_EMIT;Username=postgres;Password=1234"
}
```

### Migrations
Pour appliquer les migrations existantes via le bon `DbContext` :

```powershell
cd C:\Users\Manou\Desktop\Projet_ASP.NET\GESTION_S_E\GESTION_S_E
dotnet ef database update --context MonDbContext
```

## ✨ Fonctionnalités principales
### Gestion des clubs
- création, modification, suppression des clubs
- affectation d’un responsable
- affichage du nom complet du responsable selon son rôle

### Gestion des membres de club
- affichage des membres par club
- sélection multiple d’utilisateurs pour ajout en une fois
- possibilité pour une personne d’être membre de plusieurs clubs
- suppression d’un membre du club

### Authentification/autorisation
- connexion via cookies
- restrictions d’accès selon les rôles

### Autres modules
- gestion des élèves, enseignants et scolarité
- réservations de salles
- emplois du temps
- notifications

## 🧠 Schéma relationnel / algorithmique
### Relations principales
- `Utilisateur` est lié à `Eleve`, `Enseignant` ou `Scolarite` selon le rôle
- `Club` possède un `Responsable` (utilisateur)
- `MembreClub` lie un `Utilisateur` à un `Club`
- `ReservationSalle` lie un `Utilisateur`, une `Salle` et éventuellement un `Club`
- `EmploiDuTemps` peut cibler une `Classe` ou un `Groupe`

### Représentation algorithmique des relations
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
    CLUB {
        int id_club PK
        int id_responsable FK
        string NomClub
        string Description
        bool Actif
    }
    MEMBRE_CLUB {
        int id_utilisateur FK
        int id_club FK
        string role_membre
        DateTime date_adhesion
    }
    RESERVATION_SALLE {
        int id_reservation PK
        int id_utilisateur FK
        int id_salle FK
        int? id_club FK
    }
    SALLE {
        int id_salle PK
        string NomSalle
        string type
    }

    UTILISATEUR ||--o{ ELEVE : "est"
    UTILISATEUR ||--o{ ENSEIGNANT : "est"
    UTILISATEUR ||--o{ SCOLARITE : "est"
    UTILISATEUR ||--o{ MEMBRE_CLUB : "participe à"
    CLUB ||--o{ MEMBRE_CLUB : "contient"
    UTILISATEUR ||--o{ RESERVATION_SALLE : "réserve"
    SALLE ||--o{ RESERVATION_SALLE : "est réservée"
    CLUB ||--o{ RESERVATION_SALLE : "peut être liée"
    UTILISATEUR ||--o{ CLUB : "peut être responsable de"
```

## 📝 Notes importantes
- Le modèle `MembreClub` est géré comme une association `Utilisateur <-> Club` à clé composite (`IdUtilisateur`, `IdClub`).
- Le champ `RoleMembre` est réglé par défaut sur `membre` lors de l’ajout automatique.
- L’application est conçue pour que chaque utilisateur puisse appartenir à plusieurs clubs.

## 🎯 Bonnes pratiques pour l’utilisation
- Toujours arrêter l’application avant de reconstruire pour éviter les fichiers verrouillés
- Exécuter `dotnet ef database update --context MonDbContext` après modification du modèle
- Vérifier les logs PostgreSQL pour les erreurs de mapping ou de contrainte

## 📌 Chemin du projet
- Code source : `C:\Users\Manou\Desktop\Projet_ASP.NET\GESTION_S_E\GESTION_S_E`
- README global : `C:\Users\Manou\Desktop\Projet_ASP.NET\README.md`

---

Bonne utilisation de l’application ! Si tu veux, je peux aussi ajouter un diagramme visuel des tables dans le dossier du projet. 