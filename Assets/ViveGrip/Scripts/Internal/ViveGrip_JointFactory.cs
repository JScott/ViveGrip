using UnityEngine;

public static class ViveGrip_JointFactory {
  public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Vector3 anchor, Quaternion desiredRotation) {
    ViveGrip_Grabbable grabbable = desiredObject.gameObject.GetComponent<ViveGrip_Grabbable>();
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
    ViveGrip_JointFactory.SetLinearDrive(joint, desiredObject.mass);
    ViveGrip_JointFactory.ConfigureAnchor(joint, desiredObject, anchor, grabbable.applyGripRotation);
    if (grabbable.applyGripRotation) {
      ViveGrip_JointFactory.SetAngularDrive(joint, desiredObject.mass);
    }
    joint.targetRotation = desiredRotation;
    joint.connectedBody = desiredObject;
    return joint;
  }

  private static void ConfigureAnchor(ConfigurableJoint joint, Rigidbody desiredObject, Vector3 anchor, bool applyGripRotation) {
    joint.autoConfigureConnectedAnchor = false;
    joint.connectedAnchor = desiredObject.transform.InverseTransformVector(anchor);
  }

  private static void SetLinearDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    float maxPower = 70f * mass;
    JointDrive jointDrive = joint.xDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    jointDrive.maximumForce = maxPower;
    joint.xDrive = jointDrive;
    jointDrive = joint.yDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    jointDrive.maximumForce = maxPower;
    joint.yDrive = jointDrive;
    jointDrive = joint.zDrive;
    jointDrive.positionSpring = gripStrength;
    jointDrive.positionDamper = gripSpeed;
    jointDrive.maximumForce = maxPower;
    joint.zDrive = jointDrive;
  }

  private static void SetAngularDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 30f * mass;
    float gripSpeed = 1f * mass;
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
