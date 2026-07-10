# Faz 2 Kapanis Raporu - Kimlik Dogrulama ve Yetkilendirme

## 1. Yapilanlarin ozeti

- Kullanici, rol, permission, user-role, role-permission ve refresh token domain modelleri eklendi.
- Audit log temel entity'si eklendi.
- EF Core konfigurasyonlari ve `AddIdentityAndAudit` migration'i olusturuldu.
- PBKDF2 tabanli password hashing servisi eklendi.
- JWT access token ureten servis eklendi.
- Login ve mevcut kullanici endpointleri eklendi: `POST /api/auth/login`, `GET /api/auth/me`.
- Permission policy altyapisi eklendi.
- Development ortaminda migration uygulama ve admin seed mekanizmasi eklendi.
- Frontend login ekrani, auth provider, token storage, axios bearer interceptor ve protected route eklendi.
- Dashboard shell'e oturum kullanicisi ve cikis butonu eklendi.

## 2. Olusturulan veya degistirilen dosyalar

- `src/PVCAtolye.Domain/Identity/**`
- `src/PVCAtolye.Domain/Audit/AuditLog.cs`
- `src/PVCAtolye.Application/Common/Security/**`
- `src/PVCAtolye.Application/Identity/**`
- `src/PVCAtolye.Infrastructure/Identity/**`
- `src/PVCAtolye.Infrastructure/Seed/**`
- `src/PVCAtolye.Infrastructure/Persistence/Configurations/**`
- `src/PVCAtolye.Infrastructure/Persistence/Migrations/**`
- `src/PVCAtolye.Api/Authorization/**`
- `src/PVCAtolye.Api/Controllers/AuthController.cs`
- `src/PVCAtolye.Api/Program.cs`
- `src/PVCAtolye.Api/appsettings.json`
- `src/PVCAtolye.Api/appsettings.Development.json`
- `web/src/features/auth/**`
- `web/src/shared/auth/**`
- `web/src/shared/api/apiClient.ts`
- `web/src/shared/layout/AppShell.tsx`
- `web/src/app/AppProviders.tsx`
- `web/src/app/AppRouter.tsx`
- `tests/PVCAtolye.UnitTests/PasswordHasherTests.cs`

## 3. Mimari kararlar

- Login akisi `username + password` olarak baslatildi.
- Varsayilan development admin kullanicisi `admin` olarak seed edilir.
- Development admin sifresi `appsettings.Development.json` icindedir; ana `appsettings.json` production placeholder kullanir.
- Permission yapisi granular code mantigiyla baslatildi: `Users.Read`, `Users.Manage`, `Roles.Read`, `Roles.Manage`, `Settings.Manage`, `Audit.Read`.
- Backend authorization policy'leri permission code uzerinden tanimlandi.
- Access token JWT olarak uretilir ve permission claim'leri token icine yazilir.
- Refresh token entity'si eklendi, ancak refresh endpoint'i bu fazda aktif akisa dahil edilmedi.
- Frontend token'i localStorage'da saklar. Bu yerel ag MVP icin pratik baslangictir; production hardening fazinda CSP, token suresi ve XSS azaltma politikalari ele alinmalidir.

## 4. Veritabani degisiklikleri

Migration: `AddIdentityAndAudit`

Olusan tablolar:

- `users`
- `roles`
- `permissions`
- `user_roles`
- `role_permissions`
- `refresh_tokens`
- `audit_logs`

Indexler:

- `users.normalized_username` unique
- `roles.normalized_name` unique
- `permissions.code` unique
- `refresh_tokens.token_hash` unique
- `audit_logs.occurred_at`
- `audit_logs.user_id`

## 5. Yazilan testler

- `PasswordHasherTests`
  - Dogru sifre hash dogrulamasi basarili olur.
  - Yanlis sifre hash dogrulamasi basarisiz olur.
- Mevcut API response ve smoke testleri korundu.
- Mevcut dashboard render testi auth provider ile calismaya devam ediyor.

## 6. Calistirma adimlari

Backend:

```powershell
docker compose up -d postgres
dotnet restore PVCAtolyeYonetimSistemi.slnx --configfile NuGet.Config --disable-parallel -p:NuGetAudit=false
dotnet run --project src/PVCAtolye.Api/PVCAtolye.Api.csproj
```

Development admin:

```text
username: admin
password: Admin123!
```

Frontend:

```powershell
cd web
npm.cmd install
npm.cmd run dev
```

## 7. Manuel test senaryolari

- API acildiginda development ortaminda migration uygulanmali.
- Admin seed kullanicisi olusmali.
- `POST /api/auth/login` admin bilgileriyle access token donmeli.
- `GET /api/auth/me` bearer token ile kullanici, rol ve permission bilgilerini donmeli.
- Frontend `/login` ekranindan giris yapilmali.
- Giris sonrasi `/` dashboard ekranina yonlenmeli.
- Cikis butonu token'i temizleyip oturumu kapatmali.
- Token yokken `/` route'u `/login` ekranina yonlendirmeli.

## 8. Dogrulama sonuclari

- docker compose up -d postgres: basarili. Not: host 5432 portu dolu oldugu icin proje lokal .env dosyasinda PostgreSQL host portu 5433 yapildi.
- GET /health: basarili, Healthy.
- POST /api/auth/login: basarili, admin access token dondu.
- GET /api/auth/me: basarili, rol ve permission bilgileri dondu.
- GET http://127.0.0.1:5173/login: basarili, frontend login sayfasi servis edildi.

- `dotnet restore`: basarili.
- `dotnet build PVCAtolyeYonetimSistemi.slnx --no-restore`: basarili, 0 uyari, 0 hata.
- `dotnet test PVCAtolyeYonetimSistemi.slnx --no-restore --no-build`: basarili, 4 test gecti.
- `npm.cmd install`: basarili.
- `npm.cmd run lint`: basarili.
- `npm.cmd run build`: basarili.
- `npm.cmd run test -- --run`: basarili, 1 test gecti.
- `npm.cmd audit --audit-level=moderate`: basarili, 0 vulnerability.

## 9. Bilinen eksikler

- PostgreSQL container 5433 host portu ile calistirildi; migration, admin seed, health, login ve me endpoint runtime testleri basarili oldu.
- Refresh token entity'si var, ancak refresh endpoint'i henuz aktif degil.
- Kullanici/rol yonetimi CRUD ekranlari bu fazda eklenmedi.
- Ilk giriste sifre degistirme zorunlulugu modelde var, fakat UI akisi henuz yok.
- Frontend build Ant Design nedeniyle buyuk JS chunk uretmeye devam ediyor; route bazli code splitting ileride uygulanmali.

## 10. Sonraki faz plani

Faz 3'te isletme ayarlari ve temel tanimlar ele alinacak:

- Isletme bilgileri
- Depolar
- Birimler
- Malzeme kategorileri
- Urun tipleri
- Renkler
- Cam tipleri
- Profil serileri
- Aksesuar turleri
- Uretim asamalari
- Odeme yontemleri
- Teklif ve siparis numara ayarlari

Faz 3'e gecmeden once Docker Desktop baslatilip Faz 2 runtime login testi tamamlanmalidir.

