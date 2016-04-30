using UnityEngine;
using System.Collections;

public class ViveGrip_GripPoint : MonoBehaviour {
  public float grabRadius = 0.2f;
  public float holdRadius = 0.3f;
  public Shader outlineShader;
  public Color outlineColor = new Color(1f, 0.5f, 0f);
  public bool visible = false;
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  public SteamVR_TrackedObject attachedDevice;
  // private GameObject highlightedObject;
  // private Shader oldShader;
  private ViveGrip_TouchDetection touch;
  private ConfigurableJoint joint;
  private GameObject jointObject;
  private bool anchored = false;
  private SteamVR_Controller.Device device;
  private ViveGrip_Highlighter highlighter;

  void Start() {
    GameObject gripSphere = InstantiateGrabberObject();
    touch = gripSphere.AddComponent<ViveGrip_TouchDetection>();
    touch.radius = grabRadius;
    if (outlineShader == null) { outlineShader = Shader.Find("ViveGrip/Outline"); }
    highlighter = new ViveGrip_Highlighter(outlineShader, outlineColor);
	}

  void Update() {
    device = GetDevice();
    GameObject touchedObject = touch.NearestObject();
    HandleGrabbing(touchedObject);
    highlighter.disabled = SomethingHeld();
    highlighter.UpdateFor(touchedObject);
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

  void CreateConnectionTo(Rigidbody desiredObject) {
    jointObject = InstantiateJointObject();
    SetOrientationOf(desiredObject);
    joint = ViveGrip_JointFactory.JointToConnect(jointObject, desiredObject);
    jointObject.transform.position += AnchorOffsetOf(desiredObject);
  }

  void SetOrientationOf(Rigidbody desiredObject) {
    ViveGrip_Grabbable grabbable = desiredObject.gameObject.GetComponent<ViveGrip_Grabbable>();
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
    Vector3 anchor = grabbableObject.GetComponent<ViveGrip_Grabbable>().anchor;
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
      renderer.material = new Material(Shader.Find("ViveGrip/TouchSphere"));
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
