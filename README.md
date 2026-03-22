# 🚀 FinanceFlow: High-Performance & Scalable Banking System

**FinanceFlow** is a robust, production-ready banking backend designed with a core focus on **Data Integrity**, **High Availability**, and **Clean Architecture**. The system is architected to handle high-concurrency financial operations while ensuring 100% accuracy using advanced locking mechanisms and a hybrid database strategy.

---

## 🏗️ Architectural Overview

The project follows the **Clean Architecture (Onion Architecture)** pattern to ensure a strict separation of concerns, maintainability, and testability.

### 🏛️ System Layers:
- **Domain:** Contains Enterprise logic, Entities, and Exceptions.
- **Application:** Houses Business Logic, CQRS Commands/Queries, and MediatR Handlers.
- **Infrastructure:** Implements data access (SQL Server & MongoDB), Messaging (RabbitMQ), and Identity.
- **API:** The entry point, handling HTTP requests, Rate Limiting, and Global Error Management.

### 🔄 Request Flow (CQRS & Event-Driven)
```mermaid
graph TD
    A[Client Request] --> B[API Controller]
    B --> C{Rate Limiter}
    C -->|Authorized| D[MediatR Pipeline]
    D --> E[FluentValidation]
    E --> F[Command/Query Handler]
    F --> G[(SQL Server - ACID Ops)]
    F --> H[MassTransit / RabbitMQ]
    H --> I[Background Consumer]
    I --> J[(MongoDB - Transaction Logs)]
    I --> K[SMTP Email Service]
