using UnityEngine;
using System.Collections;

public class ViveGripExample_Capsule : MonoBehaviour {
  public bool inZone = false;
	public bool seated = true;
  private Vector3 seatedPosition;
  private const float FLOAT_SPEED = 3f;
  private const float FLOAT_DISTANCE = 0.01f;

  void Start () {
	  seatedPosition = transform.position;
	}

	void Update () {
    GetComponent<Rigidbody>().useGravity = !seated;
	  if (seated) {
      Vector3 floatPosition = transform.position;
      floatPosition.y = seatedPosition.y + Mathf.Sin(Time.time * FLOAT_SPEED) * FLOAT_DISTANCE;
      transform.position = floatPosition;
    }
	}

  void ViveGripGrabStart() {
    seated = false;
  }

  void ViveGripGrabStop() {
    seated = inZone;
    if (seated) { Reseat(); }
  }

  void Reseat() {
    transform.position = seatedPosition;
    transform.rotation = Quaternion.identity;
    GetComponent<Rigidbody>().velocity = Vector3.zero;
    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
  }

  void OnCollisionEnter() {
    seated = false;
  }
}
