using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGripExample_ColorChanger : MonoBehaviour {
  void Start() {}

  public void ChangeColor() {
    GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
  }
}
