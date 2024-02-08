//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Scripts/Inputs/Controls.inputactions
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

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""67ef2fad-79b9-41cf-9424-c1bc25e346b2"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""a81276ac-24a9-499a-975c-19ab671c8825"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""e8fcc494-ddfa-4e5a-95f0-3b634dc4fca8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RClick"",
                    ""type"": ""Button"",
                    ""id"": ""b79612a8-f265-44b5-88bf-8485fad7c77e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Space"",
                    ""type"": ""Button"",
                    ""id"": ""5ed1dcc7-0960-4acf-892c-d7a36ca35e73"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""fb7620da-b8f4-4653-823a-6a5d648307df"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MusicToggle"",
                    ""type"": ""Button"",
                    ""id"": ""8be20e00-75e0-42f7-8c69-cadbba791b4f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""E"",
                    ""type"": ""Button"",
                    ""id"": ""e0ba16e3-de96-48d4-bbf6-4f5b2aed8a4f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""c93d22eb-8890-4578-b015-c9c269df5bda"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drag"",
                    ""type"": ""Value"",
                    ""id"": ""c9e3c8f2-0b09-415e-8f3f-c191792948ba"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""949c6c28-0cce-4f05-8b5d-ed7fa29862ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UpDown"",
                    ""type"": ""Button"",
                    ""id"": ""73623088-9056-4a3f-96f1-a925029c0e8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftAlt"",
                    ""type"": ""Button"",
                    ""id"": ""5f2606c5-a95a-475a-bb0c-fbfa441850af"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LCtrl"",
                    ""type"": ""Button"",
                    ""id"": ""6a5daa60-6c9a-4b89-9ac3-adfe150dab45"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseHeld"",
                    ""type"": ""Value"",
                    ""id"": ""c69abe06-2064-4df5-9b31-b6682f1f70ed"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Esc"",
                    ""type"": ""Button"",
                    ""id"": ""abd36796-e878-44fe-ad81-2a3b99d09e3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""M"",
                    ""type"": ""Button"",
                    ""id"": ""40ccf40a-8c85-45ec-a120-6698492850f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ddb28091-b3a2-4867-b4d0-d6dac856b4ce"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f3bab8e-2d71-40f1-ad42-826e8cdfdc8c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""762132b5-45c6-495f-bf2c-abfe84010823"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""17c610db-643b-4fca-a13a-18c5c8f5a136"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""9adb3052-3434-4c51-9fe4-4cc063675d59"",
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
                    ""id"": ""e8a34848-60b0-4d7f-8359-4285a7ef8898"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d848d7da-c1d6-40bd-b0e2-2e518fa6716b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""451b2a8a-7112-482e-bef1-549f0e6d2ff2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""654bc2d8-0fb1-45d4-bb33-55fe4e0e8d86"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""874aea02-b996-49c5-adc3-96949eaf05b9"",
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
                    ""id"": ""ece70c8f-cd1d-424b-afc1-077638954bf9"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""11a4cb49-29fb-4bc4-95b8-f16d1c4a592f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""060e0404-ed08-4a68-b5f4-164f2f9eebd4"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c71699b2-8e93-48e8-b6d6-6120e2c6964d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8bf085e0-c712-4c51-8828-32b272ffec6d"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MusicToggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b25ddc58-2a0d-4cd9-8efb-5bd58f38a012"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""E"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad1946b9-8f3f-441d-bfc2-43126968a7e5"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a9f512b-b78d-450d-9c3a-865de637d671"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c7b23be-db39-4283-b294-3def17dd2a63"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""UpDown"",
                    ""id"": ""462af9bd-d549-4066-9630-e0d21b7c6431"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpDown"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""89006ac7-93d1-4438-ba43-7bbcea3c2ca0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""84dceeee-11e9-457b-91b7-c968c85737d7"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b15658f7-42e2-437b-8583-6786a5d50b97"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAlt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7cdb06a-f259-44aa-b111-86db2f625062"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LCtrl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d706f3b-8025-4f5b-ad60-a88791eb8ace"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseHeld"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d7aefd5-4e39-48b2-8060-cf45250073e6"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Esc"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed96e856-fe41-470b-b742-a272db989fd3"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Esc"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8a08d5b-dff3-42ff-a91e-3d7854b4212d"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""M"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Click = m_Player.FindAction("Click", throwIfNotFound: true);
        m_Player_RClick = m_Player.FindAction("RClick", throwIfNotFound: true);
        m_Player_Space = m_Player.FindAction("Space", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_MusicToggle = m_Player.FindAction("MusicToggle", throwIfNotFound: true);
        m_Player_E = m_Player.FindAction("E", throwIfNotFound: true);
        m_Player_Shift = m_Player.FindAction("Shift", throwIfNotFound: true);
        m_Player_Drag = m_Player.FindAction("Drag", throwIfNotFound: true);
        m_Player_Tab = m_Player.FindAction("Tab", throwIfNotFound: true);
        m_Player_UpDown = m_Player.FindAction("UpDown", throwIfNotFound: true);
        m_Player_LeftAlt = m_Player.FindAction("LeftAlt", throwIfNotFound: true);
        m_Player_LCtrl = m_Player.FindAction("LCtrl", throwIfNotFound: true);
        m_Player_MouseHeld = m_Player.FindAction("MouseHeld", throwIfNotFound: true);
        m_Player_Esc = m_Player.FindAction("Esc", throwIfNotFound: true);
        m_Player_M = m_Player.FindAction("M", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Click;
    private readonly InputAction m_Player_RClick;
    private readonly InputAction m_Player_Space;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_MusicToggle;
    private readonly InputAction m_Player_E;
    private readonly InputAction m_Player_Shift;
    private readonly InputAction m_Player_Drag;
    private readonly InputAction m_Player_Tab;
    private readonly InputAction m_Player_UpDown;
    private readonly InputAction m_Player_LeftAlt;
    private readonly InputAction m_Player_LCtrl;
    private readonly InputAction m_Player_MouseHeld;
    private readonly InputAction m_Player_Esc;
    private readonly InputAction m_Player_M;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Click => m_Wrapper.m_Player_Click;
        public InputAction @RClick => m_Wrapper.m_Player_RClick;
        public InputAction @Space => m_Wrapper.m_Player_Space;
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @MusicToggle => m_Wrapper.m_Player_MusicToggle;
        public InputAction @E => m_Wrapper.m_Player_E;
        public InputAction @Shift => m_Wrapper.m_Player_Shift;
        public InputAction @Drag => m_Wrapper.m_Player_Drag;
        public InputAction @Tab => m_Wrapper.m_Player_Tab;
        public InputAction @UpDown => m_Wrapper.m_Player_UpDown;
        public InputAction @LeftAlt => m_Wrapper.m_Player_LeftAlt;
        public InputAction @LCtrl => m_Wrapper.m_Player_LCtrl;
        public InputAction @MouseHeld => m_Wrapper.m_Player_MouseHeld;
        public InputAction @Esc => m_Wrapper.m_Player_Esc;
        public InputAction @M => m_Wrapper.m_Player_M;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
            @RClick.started += instance.OnRClick;
            @RClick.performed += instance.OnRClick;
            @RClick.canceled += instance.OnRClick;
            @Space.started += instance.OnSpace;
            @Space.performed += instance.OnSpace;
            @Space.canceled += instance.OnSpace;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @MusicToggle.started += instance.OnMusicToggle;
            @MusicToggle.performed += instance.OnMusicToggle;
            @MusicToggle.canceled += instance.OnMusicToggle;
            @E.started += instance.OnE;
            @E.performed += instance.OnE;
            @E.canceled += instance.OnE;
            @Shift.started += instance.OnShift;
            @Shift.performed += instance.OnShift;
            @Shift.canceled += instance.OnShift;
            @Drag.started += instance.OnDrag;
            @Drag.performed += instance.OnDrag;
            @Drag.canceled += instance.OnDrag;
            @Tab.started += instance.OnTab;
            @Tab.performed += instance.OnTab;
            @Tab.canceled += instance.OnTab;
            @UpDown.started += instance.OnUpDown;
            @UpDown.performed += instance.OnUpDown;
            @UpDown.canceled += instance.OnUpDown;
            @LeftAlt.started += instance.OnLeftAlt;
            @LeftAlt.performed += instance.OnLeftAlt;
            @LeftAlt.canceled += instance.OnLeftAlt;
            @LCtrl.started += instance.OnLCtrl;
            @LCtrl.performed += instance.OnLCtrl;
            @LCtrl.canceled += instance.OnLCtrl;
            @MouseHeld.started += instance.OnMouseHeld;
            @MouseHeld.performed += instance.OnMouseHeld;
            @MouseHeld.canceled += instance.OnMouseHeld;
            @Esc.started += instance.OnEsc;
            @Esc.performed += instance.OnEsc;
            @Esc.canceled += instance.OnEsc;
            @M.started += instance.OnM;
            @M.performed += instance.OnM;
            @M.canceled += instance.OnM;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
            @RClick.started -= instance.OnRClick;
            @RClick.performed -= instance.OnRClick;
            @RClick.canceled -= instance.OnRClick;
            @Space.started -= instance.OnSpace;
            @Space.performed -= instance.OnSpace;
            @Space.canceled -= instance.OnSpace;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @MusicToggle.started -= instance.OnMusicToggle;
            @MusicToggle.performed -= instance.OnMusicToggle;
            @MusicToggle.canceled -= instance.OnMusicToggle;
            @E.started -= instance.OnE;
            @E.performed -= instance.OnE;
            @E.canceled -= instance.OnE;
            @Shift.started -= instance.OnShift;
            @Shift.performed -= instance.OnShift;
            @Shift.canceled -= instance.OnShift;
            @Drag.started -= instance.OnDrag;
            @Drag.performed -= instance.OnDrag;
            @Drag.canceled -= instance.OnDrag;
            @Tab.started -= instance.OnTab;
            @Tab.performed -= instance.OnTab;
            @Tab.canceled -= instance.OnTab;
            @UpDown.started -= instance.OnUpDown;
            @UpDown.performed -= instance.OnUpDown;
            @UpDown.canceled -= instance.OnUpDown;
            @LeftAlt.started -= instance.OnLeftAlt;
            @LeftAlt.performed -= instance.OnLeftAlt;
            @LeftAlt.canceled -= instance.OnLeftAlt;
            @LCtrl.started -= instance.OnLCtrl;
            @LCtrl.performed -= instance.OnLCtrl;
            @LCtrl.canceled -= instance.OnLCtrl;
            @MouseHeld.started -= instance.OnMouseHeld;
            @MouseHeld.performed -= instance.OnMouseHeld;
            @MouseHeld.canceled -= instance.OnMouseHeld;
            @Esc.started -= instance.OnEsc;
            @Esc.performed -= instance.OnEsc;
            @Esc.canceled -= instance.OnEsc;
            @M.started -= instance.OnM;
            @M.performed -= instance.OnM;
            @M.canceled -= instance.OnM;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnClick(InputAction.CallbackContext context);
        void OnRClick(InputAction.CallbackContext context);
        void OnSpace(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnMusicToggle(InputAction.CallbackContext context);
        void OnE(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
        void OnDrag(InputAction.CallbackContext context);
        void OnTab(InputAction.CallbackContext context);
        void OnUpDown(InputAction.CallbackContext context);
        void OnLeftAlt(InputAction.CallbackContext context);
        void OnLCtrl(InputAction.CallbackContext context);
        void OnMouseHeld(InputAction.CallbackContext context);
        void OnEsc(InputAction.CallbackContext context);
        void OnM(InputAction.CallbackContext context);
    }
}
