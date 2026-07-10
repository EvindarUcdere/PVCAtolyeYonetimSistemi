# PVC Atolye Yonetim Sistemi - Faz 0 Analizi

Bu dokuman Faz 0 kapsamindadir. Kod, backend, frontend, migration veya proje iskeleti olusturulmamistir. Amac; ticari urunun kapsamini, MVP sinirlarini, ana is kurallarini, teknik yaklasimi, riskleri ve sonraki faz planini netlestirmektir.

## 1. Urunun problemi ve cozumu

PVC kapi, pencere ve dograma atolyelerinde isler genellikle telefon, WhatsApp, kagit olcu formlari, Excel tablolari ve muhasebe/stok programlari arasinda daginik yurutulur. Bu durum teklif revizyonlarinin kaybolmasina, siparisin hangi asamada oldugunun gorulememesine, eksik malzemenin gec fark edilmesine, montaj planinin karismasina ve tahsilat takibinin zayiflamasina neden olur.

PVC Atolye Yonetim Sistemi bu daginik sureci tek bir yerel sistemde toplar. Musteri kaydindan kesif ve olcuye, tekliften siparise, stoktan uretime, montajdan tahsilata ve servis kaydina kadar izlenebilir bir is akisi sunar. Ilk surum bulut zorunlulugu olmadan yerel bilgisayar veya yerel ag uzerinde calisir; ileride bulut yedekleme ve cok subeli kullanim eklenebilecek sekilde tasarlanir.

## 2. Hedef kullanicilar

- Isletme sahibi: genel durum, finans, stok, yetki ve raporlari izler.
- Yonetici: operasyon, uretim, montaj, personel ve onay sureclerini yonetir.
- Satis ve teklif personeli: musteri, kesif, teklif, revizyon ve teklif onay sureclerini yurutur.
- Olcu alan saha personeli: kesif planini gorur, olcu ve fotograf girer.
- Depo sorumlusu: malzeme kartlari, stok hareketleri, sayim ve rezervasyonlari yonetir.
- Uretim personeli: uretim emirleri, asamalar, fire ve tamamlanan isleri kaydeder.
- Montaj personeli: montaj planini, adresi, teslim onayini ve fotograf kayitlarini girer.
- Muhasebe/tahsilat personeli: odeme, kalan borc, vade ve makbuz kayitlarini takip eder.

## 3. Kullanici rolleri

Baslangic rolleri:

- Isletme Sahibi
- Yonetici
- Satis Personeli
- Saha/Olcu Personeli
- Depo Sorumlusu
- Uretim Personeli
- Montaj Personeli
- Muhasebe Personeli

Roller sabit kodlanmamalidir. Seed olarak olusturulabilir, ancak sistemde rol ve permission yonetimi bulunmalidir. Kritik islemler permission bazinda kontrol edilmelidir.

## 4. Rol-yetki matrisi

| Modul / Islem | Sahip | Yonetici | Satis | Olcu | Depo | Uretim | Montaj | Muhasebe |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| Dashboard goruntuleme | Tam | Tam | Kisitli | Kisitli | Kisitli | Kisitli | Kisitli | Finans agirlikli |
| Kullanici/rol yonetimi | Tam | Kisitli | Yok | Yok | Yok | Yok | Yok | Yok |
| Isletme ayarlari | Tam | Kisitli | Yok | Yok | Yok | Yok | Yok | Kisitli |
| Musteri CRUD | Tam | Tam | Tam | Okuma | Okuma | Yok | Okuma | Okuma |
| Kesif planlama | Tam | Tam | Tam | Okuma | Yok | Yok | Yok | Yok |
| Olcu girisi | Tam | Tam | Tam | Tam | Yok | Yok | Yok | Yok |
| Teklif olusturma | Tam | Tam | Tam | Yok | Yok | Yok | Yok | Okuma |
| Teklif onay/red | Tam | Tam | Yetkiliyse | Yok | Yok | Yok | Yok | Yok |
| Siparis olusturma | Tam | Tam | Tam | Yok | Okuma | Okuma | Okuma | Okuma |
| Siparis durum degistirme | Tam | Tam | Kisitli | Yok | Malzeme adimlari | Uretim adimlari | Montaj adimlari | Yok |
| Malzeme kartlari | Tam | Tam | Okuma | Yok | Tam | Okuma | Okuma | Okuma |
| Stok hareketi | Tam | Tam | Yok | Yok | Tam | Tuketim | Yok | Okuma |
| Uretim emri | Tam | Tam | Okuma | Yok | Okuma | Tam | Okuma | Yok |
| Montaj planlama | Tam | Tam | Okuma | Yok | Okuma | Okuma | Tam | Yok |
| Odeme/tahsilat | Tam | Tam | Okuma | Yok | Yok | Yok | Yok | Tam |
| Servis kaydi | Tam | Tam | Tam | Kisitli | Kisitli | Kisitli | Kisitli | Okuma |
| Raporlar | Tam | Tam | Kisitli | Yok | Stok | Uretim | Montaj | Finans |
| Audit log | Tam | Kisitli | Yok | Yok | Yok | Yok | Yok | Yok |

Not: Matris Faz 0 on kabuludur. Faz 2'de permission listesi endpoint ve UI islem seviyesinde detaylandirilmalidir.

## 5. MVP kapsami

MVP, atolyenin gunluk operasyonunu uctan uca izleyebilecegi ama ileri optimizasyon ve entegrasyonlari icermeyen surum olmalidir.

- Kimlik dogrulama, rol ve permission tabanli yetkilendirme
- Isletme ayarlari ve temel tanimlar
- Musteri yonetimi
- Kesif ve olcu kayitlari
- Teklif olusturma, revizyon, PDF/yazdirma icin altyapi
- Onayli tekliften siparis olusturma
- Siparis durum takibi ve durum gecis kurallari
- Malzeme kartlari, depo, stok bakiyesi ve stok hareketleri
- Basit recete/malzeme ihtiyaci hesaplama
- Eksik malzeme listesi
- Uretim emri ve temel uretim asama takibi
- Montaj planlama ve teslim kaydi
- Temel tahsilat ve kalan borc takibi
- Servis/garanti kaydi
- Dashboard ve temel raporlar
- Uygulama ici bildirimler
- Yerel kurulum, yedekleme ve geri yukleme

## 6. MVP disi kapsam

- Cok subeli merkezi bulut kullanim
- Bulut yedekleme aboneligi
- SMS, e-posta ve mobil push bildirimleri
- Gelismis kesim optimizasyonu
- Barkod okuyucu ve etiket yazici entegrasyonu
- Tam kapsamli muhasebe, e-fatura, e-arsiv entegrasyonu
- Banka/POS entegrasyonu
- Tedarikci portali veya musteri portali
- Harita optimizasyonlu montaj rota planlama
- IoT/makine entegrasyonu
- Cok para birimli finansal muhasebe
- Gelismis BI raporlama

## 7. Ana moduller

- Kimlik ve yetkilendirme
- Isletme ayarlari ve temel tanimlar
- Musteri yonetimi
- Kesif ve olcu yonetimi
- Teklif yonetimi
- Siparis yonetimi
- Malzeme, depo ve stok yonetimi
- Recete ve malzeme ihtiyaci
- Tedarikci ve satin alma
- Uretim yonetimi
- Montaj yonetimi
- Tahsilat yonetimi
- Servis ve garanti
- Dashboard, rapor ve bildirim
- Yedekleme, geri yukleme ve yerel kurulum
- Audit log ve sistem guvenligi

## 8. Moduller arasindaki iliskiler

Musteri, kesif, teklif, siparis, montaj, odeme ve servis kayitlarinin merkezindedir. Kesif kayitlari teklif kalemlerine kaynak olabilir. Onaylanan teklif siparise donusur ve teklif revizyonlari izlenebilir kalir. Siparis, malzeme ihtiyaci hesaplamasini, stok rezervasyonunu, uretim emrini, montaj planini ve tahsilat kayitlarini tetikler.

Stok modulu malzeme kartlari, depo bakiyeleri ve hareketler uzerinden calisir. Uretim emrinde rezervasyon ve gercek tuketim stok hareketi uretir. Satin alma eksik malzeme listesinden dogabilir ve mal kabul stok girisi olusturur. Odeme modulu siparis toplami, tahsilatlar, iptal/ters kayitlar ve kalan borc uzerinden calisir. Servis kaydi musteriye, siparise ve gerekirse urune baglanir.

## 9. Temel kullanici senaryolari

- Satis personeli yeni musteri acar, kesif talebi olusturur ve saha personeline atar.
- Saha personeli mobil/tablet uyumlu formdan olcu, urun ozellikleri, not ve fotograf girer.
- Satis personeli kesiften teklif olusturur, kalemleri fiyatlandirir, PDF olarak musteriye sunar.
- Musteri teklifi onaylar; sistem teklifi siparise donusturur ve teklifin o anki halini korur.
- Depo sorumlusu siparise gore malzeme ihtiyacini ve eksikleri gorur.
- Yonetici uretim emri olusturur, uretim personeli asamalari tamamlar.
- Montaj ekibi planlanan isleri gorur, montaj sonucunu ve teslim onayini kaydeder.
- Muhasebe personeli on odeme ve kalan tahsilatlari girer, vadesi gecenleri izler.
- Servis personeli garanti kapsamindaki veya ucretli servis talebini takip eder.

## 10. Musteriden servise kadar uctan uca is akisi

```text
Musteri kaydi
  -> Kesif talebi
  -> Olcu ve urun ozellikleri
  -> Teklif taslagi
  -> Teklif revizyonu/gonderimi
  -> Teklif onayi
  -> Siparis
  -> Malzeme ihtiyaci
  -> Stok kontrolu / eksik malzeme
  -> Satin alma veya stok rezervasyonu
  -> Uretim emri
  -> Uretim asamalari
  -> Kalite kontrol
  -> Montaj planlama
  -> Teslimat ve musteri onayi
  -> Tahsilat kapama
  -> Garanti/servis kaydi
```

## 11. Tekliften siparise gecis kurallari

- Sadece Onaylandi durumundaki teklif siparise donusturulebilir.
- Ayni teklif yalnizca bir kez siparise donusturulebilir.
- Suresi dolmus, reddedilmis veya taslak teklifler dogrudan siparise donusturulemez.
- Onaylanan teklifin fiyat, KDV, indirim, kalem ve musteri bilgileri snapshot olarak korunmalidir.
- Onay sonrasi degisiklik gerekiyorsa yeni teklif revizyonu olusturulmalidir.
- Siparis numarasi sistem tarafindan benzersiz uretilmelidir.
- Donusum transaction icinde yapilmalidir: siparis olusamazsa teklif durumu degismemelidir.
- Donusum audit log'a yazilmalidir.

## 12. Siparis durum gecisleri

Onerilen durumlar:

```text
Siparis Alindi
  -> Malzeme Bekliyor
  -> Uretim Planlandi
  -> Uretimde
  -> Kalite Kontrolde
  -> Montaja Hazir
  -> Montaj Planlandi
  -> Montajda
  -> Tamamlandi
```

Yan durumlar:

- Iptal Edildi: yalnizca yetkili kullanici, gerekceyle.
- Ertelendi: teslim/montaj planinda gecici bekleme.
- Revizyon Bekliyor: olcu veya musteri talebi degistiyse.

Kurallar:

- Tamamlandi durumundan once montaj/tahsilat politikasi kontrol edilmelidir.
- Tamamlanmis veya iptal edilmis siparis normal kullanici tarafindan geri alinmamalidir.
- Her gecis permission ve onceki durum kontroluyle backend tarafindan uygulanmalidir.
- Kritik gecisler islem gecmisine ve audit log'a yazilmalidir.

## 13. Stok hareket mantigi

Stok hareketleri silinmemeli, hatalar ters kayitla duzeltilmelidir. Stok bakiyesi hareketlerden turetilen veya hareketlerle senkron tutulan kontrollu bir bakiye tablosunda izlenebilir.

Hareket tipleri:

- Satin Alma Girisi
- Uretim Tuketimi
- Manuel Giris
- Manuel Cikis
- Sayim Fazlasi
- Sayim Eksigi
- Fire
- Iade
- Transfer
- Rezervasyon
- Rezervasyon Iptali

Kurallar:

- Negatif stok varsayilan olarak engellenmelidir.
- Yetkili istisna gerekiyorsa gerekce, onay ve audit log zorunlu olmalidir.
- Ayni stok kalemine es zamanli hareketlerde transaction ve concurrency kontrolu uygulanmalidir.
- Rezervasyon fiili tuketim degildir; stok kullanilabilir miktarini azaltir.
- Uretim tuketimi uretim emriyle iliskili olmalidir.
- Kritik/minimum stok esikleri bildirim uretmelidir.

## 14. Uretim akisi

Uretim siparisten veya siparis kalemlerinden uretim emri olusturularak baslar. Uretim asamalari isletmeye gore yonetilebilir olmalidir, ancak baslangic seed seti sunulabilir.

Onerilen asamalar:

- Malzeme Hazirligi
- Profil Kesim
- Destek Saci
- Kaynak
- Kose Temizleme
- Aksesuar Montaji
- Cam Montaji
- Kalite Kontrol
- Paketleme
- Montaja Hazir

Kurallar:

- Uretim emri siparise bagli olmalidir.
- Planlanan ve gerceklesen malzeme ayrilmalidir.
- Fire kayitlari stok ve raporlamaya yansimalidir.
- Asama gecisleri yetkiye ve siraya gore kontrol edilmelidir.
- Kalite kontrol sonucu yeniden isleme veya montaja hazir sonucunu uretmelidir.

## 15. Odeme ve tahsilat kurallari

Bu modul tam muhasebe degil, siparis bazli alacak ve tahsilat takibidir.

- Her siparisin toplam tutari, tahsil edilen tutari ve kalan bakiyesi izlenmelidir.
- On odeme, ara odeme ve son odeme ayni tahsilat modeliyle kaydedilebilir.
- Odeme yontemi, tarih, kasa/banka ve aciklama zorunlu alanlar olarak dusunulmelidir.
- Tahsilat toplami siparis toplamini yetkisiz asamamali; fazla odeme istisnasi gerekirse ayrica onaylanmalidir.
- Finansal kayitlar fiziksel olarak silinmemelidir.
- Iptal, iade veya duzeltme ters kayitla yapilmalidir.
- Vadesi gecen bakiyeler dashboard ve bildirimlerde gorunmelidir.

## 16. Onerilen teknik mimari

Ana teknoloji yigini korunmalidir:

- Backend: .NET 8 Web API, C#, EF Core, PostgreSQL, FluentValidation, JWT, Serilog, Swagger/OpenAPI
- Frontend: React, TypeScript, Vite, React Router, TanStack Query, React Hook Form, Zod
- Mimari: moduler monolith, katmanli yapi, ince controller, application service odakli is kurallari

Onerilen backend katmanlari:

- Api: controller, middleware, auth policy, Swagger
- Application: use case servisleri, DTO, validation, transaction orkestrasyonu
- Domain: entity, value object, enum, domain rule, durum gecis kurallari
- Infrastructure: EF Core, repository, file storage, backup, logging, auth token servisleri
- Tests: unit, application, integration ve API testleri

Frontend yapisi:

- Feature bazli moduller: customers, surveys, quotes, orders, inventory, production, installation, payments, service, settings
- Ortak UI: tablo, form, modal, badge, layout, notification, empty/error/loading state
- API istemcisi ve TanStack Query hook'lari
- Route guard ve permission bazli menuler

## 17. Yerel ag ve offline calisma yaklasimi

Ilk MVP'de hedef model "yerel sunucu + yerel ag istemcileri" olmalidir. Backend ve PostgreSQL isletmenin ana bilgisayarinda veya yerel sunucusunda calisir. Ayni agdaki diger bilgisayarlar tarayici uzerinden backend'in sundugu web uygulamasina erisir.

Internet kesintisinde:

- Yerel ag ve ana bilgisayar calisiyorsa uygulama temel islevlerini surdurur.
- Dis servis bagimliligi olmadigi icin musteri, teklif, siparis, stok, uretim ve tahsilat kaydi devam eder.
- Bulut yedekleme, SMS veya e-posta gibi opsiyonel servisler devre disi kalabilir.

Tam istemci offline modu MVP icin zorunlu olmamalidir. Tablet/telefon ile yerel agda kullanim hedeflenmelidir. Ileride saha personeli icin offline olcu toplama gerekiyorsa mobil/PWA senkronizasyon kuyrugu tasarlanabilir.

## 18. Yedekleme ve geri yukleme yaklasimi

- PostgreSQL icin pg_dump tabanli manuel yedekleme.
- Zamanlanmis otomatik yedekleme: gunluk/haftalik plan.
- Yedek dosyasi uygulama disinda secilen klasore yazilabilmeli.
- Yedek gecmisi, boyut, tarih, alan kullanici ve sonuc bilgisiyle tutulmali.
- Geri yukleme oncesi mevcut verinin otomatik emniyet yedegi alinmali.
- Yedek dosyasi versiyon bilgisi ve basit butunluk kontrolu icermeli.
- Dosya ekleri veritabanindan ayri tutuluyorsa yedek paketi veritabani + uploads klasorunu birlikte kapsamalidir.
- Bulut yedekleme ileride opsiyonel adaptor olarak eklenmelidir.

## 19. Guvenlik yaklasimi

- Sifreler guclu hash algoritmasiyla saklanmali; duz metin tutulmamalidir.
- JWT access token ve gerekirse refresh token kullanilmalidir.
- Backend permission policy zorunlu olmalidir; frontend menu gizleme guvenlik kabul edilmemelidir.
- Kritik islemler audit log'a yazilmalidir.
- Validation hem backend hem frontend tarafinda uygulanmalidir.
- Hassas bilgiler loglanmamalidir.
- Dosya yuklemelerinde uzanti, boyut ve MIME kontrolu yapilmalidir.
- Varsayilan admin sifresi ilk kurulumda degistirilmeden sistem kullanima acilmamalidir.
- Yerel agda HTTPS opsiyonel ama onerilen kurulum hedefi olmalidir; en azindan token ve secret degerleri config dosyalarinda acikca korunmalidir.
- Production secret degerleri repository'ye eklenmemelidir.

## 20. Teknik ve ticari riskler

Teknik riskler:

- PVC recete ve malzeme ihtiyaci hesaplari isletmeden isletmeye degisebilir.
- Stok concurrency hatalari negatif stok ve finansal tutarsizlik yaratabilir.
- Yerel kurulum teknik olmayan kullanici icin zor olabilir.
- PostgreSQL yedek/geri yukleme hatalari veri kaybi riski tasir.
- Dosya ekleri ve veritabani yedeklerinin birlikte yonetilmemesi eksik geri yukleme yaratabilir.
- PDF teklif formatinin ticari beklentileri karsilamasi zaman alabilir.

Ticari riskler:

- Isletmelerin mevcut aliskanliklarini degistirmesi zor olabilir.
- Tek seferlik lisans modeli guncelleme ve destek maliyetini karsilamayabilir.
- Her atolye farkli fiyatlandirma ve uretim kurali isteyebilir.
- Cok kapsamli modul beklentisi MVP teslimini geciktirebilir.
- Kurulum, destek ve veri aktarimi satis surecinin kritik parcasi olabilir.

## 21. Kabul kriterleri

Faz 0 kabul kriterleri:

- MVP ve MVP disi kapsam ayrildi.
- Kullanici rolleri ve baslangic yetki matrisi tanimlandi.
- Ana moduller ve modul iliskileri aciklandi.
- Musteriden servise uctan uca akis cikarildi.
- Tekliften siparise, siparis durumlarina, stok hareketlerine, uretime ve tahsilata ait temel kurallar belirlendi.
- Yerel ag, offline calisma, yedekleme ve guvenlik yaklasimi netlestirildi.
- Teknik/ticari riskler ve kritik kararlar listelendi.
- Fazlara bolunmus gelistirme plani sunuldu.

MVP kabul kriterleri:

- Kullanici yetkileri backend tarafindan uygulanir.
- Onaylanmamis teklif siparise donusmez.
- Ayni teklif iki kez siparise donusmez.
- Kritik stok ve minimum stok gorunur olur.
- Negatif stok varsayilan olarak engellenir.
- Uretim ve montaj durumlari izlenebilir.
- Tahsilat ve kalan borc siparis bazinda dogru hesaplanir.
- Kritik kayitlar silinmez; ters kayit veya soft delete uygulanir.
- Yedekleme ve geri yukleme temel seviyede calisir.

## 22. Fazlara ayrilmis kesin gelistirme plani

1. Faz 0 - Analiz ve kapsam: bu dokuman.
2. Faz 1 - Proje altyapisi: .NET solution, React app, Docker Compose, PostgreSQL, logging, error handling, Swagger, health check, test altyapisi.
3. Faz 2 - Kimlik dogrulama ve yetkilendirme: user, role, permission, JWT, admin seed, login UI, protected route, audit temeli.
4. Faz 3 - Isletme ayarlari ve temel tanimlar: firma bilgileri, depolar, birimler, malzeme kategorileri, urun tipleri, uretim asamalari, numara formatlari.
5. Faz 4 - Musteri yonetimi: bireysel/kurumsal musteri, adres, not, arama, filtre, detay, validation, audit.
6. Faz 5 - Kesif ve olcu yonetimi: kesif planlama, olcu formu, urun ozellikleri, belge/fotograf, revizyon, teklife aktarim.
7. Faz 6 - Teklif yonetimi: teklif kalemleri, fiyatlama, revizyon, durum, PDF/yazdirma, onay/red, testler.
8. Faz 7 - Siparis yonetimi: tekliften siparis, manuel siparis, durum gecisleri, islem gecmisi, transaction.
9. Faz 8 - Malzeme ve stok yonetimi: malzeme karti, depo bakiyesi, hareketler, sayim, fire, iade, rezervasyon, concurrency.
10. Faz 9 - Recete ve malzeme ihtiyaci: kural yapisi, olcuye gore ihtiyac, stok karsilastirma, eksik malzeme listesi.
11. Faz 10 - Tedarikci ve satin alma: tedarikci, talep, siparis, mal kabul, stok entegrasyonu.
12. Faz 11 - Uretim yonetimi: uretim emri, asamalar, personel, malzeme tuketimi, fire, kalite kontrol.
13. Faz 12 - Montaj yonetimi: montaj plani, ekip, durumlar, teslim onayi, mobil uyum.
14. Faz 13 - Tahsilat yonetimi: odeme planlari, tahsilat, kalan borc, vade, ters kayit, makbuz.
15. Faz 14 - Servis ve garanti: servis talebi, garanti, personel, malzeme, sonuc, musteri onayi.
16. Faz 15 - Dashboard, rapor ve bildirim: KPI, kritik stok, geciken siparis, montaj, tahsilat, export, uygulama ici bildirim.
17. Faz 16 - Yedekleme, geri yukleme ve yerel kurulum: pg_dump, restore, Windows service, kurulum sihirbazi, dokuman.
18. Faz 17 - Guvenlik ve kalite iyilestirmeleri: yetki testleri, dosya guvenligi, performans, index, N+1, UX ve erisilebilirlik.
19. Faz 18 - Pilot surum ve yayin hazirligi: demo veri, kabul testleri, kurulum paketi, kullanici dokumani, surum notlari.

## 23. Acikliga kavusturulmasi gereken kritik kararlar

- Fiyatlandirma modeli nasil olacak: m2, metre, urun tipi, manuel birim fiyat veya karma model mi?
- Teklif PDF tasarimi icin standart bir sablon yeterli mi, yoksa firma bazli ozellestirme gerekli mi?
- Ilk MVP'de dosya/fotograf ekleri yerel disk klasorunde mi, veritabaninda mi tutulacak?
- Yerel kurulumda PostgreSQL uygulama paketine dahil mi olacak, yoksa ayri kurulum mu yapilacak?
- Ayni anda kac kullanici hedefleniyor ve tipik veri hacmi nedir?
- Negatif stok istisnasi tamamen yasak mi, yoksa sahip/yonetici onayi ile mumkun mu?
- Tahsilat modulu cari hesap mantigina genisletilecek mi, yoksa sadece siparis bazli mi kalacak?
- Uretim asamalari her isletme icin serbest siralanabilir mi, yoksa bazi asamalar sistem tarafindan zorunlu mu olacak?
- Garanti suresi urun tipi bazinda mi, siparis bazinda mi tanimlanacak?
- Lisanslama online aktivasyon mu, offline lisans dosyasi mi, yoksa ikisinin birlikte calistigi hibrit model mi olacak?

## Faz 0 sonucu

Faz 0 tamamlandi. Bu noktada kod yazilmamali ve Faz 1'e gecilmemelidir. Sonraki adim icin acik onay cumlesi gereklidir: "Faz 1'e gec".
