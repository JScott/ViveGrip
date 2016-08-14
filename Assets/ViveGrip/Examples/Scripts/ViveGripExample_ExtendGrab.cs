using UnityEngine;
using System.Collections;

// See EXTENSIONS.md for more information

public class ViveGripExample_ExtendGrab : MonoBehaviour {
  float counter;
  const float THRESHOLD = 0.2f;

  void Start() {}

  void Update() {
    if (counter <= 0) { return; }
    counter -= Time.deltaTime;
  }

  void ViveGripGrabStart() {
    if (!this.enabled) { return; }
    counter = THRESHOLD;
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    if (counter <= 0) { return; }
    gripPoint.ToggleGrab();
  }
}
