# WeCount - Spécification des APIs Backend

## Vue d'ensemble
Ce document spécifie les APIs nécessaires pour l'application WeCount, une application de gestion financière pour couples basée sur l'architecture CQRS (Command Query Responsibility Segregation).

## Architecture
- **Pattern**: CQRS avec MediatR
- **Base de données**: Entity Framework Core
- **Authentification**: JWT Bearer Token
- **Validation**: FluentValidation
- **Logging**: Serilog

## Modules principaux

### 1. Module Authentification & Utilisateurs

#### Endpoints:
- `POST /api/auth/register` - Inscription d'un utilisateur
- `POST /api/auth/login` - Connexion
- `POST /api/auth/refresh` - Rafraîchir le token
- `GET /api/users/profile` - Profil utilisateur
- `PUT /api/users/profile` - Mettre à jour le profil

### 2. Module Couples

#### Endpoints:
- `POST /api/couples` - Créer un couple
- `GET /api/couples/{id}` - Obtenir les détails du couple
- `PUT /api/couples/{id}` - Mettre à jour les informations du couple
- `POST /api/couples/{id}/invite` - Inviter un partenaire
- `POST /api/couples/{id}/accept-invitation` - Accepter une invitation
- `GET /api/couples/{id}/members` - Obtenir les membres du couple

### 3. Module Transactions

#### Endpoints:
- `POST /api/transactions` - Créer une transaction
- `GET /api/transactions` - Obtenir les transactions (avec pagination et filtres)
- `GET /api/transactions/{id}` - Obtenir une transaction spécifique
- `PUT /api/transactions/{id}` - Mettre à jour une transaction
- `DELETE /api/transactions/{id}` - Supprimer une transaction
- `GET /api/transactions/categories` - Obtenir les catégories de transactions

### 4. Module Budget

#### Endpoints:
- `POST /api/budgets` - Créer un budget
- `GET /api/budgets` - Obtenir les budgets
- `GET /api/budgets/{id}` - Obtenir un budget spécifique
- `PUT /api/budgets/{id}` - Mettre à jour un budget
- `DELETE /api/budgets/{id}` - Supprimer un budget
- `GET /api/budgets/{id}/status` - Obtenir le statut du budget (dépensé/restant)

### 5. Module Objectifs Financiers

#### Endpoints:
- `POST /api/goals` - Créer un objectif
- `GET /api/goals` - Obtenir les objectifs
- `GET /api/goals/{id}` - Obtenir un objectif spécifique
- `PUT /api/goals/{id}` - Mettre à jour un objectif
- `DELETE /api/goals/{id}` - Supprimer un objectif
- `POST /api/goals/{id}/contributions` - Ajouter une contribution à un objectif
- `GET /api/goals/{id}/progress` - Obtenir le progrès d'un objectif

### 6. Module Dettes

#### Endpoints:
- `POST /api/debts` - Créer une dette
- `GET /api/debts` - Obtenir les dettes
- `GET /api/debts/{id}` - Obtenir une dette spécifique
- `PUT /api/debts/{id}` - Mettre à jour une dette
- `DELETE /api/debts/{id}` - Supprimer une dette
- `POST /api/debts/{id}/payments` - Enregistrer un paiement
- `GET /api/debts/{id}/schedule` - Obtenir l'échéancier de paiement

### 7. Module Analytics

#### Endpoints:
- `GET /api/analytics/dashboard` - Données du tableau de bord
- `GET /api/analytics/monthly-summary` - Résumé mensuel
- `GET /api/analytics/spending-trends` - Tendances de dépenses
- `GET /api/analytics/category-breakdown` - Répartition par catégorie
- `GET /api/analytics/predictions` - Prédictions financières
- `GET /api/analytics/couple-comparison` - Comparaison des dépenses du couple

### 8. Module Score du Couple

#### Endpoints:
- `GET /api/couple-score` - Obtenir le score du couple
- `GET /api/couple-score/history` - Historique du score
- `GET /api/couple-score/metrics` - Métriques détaillées
- `POST /api/couple-score/calculate` - Recalculer le score

## Modèles de données principaux

### User
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### Couple
```csharp
public class Couple
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CoupleUser> Members { get; set; }
}
```

### Transaction
```csharp
public class Transaction
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public Guid CoupleId { get; set; }
    public TransactionType Type { get; set; } // Income, Expense
}
```

### Budget
```csharp
public class Budget
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public decimal Amount { get; set; }
    public decimal Spent { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CoupleId { get; set; }
}
```

### Goal
```csharp
public class Goal
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal TargetAmount { get; set; }
    public decimal SavedAmount { get; set; }
    public DateTime Deadline { get; set; 
    public string Icon { get; set; }
    public Guid CoupleId { get; set; }
}
```

### Debt
```csharp
public class Debt
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal InterestRate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DebtType Type { get; set; } // Loan, Borrowing
    public PaymentStatus Status { get; set; } // Active, Paid, Late
    public Guid CoupleId { get; set; }
}
```

## Réponses API standardisées

### Succès
```json
{
  "success": true,
  "data": {},
  "message": "Operation completed successfully"
}
```

### Erreur
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Validation failed",
    "details": []
  }
}
```

## Authentification
Toutes les APIs (sauf auth/register et auth/login) nécessitent un token JWT dans l'en-tête:
```
Authorization: Bearer <token>
```

## Pagination
Les endpoints de liste supportent la pagination:
```
GET /api/transactions?page=1&pageSize=20&sortBy=date&sortDirection=desc
```

## Filtres
Les endpoints supportent des filtres spécifiques:
```
GET /api/transactions?category=food&dateFrom=2024-01-01&dateTo=2024-01-31&userId=guid
```

## Codes de statut HTTP
- `200` - OK
- `201` - Created
- `400` - Bad Request
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `500` - Internal Server Error
