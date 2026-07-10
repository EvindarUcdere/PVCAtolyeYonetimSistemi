# PVC Atolye Yonetim Sistemi

PVC kapi, pencere ve dograma atolyeleri icin yerel agda calisabilen, musteri-kesif-teklif-siparis-stok-uretim-montaj-tahsilat-servis akislarini yoneten ticari uygulama.

## Durum

Proje baslangic asamasindadir. Faz 0 analiz dokumani hazirlandi. Faz 1'e gecilmeden once ayri klasor ve Git/GitHub hazirligi yapilmaktadir.

## Ana teknoloji yigini

- Backend: .NET 8 Web API, C#, EF Core, PostgreSQL, FluentValidation, JWT, Serilog, Swagger/OpenAPI, SignalR
- Frontend: React, TypeScript, Vite, React Router, TanStack Query, React Hook Form, Zod, Axios, Ant Design
- Test: xUnit, FluentAssertions, Moq, Testcontainers, React Testing Library, Vitest
- Raporlama: QuestPDF, ClosedXML
- Ortam: Docker Compose, Git, GitHub, PostgreSQL

## Mimari

Moduler monolith, katmanli mimari: Domain, Application, Infrastructure, API ve feature-based Frontend.
