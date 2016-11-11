using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViveGrip_ExtensionBridge : MonoBehaviour {
  public enum ViveGripTrigger {
    InteractionStart,
    InteractionStop,
    GrabStart,
    GrabStop,
    HighlightStart,
    HighlightStop,
    TouchStart,
    TouchStop,
  }
  public ViveGripTrigger viveGripEvent;
  public UnityEvent attachedFunction;

  void Start() {}

  void ViveGripInteractionStart() {
    if (ShouldTrigger(ViveGripTrigger.InteractionStart)) { attachedFunction.Invoke(); }
  }

  void ViveGripInteractionStop() {
    if (ShouldTrigger(ViveGripTrigger.InteractionStop)) { attachedFunction.Invoke(); }
  }

  void ViveGripGrabStart() {
    if (ShouldTrigger(ViveGripTrigger.GrabStart)) { attachedFunction.Invoke(); }
  }

  void ViveGripGrabStop() {
    if (ShouldTrigger(ViveGripTrigger.GrabStop)) { attachedFunction.Invoke(); }
  }

  void ViveGripHighlightStart() {
    if (ShouldTrigger(ViveGripTrigger.HighlightStart)) { attachedFunction.Invoke(); }
  }

  void ViveGripHighlightStop() {
    if (ShouldTrigger(ViveGripTrigger.HighlightStop)) { attachedFunction.Invoke(); }
  }

  void ViveGripTouchStart() {
    if (ShouldTrigger(ViveGripTrigger.TouchStart)) { attachedFunction.Invoke(); }
  }

  void ViveGripTouchStop() {
    if (ShouldTrigger(ViveGripTrigger.TouchStop)) { attachedFunction.Invoke(); }
  }

  bool ShouldTrigger(ViveGripTrigger thisEvent) {
    return attachedFunction != null && viveGripEvent == thisEvent;
  }
}
