using UnityEngine;
using System.Collections;

public class ViveGripExample_Button : MonoBehaviour {
  private const float SPEED = 0.1f;
  private float distance;
  private int direction = 1;

  void Start () {
    ResetDistance();
  }

  void OnViveGripInteraction() {
    Destroy(GetComponent<ViveGrip_Interactable>());
    StartCoroutine("Move");
  }

  IEnumerator Move() {
    while (distance > 0) {
      Increment();
      yield return null;
    }
    yield return StartCoroutine("MoveBack");
  }

  IEnumerator MoveBack() {
    direction *= -1;
    ResetDistance();
    while (distance > 0) {
      Increment();
      yield return null;
    }
    direction *= -1;
    ResetDistance();
    gameObject.AddComponent<ViveGrip_Interactable>();
  }

  void Increment() {
    float increment = Time.deltaTime * SPEED;
    increment = Mathf.Min(increment, distance);
    transform.Translate(0, 0, increment * direction);
    distance -= increment;
  }

  void ResetDistance() {
    distance = 0.03f;
  }
}
