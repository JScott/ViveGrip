using UnityEngine;

[AddComponentMenu("ViveGrip/Interactable")]
[DisallowMultipleComponent]
public class ViveGrip_Interactable : MonoBehaviour {
  void Start() {
    if (GetComponent<ViveGrip_Highlight>() == null) {
      gameObject.AddComponent<ViveGrip_Highlight>();
    }
  }
}
