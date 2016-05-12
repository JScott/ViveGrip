using UnityEngine;

public class ViveGripExample_Dial : MonoBehaviour {
  public Transform attachedLight;

	void Start () {}

	void Update () {
    HingeJoint joint = GetComponent<HingeJoint>();
    float g = (joint.angle + 90) / 180;
    float r = 1 - g;
    Color newColor = new Color(r, g, 0);
    attachedLight.gameObject.GetComponent<Renderer>().material.color = newColor;
    attachedLight.GetChild(0).GetComponent<Light>().color = newColor;
  }
}
