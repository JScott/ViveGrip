using UnityEngine;

[AddComponentMenu("ViveGrip/Grabbable")]
//[RequireComponent (typeof (Rigidbody))]
[DisallowMultipleComponent]
public class ViveGrip_Grabbable : MonoBehaviour {
  [Tooltip("The local position that will be gripped.")]
  public Vector3 anchor = Vector3.zero;
  [Tooltip("Whether ")]
  public bool applyGripRotation = true;
  [Tooltip("")]
  public bool snapToOrientation = false;
  [Tooltip("")]
  public Vector3 localOrientation = Vector3.zero;

  void Start() {
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
