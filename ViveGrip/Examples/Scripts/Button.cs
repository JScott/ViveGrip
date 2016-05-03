using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
  private float speed = 0.1f;
  private float distance = 0.03f;

	void Start () {}

  void OnInteraction() {
    Destroy(GetComponent<ViveGrip_Interactable>());
    StartCoroutine("Move");
  }

  IEnumerator Move() {
    while (distance >= 0) {
      float increment = Time.deltaTime * speed;
      increment = Mathf.Min(increment, distance);
      transform.Translate(0, 0, increment);
      distance -= increment;
      yield return null;
    }
  }
}
