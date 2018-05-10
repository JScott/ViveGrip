using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent (typeof (Rigidbody))]
[DisallowMultipleComponent]
public class ViveGrip_Grabbable : ViveGrip_Object {
  public enum RotationMode {
    Disabled,
    ApplyGrip,
    ApplyGripAndOrientation
  }
  [System.Serializable]
  public class Position {
    [Tooltip("Should the grip connect to the Local Anchor position?")]
    public bool enabled = false;
    [Tooltip("The local position that will be gripped if enabled.")]
    public Vector3 localPosition = Vector3.zero;
  }
  [System.Serializable]
  public class Rotation {
    [Tooltip("The rotations that will be applied to a grabbed object.")]
    public RotationMode mode = RotationMode.ApplyGrip;
    [Tooltip("The local orientation that can be snapped to when grabbed.")]
    public Vector3 localOrientation = Vector3.zero;
  }
  public Position anchor;
  public Rotation rotation;
  private Vector3 grabCentre;
  private HashSet<ViveGrip_GripPoint> attachedGripPoints;

  // These are called this on the scripts of the attached object and children of the controller:

  // Called when touched and moved away from, respectively
  //   void ViveGripTouchStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripTouchStop(ViveGrip_GripPoint gripPoint) {}

  // Called when touched and the grab button is pressed and released, respectively
  //   void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {}

  // Called when highlighting changes
  //   void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {}

  public void OnDrawGizmosSelected() {
    if (anchor != null && anchor.enabled) {
      Gizmos.DrawIcon(transform.position + RotatedAnchor(), "ViveGrip/anchor.png", true);
    }
  }

  public Vector3 RotatedAnchor() {
    return transform.rotation * anchor.localPosition;
  }

  public void GrabFrom(Vector3 jointLocation) {
    grabCentre = anchor.enabled ? anchor.localPosition : (jointLocation - transform.position);
  }

  public Vector3 WorldAnchorPosition() {
    return transform.position + (transform.rotation * grabCentre);
  }

  public bool ApplyGripRotation() {
    return rotation.mode != RotationMode.Disabled;
  }

  public bool SnapToOrientation() {
    return rotation.mode == RotationMode.ApplyGripAndOrientation;
  }

  void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {
    attachedGripPoints.Add(gripPoint);
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    attachedGripPoints.Remove(gripPoint);
  }

  public HashSet<ViveGrip_GripPoint> AttachedGripPoints() {
    return attachedGripPoints;
  }
}
