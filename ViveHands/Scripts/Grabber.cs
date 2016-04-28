using UnityEngine;
using System.Collections;

public class Grabber : MonoBehaviour {
	public Vector3 anchor = new Vector3(0,0,1f);
  public float grabRadius = 0.5f;
  public Shader outline;
  public bool grabberSphereVisible = false;
  private Shader oldShader;
  // TODO: set outline in script with Shader.Find
  private GameObject currentObject;
  private GrabberSphere grabberSphere;
  private ConfigurableJoint joint;

  public Rigidbody CONNECTEDOBJECT;

  void Start () {
    anchor = Vector3.Scale(anchor, transform.GetComponent<Renderer>().bounds.size/2);
    GameObject grabberObject = InstantiateGrabberObject();
    grabberSphere = grabberObject.AddComponent<GrabberSphere>();
    grabberSphere.radius = grabRadius;

    joint = InstantiateJoint();
    joint.connectedBody = CONNECTEDOBJECT;
    joint.connectedBody.useGravity = false;
    Vector3 positionDifference = transform.position - joint.connectedBody.transform.position; // Move to controller
    positionDifference += Vector3.Scale(joint.anchor, transform.GetComponent<Renderer>().bounds.size); // Offset by anchor
    joint.targetPosition = positionDifference;
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
    jointDrive.positionSpring = 10000; // TODO: the higher the better but Mathf.Infinity breaks it...
    // jointDrive.positionDamper = 1;
    joint.xDrive = jointDrive;
    jointDrive = joint.yDrive;
    jointDrive.positionSpring = 10000;
    // jointDrive.positionDamper = 1;
    joint.yDrive = jointDrive;
    jointDrive = joint.zDrive;
    jointDrive.positionSpring = 10000;
    // jointDrive.positionDamper = 1;
    joint.zDrive = jointDrive;
    return joint;
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
