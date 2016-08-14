using UnityEngine;
using System.Collections;

public class ViveGripExample_ExtendGrab : MonoBehaviour {
  float counter;
  const float THRESHOLD = 0.2f;

  void Start() {}

  void Update() {
    if (counter <= 0) { return; }
    counter -= Time.deltaTime;
  }

  void ViveGripGrabStart() {
    counter = THRESHOLD;
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    if (counter <= 0) { return; }
    gripPoint.ToggleGrab();
  }
}
