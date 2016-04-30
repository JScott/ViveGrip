using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
  public float grabRadius = 0.2f;
  public float holdRadius = 0.3f;
  public Shader outlineShader;
  public Color outlineColor;
  public bool grabberSphereVisible = false;
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  public Vector3 defaultAnchor = new Vector3(0, 0, 0.3f);
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
    Debug.Log("Trying to grab "+touchedObject);
    bool shouldConnect = !SomethingHeld() && touchedObject != null && device.GetTouchDown(gripInput);
    Debug.Log(device.GetTouch(gripInput));
    Debug.Log(shouldConnect);
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
      float grabDistance = Vector3.Distance(AnchorDefaultWorldPosition(), grabbableAnchorPosition);
      bool pulledToMiddle = grabDistance < holdRadius;
      anchored = anchored || pulledToMiddle;
      if (anchored && grabDistance > holdRadius) {
        DestroyConnection();
      }
    }
  }

  void UpdateHighlighting(GameObject touchedObject) {
    if (SomethingHeld()) {
      touchedObject = null;
    }
    if (touchedObject == highlightedObject) { return; }
    if (highlightedObject != null && oldShader != null) {
      highlightedObject.GetComponent<Renderer>().material.shader = oldShader;
    }
    highlightedObject = touchedObject;
    if (touchedObject != null) {
      Shader currentShader = highlightedObject.GetComponent<Renderer>().material.shader;
      if (currentShader != outlineShader) {
        oldShader = currentShader;
      }
      highlightedObject.GetComponent<Renderer>().material.shader = outlineShader;
      highlightedObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.0005f);
      highlightedObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
    }
  }

  void CreateConnectionTo(Rigidbody desiredObject) {
    Debug.Log("Connecting to " + desiredObject.gameObject.name);
    jointObject = InstantiateJointObject();
    SetOrientationOf(desiredObject);
    grabberJoint = JointFactory.JointToConnect(jointObject, desiredObject);
    jointObject.transform.position += AnchorOffset(desiredObject.transform);
    Debug.Log(jointObject.transform.position);
  }

  void SetOrientationOf(Rigidbody desiredObject) {
    Debug.Log("Setting orientation");
    Grabbable grabbable = desiredObject.gameObject.GetComponent<Grabbable>();
    if (grabbable.snapToOrientation) {
      desiredObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbable.orientation);
    }
  }

  Vector3 AnchorOffset(Transform grabbableTransform) {
    return AnchorDefaultWorldPosition() - GrabbableAnchorWorldPosition(grabbableTransform);
  }

  void DestroyConnection() {
    grabberJoint.connectedBody.useGravity = true;
    Destroy(jointObject);
    anchored = false;
  }

  Vector3 AnchorDefaultWorldPosition() {
    return transform.position + transform.TransformVector(defaultAnchor);
  }

  Vector3 GrabbableAnchorWorldPosition(Transform desiredObjectTransform) {
    Vector3 anchor = desiredObjectTransform.GetComponent<Grabbable>().anchor;
    return desiredObjectTransform.position + desiredObjectTransform.TransformVector(anchor);
  }

  GameObject InstantiateJointObject() {
    Debug.Log("Making joint object");
    jointObject = new GameObject("Joint Object");
    jointObject.transform.parent = transform;
    jointObject.transform.localPosition = defaultAnchor;
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
    grabberObject.transform.position = AnchorDefaultWorldPosition();
    grabberObject.transform.SetParent(transform.parent);
    Debug.DrawLine(Vector3.zero, transform.parent.position, Color.blue, 20, false);
    Debug.DrawLine(grabberObject.transform.position, transform.parent.position, Color.red, 20, false);
    grabberObject.name = "Grabber Sphere";
    return grabberObject;
  }

  bool SomethingHeld() {
    return jointObject != null;
  }
}
