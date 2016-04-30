using UnityEngine;
using System.Collections;

// TODO: Grabber -> GripPoint
public class Grabber : MonoBehaviour {
  public float grabRadius = 0.2f;
  public float holdRadius = 0.3f;
  public Shader outlineShader;
  public Color outlineColor = new Color(1f, 0.5f, 0f);
  public bool grabberSphereVisible = false; // TODO: -> visible
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  public SteamVR_TrackedObject attachedDevice;
  private GameObject highlightedObject;
  private Shader oldShader;
  private GrabberSphere grabberSphere;
  private ConfigurableJoint grabberJoint;
  private GameObject jointObject;
  private bool anchored = false;
  private SteamVR_Controller.Device device;

  // TODO: only let one hand grab at a time?

  void Start() {
    GameObject grabberObject = InstantiateGrabberObject();
    grabberSphere = grabberObject.AddComponent<GrabberSphere>();
    grabberSphere.radius = grabRadius;
	}

  void Update() {
    device = GetDevice();
    GameObject touchedObject = grabberSphere.ClosestObject();
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
      Vector3 grabbableAnchorPosition = GrabbableAnchorWorldPosition(grabberJoint.connectedBody.transform);
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
    grabberJoint = JointFactory.JointToConnect(jointObject, desiredObject);
    jointObject.transform.position += AnchorOffset(desiredObject.transform);
  }

  void SetOrientationOf(Rigidbody desiredObject) {
    Grabbable grabbable = desiredObject.gameObject.GetComponent<Grabbable>();
    if (grabbable.snapToOrientation) {
      desiredObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbable.orientation);
    }
  }

  Vector3 AnchorOffset(Transform grabbableTransform) {
    return transform.position - GrabbableAnchorWorldPosition(grabbableTransform);
  }

  void DestroyConnection() {
    grabberJoint.connectedBody.useGravity = true;
    Destroy(jointObject);
    anchored = false;
  }

  Vector3 GrabbableAnchorWorldPosition(Transform desiredObjectTransform) {
    Vector3 anchor = desiredObjectTransform.GetComponent<Grabbable>().anchor;
    return desiredObjectTransform.position + desiredObjectTransform.TransformVector(anchor);
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
    GameObject grabberObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    Renderer renderer = grabberObject.GetComponent<Renderer>();
    renderer.enabled = grabberSphereVisible;
    if (grabberSphereVisible) {
      renderer.material = new Material(Shader.Find("GrabSphere"));
      renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      renderer.receiveShadows = false;
    }
    grabberObject.transform.localScale = Vector3.one * grabRadius;
    grabberObject.transform.position = transform.position;
    grabberObject.transform.SetParent(transform);
    grabberObject.name = "Grabber Sphere";
    return grabberObject;
  }

  bool SomethingHeld() {
    return jointObject != null;
  }
}
