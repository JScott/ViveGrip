using UnityEngine;
using System.Collections;

public class ViveGrip_Grabbable : MonoBehaviour {
  public Vector3 anchor = Vector3.zero;
  public bool snapToOrientation = false;
  public Vector3 orientation = Vector3.zero;

  void Start() {
    if (GetComponent<Rigidbody>() == null) {
      gameObject.AddComponent<Rigidbody>();
    }
  }
}
