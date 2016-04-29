using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
  public float grabRadius = 0.5f;
  public Shader outline; // TODO: rename
  public Color outlineColor;
  public bool grabberSphereVisible = false;
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  private GameObject highlightedObject;
  private Shader oldShader;
  private GrabberSphere grabberSphere;
  private ConfigurableJoint grabberJoint;
  private GameObject jointObject;
  private bool anchored = false;
  private SteamVR_Controller.Device device;

  private Vector3 defaultAnchor = new Vector3(0, 0, 0.5f);

  void Start() {
    GameObject grabberObject = InstantiateGrabberObjectOn(grabberJoint);
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
      float grabDistance = Vector3.Distance(WorldAnchorDefaultPosition(), grabberJoint.connectedBody.transform.position);
      anchored = anchored || PulledToMiddle(grabDistance);
      float holdRadius = grabRadius; // TODO: variable hold radius
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

  bool PulledToMiddle(float distance) {
    return distance < grabRadius;
  }

  SteamVR_Controller.Device GetDevice() {
    // TODO: assumes that parent is the controller object
    SteamVR_TrackedObject trackedObject = transform.parent.GetComponent<SteamVR_TrackedObject>();
    return SteamVR_Controller.Input((int)trackedObject.index);
  }

  void CreateConnectionTo(Rigidbody desiredObject) {
    jointObject = InstantiateJointObject();
    grabberJoint = jointObject.GetComponent<ConfigurableJoint>();
    grabberJoint.connectedBody = desiredObject;
    grabberJoint.connectedBody.useGravity = false;
    jointObject.transform.position += WorldAnchorDefaultPosition() - desiredObject.transform.position;
    SetJointDrive(grabberJoint, grabberJoint.connectedBody.mass); // TODO: simplify inputs but stay safe?
  }

  void DestroyConnection() {
    grabberJoint.connectedBody.useGravity = true;
    Destroy(jointObject);
    anchored = false;
  }

  Vector3 WorldAnchorDefaultPosition() {
    return transform.position + transform.TransformVector(defaultAnchor);
  }

  Vector3 LocalAnchorDefaultPosition() {
    return Vector3.Scale(defaultAnchor, transform.localScale);
  }

  GameObject InstantiateJointObject() {
    jointObject = new GameObject("Joint Object");
    jointObject.transform.parent = transform;
    jointObject.transform.localPosition = Vector3.zero;
    jointObject.transform.localScale = Vector3.one;
    InstantiateJointOn(jointObject);
    return jointObject;
  }

  ConfigurableJoint InstantiateJointOn(GameObject jointObject) {
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
    jointObject.GetComponent<Rigidbody>().useGravity = false;
    jointObject.GetComponent<Rigidbody>().isKinematic = true;
    joint.xMotion = ConfigurableJointMotion.Limited;
    joint.yMotion = ConfigurableJointMotion.Limited;
    joint.zMotion = ConfigurableJointMotion.Limited;
    joint.angularXMotion = ConfigurableJointMotion.Locked;
    joint.angularYMotion = ConfigurableJointMotion.Locked;
    joint.angularZMotion = ConfigurableJointMotion.Locked;
    joint.anchor = defaultAnchor;
    SoftJointLimit jointLimit = joint.linearLimit;
    jointLimit.limit = 10;
    joint.linearLimit = jointLimit;
    return joint;
  }

  void SetJointDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    JointDrive jointDrive = joint.xDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.xDrive = jointDrive;
    jointDrive = joint.yDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.yDrive = jointDrive;
    jointDrive = joint.zDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.zDrive = jointDrive;
  }

  GameObject InstantiateGrabberObjectOn(ConfigurableJoint joint) {
    GameObject grabberObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    if (!grabberSphereVisible) {
      grabberObject.GetComponent<Renderer>().enabled = false;
    }
    grabberObject.transform.localScale = new Vector3(grabRadius, grabRadius, grabRadius);
    grabberObject.transform.SetParent(transform.parent);
    grabberObject.transform.localPosition = LocalAnchorDefaultPosition();
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
