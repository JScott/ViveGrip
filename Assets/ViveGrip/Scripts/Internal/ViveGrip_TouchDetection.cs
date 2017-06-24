﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ViveGrip_TouchDetection : MonoBehaviour {
  private List<ViveGrip_Object> collidingObjects = new List<ViveGrip_Object>();

  void Start () {
    GetComponent<SphereCollider>().isTrigger = true;
  }

  void OnTriggerEnter(Collider other) {
    ViveGrip_Object component = ActiveComponent(other.gameObject);
    if (component == null) { return; }
    collidingObjects.Add(component);
    component.Remember(this);
  }

  void OnTriggerExit(Collider other) {
    ViveGrip_Object component = ActiveComponent(other.gameObject);
    if (component == null) { return; }
    collidingObjects.Remove(component);
    component.Forget(this);
  }

  public GameObject NearestObject() {
    float closestDistance = Mathf.Infinity;
    GameObject touchedObject = null;
    foreach (GameObject gameObject in TouchingGameObjects()) {
      float distance = Vector3.Distance(transform.position, gameObject.transform.position);
      if (distance < closestDistance) {
        touchedObject = gameObject;
        closestDistance = distance;
      }
    }
    return touchedObject;
  }

  ViveGrip_Object ActiveComponent(GameObject gameObject) {
    if (gameObject == null) { return null; } // Happens with Destroy() sometimes
    ViveGrip_Object component = ValidComponent(gameObject.transform);
    if (component == null) {
      component = ValidComponent(gameObject.transform.parent);
    }
    if (component != null) {
      return component;
    }
    return null;
  }

  ViveGrip_Object ValidComponent(Transform transform) {
    if (transform == null) { return null; }
    ViveGrip_Object component = transform.GetComponent<ViveGrip_Grabbable>();
    if (component != null && component.enabled) { return component; }
    component = transform.GetComponent<ViveGrip_Interactable>();
    if (component != null && component.enabled) { return component; }
    return null;
  }

  GameObject[] TouchingGameObjects() {
    return collidingObjects.Select(obj => obj.gameObject).ToArray();
  }
}
