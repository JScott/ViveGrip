using UnityEngine;
using System.Collections;

public class ViveGrip_Grabbable : MonoBehaviour {
  public Vector3 anchor = Vector3.zero;
  public bool snapToOrientation = false;
  public Vector3 orientation = Vector3.zero;
  // TODO: public bool highlighted = false;

  void Start() {
    if (GetComponent<Rigidbody>() == null) {
      gameObject.AddComponent<Rigidbody>();
    }
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
