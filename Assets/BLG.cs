// GENERATED AUTOMATICALLY FROM 'Assets/BLG.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @BLG : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @BLG()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BLG"",
    ""maps"": [
        {
            ""name"": ""BLGMove"",
            ""id"": ""a0e7fa65-9b67-43d6-8a88-e1c0428afe87"",
            ""actions"": [
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""d9102f7d-3fa7-426c-bf9e-c2d9d1c1496b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveForward"",
                    ""type"": ""Value"",
                    ""id"": ""1c68fc97-42dd-4165-8dca-08982437f5eb"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4b18d221-352e-4daf-8f96-d260b8fa6fb7"",
                    ""path"": ""<HID::E502-CDAB>/ry"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ded2de11-29ef-4f40-89cb-a8130e60a48f"",
                    ""path"": ""<HID::E502-CDAB>/rx"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme"",
                    ""action"": ""MoveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<HID::E502-CDAB>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // BLGMove
        m_BLGMove = asset.FindActionMap("BLGMove", throwIfNotFound: true);
        m_BLGMove_Rotate = m_BLGMove.FindAction("Rotate", throwIfNotFound: true);
        m_BLGMove_MoveForward = m_BLGMove.FindAction("MoveForward", throwIfNotFound: true);
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

    // BLGMove
    private readonly InputActionMap m_BLGMove;
    private IBLGMoveActions m_BLGMoveActionsCallbackInterface;
    private readonly InputAction m_BLGMove_Rotate;
    private readonly InputAction m_BLGMove_MoveForward;
    public struct BLGMoveActions
    {
        private @BLG m_Wrapper;
        public BLGMoveActions(@BLG wrapper) { m_Wrapper = wrapper; }
        public InputAction @Rotate => m_Wrapper.m_BLGMove_Rotate;
        public InputAction @MoveForward => m_Wrapper.m_BLGMove_MoveForward;
        public InputActionMap Get() { return m_Wrapper.m_BLGMove; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BLGMoveActions set) { return set.Get(); }
        public void SetCallbacks(IBLGMoveActions instance)
        {
            if (m_Wrapper.m_BLGMoveActionsCallbackInterface != null)
            {
                @Rotate.started -= m_Wrapper.m_BLGMoveActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_BLGMoveActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_BLGMoveActionsCallbackInterface.OnRotate;
                @MoveForward.started -= m_Wrapper.m_BLGMoveActionsCallbackInterface.OnMoveForward;
                @MoveForward.performed -= m_Wrapper.m_BLGMoveActionsCallbackInterface.OnMoveForward;
                @MoveForward.canceled -= m_Wrapper.m_BLGMoveActionsCallbackInterface.OnMoveForward;
            }
            m_Wrapper.m_BLGMoveActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @MoveForward.started += instance.OnMoveForward;
                @MoveForward.performed += instance.OnMoveForward;
                @MoveForward.canceled += instance.OnMoveForward;
            }
        }
    }
    public BLGMoveActions @BLGMove => new BLGMoveActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface IBLGMoveActions
    {
        void OnRotate(InputAction.CallbackContext context);
        void OnMoveForward(InputAction.CallbackContext context);
    }
}
