using UnityEngine;

public class ViveGrip_Grabbable : MonoBehaviour {
  // TODO: snapToAnchor
  public bool snapToAnchor = true;
  public Vector3 anchor = Vector3.zero;
  public bool useGripRotation = true;
  public bool snapToOrientation = false;
  public Vector3 orientation = Vector3.zero;

  void Start() {
    if (GetComponent<Rigidbody>() == null) {
      gameObject.AddComponent<Rigidbody>();
    }
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
