using UnityEngine;

public class ViveGripExample_Bubbler : MonoBehaviour {
  public GameObject bubble;
	private float maxSize = 0.2f;
  private float minSize = 0.1f;
  private float speed = 5f;
  private float cooldown = 0f;
  private bool bubbling = false;

  void Start() {}

  void Update() {
    if (!bubbling) { return; }
    if (cooldown > 0) {
      cooldown -= Time.deltaTime;
    }
    else {
      CreateBubble();
      cooldown = 0.1f;
    }
  }

  void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint) {
    if (gripPoint.HoldingSomething()) { bubbling = true; }
  }

  void ViveGripInteractionStop() {
    bubbling = false;
    cooldown = 0f;
  }

  void CreateBubble() {
    Vector3 location = transform.position + (transform.forward*0.2f);
    GameObject instance = (GameObject)Instantiate(bubble, location, Quaternion.identity);
    float size = Random.Range(minSize, maxSize);
    instance.transform.localScale = Vector3.one * size;
    instance.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
  }
}
