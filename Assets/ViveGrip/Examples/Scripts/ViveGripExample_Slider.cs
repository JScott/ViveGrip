using UnityEngine;
using System.Collections;

public class ViveGripExample_Slider : MonoBehaviour {
  private ViveGrip_ControllerHandler controller;
  private float oldX;
  private int VIBRATION_DURATION_IN_MILLISECONDS = 50;
  private float MAX_VIBRATION_STRENGTH = 0.2f;
  private float MAX_VIBRATION_DISTANCE = 0.03f;

	void Start () {
    oldX = transform.position.x;
  }

  void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {
    controller = gripPoint.controller;
  }

  void ViveGripGrabStop() {
    controller = null;
  }

	void Update () {
    float newX = transform.position.x;
    if (controller != null) {
      float distance = Mathf.Min(Mathf.Abs(newX - oldX), MAX_VIBRATION_DISTANCE);
      float vibrationStrength = (distance / MAX_VIBRATION_DISTANCE) * MAX_VIBRATION_STRENGTH;
      controller.Vibrate(VIBRATION_DURATION_IN_MILLISECONDS, vibrationStrength);
    }
    oldX = newX;
	}
}
