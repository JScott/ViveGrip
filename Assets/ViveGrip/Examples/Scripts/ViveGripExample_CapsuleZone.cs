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
    if (CapsuleIs(other.gameObject)) {
      SetEnteredTo(!CapsuleSeated());
    }
  }

  void OnTriggerExit(Collider other) {
    if (CapsuleIs(other.gameObject)) {
      SetEnteredTo(false);
    }
  }

  void SetEnteredTo(bool state) {
    entered = state;
    capsule.gameObject.GetComponent<ViveGripExample_Capsule>().inZone = state;
  }

  bool CapsuleSeated() {
    return capsule.gameObject.GetComponent<ViveGripExample_Capsule>().seated;
  }

  bool CapsuleIs(GameObject gameObject) {
    return capsule.gameObject.GetInstanceID() == gameObject.GetInstanceID();
  }
}
