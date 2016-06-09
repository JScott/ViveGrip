using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (Rigidbody))]
[DisallowMultipleComponent]
public class ViveGrip_Grabbable : ViveGrip_Highlight {
  [Tooltip("Should the grip connect to the Local Anchor position?")]
  public bool snapToAnchor = true;
  [Tooltip("The local position that will be gripped.")]
  public Vector3 localAnchor = Vector3.zero;
  [Tooltip("Should this object snap to Local Orientation when grabbed?")]
  public bool snapToOrientation = false;
  [Tooltip("The local orientation that can be snapped to when grabbed.")]
  public Vector3 localOrientation = Vector3.zero;
  [Tooltip("Should the controller rotation be applied when grabbed?")]
  public bool applyGripRotation = true;
  private Vector3 grabCentre;

  void Start() {}

  public void OnDrawGizmosSelected() {
    if (snapToAnchor) {
      Gizmos.DrawIcon(transform.position + RotatedAnchor(), "ViveGrip/anchor.png", true);
    }
  }

  public Vector3 RotatedAnchor() {
    return transform.rotation * localAnchor;
  }

  public void GrabFrom(Vector3 jointLocation) {
    grabCentre = snapToAnchor ? localAnchor : (jointLocation - transform.position);
  }

  public Vector3 WorldAnchorPosition() {
    return transform.position + (transform.rotation * grabCentre);
  }

  // These are called this on all scripts in the attached object:

  // When touched and moved away from, respectively
  //   void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {}

  // When touched and the grab button is pressed and released, respectively
  //   void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {}
}
