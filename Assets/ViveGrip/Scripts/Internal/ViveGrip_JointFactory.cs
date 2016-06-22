using UnityEngine;

public static class ViveGrip_JointFactory {
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

  private static void SetLinearDrive(ConfigurableJoint joint, float mass) {
    float gripStrength = 30000f * mass;
    float gripSpeed = 100f * mass;
    float maxPower = 700f * mass;
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
    float gripStrength = 300f * mass;
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
