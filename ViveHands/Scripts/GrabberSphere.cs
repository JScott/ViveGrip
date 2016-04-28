using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabberSphere : MonoBehaviour {
  public float radius = 1f;
  private List<GameObject> collidingObjects = new List<GameObject>();

	void Start () {
    GetComponent<SphereCollider>().isTrigger = true;
	}

  void OnTriggerEnter(Collider other) {
    collidingObjects.Add(other.gameObject);
  }

  void OnTriggerExit(Collider other) {
    collidingObjects.Remove(other.gameObject);
  }

  public GameObject ClosestObject() {
    float closestDistance = radius + 1f;
    GameObject touchedObject = null;
    foreach (GameObject gameObject in collidingObjects) {
      if (gameObject.GetComponent<Grabbable>() == null) {
        continue;
      }
      float distance = Vector3.Distance(transform.position, gameObject.transform.position);
      if (distance < closestDistance) {
        touchedObject = gameObject;
        closestDistance = distance;
      }
    }
    return touchedObject;
  }
}
