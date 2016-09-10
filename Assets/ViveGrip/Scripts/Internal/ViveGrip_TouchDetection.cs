using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_TouchDetection : MonoBehaviour {
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

  public GameObject NearestObject() {
    float closestDistance = radius + 1f;
    GameObject touchedObject = null;
    foreach (GameObject gameObject in collidingObjects) {
      if (!ActiveViveGripObject(gameObject)) { continue; }
      float distance = Vector3.Distance(transform.position, gameObject.transform.position);
      if (distance < closestDistance) {
        touchedObject = gameObject;
        closestDistance = distance;
      }
    }
    return touchedObject;
  }

  bool ActiveViveGripObject(GameObject gameObject) {
    if (gameObject == null) { return false; } // Happens with Destroy() sometimes
    ViveGrip_Grabbable grabbable = gameObject.GetComponent<ViveGrip_Grabbable>();
    ViveGrip_Interactable interactable = gameObject.GetComponent<ViveGrip_Interactable>();
    bool validGrabbable = grabbable != null && grabbable.enabled;
    bool validInteractable = interactable != null && interactable.enabled;
    return validGrabbable || validInteractable;
  }
}
