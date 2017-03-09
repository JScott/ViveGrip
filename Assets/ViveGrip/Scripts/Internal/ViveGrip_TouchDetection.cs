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
    ViveGrip_Grabbable grabbable = FindAnyValid<ViveGrip_Grabbable>(gameObject);
    if (grabbable != null) { return grabbable.gameObject; }
    ViveGrip_Interactable interactable = FindAnyValid<ViveGrip_Interactable>(gameObject);
    if (interactable != null) { return interactable.gameObject; }
    return null;
  }

  T FindAnyValid<T>(GameObject gameObject) where T : class {
    // var component = gameObject.GetComponent<T>();
    // if (component != null && Enabled<T>(component)) { return component; }
    // Transform parent = gameObject.transform.parent;
    // if (parent != null) {
    //   component = parent.GetComponent<T>();
    //   if (component != null && Enabled<T>(component)) { return component; }
    // }
    // return null;
    T component = FindValid<T>(gameObject.transform);
    if (component == null) {
      component = FindValid<T>(gameObject.transform.parent);
    }
    return component;
  }

  T FindValid<T>(Transform transform) where T : class {
    if (transform == null) { return null; }
    T component = transform.GetComponent<T>();
    if (component != null && Enabled<T>(component)) { return component; }
    return null;
  }

  bool Enabled<T>(T component) {
    if (component is ViveGrip_Grabbable) {
      return (component as ViveGrip_Grabbable).enabled;
    }
    if (component is ViveGrip_Interactable) {
      return (component as ViveGrip_Interactable).enabled;
    }
    return false;
  }
}
