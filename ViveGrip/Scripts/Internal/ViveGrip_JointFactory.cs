using UnityEngine;

public static class ViveGrip_JointFactory {
  public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Vector3 offset, Quaternion desiredRotation) {
    ViveGrip_Grabbable grabbable = desiredObject.gameObject.GetComponent<ViveGrip_Grabbable>();
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
    ViveGrip_JointFactory.SetLinearDrive(joint, desiredObject.mass);
    ViveGrip_JointFactory.ConfigureAnchor(joint, offset, grabbable.applyGripRotation);
    if (grabbable.applyGripRotation) {
      ViveGrip_JointFactory.SetAngularDrive(joint, desiredObject.mass);
    }
    joint.targetRotation = desiredRotation;
    ViveGrip_JointFactory.Attach(joint, desiredObject);
    return joint;
  }

  private static void ConfigureAnchor(ConfigurableJoint joint, Vector3 offset, bool applyGripRotation) {
    if (applyGripRotation) { // TODO: Why is this important when we rotate? We pass in a local offset...
      joint.autoConfigureConnectedAnchor = false;
      joint.connectedAnchor = offset;
    }
    else {
      joint.anchor = offset;
    }
  }

    private static void SetLinearDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 3000f * mass;
    float gripSpeed = 10f * mass;
    float maxPower = 50f * mass;
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

  private static void ConfigureRotation(ConfigurableJoint joint, Rigidbody desiredObject, Quaternion desiredRotation) {
    joint.targetRotation = desiredRotation;
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

  private static void Attach(ConfigurableJoint joint, Rigidbody desiredObject) {
    joint.connectedBody = desiredObject;
  }
}
