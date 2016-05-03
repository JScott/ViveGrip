using UnityEngine;
using System.Collections;

public class ViveGrip_JointConnection {
  private ConfigurableJoint joint;

  public ViveGrip_JointConnection(GameObject jointObject, Rigidbody desiredObject, Vector3 offset, Quaternion desiredRotation) {
    joint = jointObject.AddComponent<ConfigurableJoint>();
    SetLinearDrive(desiredObject.mass);
    SetAngularDrive(desiredObject.mass);
    ConfigureAnchor(offset);
    ConfigureRotation(desiredObject, desiredRotation);
    Attach(desiredObject);
  }

  public ConfigurableJoint Joint() {
    return joint;
  }

  private void ConfigureAnchor(Vector3 offset) {
    joint.autoConfigureConnectedAnchor = false;
    joint.connectedAnchor = offset;
  }

  private void SetLinearDrive(float mass) {
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

  private void ConfigureRotation(Rigidbody desiredObject, Quaternion desiredRotation) {
    Quaternion currentRotation = desiredObject.transform.rotation;
    joint.SetTargetRotationLocal(desiredRotation, currentRotation);
  }

  private void SetAngularDrive(float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    joint.rotationDriveMode = RotationDriveMode.XYAndZ;
    JointDrive jointDrive = joint.angularYZDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.angularYZDrive = jointDrive;
    jointDrive = joint.angularXDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    joint.angularXDrive = jointDrive;
  }

  private void Attach(Rigidbody desiredObject) {
    joint.connectedBody = desiredObject;
    joint.connectedBody.useGravity = false;
  }
}
