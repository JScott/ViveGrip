using UnityEngine;
using System.Collections;

public class ViveGripExample_Tar : MonoBehaviour {
	private ViveGrip_GripPoint attachedGripPoint;
  private const float SPEED_THRESHOLD = 6f;
  private bool attached = false;

  void Start() {}

  void Update() {
    if (attachedGripPoint == null) { return; }
    float speed = GetComponent<Rigidbody>().velocity.magnitude;
    attached = attached || speed < SPEED_THRESHOLD;
    if (attached && speed > SPEED_THRESHOLD) {
      attachedGripPoint.enabled = true;
      attachedGripPoint.ToggleGrab();
      attachedGripPoint = null;
    }
  }

	void ViveGripTouchStart(ViveGrip_GripPoint gripPoint) {
    if (gripPoint.HoldingSomething()) { return; }
    gripPoint.ToggleGrab();
    attachedGripPoint = gripPoint;
    gripPoint.enabled = false;
    attached = false;
  }
}
