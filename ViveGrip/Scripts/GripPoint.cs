using UnityEngine;
using System.Collections;

public class GripPoint : MonoBehaviour {
  public float grabRadius = 0.2f;
  public float holdRadius = 0.3f;
  public Shader outlineShader;
  public Color outlineColor = new Color(1f, 0.5f, 0f);
  public bool visible = false;
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  public SteamVR_TrackedObject attachedDevice;
  private GameObject highlightedObject;
  private Shader oldShader;
  private TouchDetection touch;
  private ConfigurableJoint joint;
  private GameObject jointObject;
  private bool anchored = false;
  private SteamVR_Controller.Device device;

  void Start() {
    GameObject gripSphere = InstantiateGrabberObject();
    touch = gripSphere.AddComponent<TouchDetection>();
    touch.radius = grabRadius;
    if (outlineShader == null) { outlineShader = Shader.Find("Outlined/Diffuse"); }
	}

  void Update() {
    device = GetDevice();
    GameObject touchedObject = touch.NearestObject();
    HandleGrabbing(touchedObject);
    UpdateHighlighting(touchedObject);
    HandleFumbling();
  }

  SteamVR_Controller.Device GetDevice() {
    SteamVR_TrackedObject trackedObject = attachedDevice;
    return SteamVR_Controller.Input((int)trackedObject.index);
  }

  void HandleGrabbing(GameObject touchedObject) {
    bool shouldConnect = !SomethingHeld() && touchedObject != null && device.GetTouchDown(gripInput);
    if (shouldConnect) {
      CreateConnectionTo(touchedObject.GetComponent<Rigidbody>());
    }
    if (SomethingHeld() && device.GetTouchUp(gripInput)) {
      DestroyConnection();
    }
  }

  void HandleFumbling() {
    if (SomethingHeld()) {
      Vector3 grabbableAnchorPosition = AnchorWorldPositionOf(joint.connectedBody.gameObject);
      float grabDistance = Vector3.Distance(transform.position, grabbableAnchorPosition);
      bool pulledToMiddle = grabDistance < holdRadius;
      anchored = anchored || pulledToMiddle;
      if (anchored && grabDistance > holdRadius) {
        DestroyConnection();
      }
    }
  }

  void UpdateHighlighting(GameObject touchedObject) {
    if (touchedObject == highlightedObject) { return; }
    if (highlightedObject != null) { RemoveHighlighting(); }
    if (!SomethingHeld() && touchedObject != null) { Highlight(touchedObject); }
  }

  void RemoveHighlighting() {
    if (oldShader != null) {
      highlightedObject.GetComponent<Renderer>().material.shader = oldShader;
    }
    highlightedObject = null;
  }

  void Highlight(GameObject gameObject) {
    highlightedObject = gameObject;
    Shader currentShader = highlightedObject.GetComponent<Renderer>().material.shader;
    if (currentShader != outlineShader) {
      oldShader = currentShader;
    }
    highlightedObject.GetComponent<Renderer>().material.shader = outlineShader;
    highlightedObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.0005f);
    highlightedObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
  }

  void CreateConnectionTo(Rigidbody desiredObject) {
    jointObject = InstantiateJointObject();
    SetOrientationOf(desiredObject);
    joint = JointFactory.JointToConnect(jointObject, desiredObject);
    jointObject.transform.position += AnchorOffsetOf(desiredObject);
  }

  void SetOrientationOf(Rigidbody desiredObject) {
    Grabbable grabbable = desiredObject.gameObject.GetComponent<Grabbable>();
    if (grabbable.snapToOrientation) {
      desiredObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbable.orientation);
    }
  }

  Vector3 AnchorOffsetOf(Rigidbody rigidbody) {
    return transform.position - AnchorWorldPositionOf(rigidbody.gameObject);
  }

  void DestroyConnection() {
    joint.connectedBody.useGravity = true;
    Destroy(jointObject);
    anchored = false;
  }

  Vector3 AnchorWorldPositionOf(GameObject grabbableObject) {
    Vector3 anchor = grabbableObject.GetComponent<Grabbable>().anchor;
    Transform transform = grabbableObject.transform;
    return transform.position + transform.TransformVector(anchor);
  }

  GameObject InstantiateJointObject() {
    jointObject = new GameObject("Joint Object");
    jointObject.transform.parent = transform;
    jointObject.transform.localPosition = Vector3.zero;
    jointObject.transform.localScale = Vector3.one;
    jointObject.transform.rotation = transform.rotation;
    jointObject.AddComponent<Rigidbody>();
    jointObject.GetComponent<Rigidbody>().useGravity = false;
    jointObject.GetComponent<Rigidbody>().isKinematic = true;
    return jointObject;
  }

  GameObject InstantiateGrabberObject() {
    GameObject gripSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    Renderer renderer = gripSphere.GetComponent<Renderer>();
    renderer.enabled = visible;
    if (visible) {
      renderer.material = new Material(Shader.Find("GrabSphere"));
      renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      renderer.receiveShadows = false;
    }
    gripSphere.transform.localScale = Vector3.one * grabRadius;
    gripSphere.transform.position = transform.position;
    gripSphere.transform.SetParent(transform);
    gripSphere.name = "Grip Sphere";
    return gripSphere;
  }

  bool SomethingHeld() {
    return jointObject != null;
  }
}
