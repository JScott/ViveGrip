using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
	public Vector3 anchor = new Vector3(0,0,1f);
  public float grabRadius = 0.5f;
  public Shader outline;
  public bool grabberSphereVisible = false;
  public ulong gripInput = SteamVR_Controller.ButtonMask.Grip;
  private Shader oldShader;
  // TODO: set outline in script with Shader.Find
  private GameObject currentObject;
  private GrabberSphere grabberSphere;
  private ConfigurableJoint joint;
  private bool anchored = false;
  private SteamVR_Controller.Device device;

  public Rigidbody CONNECTEDOBJECT;

  void Awake() {
    //device = GetDevice();
  }

  void Start() {
    anchor = Vector3.Scale(anchor, transform.GetComponent<Renderer>().bounds.size/2);
    GameObject grabberObject = InstantiateGrabberObject();
    grabberSphere = grabberObject.AddComponent<GrabberSphere>();
    grabberSphere.radius = grabRadius;
    joint = InstantiateJoint();

    //Connect(joint, CONNECTEDOBJECT);
	}

  void Update() {
    device = GetDevice();

    GameObject touchedObject = grabberSphere.ClosestObject();
    if (!HoldingSomething() && device.GetTouchDown(gripInput)) {
      Connect(joint, touchedObject.GetComponent<Rigidbody>());
    }

    UpdateHighlighting(touchedObject);

    if (HoldingSomething() && device.GetTouchUp(gripInput)) {
      Disconnect(joint);
    }

    if (HoldingSomething()) {
      float grabDistance = Vector3.Distance(WorldAnchorPosition(), joint.connectedBody.transform.position);
      anchored = anchored || PulledToMiddle(grabDistance);
      if (anchored && grabDistance > grabRadius) { // TODO: togglable please
        Debug.Log(grabDistance + " > " + grabRadius);
        Debug.Log(touchedObject + " // " + joint.connectedBody);
        Disconnect(joint);
      }
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

  void Connect(ConfigurableJoint joint, Rigidbody connectedObject) {
    joint.connectedBody = connectedObject;
    joint.connectedBody.useGravity = false;
    Vector3 positionDifference = WorldAnchorPosition() - joint.connectedBody.transform.position; // Move to controller
    joint.targetPosition = positionDifference;
    Debug.Log("Connected to " + connectedObject.gameObject.name);
  }

  void Disconnect(ConfigurableJoint joint) {
    Debug.Log(joint);
    Debug.Log(joint.connectedBody);
    joint.connectedBody.useGravity = true;
    joint.connectedBody = null;
    joint.targetPosition = Vector3.zero; // TODO: not really needed...
    anchored = false;
    Debug.Log("Disconnected");
  }

  Vector3 WorldAnchorPosition() {
    return transform.position + ScaledAnchorPosition();
  }

  Vector3 ScaledAnchorPosition() { // TODO: combine back with WorldAnchorPosition?
    return Vector3.Scale(joint.anchor, transform.GetComponent<Renderer>().bounds.size);
  }

  ConfigurableJoint InstantiateJoint() {
    ConfigurableJoint joint = GetComponent<ConfigurableJoint>();
    // ConfigurableJoint joint = parent.AddComponent<ConfigurableJoint>();
    // joint.anchor = new Vector3(0, 0, 0);
    joint.xMotion = ConfigurableJointMotion.Limited;
    joint.yMotion = ConfigurableJointMotion.Limited;
    joint.zMotion = ConfigurableJointMotion.Limited;
    joint.angularXMotion = ConfigurableJointMotion.Locked;
    joint.angularYMotion = ConfigurableJointMotion.Locked;
    joint.angularZMotion = ConfigurableJointMotion.Locked;

    // SoftJointLimitSpring springLimit = joint.linearLimitSpring;
    // springLimit.spring = 10;
    // springLimit.damper = 5;
    // joint.linearLimitSpring = springLimit;

    JointDrive jointDrive = joint.xDrive;
    jointDrive.positionSpring = 10000f; // TODO: the higher the better but Mathf.Infinity breaks it...
    jointDrive.positionDamper = 1f;
    joint.xDrive = jointDrive;
    jointDrive = joint.yDrive;
    jointDrive.positionSpring = 10000f;
    jointDrive.positionDamper = 1f;
    joint.yDrive = jointDrive;
    jointDrive = joint.zDrive;
    jointDrive.positionSpring = 10000f;
    jointDrive.positionDamper = 1f;
    joint.zDrive = jointDrive;

    //joint.breakForce = 0.001f; // TODO: var
    // TODO: break at a distance, not a force
    return joint;
  }

  GameObject InstantiateGrabberObject() {
    GameObject grabberObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    if (!grabberSphereVisible) {
      grabberObject.GetComponent<Renderer>().enabled = false;
    }
    grabberObject.transform.localScale = new Vector3(grabRadius, grabRadius, grabRadius);
    grabberObject.transform.SetParent(transform.parent);
    grabberObject.transform.localPosition = anchor;
    grabberObject.name = "GrabberSphere";
    return grabberObject;
  }

  void UpdateHighlighting(GameObject touchedObject) {
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

  bool HoldingSomething() {
    return joint.connectedBody != null;
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
