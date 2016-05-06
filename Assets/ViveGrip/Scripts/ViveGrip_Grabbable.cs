using UnityEngine;

[AddComponentMenu("ViveGrip/Grabbable")]
[RequireComponent (typeof (Rigidbody))]
[DisallowMultipleComponent]
public class ViveGrip_Grabbable : MonoBehaviour {
  [Tooltip("The local position that will be gripped.")]
  public Vector3 anchor = Vector3.zero;
  [Tooltip("Should the controller rotation be applied when grabbed?")]
  public bool applyGripRotation = true;
  [Tooltip("Should this object snap to localOrientation when grabbed?")]
  public bool snapToOrientation = false;
  [Tooltip("The local orientation that can be snapped to when grabbed.")]
  public Vector3 localOrientation = Vector3.zero;

  void Start() {
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
