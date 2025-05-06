# PaparaBootcampFinalCase

# 📊 ExpenseManager API


Bu proje, bir şirketin saha çalışanlarının masraf girişlerini, onay süreçlerini ve ödemelerini yönetmek için geliştirilmiş bir backend uygulamasıdır.  
Hem **Admin (yönetici)** hem **Employee (personel)** rolleriyle JWT bazlı kimlik doğrulaması sağlar.

---

## 📌 Proje Özellikleri

✅ Saha çalışanı masraf talebi oluşturur, onay/red durumunu takip eder.  
✅ Yönetici tüm talepleri görür, onaylar veya reddeder, ödemeyi başlatır.  
✅ Sanal banka ödeme simülasyonu.  
✅ Masraf kategorileri ve kullanıcı yönetimi.  
✅ JWT ile yetkilendirme ve rol bazlı erişim.  
✅ Swagger arayüzü üzerinden test edilebilir.  
✅ Redis cache, Serilog loglama, FluentValidation, MediatR CQRS, AutoMapper.

---

## 🏗 Kullanılan Teknolojiler

| Katman               | Teknoloji / Kütüphane                     |
|----------------------|------------------------------------------|
| Backend Framework    | .NET 8  ASP.NET Core Web API     |
| Veri Tabanı         | SQL Server (EF Core Code First + Dapper) |
| Mimari Desenler      | Unit of Work, Generic Repository, CQRS + MediatR        |
| Kimlik Doğrulama    | JWT Token                                |
| Caching            | Redis + StackExchange.Redis               |
| Loglama            | Serilog                                   |
| API Dokümantasyonu  | Swagger / OpenAPI                        |
| Validasyon         | FluentValidation                          |

---

## ⚙ Gereksinimler

✅ [.NET 8 SDK](https://dotnet.microsoft.com/download)  
✅ [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
✅ [Redis](https://redis.io/)  
✅ Postman (isteğe bağlı) veya web tarayıcı (Swagger testleri için)

---

## 🚀 Kurulum Adımları

### 1️⃣ Kaynak Kodunu Klonla
```bash
git clone https://github.com/kullanici-adi/ExpenseManager.git
cd ExpenseManager
```

---

### 2️⃣ Veri Tabanını Hazırla
✅ `appsettings.json` içinde bağlantı cümleni güncelle:
```json
"ConnectionStrings": {
    "ExpenseManagerDbConnection": "Server=localhost;Database=ExpenseManagerDb;User Id=sa;Password=YourPassword;"
}
```

✅ Migration uygula:
```bash
dotnet ef database update
```

---

### 3️⃣ Redis’i Başlat
- Docker:
```bash
docker run -d -p 6379:6379 redis
```
- Yerel kurulum:
```bash
redis-server
```

---

### 4️⃣ Uygulamayı Çalıştır
```bash
dotnet run --project ExpenseManager.Api
```

Varsayılan adres:
```
https://localhost:5001
```
---

### 5️⃣ Scriptleri Çalıştır

Database Script adres:
```
ExpenseManager.Api -> DatabaseScripts
```
StoredProcedures(SP)
```
CreateSpApprovalRejectionSummaryByDateRange.sql
CreateSpEmployeeExpenseSummaryByDateRange.sql
CreateSpCompanyExpenseSummaryByDateRange.sql
```
VIEW
```
CreateVW_UserExpenses.sql
```
Test Datas
```
InsertOrnekUser.sql
Insert_AccountHistoryData.sql
Insert_ExpenseCategory.sql
Insert_ExpenseData.sql
```


---

### 6️⃣ Swagger Arayüzü
Tarayıcıdan aç:
```
https://localhost:5001/swagger
```

---

## 🔑 Başlangıç Kullanıcıları

| Rol     | Email                 | Şifre        |
|---------|------------------------|-------------|
| Admin   | dilara@gmail.com      | 12345   |
| Employee| gokhan@gmail.com   | 12345|
| Employee| ahmetmehmet@gmail.com   | 123456|

✅ Örnek kullanıcıların insert scripti repo içerisinde ExpenseManager.Api->DatabaseScripts->InsertOrnekUser.sql
içerisindedir.


✅ `/api/Authorization/Token` endpoint’i ile JWT token al.  
✅ Swagger’daki Authorize butonuna yapıştır.  
✅ Artık yetkili endpoint’leri test edebilirsin!

---

## 🌐 API Endpoint Listesi

### 🔑 Authentication
| Method | Route                        | Açıklama                      |
|--------|------------------------------|-------------------------------|
| POST   | /api/Authorization/Token     | Kullanıcı girişi ve JWT token al |

### 👤 Users
| Method | Route                      | Açıklama                       |
|--------|----------------------------|-------------------------------|
| GET    | /api/User                  | Tüm kullanıcıları getir (Admin) |
| GET    | /api/User/{id}             | Kullanıcı detayını getir        |
| POST   | /api/User                  | Yeni kullanıcı yarat            |
| PUT    | /api/User                  | Kullanıcıyı güncelle            |
| PUT    | /api/User/ChangePassword   | Şifre değiştir (Kullanıcı)      |
| DELETE | /api/User/{id}             | Kullanıcıyı sil                 |

### 💰 Expenses
| Method | Route                            | Açıklama                        |
|--------|----------------------------------|---------------------------------|
| GET    | /api/Expense                     | Tüm masraflar (Admin)           |
| GET    | /api/Expense/YourExpenses        | Kendi masrafların               |
| GET    | /api/Expense/{id}                | Masraf detayını getir           |
| POST   | /api/Expense/CreateExpense       | Yeni masraf talebi              |
| PUT    | /api/Expense/UpdateExpense/{id}  | Masrafı güncelle                |
| PUT    | /api/Expense/ApproveExpense/{id} | Masrafı onayla (Admin)          |
| PUT    | /api/Expense/RejectExpense/{id}  | Masrafı reddet (Admin)          |
| DELETE | /api/Expense/CancelExpense/{id}  | Masrafı iptal et                |

### 🗂 Expense Categories
| Method | Route                            | Açıklama                        |
|--------|----------------------------------|---------------------------------|
| GET    | /api/ExpenseCategory             | Tüm kategoriler                 |
| GET    | /api/ExpenseCategory/{id}        | Kategori detayını getir         |
| POST   | /api/ExpenseCategory             | Yeni kategori yarat (Admin)     |
| PUT    | /api/ExpenseCategory/{id}        | Kategoriyi güncelle (Admin)     |
| DELETE | /api/ExpenseCategory/{id}        | Kategoriyi sil (Admin)          |

### 🏦 Account History
| Method | Route                                              | Açıklama                         |
|--------|----------------------------------------------------|----------------------------------|
| GET    | /api/AccountHistory/MyAccountTransactions          | Kendi hesap hareketlerin         |
| GET    | /api/AccountHistory/AccountTransactionDetail/{id}  | Hareket detayını getir           |

### 📊 Reports
| Method | Route                                        | Açıklama                         |
|--------|---------------------------------------------|----------------------------------|
| GET    | /api/Report/UserExpenses/{userId}           | Kullanıcı bazlı rapor            |
| GET    | /api/Report/CompanyExpenseSummary          | Şirket harcama özeti             |
| GET    | /api/Report/EmployeeExpenseSummary        | Çalışan bazlı harcama özeti      |
| GET    | /api/Report/ApprovalRejectionSummary      | Onay/red durum raporu            |

---

## 🛠 Code Overview

```
ExpenseManager/
│
├── ExpenseManager.Api/
│   ├── Controllers/                  # API endpoint controller'ları
│   ├── Entities/                     # Veri tabanı entity sınıfları
│   ├── Impl/
│   │   ├── Command/                  # MediatR command handler'ları
│   │   ├── Cqrs/                     # CQRS command & query tanımları
│   │   ├── Query/                    # MediatR query handler'ları
│   │   ├── GenericRepository/        # Generic repository implementasyonu
│   │   ├── UnitOfWork/               # UnitOfWork deseni
│   │   └── Validation/               # FluentValidation sınıfları
│   ├── Middleware/                   # Custom middleware (örn. ErrorHandler)
│   ├── Services/
│   │   ├── BankPaymentService/       # Sanal ödeme servisleri
│   │   ├── Cashe/                    # Cache interface’leri
│   │   ├── ReportService/            # Raporlama servisleri (Dapper & SP)
│   │   └── Token/                    # JWT token üretimi
│   ├── Mapper/                       # AutoMapper konfigürasyonu
│   ├── Request/                      # Request DTO’ları
│   ├── Filter/                       # Custom filtreler (Log, Exception vb.)
│   └── ExpenseManagerDbContext.cs    # DbContext + AuditLog işlemleri
│
├── ExpenseManager.Base/              # Ortak servisler, AppSession, JWT, Logging
├── ExpenseManager.Schema/            # Request/Response/DTO modelleri
├── appsettings.json                  # Konfigürasyon dosyası
└── Startup.cs                        # DI, Middleware, Swagger ve JWT setup
```

---

## 📊 Ekran Görüntüleri

### 🔑 Login ve Token Alma
![Image](https://github.com/user-attachments/assets/6cb8716d-06c2-406f-b316-aa7949065298)

### ✅ Token Başarılı Döndü
![Image](https://github.com/user-attachments/assets/4fda99fc-61c3-4448-83f2-f974b88b149f)

### 🔒 Bearer Token Yetkilendirme
![Image](https://github.com/user-attachments/assets/df69f7dd-1555-45d4-ae8b-8a1addb8b17a)

![Image](https://github.com/user-attachments/assets/d39f52d5-abe2-4099-927d-d9111b4f1815)


### 🏦 Masraf Listeleme
![Image](https://github.com/user-attachments/assets/f241a917-e843-4850-865c-61985f9903cc)

-Admin girilen tüm masrafları listeyeleyebilir
![Image](https://github.com/user-attachments/assets/970ce48d-9462-46b0-82b0-d836594b5aa4)

-Personel kendi girdiği masrafları görüntüleyebilir
![Image](https://github.com/user-attachments/assets/39b41fa2-0b65-49a3-ab06-8cf32098cb81)

-Masraf girişi yapıp fişini yükler
![Image](https://github.com/user-attachments/assets/571ef350-556f-407e-ad09-38b0fb144e43)

-Admin girilen masrafları onaylayıp reddedebilir. Masraflar onaylandığı anda kişinin hesabına masraf tutarı yatırılır.
Ödemesi onaylanıp yatırılan masrafları kişiler AccountHistory/MyAccountTransactions 'dan listeleyebilir
![Image](https://github.com/user-attachments/assets/3bd042cc-0872-4bca-be65-33bf84bca51a)
![Image](https://github.com/user-attachments/assets/8d2932ba-1853-4cb1-b2af-68586ddf146a)

---

### 🗂 Expense Category Listeleme
![Image](https://github.com/user-attachments/assets/2313bce9-b07b-406d-810e-8cc115d21aeb)

### ❌ Expense Category Silme Hatası
Eğer bir kategoriye ait aktif masraf varsa, o kategori silinemez , hata verir.  
![Image](https://github.com/user-attachments/assets/a0f080ba-2058-452d-92ae-061c758854ad)

---

### 👥 Kullanıcı İşlemleri
- Kullanıcı listeleme, detay görme ve yeni kullanıcı ekleme , şifre değiştirme ekranları:
- Yeni kullanıcıları admin rolündeki kişiler ekleyebilir .
![Image](https://github.com/user-attachments/assets/6c1cc819-8e6d-4a7f-9f65-ec444a98ed03)
- Her kullanıcı sadece kendi şifresini değiştirebilir
![Image](https://github.com/user-attachments/assets/b7e38dc3-cdab-43cc-90a9-4c0210e500e7)

---

### 🏦 Account History Görüntüleme
- Kullanıcı kendi hesap hareketlerini listeler
![Image](https://github.com/user-attachments/assets/0feacb33-03d5-4667-a7b4-3875eaf0b70b)

---

### 📈 Raporlama Görselleri
- Şirket harcama raporları, çalışan bazlı raporlar, onay/red durum raporları:
![Image](https://github.com/user-attachments/assets/bdbf00ce-e388-40c7-86bd-b4f58d030d25)
![Image](https://github.com/user-attachments/assets/a074c4bc-4d4b-48cd-a3bf-d3e6a7c1193f)
![Image](https://github.com/user-attachments/assets/5196714e-7419-4550-8228-5093d212fedb)
![Image](https://github.com/user-attachments/assets/27d06b7c-e714-4172-9315-077a0f5f5a97)
![Image](https://github.com/user-attachments/assets/4bc3f5d0-c254-4acc-a580-64cb787c471b)
![Image](https://github.com/user-attachments/assets/e2dd280a-c4ba-4bce-ba3f-c5ea009b5659)
![Image](https://github.com/user-attachments/assets/a1931e6a-1ca3-4a46-81d4-7c01b0fefa22)

---

