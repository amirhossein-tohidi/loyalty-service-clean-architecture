# Loyalty Service – Customer Score Calculation Microservice 🎖️

> ⚠️ **Note**  
> This repository is a **code sample demonstrating the coding style and software architecture approach of Amirhossein Tohidi**.  
> It was created as a **technical portfolio project** to showcase implementation practices, Clean Architecture structure, and microservice development patterns.

This project is a standalone microservice for calculating customer loyalty scores for a retail system.  
It is implemented using **.NET 9** with **Clean Architecture** and **Domain Driven Design (DDD)** principles. 🏗️

The service supports both **REST API (HTTP1)** and **gRPC (HTTP2)** communication. 🚀

---

## 🛠 Technologies

- 💻 **.NET 9**
- 🌐 **ASP.NET Core**
- 🔌 **gRPC (HTTP2)**
- 📡 **REST API (HTTP1)**
- 🏛️ **Clean Architecture**
- 🧩 **Domain Driven Design (DDD)**
- 📝 **JSON File Logging**
- 🧪 **Unit Tests**
- 🧪 **Integration Tests**

---

## 🏗 Architecture

The project follows the **Clean Architecture** pattern.

- 🔴 **Loyalty.Api**  
- 🔵 **Loyalty.Application**  
- 🟢 **Loyalty.Domain**  
- 🟡 **Loyalty.Infrastructure**  
- 🧪 **Loyalty.Tests**

### 🟢 Domain
Contains the core business logic of the system:
- Loyalty score calculation algorithm
- Value Objects
- Enums
- Domain Exceptions

### 🔵 Application
Responsible for coordinating between Domain and Infrastructure:
- DTOs
- Validators
- Mappers
- Service orchestration
- Interfaces

### 🟡 Infrastructure
Implements external services and infrastructure concerns:
- `FileStorageService`
- JSON file logging

### 🔴 Api
Responsible for exposing the services:
- REST API endpoints
- gRPC services
- Global exception handling
- Dependency injection configuration

---

## 🌐 REST API (HTTP1)

📍 **Base Address:** `http://localhost:6014`  
📄 **API Documentation:** [Scalar Docs](http://localhost:6014/scalar)

> In the **appsettings** configuration, this endpoint is defined with the name: `HTTP1`

⚠️ All APIs are versioned and start with the following prefix: `api/v1/`

### 📥 Example Endpoint
`POST api/v1/loyalty/calculate`

### ✉️ Example Request
```json
{
  "purchaseAmount": 12000000,
  "customerType": "Gold",
  "lastMonthPurchases": [2000000, 3000000, 1500000, 500000, 800000]
}
```

---

## 🚀 gRPC (HTTP2)

📍 **Base Address:** `http://localhost:6015`

> In the **appsettings** configuration, this endpoint is defined with the name: `HTTP2`

This endpoint is used for gRPC communication. 🔌

---

## 🧮 Scoring Algorithm

The system receives the following inputs:
- `PurchaseAmount`
- `CustomerType` (Bronze | Silver | Gold)
- `LastMonthPurchases`

### 📊 Base Score
BaseScore is calculated based on the CustomerType and PurchaseAmount.

| Customer Type | Formula |
| :--- | :--- |
| **Bronze** | `PurchaseAmount / 100` |
| **Silver** | `PurchaseAmount / 80` |
| **Gold** | `PurchaseAmount / 60` |

### 🎁 Bonus Rules
Bonus represents the total bonus percentage calculated from the following rules:

1. **High Frequency:** If the number of purchases in the last month is >= 5: **10% bonus**
2. **High Value:** If the purchase amount is > 10,000,000: **5% base bonus**
3. **Extra Loyalty:** For every additional 10,000,000: **2.5% extra bonus**

### 📐 Final Formula
> **FinalScore** = `BaseScore + (BaseScore × Bonus / 100)`

This algorithm is implemented in the **Domain Service** layer.

---

## 📝 Logging

After every request:
- ✅ Request data
- ✅ Response data
- ✅ Execution time (UTC)
- ✅ Operation name

These are stored in a JSON log file.

📂 **Log directory:** `logs`  
📅 **Format:** `loyalty-log-YYYYMMDD.json`  
📄 **Example:** `loyalty-log-20260508.json`

---

## ✅ Testing

The project includes automated tests:
- 🧪 **Unit Tests**
- 🧪 **Integration Tests**

These tests cover the Domain logic and API endpoints.

---

## ▶️ Run the Project

To run the project, execute the following commands:

```bash
dotnet restore
dotnet build
dotnet run --project Loyalty.Api
```

After running the project:
- 📄 **REST API Documentation:** `http://localhost:6014/scalar`
- 🔌 **gRPC Service:** `http://localhost:6015`

---

## ⚠️ Notes

- 🚫 **No database** is used in this project.
- 💾 All request and response data are stored in **JSON log files**.
- 📌 APIs are versioned and available using the prefix: `api/v1/`
  - *Example:* `api/v1/loyalty/calculate`

