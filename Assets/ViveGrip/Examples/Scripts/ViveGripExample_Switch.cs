using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGripExample_Switch : MonoBehaviour {
  void Start() {}

  public void Flip() {
    Vector3 rotation = transform.eulerAngles;
    rotation.x *= -1;
    transform.eulerAngles = rotation;
  }
}
