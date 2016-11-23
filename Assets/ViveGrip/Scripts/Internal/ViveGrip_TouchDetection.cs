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
      GameObject activeGameObject = ActiveViveGripObject(gameObject);
      if (activeGameObject!=null)
      {
        float distance = Vector3.Distance(transform.position, gameObject.transform.position);
        if (distance < closestDistance)
        {
          touchedObject = activeGameObject;
          closestDistance = distance;
        }
      }
    }
    return touchedObject;
  }

  GameObject ActiveViveGripObject(GameObject gameObject) {
    if (gameObject == null) { return null; } // Happens with Destroy() sometimes
    ViveGrip_Grabbable grabbable = gameObject.GetComponentInParent<ViveGrip_Grabbable>();
    bool validGrabbable = grabbable != null && grabbable.enabled;
    if (validGrabbable) return grabbable.gameObject;
    ViveGrip_Interactable interactable = gameObject.GetComponentInParent<ViveGrip_Interactable>();
    bool validInteractable = interactable != null && interactable.enabled;
    if (validInteractable) return interactable.gameObject;
    return null;
  }
}
