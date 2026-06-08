# CurrencyApp
Приложение для работы с курсами валют

## Стек
- .NET 8, C#
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL
- YARP (API Gateway)
- JWT авторизация
- Docker / Docker Compose
- xUnit, Moq (тесты)

## Архитектура
- Clean Architecture
- CQRS
- Микросервисы: UserService, FinanceService, ApiGateway, MigrationService, CurrencyBackgroundService

## Запуск через Docker

1. Установить Docker Desktop
2. Клонировать репозиторий
3. В корне проекта выполнить:
```
docker compose up --build
```
4. API Gateway доступен на http://localhost:5105

## API эндпоинты

Все запросы через API Gateway: http://localhost:5105

### UserService

**Регистрация**
```
POST /api/User/register
Content-Type: application/json

{
  "name": "string",
  "password": "string"
}
```

**Логин**
```
POST /api/User/login
Content-Type: application/json

{
  "name": "string",
  "password": "string"
}
```

**Логаут**
```
POST /api/User/logout
Authorization: Bearer {token}
```

**Добавить валюту в избранное**
```
POST /api/User/favorites/add
Authorization: Bearer {token}
Content-Type: application/json

{
  "currencyId": 1
}
```

**Удалить валюту из избранного**
```
DELETE /api/User/favorites/remove
Authorization: Bearer {token}
Content-Type: application/json

{
  "currencyId": 1
}
```

### FinanceService

**Получить курсы избранных валют**
```
GET /api/Currency
Authorization: Bearer {token}
```
