using UnityEngine;

[AddComponentMenu("ViveGrip/Interactable")]
[DisallowMultipleComponent]
public class ViveGrip_Interactable : MonoBehaviour {
  void Start() {
    gameObject.AddComponent<ViveGrip_Highlight>();
  }
}
