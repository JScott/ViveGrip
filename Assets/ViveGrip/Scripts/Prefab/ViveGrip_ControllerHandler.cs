using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

delegate bool InputFunction(ulong key);

[DisallowMultipleComponent]
public class ViveGrip_ControllerHandler : MonoBehaviour {
  public enum ViveInput {
    Grip,
    Trigger,
    Both,
    None
  };
  public enum Action {
    Grab,
    Interact
  };
  [Tooltip("The device that will be giving the input.")]
  public SteamVR_TrackedObject trackedObject;
  [Tooltip("The button used for gripping.")]
  public ViveInput grabInput = ViveInput.Grip;
  [Tooltip("The button used for interacting.")]
  public ViveInput interactInput = ViveInput.Trigger;
  private bool gripOrTriggerHeld = false;
  private bool gripOrTriggerPressed = false;
  // NOTE: deviceIndex may be set without having a trackedObject
  private int deviceIndex = -1;
  private const float MAX_VIBRATION_STRENGTH = 3999f;

  void Start() {
    SetDeviceIndex();
  }

  void Update() {
    if (Device() == null) {
      SetDeviceIndex();
      return;
    }
    if (InputPerformed(ViveInput.Grip, Device().GetPress) ||
        InputPerformed(ViveInput.Trigger, Device().GetPress)) {
      gripOrTriggerPressed = !gripOrTriggerHeld;
      gripOrTriggerHeld = true;
    }
    if (!InputPerformed(ViveInput.Grip, Device().GetPress) &&
        !InputPerformed(ViveInput.Trigger, Device().GetPress)) {
      gripOrTriggerPressed = false;
      gripOrTriggerHeld = false;
    }
  }

  void SetDeviceIndex() {
    if (!trackedObject) { return; }
    deviceIndex = (int)trackedObject.index;
  }

  // Support for SteamVR's Interaction System (see Hand#InitController)
  void OnHandInitialized(int index) {
    deviceIndex = index;
  }

  public bool Pressed(Action action) {
    if (Device() == null) { return false; }
    ViveInput input = InputFor(action);
    return InputPerformed(input, Device().GetPressDown);
  }

  public bool Released(Action action) {
    if (Device() == null) { return false; }
    ViveInput input = InputFor(action);
    return InputPerformed(input, Device().GetPressUp);
  }

  public bool Holding(Action action) {
    if (Device() == null) { return false; }
    ViveInput input = InputFor(action);
    return InputPerformed(input, Device().GetPress);
  }

  ViveInput InputFor(Action action) {
    switch (action) {
      case Action.Grab:
        return grabInput;
      case Action.Interact:
        return interactInput;
      default:
        return ViveInput.None;
    }
  }

  bool InputPerformed(ViveInput input, InputFunction func) {
    switch (input) {
      case ViveInput.Grip:
        return func(ButtonMaskFor(ViveInput.Grip));
      case ViveInput.Trigger:
        return func(ButtonMaskFor(ViveInput.Trigger));
      case ViveInput.Both:
        return BothInputPerformed(func);
      case ViveInput.None:
      default:
        return false;
    }
  }

  bool BothInputPerformed(InputFunction func) {
    switch (func.Method.Name) {
      case "GetPressDown":
        return gripOrTriggerPressed;
      case "GetPress":
        return gripOrTriggerHeld;
      case "GetPressUp":
        return !gripOrTriggerHeld;
      default:
        return false;
    }
  }

  ulong ButtonMaskFor(ViveInput input) {
    switch (input) {
      case ViveInput.Grip:
        return SteamVR_Controller.ButtonMask.Grip;
      case ViveInput.Trigger:
        return SteamVR_Controller.ButtonMask.Trigger;
      case ViveInput.Both:
        return SteamVR_Controller.ButtonMask.Touchpad;
      default:
      case ViveInput.None:
        return (1ul << (int)EVRButtonId.k_EButton_Max+1);
    }
  }

  public SteamVR_Controller.Device Device() {
    if (deviceIndex == -1) { return null; }
    if (deviceIndex == (int)SteamVR_TrackedObject.EIndex.None) { return null; }
    return SteamVR_Controller.Input(deviceIndex);
  }

  // strength is a value from 0-1
  public void Vibrate(int milliseconds, float strength) {
    float seconds = milliseconds / 1000f;
    StartCoroutine(LongVibration(seconds, strength));
  }

  IEnumerator LongVibration(float length, float strength) {
    for(float i = 0; i < length; i += Time.deltaTime) {
      if (Device() != null) {
        ushort vibration = (ushort)Mathf.Lerp(0, MAX_VIBRATION_STRENGTH, strength);
        Device().TriggerHapticPulse(vibration);
      }
      yield return null;
    }
  }

  public GameObject TrackedObject() {
    if (trackedObject == null) {
      return transform.parent.gameObject;
    }
    return trackedObject.gameObject;
  }
}
