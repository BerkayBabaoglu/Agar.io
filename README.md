# Agar.io Clone

🦠 **Agar.io Clone** — A fan-made clone of the popular browser-based arcade game *Agar.io*. This project was built for learning and portfolio purposes, recreating the core gameplay mechanics using Unity.

## 🎮 Gameplay

You control a small cell that must consume smaller pellets to grow while avoiding larger enemies. AI-controlled enemies roam the map, behaving like real players — they may chase or flee based on size.

- Realistic enemy AI that mimics player behavior  
- Dynamic growth system where movement slows as the player grows  
- Smooth scaling animations using **DoTween**  
- Simple yet addictive arcade-style gameplay  
- Unlockable skins through a **shop system**
- Touch joystick support for mobile devices

## 🛠️ Features

- Developed with Unity Engine
- **New Unity Input System** with on-screen joystick for mobile
- Smart enemy AI with real-time decision making (chase/flee)
- Growth mechanics for player and enemies
- Smooth animations using DoTween
- Mobile-friendly UI (score display, shop, skin selector)
- Skin unlock and persistent save system
- Restart screen upon player death

## 🚀 How to Run

1. Download Unity 6.0 (6000.0.48f1) or newer.
2. Open the project via Unity Hub.
3. Open the scene located at `Scenes/SampleScene.unity`.
4. Press **Play** in the Editor or build to Android/iOS for mobile testing.

> 🛠️ For mobile, joystick input is enabled by default.  
> For desktop testing, mouse input is also supported.

## 🎮 Controls

| Input Type           | Action                          |
|----------------------|---------------------------------|
| Mouse / Touch Drag   | Move the cell                   |
| On-Screen Joystick   | Move the cell (mobile only)     |
| Mouse Click / Tap    | Select skin in the shop (menu)  |

## 🧠 Enemy AI Behavior

- Enemies don’t move randomly; they **make decisions based on their size compared to yours**
- If smaller than the player → **they flee**
- If larger than the player → **they chase**
- They grow over time just like the player

## 📦 Tools & Assets Used

- [DoTween (Demigiant)](http://dotween.demigiant.com/) — For tween-based smooth animations
- **Unity New Input System** — For touch and joystick support
- Free sound effects and background music
- Custom-made or free-to-use visual assets

## 🛒 Shop System

- Unlock new skins using collected points
- Skins are applied visually in-game
- Selections are saved locally using `PlayerPrefs`

## 👤 Developer

**Berkay Babaoğlu**  
You can reach me here: [LinkedIn](https://www.linkedin.com/in/berkaybabaoglu01/)

## 📸 Screenshots
<img width="925" height="518" alt="Ekran görüntüsü 2025-08-07 130217" src="https://github.com/user-attachments/assets/babf0364-35d0-4c8e-9133-ddfe85649e6f" />

## ⚠️ Disclaimer

> This project is a **fan-made clone for educational and portfolio purposes only**. It is not affiliated with the official *Agar.io* game. All assets used are either original or free-to-use under proper licenses.
