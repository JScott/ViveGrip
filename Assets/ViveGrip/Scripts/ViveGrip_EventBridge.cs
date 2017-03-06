using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViveGrip_EventBridge : MonoBehaviour {
  public enum ViveGripEvent {
    InteractionStart,
    InteractionStop,
    GrabStart,
    GrabStop,
    HighlightStart,
    HighlightStop,
    TouchStart,
    TouchStop,
  }
  public ViveGripEvent viveGripEvent;
  public UnityEvent attachedFunction;

  void Start() {}

  void InvokeIf(ViveGripEvent thisEvent) {
    if (attachedFunction != null && viveGripEvent == thisEvent) {
      attachedFunction.Invoke();
    }
  }

  void ViveGripInteractionStart() {
    InvokeIf(ViveGripEvent.InteractionStart);
  }

  void ViveGripInteractionStop() {
    InvokeIf(ViveGripEvent.InteractionStop);
  }

  void ViveGripGrabStart() {
    InvokeIf(ViveGripEvent.GrabStart);
  }

  void ViveGripGrabStop() {
    InvokeIf(ViveGripEvent.GrabStop);
  }

  void ViveGripHighlightStart() {
    InvokeIf(ViveGripEvent.HighlightStart);
  }

  void ViveGripHighlightStop() {
    InvokeIf(ViveGripEvent.HighlightStop);
  }

  void ViveGripTouchStart() {
    InvokeIf(ViveGripEvent.TouchStart);
  }

  void ViveGripTouchStop() {
    InvokeIf(ViveGripEvent.TouchStop);
  }
}
