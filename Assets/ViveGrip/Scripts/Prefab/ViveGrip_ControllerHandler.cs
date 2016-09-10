using UnityEngine;
using System.Collections;
using Valve.VR;

[DisallowMultipleComponent]
public class ViveGrip_ControllerHandler : MonoBehaviour {
  public enum ViveInput {
    Grip,
    Trigger,
    Touchpad,
    None
  };
  [Tooltip("The device that will be giving the input.")]
  public SteamVR_TrackedObject trackedObject;
  [Tooltip("The button used for gripping.")]
  public ViveInput grab = ViveInput.Grip;
  [Tooltip("The button used for interacting.")]
  public ViveInput interact = ViveInput.Trigger;
  private float MAX_VIBRATION_STRENGTH = 3999f;

  void Start() {}

  public bool Pressed(string action) {
    ulong rawInput = ConvertString(action);
    return Device().GetPressDown(rawInput);
  }

  public bool Released(string action) {
    ulong rawInput = ConvertString(action);
    return Device().GetPressUp(rawInput);
  }

  public bool Holding(string action) {
    ulong rawInput = ConvertString(action);
    return Device().GetPress(rawInput);
  }

  ulong ConvertString(string action) {
    ViveInput input = GetInputFor(action);
    return Decode(input);
  }

  public SteamVR_Controller.Device Device() {
    return SteamVR_Controller.Input((int)trackedObject.index);
  }

  ViveInput GetInputFor(string action) {
    switch (action.ToLower()) {
      default:
      case "grab":
        return grab;
      case "interact":
        return interact;
    }
  }

  ulong Decode(ViveInput input) {
    switch ((int)input) {
      case 0:
        return SteamVR_Controller.ButtonMask.Grip;
      case 1:
        return SteamVR_Controller.ButtonMask.Trigger;
      case 2:
        return SteamVR_Controller.ButtonMask.Touchpad;
      default:
      case 3:
        return (1ul << (int)EVRButtonId.k_EButton_Max+1);
    }
  }

  public void Vibrate(float milliseconds, float strength) {
    float seconds = milliseconds / 1000f;
    StartCoroutine(LongVibration(seconds, strength));
  }

  IEnumerator LongVibration(float length, float strength) {
    for(float i = 0; i < length; i += Time.deltaTime) {
      Device().TriggerHapticPulse((ushort)Mathf.Lerp(0, MAX_VIBRATION_STRENGTH, strength));
      yield return null;
    }
  }
}
