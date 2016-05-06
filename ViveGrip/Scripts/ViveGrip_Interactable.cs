using UnityEngine;

[AddComponentMenu("ViveGrip/Interactable")]
// [RequireComponent (typeof (Rigidbody))]
[DisallowMultipleComponent]
public class ViveGrip_Interactable : MonoBehaviour {
  void Start() {
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
