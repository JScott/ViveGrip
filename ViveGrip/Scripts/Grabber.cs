using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
  public float grabRadius = 0.2f;
  public float holdRadius = 0.3f;
  public Shader outline; // TODO: rename
  public Color outlineColor;
  public bool grabberSphereVisible = false;
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  public Vector3 defaultAnchor = new Vector3(0, 0, 0.5f);
  private GameObject highlightedObject;
  private Shader oldShader;
  private GrabberSphere grabberSphere;
  private ConfigurableJoint grabberJoint;
  private GameObject jointObject;
  private bool anchored = false;
  private SteamVR_Controller.Device device;

  void Start() {
    GameObject grabberObject = InstantiateGrabberObjectOn(grabberJoint);
    grabberSphere = grabberObject.AddComponent<GrabberSphere>();
    grabberSphere.radius = grabRadius;
    JointFactory.defaultAnchor = defaultAnchor;
	}

  void Update() {
    device = GetDevice(); // TODO: what happens with both controllers?
    GameObject touchedObject = grabberSphere.ClosestObject();
    HandleGrabbing(touchedObject);
    UpdateHighlighting(touchedObject);
    HandleFumbling();
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
      float grabDistance = Vector3.Distance(AnchorDefaultWorldPosition(), grabbableAnchorPosition);
      bool pulledToMiddle = grabDistance < holdRadius;
      anchored = anchored || pulledToMiddle;
      if (anchored && grabDistance > holdRadius) { // TODO: togglable please
        DestroyConnection();
      }
    }
  }

  void UpdateHighlighting(GameObject touchedObject) {
    if (touchedObject == highlightedObject) { return; }
    if (highlightedObject != null) {
      highlightedObject.GetComponent<Renderer>().material.shader = oldShader;
    }
    highlightedObject = touchedObject;
    if (touchedObject != null) {
      oldShader = highlightedObject.GetComponent<Renderer>().material.shader;
      highlightedObject.GetComponent<Renderer>().material.shader = outline;
      highlightedObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.0005f);
      highlightedObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
    }
  }

  SteamVR_Controller.Device GetDevice() {
    // TODO: assumes that parent is the controller object
    SteamVR_TrackedObject trackedObject = transform.parent.GetComponent<SteamVR_TrackedObject>();
    return SteamVR_Controller.Input((int)trackedObject.index);
  }

  void CreateConnectionTo(Rigidbody desiredObject) {
    jointObject = InstantiateJointObject(desiredObject);
    grabberJoint = jointObject.GetComponent<ConfigurableJoint>();
    grabberJoint.connectedBody = desiredObject;
    grabberJoint.connectedBody.useGravity = false;
    jointObject.transform.position += GrabbableOffset(desiredObject.transform);
  }

  Vector3 GrabbableOffset(Transform grabbableTransform) {
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

  Vector3 AnchorDefaultLocalPosition() {
    return Vector3.Scale(defaultAnchor, transform.localScale);
  }

  Vector3 GrabbableAnchorWorldPosition(Transform desiredObjectTransform) {
    Vector3 anchor = desiredObjectTransform.GetComponent<Grabbable>().anchor;
    return desiredObjectTransform.position + desiredObjectTransform.TransformVector(anchor);
  }

  GameObject InstantiateJointObject(Rigidbody desiredObject) {
    jointObject = new GameObject("Joint Object");
    jointObject.transform.parent = transform;
    jointObject.transform.localPosition = defaultAnchor;// Vector3.zero;
    jointObject.transform.localScale = transform.localScale;//Vector3.one;
    jointObject.transform.localRotation = transform.localRotation;
    JointFactory.AddJointTo(jointObject, desiredObject.mass);
    jointObject.GetComponent<Rigidbody>().useGravity = false;
    jointObject.GetComponent<Rigidbody>().isKinematic = true;
    return jointObject;
  }

  GameObject InstantiateGrabberObjectOn(ConfigurableJoint joint) {
    GameObject grabberObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    if (!grabberSphereVisible) {
      grabberObject.GetComponent<Renderer>().enabled = false;
    }
    grabberObject.transform.localScale = new Vector3(grabRadius, grabRadius, grabRadius);
    grabberObject.transform.SetParent(transform.parent);
    grabberObject.transform.localPosition = AnchorDefaultLocalPosition();
    grabberObject.name = "Grabber Sphere";
    return grabberObject;
  }

  bool SomethingHeld() {
    return jointObject != null;
  }
}


  // static void SetTargetRotationInternal (ConfigurableJoint joint, Quaternion targetRotation, Quaternion startRotation)
  // {
  //   // Calculate the rotation expressed by the joint's axis and secondary axis
  //   var right = joint.axis;
  //   var forward = Vector3.Cross (joint.axis, joint.secondaryAxis).normalized;
  //   var up = Vector3.Cross (forward, right).normalized;
  //   Quaternion worldToJointSpace = Quaternion.LookRotation (forward, up);

  //   // Transform into world space
  //   Quaternion resultRotation = Quaternion.Inverse (worldToJointSpace);

  //   // Counter-rotate and apply the new local rotation.
  //   // Joint space is the inverse of world space, so we need to invert our value
  //   // world: resultRotation *= startRotation * Quaternion.Inverse (targetRotation);
  //   resultRotation *= Quaternion.Inverse (targetRotation) * startRotation;

  //   // Transform back into joint space
  //   resultRotation *= worldToJointSpace;

  //   // Set target rotation to our newly calculated rotation
  //   joint.targetRotation = resultRotation;
  // }
