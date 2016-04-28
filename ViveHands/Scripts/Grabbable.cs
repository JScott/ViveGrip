using UnityEngine;
using System.Collections;

public class Grabbable : MonoBehaviour {
  float energy = 0f;
  Rigidbody rigidbody;

	void Start () {
    rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
    energy = KineticEnergy();
	}

  void OnCollisionEnter(Collision collision) {
    rigidbody.AddForce(-rigidbody.velocity);
    Debug.Log(collision.gameObject.name);
    Debug.Log(rigidbody.velocity.magnitude);
  }

  public static float KineticEnergy() {
      // mass in kg, velocity in meters per second, result is joules
      return 0f;// 0.5f * rigidbody.mass * Mathf.Pow(rigidbody.velocity.magnitude, 2);
  }
}
