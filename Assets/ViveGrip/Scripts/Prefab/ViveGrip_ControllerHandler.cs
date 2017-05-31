﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

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
  private DualInputTracker dualInput;
  private const float MAX_VIBRATION_STRENGTH = 3999f;
  private delegate bool InputFunction(ulong key);
  void Start() {
    dualInput = new DualInputTracker();
  }

  void Update() {
    if (InputPerformed(ViveInput.Grip, Device().GetPressDown) ||
        InputPerformed(ViveInput.Trigger, Device().GetPressDown)) {
      dualInput.Increment();
    }
    if (InputPerformed(ViveInput.Grip, Device().GetPressUp) ||
        InputPerformed(ViveInput.Trigger, Device().GetPressUp)) {
      dualInput.Decrement();
    }
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

  // public bool Holding(Action action) {
  //   if (Device() == null) { return false; }
  //   ViveInput input = InputFor(action);
  //   return InputPerformed(input, Device().GetPress);
  // }

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

  // THEN we need to have a counter to only actually trigger if we hit 0/2 for both
  bool InputPerformed(ViveInput input, InputFunction func) {
    switch (input) {
      case ViveInput.Grip:
        return func(ButtonMaskFor(ViveInput.Grip));
      case ViveInput.Trigger:
        return func(ButtonMaskFor(ViveInput.Trigger));
      case ViveInput.Both:
        // return dualInput.Actually
        return func(ButtonMaskFor(ViveInput.Grip)) || func(ButtonMaskFor(ViveInput.Trigger));
      case ViveInput.None:
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
    if (trackedObject.index == SteamVR_TrackedObject.EIndex.None) { return null; }
    return SteamVR_Controller.Input((int)trackedObject.index);
  }

  // strength is a value from 0-1
  public void Vibrate(int milliseconds, float strength) {
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

class DualInputTracker {
  private int inputCounter = 0;
  private bool incrementing = false;

  public DualInputTracker() {}

  public void Increment() {
      inputCounter++;
      incrementing = true;
  }

  public void Decrement() {
      inputCounter--;
      incrementing = false;
  }

  public bool ActuallyPressed() {
    return inputCounter == 1 && incrementing;
  }

  public bool ActuallyReleased() {
    return inputCounter == 0;
  }
}
