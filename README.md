# Gestion d'un club sportif (C#)

Mini-projet de Programmation Avancée : gestion des membres, du planning des entraînements et des compétitions d'un club sportif.

Application **Windows Forms** (.NET 10) avec **Entity Framework Core** (approche **Code First**) et une base **SQLite** (`club.db`).

## Lancer le projet
Nécessite Visual Studio avec le SDK .NET 10. Ouvrir GestionClub.csproj puis F5 : la base club.db est copiée automatiquement à côté de l'exécutable.

1. Ouvrir le projet dans Visual Studio (ou `dotnet restore` en ligne de commande).
2. Créer la base avec les migrations (une seule fois) :
   - Console du Gestionnaire de package :
     ```
     Add-Migration InitialCreate
     Update-Database
     ```
   - ou en ligne de commande :
     ```
     dotnet tool install --global dotnet-ef
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```
3. Lancer l'application (F5 ou `dotnet run`).

## Objectif 3 : intégration de la base de données

- `Data/ClubContext.cs` : le DbContext qui relie nos classes à la base SQLite.
- `Migrations/` : générées par EF Core à partir des classes du dossier `Models/`.
- `Services/Club.cs` : le club lit et enregistre maintenant ses données en base (et non plus dans des listes en mémoire).
- Deux interfaces CRUD :
  - **Gestion des membres** : ajouter, modifier, supprimer, rechercher, payer la cotisation.
  - **Planning des séances** : planifier une séance (entraîneur + discipline), la modifier, l'annuler ou la supprimer.

### Choix de conception

- **Héritage** : `Membre` et `Entraineur` (qui héritent de `Personne`) sont stockés dans **une seule table `Personnes`**, avec une colonne `TypePersonne` (stratégie TPH, la « généralisation »).
- **Membre ↔ Séance** (plusieurs-à-plusieurs) : table de liaison `Participations`.
- **Inscription** : classe d'association entre `Membre` et `Competition`, avec une clé primaire composée (`MembreId`, `CompetitionId`).
- **Suppression** : un entraîneur ou une discipline utilisés par une séance ne peuvent pas être supprimés (`Restrict`). Supprimer un membre supprime ses inscriptions et participations (`Cascade`).
- **Changements dans les classes pour EF Core** :
  - un constructeur vide privé dans chaque classe, qu'EF Core utilise pour recréer les objets lus en base ;
  - un `set` privé sur `Id`, `Annulee`, `Membre` et `Competition`, pour qu'EF Core puisse remplir ces propriétés ;
  - les identifiants sont maintenant générés par la base (on passe `0` au constructeur).

## Faire évoluer la base

Après une modification d'une classe du modèle :

```
Add-Migration NomDeLaModification
Update-Database
```

