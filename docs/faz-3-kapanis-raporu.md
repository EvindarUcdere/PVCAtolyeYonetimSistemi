# Faz 3 Kapanis Raporu - Ayarlar ve Ana Tanimlar

## Faz Amaci

Faz 3'un amaci, musteri, stok, teklif, siparis ve uretim modullerinden once sistemin ortak kullanacagi temel ayar ve ana tanim altyapisini kurmaktir.

Bu fazda islem modulu gelistirilmedi. Odak; isletme profili, depo tanimlari, urun/malzeme tanimlari, uretim asamalari ve belge numaralandirma ayarlaridir.

## Yapilanlar

- Isletme profili modeli ve API endpointleri eklendi.
- Depo tanimlari icin listeleme, ekleme ve guncelleme endpointleri eklendi.
- Ana tanim altyapisi eklendi:
  - Olcu birimleri
  - Malzeme kategorileri
  - Urun tipleri
  - Renkler
  - Cam tipleri
  - Profil serileri
  - Aksesuar tipleri
  - Uretim asamalari
  - Odeme yontemleri
- Teklif ve siparis icin numara serisi ayarlari eklendi.
- PostgreSQL migration olusturuldu ve yerel veritabanina uygulandi.
- Seed verileri eklendi.
- Ayarlar endpointleri JWT ve permission policy ile korundu.
- Frontend Ayarlar ekrani eklendi.
- Dashboard Faz 3 durumuna gore guncellendi.

## Mimari Kararlar

- Ana tanimlar domain entity olarak tutuldu.
- Entity'ler API response olarak dondurulmedi; request/response DTO'lari kullanildi.
- Controller ince tutuldu; is kurallari `ISettingsService` uygulamasinda toplandi.
- Veri erisimi Infrastructure katmaninda kaldi.
- Tanim kodlari ve depo kodlari icin veritabaninda unique index kullanildi.
- Depo silme yerine aktif/pasif alanlari kullanildi.
- Yetkilendirme frontend ile sinirli tutulmadi; backend policy zorunlu kilindi.

## Veritabani Degisiklikleri

Olusturulan migration:

- `20260710190022_AddSettingsDefinitions`

Eklenen tablolar:

- `company_profiles`
- `warehouses`
- `unit_of_measures`
- `material_categories`
- `product_types`
- `color_definitions`
- `glass_types`
- `profile_series`
- `accessory_types`
- `production_stages`
- `payment_methods`
- `number_sequence_settings`

## Guvenlik

- Ayarlar okuma endpointleri `Settings.Read` policy ile korunur.
- Ayarlar guncelleme endpointleri `Settings.Manage` policy ile korunur.
- Admin rol seed mekanizmasi yeni permission kodlarini otomatik alir.
- Frontend route korumasi devam eder, fakat kritik kontroller backend tarafindadir.

## Test ve Dogrulama

Calistirilan komutlar:

- `dotnet build PVCAtolyeYonetimSistemi.slnx`
- `dotnet test PVCAtolyeYonetimSistemi.slnx --no-build`
- `npm.cmd run lint`
- `npm.cmd run test -- --run`
- `npm.cmd run build`
- `npm.cmd audit --audit-level=moderate`
- `dotnet ef database update --project src\PVCAtolye.Infrastructure\PVCAtolye.Infrastructure.csproj --startup-project src\PVCAtolye.Api\PVCAtolye.Api.csproj`

Runtime kontrolu:

- `/health` saglikli dondu.
- `admin / Admin123!` ile login basarili oldu.
- `/api/settings/company-profile` `PVC Atolye` dondu.
- `/api/settings/warehouses` 1 depo dondu.
- `/api/settings/definitions/units` 7 olcu birimi dondu.
- `/api/settings/number-sequences` 2 numara serisi dondu.

## Bilinen Eksikler

- Soft-delete davranisi henuz global query filter olarak uygulanmadi.
- Audit log davranisi henuz otomatik interceptor ile genellestirilmedi.
- Ayarlar ekrani temel CRUD seviyesindedir; ileri arama/filtreleme sonraki fazlarda genisletilebilir.
- Frontend bundle boyutu Ant Design nedeniyle 500 KB uyarisi veriyor; bu ilk surum icin kabul edildi.

## Siradaki Faz

Faz 4 icin onerilen odak:

- Musteri ve adres yonetimi
- Yetkili kisi/contact bilgileri
- Musteri notlari
- Musteri listeleme, arama, ekleme, guncelleme
- Backend validation ve yetki kontrolleri
- Frontend feature-based musteri ekrani
