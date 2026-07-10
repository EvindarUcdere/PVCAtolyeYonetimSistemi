# Faz 4 Kapanis Raporu - Musteri ve Adres Yonetimi

## Faz Amaci

Faz 4'un amaci, teklif, siparis, kesif, montaj ve tahsilat sureclerinde kullanilacak musteri karti altyapisini kurmaktir.

Bu fazda teklif, siparis veya stok islemlerine girilmedi. Odak; musteri ana karti, adresler, yetkili kisiler, listeleme, arama ve yetkili CRUD akisi oldu.

## Yapilanlar

- Musteri domain modeli eklendi.
- Bireysel ve kurumsal musteri tipi eklendi.
- Musteri adresleri eklendi.
- Musteri yetkili kisi/contact bilgileri eklendi.
- Musteri request/response DTO'lari eklendi.
- Musteri validation kurallari eklendi.
- Musteri servis sozlesmesi ve Infrastructure uygulamasi eklendi.
- Musteri API endpointleri eklendi.
- PostgreSQL migration olusturuldu ve yerel veritabanina uygulandi.
- Frontend Musteriler ekrani eklendi.
- Sol menude Musteriler sayfasi route'a baglandi.
- JSON enum serilestirme string formatina alindi.

## Mimari Kararlar

- Entity'ler API response olarak dondurulmedi.
- Controller ince tutuldu; is kurallari servis katmaninda tutuldu.
- Veri erisimi Infrastructure katmaninda kaldi.
- Musteri adres ve yetkili kisi kayitlari musteri alt koleksiyonlari olarak modellendi.
- Fiziksel silme eklenmedi; musteri karti aktif/pasif olarak yonetiliyor.
- Arama PostgreSQL `ILIKE` ile case-insensitive calisir.
- Backend permission policy kullanildi; frontend gizleme tek basina yeterli sayilmadi.

## Veritabani Degisiklikleri

Olusturulan migration:

- `20260710195001_AddCustomers`

Eklenen tablolar:

- `customers`
- `customer_addresses`
- `customer_contacts`

Eklenen indexler:

- Musteri adi
- Telefon
- E-posta
- Vergi no
- Kimlik no
- Adres/contact customer foreign key alanlari

## Guvenlik

- Listeleme ve detay endpointleri `Customers.Read` policy ile korunur.
- Ekleme ve guncelleme endpointleri `Customers.Manage` policy ile korunur.
- Yeni permission kodlari seed mekanizmasina dahil edildi.
- Admin rol yeni yetkileri seed calistiginda otomatik alir.

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
- `/api/customers` uzerinden kurumsal demo musteri olusturuldu.
- Arama ile olusturulan musteri listelendi.
- Detay endpointinde 1 adres ve 1 yetkili kisi dondu.

## Bilinen Eksikler

- Müşteri silme/pasife alma ayri endpoint olarak eklenmedi; guncelleme formunda `IsActive` ile yonetiliyor.
- Müşteri duplikasyon kontrolu henuz vergi no/kimlik no seviyesinde zorunlu unique degil.
- Frontend liste temel seviyededir; ileri filtreleme sonraki fazlarda genisletilebilir.
- Global soft-delete query filter ve otomatik audit interceptor henuz genel hale getirilmedi.

## Siradaki Faz

Faz 5 icin onerilen odak:

- Kesif ve olcu yonetimi
- Musteriye bagli kesif kaydi
- Olcu satirlari
- Mekan/oda bilgileri
- Urun tipi, renk, cam tipi secimleri
- Kesiften teklif hazirlama altyapisina gecis
