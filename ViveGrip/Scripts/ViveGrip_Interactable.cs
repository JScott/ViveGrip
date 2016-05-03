using UnityEngine;

public class ViveGrip_Interactable : MonoBehaviour {
  void Start() {
    if (GetComponent<Rigidbody>() == null) {
      gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
