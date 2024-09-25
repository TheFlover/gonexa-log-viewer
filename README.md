# Log Viewer Application

Ce projet est une application de visualisation de logs développée avec Vue.js pour le frontend et ASP.NET Core pour le backend.

## Prérequis

- Node.js (v20.17.0)
- pnpm (v7.28.0)
- .NET 8.0 SDK

## Structure du Projet

Le projet est divisé en deux parties principales :

- `frontend/` : Contient l'application Vue.js
- `backend/` : Contient l'API ASP.NET Core

## Configuration

### Frontend

1. Naviguez dans le dossier frontend :
   ```
   cd frontend
   ```

2. Installez les dépendances :
   ```
   pnpm install
   ```

### Backend

1. Naviguez dans le dossier backend :
   ```
   cd backend\LogViewerApi\
   ```

2. Restaurez les packages NuGet :
   ```
   dotnet restore
   ```

## Lancement du Projet

### Développement

1. Lancez le backend :
   ```
   cd backend\LogViewerApi\
   dotnet run
   ```
   L'API sera accessible à `http://localhost:5008`.

2. Dans un nouveau terminal, lancez le frontend :
   ```
   cd frontend
   pnpm run dev
   ```
   L'application sera accessible à `http://localhost:8080`.

### Production

1. Construisez le frontend :
   ```
   cd frontend
   npm run build
   ```

2. Lancez le backend en mode production :
   ```
   cd backend\LogViewerApi\
   dotnet run --configuration Release
   ```

3. Pour servir le frontend en production localement :
   ```
   cd frontend
   pnpm run prod
   ```

## Linting

Pour lancer le linter sur le code frontend :
```
cd frontend
npm run lint
```

## Environnements

- Le fichier `.env.development` est utilisé pour le développement.
- Le fichier `.env.production` est utilisé pour la production.

Assurez-vous de configurer ces fichiers avec les bonnes variables d'environnement.
