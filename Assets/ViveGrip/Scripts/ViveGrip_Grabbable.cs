using UnityEngine;
using UnityEditor;

[RequireComponent (typeof (Rigidbody))]
[DisallowMultipleComponent]
public class ViveGrip_Grabbable : ViveGrip_Highlight {
  [Tooltip("The local position that will be gripped.")]
  public Vector3 anchor = Vector3.zero;
  [Tooltip("Should the controller rotation be applied when grabbed?")]
  public bool applyGripRotation = true;
  [Tooltip("Should this object snap to localOrientation when grabbed?")]
  public bool snapToOrientation = false;
  [Tooltip("The local orientation that can be snapped to when grabbed.")]
  public Vector3 localOrientation = Vector3.zero;
  // TODO: snapToOrientation doesn't make much sense without applyGripRotation. Make it an enum?

  void Start() {}

  public void OnDrawGizmosSelected() {
    Vector3 localAnchor = Vector3.zero;
    localAnchor += transform.right * anchor.x;
    localAnchor += transform.up * anchor.y;
    localAnchor += transform.forward * anchor.z;
    Gizmos.DrawIcon(transform.position + localAnchor, "anchor.png", true);
  }
}
