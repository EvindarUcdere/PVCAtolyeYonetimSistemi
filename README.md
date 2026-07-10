# PVC Atolye Yonetim Sistemi

PVC kapi, pencere ve dograma atolyeleri icin yerel agda calisabilen, musteri-kesif-teklif-siparis-stok-uretim-montaj-tahsilat-servis akislarini yoneten ticari uygulama.

## Durum

Faz 0 analiz dokumani hazirlandi. Faz 1 kapsaminda proje altyapisi kuruluyor: .NET backend, React frontend, PostgreSQL gelistirme ortami, logging, Swagger, health check, standart API response ve temel test iskeleti.

## Ana teknoloji yigini

- Backend: .NET 8 Web API, C#, EF Core, PostgreSQL, FluentValidation, JWT, Serilog, Swagger/OpenAPI, SignalR
- Frontend: React, TypeScript, Vite, React Router, TanStack Query, React Hook Form, Zod, Axios, Ant Design
- Test: xUnit, FluentAssertions, Moq, Testcontainers, React Testing Library, Vitest
- Raporlama: QuestPDF, ClosedXML
- Ortam: Docker Compose, Git, GitHub, PostgreSQL

## Mimari

Moduler monolith, katmanli mimari:

- `src/PVCAtolye.Domain`: domain ortak tipleri ve ileride eklenecek is kurallari
- `src/PVCAtolye.Application`: use case, DTO, validation ve application servisleri
- `src/PVCAtolye.Infrastructure`: EF Core, PostgreSQL, dosya, rapor ve dis servis adaptörleri
- `src/PVCAtolye.Api`: Web API, middleware, Swagger, SignalR, static file hosting
- `web`: React + TypeScript frontend, feature-based klasor yapisi
- `tests`: unit ve integration test projeleri

## Gereksinimler

- .NET 8 SDK
- Node.js 20 veya ustu
- Docker Desktop
- PostgreSQL istemcisi olarak DBeaver veya pgAdmin

Not: Bu makinede su an yalnizca .NET 10 SDK gorundu ve cekirdek `net8.0` projeleri derlenebildi. Proje standardi yine de .NET 8 SDK uzerinden gelistirme ve dogrulama yapmaktir.

## Lokal gelistirme

1. Ortam dosyasini hazirla:

```powershell
Copy-Item .env.example .env
Copy-Item web/.env.example web/.env
```

2. PostgreSQL ve pgAdmin'i baslat:

```powershell
docker compose up -d postgres pgadmin
```

3. Backend paketlerini yukle ve API'yi calistir:

```powershell
dotnet restore PVCAtolyeYonetimSistemi.slnx
dotnet run --project src/PVCAtolye.Api/PVCAtolye.Api.csproj
```

4. Frontend paketlerini yukle ve web uygulamasini calistir:

```powershell
cd web
npm.cmd install
npm.cmd run dev
```

## Lokal URL'ler

- API: `http://localhost:5080`
- Swagger: `http://localhost:5080/swagger`
- Health: `http://localhost:5080/health`
- Frontend: `http://localhost:5173`
- pgAdmin: `http://localhost:5050`

## Faz 1 kapsami disinda olanlar

- Kimlik dogrulama ve authorization
- Musteri, teklif, siparis, stok, uretim, montaj, tahsilat ve servis modulleri
- Migration ve seed verileri
- Windows Service installer
- Inno Setup kurulum paketi



## Development admin

Faz 2 development seed kullanicisi:

```text
username: admin
password: Admin123!
```

API development ortaminda acildiginda migration uygulanir ve admin kullanicisi seed edilir.

## Port notu

Bilgisayarda 5432 portu baska bir PostgreSQL tarafindan kullaniliyorsa lokal `.env` dosyasinda `PVC_ATOLYE_POSTGRES_PORT=5433` kullanin ve API connection string portunu development icin 5433 yapin.
