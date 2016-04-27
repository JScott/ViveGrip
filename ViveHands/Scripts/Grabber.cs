using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
	public Vector3 anchor = new Vector3(0,0,1f);
  public float grabRadius = 0.5f;
  public Shader outline;
  private Shader oldShader;
  // TODO: set outline in script with Shader.Find
  private GameObject currentObject;
  private GrabberSphere grabberSphere;

  void Start () {
    anchor = Vector3.Scale(anchor, transform.GetComponent<Renderer>().bounds.size/2);
    GameObject grabberObject = InstantiateGrabberObject();
    grabberSphere = grabberObject.AddComponent<GrabberSphere>();
    grabberSphere.radius = grabRadius;
	}

  GameObject InstantiateGrabberObject() {
    GameObject grabberObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    grabberObject.GetComponent<Renderer>().enabled = false;
    grabberObject.transform.localScale = new Vector3(grabRadius, grabRadius, grabRadius);
    grabberObject.transform.SetParent(transform.parent);
    grabberObject.transform.localPosition = anchor;
    grabberObject.name = "GrabberSphere";
    return grabberObject;
  }

	void Update () {
    GameObject touchedObject = grabberSphere.ClosestObject();
    if (touchedObject != currentObject) {
      if (currentObject != null) {
        currentObject.GetComponent<Renderer>().material.shader = oldShader;
      }
      currentObject = touchedObject;
      if (touchedObject != null) {
        oldShader = currentObject.GetComponent<Renderer>().material.shader;
        currentObject.GetComponent<Renderer>().material.shader = outline;
      }
    }
	}
}
