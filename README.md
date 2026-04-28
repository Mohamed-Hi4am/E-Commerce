# 🛒 E-Commerce Store

A complete e-commerce solution built with **ASP.NET Core 8** (Onion Architecture) as the backend, and **Angular** as the customer-facing frontend. This application integrates with **Redis** as the caching service, and **Stripe** as the payment service.

---

## 📸 Project Screenshots

### Customer Frontend (Angular)
| Shop Page | Authentication |
|---|---|
| ![Shop Page](<img width="1872" height="881" alt="Shop Page" src="https://github.com/user-attachments/assets/a90888ed-5de0-46a4-b39e-cf5dc0e3503d" />
) | ![Authentication](<img width="1897" height="883" alt="Authentication" src="https://github.com/user-attachments/assets/e43b36d3-5f7b-467f-897b-55634723ac0c" />
) |

| Basket (Cart) | Checkout |
|---|---|
| ![Basket](<img width="1870" height="881" alt="Basket (Cart)" src="https://github.com/user-attachments/assets/29980dac-19c5-487b-ae49-321bfcb0d6a8" />
) | ![Checkout](<img width="1872" height="883" alt="Checkout" src="https://github.com/user-attachments/assets/f58d9ad3-53af-4916-831b-ae22cb3cba46" />
) |

| Order Confirmation | Orders Tracking |
|---|---|
| ![Order Confirmation](<img width="1892" height="882" alt="Order Confirmation" src="https://github.com/user-attachments/assets/ab4c67cb-8ed3-4f68-a952-35f5a746a1fb" />
) | ![Orders Tracking](<img width="1892" height="881" alt="Order Tracking" src="https://github.com/user-attachments/assets/3b16371d-36c3-44b3-9468-b51c2f48b1d5" />
) |

### API Documentation
| Swagger UI |
|---|
| ![Swagger](<img width="1731" height="2069" alt="Swagger UI" src="https://github.com/user-attachments/assets/7033ecc8-bb5f-442f-a3da-e235e3ff6404" />
) |

---

## 🏗️ Architecture

This project follows **Onion (Clean) Architecture** with full separation of concerns:

```
E-Commerce/
├── 📁 Core/
│   ├── E-Commerce.Domains/               # Entities, Contracts, Exceptions
│   ├── E-Commerce.Services.Abstractions/ # Service Interfaces/Contracts
│   └── E-Commerce.Services/              # Business Logic, Specifications, Mapping
│
├── 📁 Infrastructure/
│   ├── E-Commerce.Persistence/           # Data Access, Repositories, DbContexts
│   └── E-Commerce.Presentation/          # API Controllers
│
├── 📁 E-Commerce.API/                    # Middlewares, Configurations, Factories
│                  
├── 📁 E-Commerce.Shared/                 # Shared DTOs, Enums
│
└── E-Commerce.sln
```

### Architecture Flow

```
`Client Request` ➔ `Middleware (Auth/Error Handling)` ➔ `API Controller` ➔ `Service Logic` ➔ `Repository/Unit of Work` ➔ `Database/Redis/Stripe` ➔ `Response to Client`
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| **Backend API** | ASP.NET Core 8 Web API |
| **Frontend** | Angular 16+ (TypeScript, RxJS) |
| **Database** | SQL Server |
| **Caching** | Redis |
| **Payments** | Stripe (Elements API) |
| **Authentication** | ASP.NET Core Identity + JWT |
| **ORM** | Entity Framework Core 8 |
| **API Documentation** | Swagger / OpenAPI |
| **Mapping** | AutoMapper |
| **API Testing** | Postman |
| **Version Control** | Git + GitHub |

---

## ✨ Key Features

### 🛍️ Customer Features
- Browse products with filtering, sorting, and pagination
- Product search by name
- Shopping cart with real-time Redis caching
- Secure checkout with Stripe payment integration
- Order history and detailed order tracking
- User authentication (Register / Login / Logout)
- Address management

### 🔐 Authentication & Authorization
- **JWT Bearer Tokens** for the Angular SPA (stateless API auth)
- Password hashing with ASP.NET Core Identity

### 💳 Stripe Payment Integration
- Payment Intent creation on the backend
- Secure card input with Stripe Elements

### 🏗️ Backend Design Patterns
- **Generic Repository Pattern** with Specifications
- **Unit of Work Pattern** for transaction management
- **Specification Pattern** for flexible query composition
- **Global Exception Handling Middleware**
- **Onion Architecture** with dependency injection
- **Data Seeding** for initial products, brands, types, and delivery methods

---

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 16 & npm](https://nodejs.org/) (for Angular CLI 11.2.14)
- SQL Server
- [Redis Server](https://redis.io/download) (Running locally or via Docker)
- A [Stripe Developer Account](https://stripe.com/) (for the Stripe CLI)
- Stripe CLI (by "Scoop" or any other method)
- Visual Studio 2022 / VS Code

### 1. Clone the Repository

```bash
git clone https://github.com/Mohamed-Hi4am/E-Commerce.git
cd E-Commerce
```

### 2. Database Setup

The database is configured with **automatic seeding**. Just update the connection strings:

Open `E-Commerce.API/appsettings.json` and set your connection strings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server = .; Database = E-Commerce; Trusted_Connection = true; TrustServerCertificate = true",
    "IdentitySQLConnection": "server = .; Database = E-Commerce.Identity; Trusted_Connection = true; TrustServerCertificate = true",
    "Redis": "localhost"
  }
}
```

> The database tables and seed data will be created automatically on first run.

### 3. Configure Secrets

In the `E-Commerce.API/appsettings.json` or `secrets.json` files:

```json
{
  "JwtOptions": {
    "SecretKey": "YOUR_JWT_SECRET_KEY"
  },
  "StripeSettings": {
    "SecretKey": "YOUR_STRIPE_APP_SECRET_KEY",
    "EndPointSecret": "YOUR_WEBHOOK_SIGNING_SECRET"
  }
}
```

### 4. Run the local Redis database

### 5. Set up the Stripe CLI (either by "Scoop" package manager or any other method)

- After setting up Stripe CLI, run these commands in the PowerShell to login to your Stripe account, then activate the Stripe CLI listener to listen to Stripe
```bash
stripe login
stripe listen -f https://localhost:7057/api/payments/webhook -e payment_intent.succeeded,payment_intent.payment_failed
```

- Then add the webhook endpoint's secret that will dsiplay to the `"appsettings"` file `EndPointSecret` subsection:
```csharp
"StripeSttings": {
	"SecretKey": "application's_stripe_account_secret_key",
	"EndPointSecret": "Whatever_for_now_lol"
}
```

### 6. Run the Backend API

```bash
cd E-Commerce.API
dotnet run
```

API available at: `https://localhost:7202/swagger`

### 7. Run the Angular Frontend

1. Install Node.js version 16
2. Install Angular 11.2.14
```bash
npm i -g  @angular/cli@11.2.14
```
3. Download the Angular project from here: 
4. Open the Angular client directory using `VS Code` (or something like it)
5. Navigate to the `src` folder -> `environments` folder -> `environment.ts` file, and link your backend project in `apiUrl`:
```bash
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7057/api/'
}; 
```
6. Navigate to the `src` folder -> `app` folder -> `checkout` folder -> `checkout-payment` folder -> `checkout-payment.component.ts`, and put your app's Stripe account publishable key in this line:
```bash
this.stripe = Stripe('PUT_YOUR_SECRET_KEY_HERE');
```
7. Open a new terminal and run the project
```bash
cd client
ng serve -o
```
or
```bash
cd client
npm start
```

Frontend available at: `https://localhost:4200`

---

## 🧪 API Testing

Import the Postman collection and test all endpoints:

| Category | Endpoints |
|---|---|
| **Auth** | Register, Login, Get Current User |
| **Products** | Get All (with pagination), Get By Id, Get Brands, Get Types |
| **Basket** | Get Basket, Update Basket, Delete Basket |
| **Orders** | Create Order, Get Orders, Get Order Details |
| **Payments** | Create Payment Intent, Update Payment Intent |

---

## 📁 Project Structure Details

### Domains Layer
- **Entities**: Product, ProductBrand, ProductType, Order, OrderItem, Basket, AppUser
- **Contracts**: IGenericRepository, IUnitOfWork, ISpecification, IBasketRepository
- **Exceptions**: NotFound, BadRequest, Unauthorized, Validation custom exceptions

### Services Layer
- **ProductService**: Product retrieval with specifications
- **BasketService**: Redis-based basket management
- **OrderService**: Order creation and tracking
- **PaymentService**: Stripe payment intent management
- **AuthService**: JWT token generation and user registration
- **Specifications**: ProductWithBrandAndType, Order, OrderWithPaymentIntent
