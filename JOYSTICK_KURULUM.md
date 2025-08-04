# Joystick Sistemi Kurulum Rehberi

## Yapılan Değişiklikler

1. **JoystickController.cs** - Singleton pattern eklendi
2. **CharacterMovement.cs** - Unity Remote ile ekrana dokunma sistemi kaldırıldı, sadece joystick kontrolü eklendi
3. **JoystickSetup.cs** - Joystick kurulum scripti eklendi

## Unity'de Kurulum Adımları

### 1. Joystick UI Elementini Oluşturun

1. Hierarchy'de sağ tık → UI → Canvas oluşturun
2. Canvas'a sağ tık → UI → Image (joystick background için)
3. Background image'e sağ tık → UI → Image (joystick handle için)

### 2. JoystickController Scriptini Ekleyin

1. Background image'e `JoystickController` scriptini ekleyin
2. Inspector'da:
   - **Background**: Background image'in RectTransform'ını sürükleyin
   - **Handle**: Handle image'in RectTransform'ını sürükleyin

### 3. JoystickSetup Scriptini Ekleyin

1. Boş bir GameObject oluşturun
2. `JoystickSetup` scriptini ekleyin
3. Inspector'da:
   - **Joystick Controller**: JoystickController'ı sürükleyin
   - **Joystick Canvas**: Canvas'ı sürükleyin

### 4. Canvas Ayarları

Canvas'ın Inspector'ında:
- **Render Mode**: Screen Space - Overlay
- **Sorting Order**: 100 (en üstte görünmesi için)

### 5. Joystick Pozisyonu

Joystick'i ekranın sol alt köşesine yerleştirin:
- Anchor: Bottom Left
- Position: (100, 100, 0)

## Test Etme

1. Unity Remote'u mobil cihazınızda açın
2. Unity'de Play tuşuna basın
3. Joystick'e dokunup sürükleyin
4. Karakter joystick yönünde hareket etmelidir

## Sorun Giderme

### Joystick Çalışmıyor
- JoystickController'ın singleton olarak çalıştığından emin olun
- Canvas'ın Screen Space - Overlay modunda olduğunu kontrol edin
- Joystick UI elementlerinin aktif olduğunu kontrol edin

### Karakter Hareket Etmiyor
- CharacterMovement scriptinde JoystickController.Instance null kontrolü yapıldığını kontrol edin
- Console'da hata mesajları olup olmadığını kontrol edin

### Unity Remote'da Çalışmıyor
- Unity Remote'un güncel olduğundan emin olun
- Mobil cihazda Unity Remote'u yeniden başlatın
- Unity'de Build Settings'den platform ayarlarını kontrol edin

## Özellikler

- ✅ Sadece joystick ile kontrol
- ✅ Unity Remote ile ekrana dokunma sistemi kaldırıldı
- ✅ Singleton pattern ile güvenli erişim
- ✅ Mobil optimizasyonu
- ✅ Debug logları ile test kolaylığı 