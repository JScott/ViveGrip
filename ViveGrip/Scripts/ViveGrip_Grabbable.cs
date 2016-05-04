using UnityEngine;

public class ViveGrip_Grabbable : MonoBehaviour {
  // TODO: snapToAnchor
  public Vector3 anchor = Vector3.zero;
  public bool applyGripRotation = true;
  public bool snapToOrientation = false;
  public Vector3 orientation = Vector3.zero;

  void Start() {
    if (GetComponent<Rigidbody>() == null) {
      gameObject.AddComponent<Rigidbody>();
    }
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
