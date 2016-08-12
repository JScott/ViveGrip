using UnityEngine;
using System.Collections;

public class ViveGrip_Grabber : MonoBehaviour {
	void Start () {}

  void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {
    gripPoint.jointObject = InstantiateJointParent();
    GrabWith(gripPoint);
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    Destroy(gripPoint.jointObject);
  }

  void GrabWith(ViveGrip_GripPoint gripPoint) {
    Rigidbody desiredBody = gripPoint.TouchedObject().GetComponent<Rigidbody>();
    desiredBody.gameObject.GetComponent<ViveGrip_Grabbable>().GrabFrom(transform.position);
    gripPoint.joint = ViveGrip_JointFactory.JointToConnect(gripPoint.jointObject, desiredBody, transform.rotation);
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
}
