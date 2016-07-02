using UnityEngine;
using System.Collections;

public class ViveGripExample_DoorHandle : MonoBehaviour {
  public Transform door;

  void Start () {}

  void Update () {
    Vector3 newRotation = new Vector3(0, 90, 0);
    newRotation.x = -door.GetComponent<HingeJoint>().angle;
    transform.localEulerAngles = newRotation;
  }
}
