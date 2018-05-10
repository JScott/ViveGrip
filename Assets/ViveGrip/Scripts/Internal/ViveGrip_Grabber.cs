using UnityEngine;
using System.Collections;

public class ViveGrip_Grabber : MonoBehaviour {
  public GameObject jointObject;
  public ConfigurableJoint joint;

  void Start () {}

  void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    jointObject = InstantiateJointParent();
    GrabWith(gripPoint);
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    Destroy(jointObject);
  }

  void GrabWith(ViveGrip_GripPoint gripPoint) {
    Rigidbody desiredBody = gripPoint.TouchedObject().GetComponent<Rigidbody>();
    desiredBody.gameObject.GetComponent<ViveGrip_Grabbable>().GrabFrom(transform.position);
    joint = ViveGrip_JointFactory.JointToConnect(jointObject, desiredBody, transform.rotation);
  }

  GameObject InstantiateJointParent() {
    GameObject newJointObject = new GameObject("ViveGrip Joint");
    newJointObject.transform.parent = transform;
    newJointObject.transform.localPosition = Vector3.zero;
    newJointObject.transform.localScale = Vector3.one;
    newJointObject.transform.rotation = Quaternion.identity;
    Rigidbody jointRigidbody = newJointObject.AddComponent<Rigidbody>();
    jointRigidbody.useGravity = false;
    jointRigidbody.isKinematic = true;
    return newJointObject;
  }

  public GameObject ConnectedGameObject() {
    return joint.connectedBody.gameObject;
  }

  public bool HoldingSomething() {
    return jointObject != null;
  }

  public void RebuildJoint() {
    ViveGrip_GripPoint gripPoint = GetComponent<ViveGrip_GripPoint>();
    ViveGripGrabStop(gripPoint);
    ViveGripGrabStart(gripPoint);
  }
}
