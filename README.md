# 🎬 MovieApp.v2 - Gelişmiş Test Altyapılı Yeni Sürüm

> Bu proje, daha önce geliştirilmiş olan **MovieApp** uygulamasından bağımsız, **sıfırdan yapılandırılmış yeni bir uygulamadır**. Test altyapısı, otomasyon, entegrasyon ve kullanıcı arayüzü testleri dahil olmak üzere uçtan uca yazılım testi süreçlerini kapsamaktadır.

---

## ✨ Yeni Özellikler

- ✅ Repository ve Controller katmanları için **birim testler**
- ✅ Frontend etkileşimleri için **Selenium UI testleri**
- ✅ API uç noktaları için **entegrasyon testleri**
- ✅ API testleri için **Postman koleksiyonu**
- ✅ Tüm testleri otomatik çalıştıran **PowerShell scripti**

---

## 🧪 Test Kapsamı

| Katman / Alan       | Kapsam                                                         |
|---------------------|----------------------------------------------------------------|
| Repository Katmanı  | CRUD işlemleri, veri kalıcılığı testleri                      |
| Controller Katmanı  | Action metodları, model doğrulama                             |
| Kimlik Doğrulama    | Kullanıcı kayıt & giriş süreçleri                             |
| Yetkilendirme       | Role dayalı erişim kontrolü                                   |
| UI (Kullanıcı Arayüzü) | Sayfa geçişleri, form gönderimi kontrolleri                  |
| API                 | Uçtan uca işlevsellik testleri                                |

---

## 🛠️ Teknik Detaylar

- Test çerçevesi olarak **xUnit**
- Mocking işlemleri için **Moq**
- Doğrulamalarda **FluentAssertions**
- Testler için **In-Memory Database**
- UI testleri için **Selenium WebDriver**
- API testleri için kapsamlı **Postman koleksiyonu**

---

## 📚 Dokümantasyon

- 📖 `README.md`: Kurulum ve test çalıştırma adımlarını içerir
- ▶️ `RunTests.ps1`: Tüm testleri kategorilere göre çalıştıran PowerShell betiği
- 📁 `Postman Collection`: API uç noktalarını test etmek için kullanılabilir
- 🧾 Proje yapısı ve ön koşullar belgelenmiştir

---

## 🔧 Yapılandırma

- Test kategorileri:
  - `Unit` (Birim Test)
  - `Integration` (Entegrasyon Testi)
  - `UI` (Kullanıcı Arayüzü Testi)
- PowerShell script ile:
  - Test kategorisine göre filtreleme
  - Otomatik test çalıştırma
- NuGet üzerinden gerekli test paketleri yüklüdür

---

## 🚀 Kurulum ve Çalıştırma

Projeyi klonlayın:
   ```bash
   git clone https://github.com/kullaniciAdi/MovieApp.v2.git

### Gerekli NuGet paketlerini yükleyin:
```bash
dotnet restore
---
### Testleri çalıştırmak için:
./RunTests.ps1
---
### UI testleri için:
Selenium WebDriver'ın sisteminizde yüklü ve tarayıcı sürümünüzle uyumlu olduğundan emin olun.
---
## 📌 Not
Bu proje, önceki MovieApp uygulamasından farklıdır. Sıfırdan geliştirilmiş yeni bir uygulamadır ve test odaklı yazılım geliştirme süreçlerini örneklemek amacıyla oluşturulmuştur.
---

## 🧩 Geliştirici Araçları
.NET SDK 7.0+
Visual Studio / VS Code
Postman
Selenium WebDriver



