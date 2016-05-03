using UnityEngine;
using System.Collections;

public static class ViveGrip_JointFactory {
  public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Vector3 offset, Quaternion desiredRotation) {
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();

    ViveGrip_JointFactory.SetLinearDrive(joint, desiredObject.mass);
    ViveGrip_JointFactory.ConfigureBase(joint, offset);

    ViveGrip_JointFactory.SetAngularDrive(joint, desiredObject.mass);
    Quaternion currentRotation = desiredObject.transform.rotation;
    joint.SetTargetRotationLocal(desiredRotation, currentRotation);

    joint.connectedBody = desiredObject;
    joint.connectedBody.useGravity = false;
    return joint;
  }

  private static void ConfigureBase(ConfigurableJoint joint, Vector3 offset) {
    ConfigurableJointMotion linearMotion = ConfigurableJointMotion.Free;
    ConfigurableJointMotion angularMotion = ConfigurableJointMotion.Free;
    joint.xMotion = linearMotion;
    joint.yMotion = linearMotion;
    joint.zMotion = linearMotion;
    joint.angularXMotion = angularMotion;
    joint.angularYMotion = angularMotion;
    joint.angularZMotion = angularMotion;
    joint.autoConfigureConnectedAnchor = false;
    joint.connectedAnchor = offset;
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
