using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_TouchDetection : MonoBehaviour {
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
    float closestDistance = Mathf.Infinity;
    GameObject touchedObject = null;
    foreach (GameObject gameObject in collidingObjects) {
      GameObject activeGameObject = ActiveViveGripObject(gameObject);
      if (activeGameObject!=null) {
        float distance = Vector3.Distance(transform.position, gameObject.transform.position);
        if (distance < closestDistance) {
          touchedObject = activeGameObject;
          closestDistance = distance;
        }
      }
    }
    return touchedObject;
  }

  GameObject ActiveViveGripObject(GameObject gameObject) {
    if (gameObject == null) { return null; } // Happens with Destroy() sometimes
    MonoBehaviour component = ValidComponent(gameObject.transform);
    if (component == null) {
      component = ValidComponent(gameObject.transform.parent);
    }
    if (component != null) {
      return component.gameObject;
    }
    return null;
  }

  MonoBehaviour ValidComponent(Transform transform) {
    if (transform == null) { return null; }
    MonoBehaviour component = transform.GetComponent<ViveGrip_Grabbable>();
    if (component != null && component.enabled) { return component; }
    component = transform.GetComponent<ViveGrip_Interactable>();
    if (component != null && component.enabled) { return component; }
    return null;
  }
}
