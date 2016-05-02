using UnityEngine;
using System.Collections;

public static class ViveGrip_JointFactory {
  public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Quaternion desiredRotation) {
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
    ViveGrip_JointFactory.ConfigureBase(joint);
    ViveGrip_JointFactory.SetLinearDrive(joint, desiredObject.mass);
    ViveGrip_JointFactory.SetAngularDrive(joint, desiredObject.mass);
    joint.SetTargetRotationLocal(desiredRotation, jointObject.transform.localRotation);
    joint.connectedBody = desiredObject;
    joint.connectedBody.useGravity = false;
    return joint;
  }

  private static void ConfigureBase(ConfigurableJoint joint) {
    ConfigurableJointMotion linearMotion = ConfigurableJointMotion.Limited;
    ConfigurableJointMotion angularMotion = ConfigurableJointMotion.Free;
    joint.xMotion = linearMotion;
    joint.yMotion = linearMotion;
    joint.zMotion = linearMotion;
    joint.angularXMotion = angularMotion;
    joint.angularYMotion = angularMotion;
    joint.angularZMotion = angularMotion;
    joint.anchor = Vector3.zero;
    SoftJointLimit jointLimit = joint.linearLimit;
    jointLimit.limit = 10;
    joint.linearLimit = jointLimit;
  }

  private static void SetLinearDrive(ConfigurableJoint joint, float mass) {
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

  private static void SetAngularDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    // joint.rotationDriveMode = RotationDriveMode.Slerp;
    // JointDrive slerpDrive = joint.slerpDrive;
    // slerpDrive.positionSpring = gripStrength;
    // slerpDrive.positionDamper = gripSpeed;
    // joint.slerpDrive = slerpDrive;

    //joint.targetAngularVelocity = Vector3.one * gripSpeed;

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
}
