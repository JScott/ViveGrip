using UnityEngine;

public class Bubbler : MonoBehaviour {
  public GameObject bubble;
	private float maxSize = 0.2f;
  private float minSize = 0.1f;
  private float speed = 5f;
  private float cooldown = 0f;

  void Start() {}

  void Update() {
    if (cooldown > 0) {
      cooldown -= Time.deltaTime;
    }
  }

  void OnViveGripInteractionHeld(bool held) {
    if (!held) { return; }
    if (cooldown <= 0) {
      Vector3 location = transform.position + (transform.forward*0.2f) + (transform.up*0.1f);
      GameObject instance = (GameObject)Instantiate(bubble, location, Quaternion.identity);
      float size = Random.Range(minSize, maxSize);
      instance.transform.localScale = Vector3.one * size;
      instance.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
      cooldown = 0.1f;
    }
  }
}
