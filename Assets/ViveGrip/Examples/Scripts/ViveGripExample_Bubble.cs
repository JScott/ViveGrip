using UnityEngine;

public class ViveGripExample_Bubble : MonoBehaviour {
	private float life = 5f;
  private float bounces = 2;

  void Start() {
    life += Random.Range(0f, 4f);
  }

  void Update() {
    life -= Time.deltaTime;
    if (life < 0) { Destroy(gameObject); }
  }

  void OnCollisionEnter() {
    bounces -= 1;
    if (bounces < 0) { Destroy(gameObject); }
  }
}
