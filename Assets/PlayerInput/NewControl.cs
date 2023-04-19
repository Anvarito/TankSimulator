//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/PlayerInput/NewControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @NewControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @NewControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""NewControl"",
    ""maps"": [
        {
            ""name"": ""Tank"",
            ""id"": ""e4796219-1cad-496a-897d-d0c400502c1e"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""e2a2bc46-7bea-4623-8675-b110a02324cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6f909e3e-cd23-4710-a8ea-5eef9d22c19c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""334621fb-09e1-4c8d-b43d-37a56ac6d074"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(max=1),NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""f84b252d-211b-4e55-9654-9690cdff9990"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ResetTurret"",
                    ""type"": ""Button"",
                    ""id"": ""5f617fd8-85be-4d27-af78-ac8b4d25e9fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a6c4af6c-b454-45d6-b94a-f98e85e666d8"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e76f22a6-e7f1-49c8-84bf-8e12a11df066"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8df65dc2-9c0d-41d7-8095-c7ae7115ea15"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03e3bbed-9f97-4c98-8601-cec06b680223"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c919a8b4-64ba-418f-91d1-2d0e9797fe42"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60990b23-4c6b-4d01-9057-dde81db17cb4"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35ab773c-b9b1-4dbc-9a8c-65e511feb7b2"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ResetTurret"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a6f69b6-3654-44c2-a73c-9dffc294416f"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""ResetTurret"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce02d8cc-2da6-4eae-92fc-572615d00e92"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9e9d06f1-185c-4338-802d-69f5650e5c6a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""03284423-cace-4f3c-986f-6a089874c39a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""55673a22-f40c-45ce-bcd5-a380c2b311cf"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""86bdb62e-c1c4-4b4d-8098-a6af4428f263"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f9008254-cd16-48b9-b570-b29455edfa04"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<XboxOneGamepadAndroid>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard + Mouse"",
            ""bindingGroup"": ""Keyboard + Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Tank
        m_Tank = asset.FindActionMap("Tank", throwIfNotFound: true);
        m_Tank_Fire = m_Tank.FindAction("Fire", throwIfNotFound: true);
        m_Tank_Move = m_Tank.FindAction("Move", throwIfNotFound: true);
        m_Tank_Look = m_Tank.FindAction("Look", throwIfNotFound: true);
        m_Tank_Aim = m_Tank.FindAction("Aim", throwIfNotFound: true);
        m_Tank_ResetTurret = m_Tank.FindAction("ResetTurret", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Tank
    private readonly InputActionMap m_Tank;
    private ITankActions m_TankActionsCallbackInterface;
    private readonly InputAction m_Tank_Fire;
    private readonly InputAction m_Tank_Move;
    private readonly InputAction m_Tank_Look;
    private readonly InputAction m_Tank_Aim;
    private readonly InputAction m_Tank_ResetTurret;
    public struct TankActions
    {
        private @NewControl m_Wrapper;
        public TankActions(@NewControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Tank_Fire;
        public InputAction @Move => m_Wrapper.m_Tank_Move;
        public InputAction @Look => m_Wrapper.m_Tank_Look;
        public InputAction @Aim => m_Wrapper.m_Tank_Aim;
        public InputAction @ResetTurret => m_Wrapper.m_Tank_ResetTurret;
        public InputActionMap Get() { return m_Wrapper.m_Tank; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TankActions set) { return set.Get(); }
        public void SetCallbacks(ITankActions instance)
        {
            if (m_Wrapper.m_TankActionsCallbackInterface != null)
            {
                @Fire.started -= m_Wrapper.m_TankActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnFire;
                @Move.started -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_TankActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnLook;
                @Aim.started -= m_Wrapper.m_TankActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnAim;
                @ResetTurret.started -= m_Wrapper.m_TankActionsCallbackInterface.OnResetTurret;
                @ResetTurret.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnResetTurret;
                @ResetTurret.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnResetTurret;
            }
            m_Wrapper.m_TankActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @ResetTurret.started += instance.OnResetTurret;
                @ResetTurret.performed += instance.OnResetTurret;
                @ResetTurret.canceled += instance.OnResetTurret;
            }
        }
    }
    public TankActions @Tank => new TankActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard + Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface ITankActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnResetTurret(InputAction.CallbackContext context);
    }
}
