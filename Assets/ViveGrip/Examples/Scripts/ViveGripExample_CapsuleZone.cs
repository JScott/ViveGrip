using UnityEngine;
using System.Collections;

public class ViveGripExample_CapsuleZone : MonoBehaviour {
  public Transform capsule;
  private bool entered = false;

	void Start() {}

  void Update() {
    GetComponent<MeshRenderer>().enabled = entered;
  }

  void OnTriggerStay(Collider other) {
    if (CapsuleSeated()) {
      SetEnteredTo(false);
    }
    else {
      if (CapsuleObjectIs(other.gameObject)) { SetEnteredTo(true); }
    }
  }

  void OnTriggerExit(Collider other) {
    if (CapsuleObjectIs(other.gameObject)) { SetEnteredTo(false); }
  }

  void SetEnteredTo(bool state) {
    entered = state;
    capsule.gameObject.GetComponent<ViveGripExample_Capsule>().inZone = state;
  }

  bool CapsuleObjectIs(GameObject gameObject) {
    return gameObject.GetInstanceID() != capsule.gameObject.GetInstanceID();
  }

  bool CapsuleSeated() {
    return capsule.gameObject.GetComponent<ViveGripExample_Capsule>().seated;
  }
}
