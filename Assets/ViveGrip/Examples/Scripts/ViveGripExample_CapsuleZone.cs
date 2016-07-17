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
    if (capsule.gameObject.GetInstanceID() != other.gameObject.GetInstanceID()) { return; }
    SetEnteredTo(!CapsuleSeated());
  }

  void OnTriggerExit(Collider other) {
    if (CapsuleObjectIs(other.gameObject)) { SetEnteredTo(false); }
  }

  void SetEnteredTo(bool state) {
    entered = state;
    capsule.gameObject.GetComponent<ViveGripExample_Capsule>().inZone = state;
  }

  bool CapsuleSeated() {
    return capsule.gameObject.GetComponent<ViveGripExample_Capsule>().seated;
  }
}
