using UnityEngine;

public static class ViveGrip_JointFactory {
  // Change these in code if you need a stronger or weaker grip
  // The default values are what I've found to be most effective in my experience
  public const float LINEAR_DRIVE_MULTIPLIER = 1f;
  public const float ANGULAR_DRIVE_MULTIPLIER = 1f;

  public static ConfigurableJoint JointToConnect(GameObject jointObject, Rigidbody desiredObject, Quaternion controllerRotation) {
    ViveGrip_Grabbable grabbable = desiredObject.gameObject.GetComponent<ViveGrip_Grabbable>();
    ConfigurableJoint joint = jointObject.AddComponent<ConfigurableJoint>();
    ViveGrip_JointFactory.SetLinearDrive(joint, desiredObject.mass);
    if (grabbable.anchor.enabled) {
      ViveGrip_JointFactory.SetAnchor(joint, desiredObject, grabbable.RotatedAnchor());
    }
    if (grabbable.ApplyGripRotation()) {
      ViveGrip_JointFactory.SetAngularDrive(joint, desiredObject.mass);
    }
    if (grabbable.SnapToOrientation()) {
      ViveGrip_JointFactory.SetTargetRotation(joint, desiredObject, grabbable.rotation.localOrientation, controllerRotation);
    }
    joint.connectedBody = desiredObject;
    return joint;
  }

  private static void SetTargetRotation(ConfigurableJoint joint, Rigidbody desiredObject, Vector3 desiredOrientation, Quaternion controllerRotation) {
    // Undo current rotation, apply the desired orientation, and translate that to controller space
    // ...but in reverse order because thats how Quaternions work
    joint.targetRotation = controllerRotation;
    joint.targetRotation *= Quaternion.Euler(desiredOrientation);
    joint.targetRotation *= Quaternion.Inverse(desiredObject.transform.rotation);
  }

  private static void SetAnchor(ConfigurableJoint joint, Rigidbody desiredObject, Vector3 anchor) {
    joint.autoConfigureConnectedAnchor = false;
    joint.connectedAnchor = desiredObject.transform.InverseTransformVector(anchor);
  }

  private static JointDrive LinearJointDrive(float strength, float damper, float maxForce) {
    JointDrive jointDrive = new JointDrive();
    jointDrive.positionSpring = strength;
    jointDrive.positionDamper = damper;
    jointDrive.maximumForce = maxForce;
    return jointDrive;
  }

  private static JointDrive AngularJointDrive(JointDrive baseJointDrive, float strength, float damper) {
    JointDrive jointDrive = baseJointDrive;
    jointDrive.positionSpring = strength;
    jointDrive.positionDamper = damper;
    return jointDrive;
  }

  private static void SetLinearDrive(ConfigurableJoint joint, float mass) {
    float multiplier = ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER;
    float gripStrength = 3000f * mass * multiplier;
    float gripSpeed = 10f * mass * multiplier;
    float maxPower = 70f * mass * multiplier;
    joint.xDrive = LinearJointDrive(gripStrength, gripSpeed, maxPower);
    joint.yDrive = LinearJointDrive(gripStrength, gripSpeed, maxPower);
    joint.zDrive = LinearJointDrive(gripStrength, gripSpeed, maxPower);
  }

  private static void SetAngularDrive(ConfigurableJoint joint, float mass) {
    float multiplier = ViveGrip_JointFactory.ANGULAR_DRIVE_MULTIPLIER;
    float gripStrength = 300f * mass * multiplier;
    float gripSpeed = 10f * mass * multiplier;
    joint.rotationDriveMode = RotationDriveMode.XYAndZ;
    joint.angularYZDrive = AngularJointDrive(joint.angularYZDrive, gripStrength, gripSpeed);
    joint.angularXDrive = AngularJointDrive(joint.angularXDrive, gripStrength, gripSpeed);
  }
}
