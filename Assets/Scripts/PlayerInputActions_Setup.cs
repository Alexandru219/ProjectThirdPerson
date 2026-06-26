// ╔══════════════════════════════════════════════════════════════════════════╗
// ║         PLAYER INPUT ACTIONS — CONFIGURARE .inputactions               ║
// ║                                                                          ║
// ║  Creează un fișier PlayerInputActions.inputactions în Unity și           ║
// ║  configurează-l exact așa cum este descris mai jos.                      ║
// ║  Sau importă JSON-ul de la sfârșitul fișierului.                         ║
// ╚══════════════════════════════════════════════════════════════════════════╝
//
// ┌─────────────────────────────────────────────────────────────────────────┐
// │  ACTION MAP: Gameplay                                                    │
// ├──────────────┬──────────────┬──────────────────────────────────────────┤
// │  Action      │  Type        │  Binding                                  │
// ├──────────────┼──────────────┼──────────────────────────────────────────┤
// │  Move        │  Value/Vec2  │  WASD / Left Stick (Gamepad)              │
// │  Look        │  Value/Vec2  │  Mouse Delta / Right Stick (Gamepad)      │
// │  Jump        │  Button      │  Space / South Button (Gamepad)           │
// │  Sprint      │  Button      │  Left Shift / Left Stick Press (Gamepad)  │
// │  Fire        │  Button      │  Left Mouse Button / Right Trigger        │
// │  OpenMenu    │  Button      │  Escape / Start (Gamepad)                 │
// ├─────────────────────────────────────────────────────────────────────────┤
// │  ACTION MAP: UI                                                          │
// ├──────────────┬──────────────┬──────────────────────────────────────────┤
// │  Navigate    │  Value/Vec2  │  Arrow Keys / Left Stick (Gamepad)        │
// │  Submit      │  Button      │  Enter / South Button (Gamepad)           │
// │  Cancel      │  Button      │  Escape / East Button (Gamepad)           │
// └─────────────────────────────────────────────────────────────────────────┘

// ══════════════════════════════════════════════════════════════════════════════
//  JSON PENTRU .inputactions  (copiază conținutul în fișierul .inputactions)
// ══════════════════════════════════════════════════════════════════════════════

/*
{
    "name": "PlayerInputActions",
    "maps": [
        {
            "name": "Gameplay",
            "id": "gameplay-map-guid",
            "actions": [
                {
                    "name": "Move",
                    "type": "Value",
                    "id": "move-action-guid",
                    "expectedControlType": "Vector2",
                    "bindings": [
                        {
                            "name": "WASD",
                            "id": "wasd-binding-guid",
                            "path": "2DVector",
                            "interactions": "",
                            "processors": "",
                            "groups": "",
                            "action": "Move",
                            "isComposite": true,
                            "isPartOfComposite": false
                        },
                        { "name": "up",    "path": "<Keyboard>/w",          "isPartOfComposite": true },
                        { "name": "down",  "path": "<Keyboard>/s",          "isPartOfComposite": true },
                        { "name": "left",  "path": "<Keyboard>/a",          "isPartOfComposite": true },
                        { "name": "right", "path": "<Keyboard>/d",          "isPartOfComposite": true },
                        { "name": "Gamepad", "path": "<Gamepad>/leftStick", "isPartOfComposite": false }
                    ]
                },
                {
                    "name": "Look",
                    "type": "Value",
                    "expectedControlType": "Vector2",
                    "bindings": [
                        { "path": "<Mouse>/delta",          "processors": "ScaleVector2(x=0.1,y=0.1)" },
                        { "path": "<Gamepad>/rightStick",   "processors": "ScaleVector2(x=5,y=5)" }
                    ]
                },
                {
                    "name": "Jump",
                    "type": "Button",
                    "bindings": [
                        { "path": "<Keyboard>/space" },
                        { "path": "<Gamepad>/buttonSouth" }
                    ]
                },
                {
                    "name": "Sprint",
                    "type": "Button",
                    "bindings": [
                        { "path": "<Keyboard>/leftShift" },
                        { "path": "<Gamepad>/leftStickPress" }
                    ]
                },
                {
                    "name": "Fire",
                    "type": "Button",
                    "bindings": [
                        { "path": "<Mouse>/leftButton" },
                        { "path": "<Gamepad>/rightTrigger" }
                    ]
                },
                {
                    "name": "OpenMenu",
                    "type": "Button",
                    "bindings": [
                        { "path": "<Keyboard>/escape" },
                        { "path": "<Gamepad>/start" }
                    ]
                }
            ]
        },
        {
            "name": "UI",
            "id": "ui-map-guid",
            "actions": [
                {
                    "name": "Navigate",
                    "type": "Value",
                    "expectedControlType": "Vector2",
                    "bindings": [
                        {
                            "name": "Arrow Keys",
                            "path": "2DVector",
                            "isComposite": true
                        },
                        { "name": "up",    "path": "<Keyboard>/upArrow",    "isPartOfComposite": true },
                        { "name": "down",  "path": "<Keyboard>/downArrow",  "isPartOfComposite": true },
                        { "name": "left",  "path": "<Keyboard>/leftArrow",  "isPartOfComposite": true },
                        { "name": "right", "path": "<Keyboard>/rightArrow", "isPartOfComposite": true },
                        { "path": "<Gamepad>/leftStick" }
                    ]
                },
                {
                    "name": "Submit",
                    "type": "Button",
                    "bindings": [
                        { "path": "<Keyboard>/enter" },
                        { "path": "<Gamepad>/buttonSouth" }
                    ]
                },
                {
                    "name": "Cancel",
                    "type": "Button",
                    "bindings": [
                        { "path": "<Keyboard>/escape" },
                        { "path": "<Gamepad>/buttonEast" }
                    ]
                }
            ]
        }
    ],
    "controlSchemes": [
        {
            "name": "KeyboardMouse",
            "bindingGroup": "KeyboardMouse",
            "devices": [
                { "devicePath": "<Keyboard>" },
                { "devicePath": "<Mouse>" }
            ]
        },
        {
            "name": "Gamepad",
            "bindingGroup": "Gamepad",
            "devices": [
                { "devicePath": "<Gamepad>" }
            ]
        }
    ]
}
*/

// ══════════════════════════════════════════════════════════════════════════════
//  PAȘI DE SETUP ÎN UNITY
// ══════════════════════════════════════════════════════════════════════════════
//
//  1. Instalează pachetul "Input System" din Package Manager (Window → Package Manager)
//     → Acceptă restart-ul când Unity îl cere.
//
//  2. Creează Input Actions Asset:
//     → Right-click în Project → Create → Input Actions → Numește "PlayerInputActions"
//     → Deschide asset-ul și adaugă Action Maps și Actions conform tabelului de mai sus.
//     → În colțul din dreapta sus, bifează "Generate C# Class" și apasă "Apply".
//     → Unity va genera automat PlayerInputActions.cs cu interfețe IGameplayActions și IUIActions.
//
//  3. Setup GameObject:
//     → Creează un GameObject "Player" cu CharacterController.
//     → Adaugă componenta FPSController.
//     → Creează un copil "CameraHolder" și asignează-l în câmpul Camera Holder.
//     → Adaugă Camera pe CameraHolder sau ca copil al acestuia.
//
//  4. Creează un alt GameObject "InputManager":
//     → Adaugă componenta InputReader.
//     → (Opțional) Dacă folosești DontDestroyOnLoad, un singur InputManager persiste.
//
//  5. Setează Ground Layer:
//     → În Inspector la FPSController → Ground Layer → selectează layer-ul tău de teren.
//
//  6. (Opțional) UI Manager:
//     → Abonează-te la InputReader.Instance.OnOpenUI / OnCloseUI pentru a afișa/ascunde meniuri.
//
// ══════════════════════════════════════════════════════════════════════════════
