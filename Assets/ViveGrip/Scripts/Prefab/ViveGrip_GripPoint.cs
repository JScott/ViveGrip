using UnityEngine;

[DisallowMultipleComponent]
public class ViveGrip_GripPoint : MonoBehaviour {
  [Tooltip("The distance at which you can touch objects.")]
  public float touchRadius = 0.2f;
  [Tooltip("The distance at which objects will automatically drop.")]
  public float holdRadius = 0.3f;
  [Tooltip("Is the touch radius visible? (Good for debugging)")]
  public bool visible = false;
  [Tooltip("Should the button toggle grabbing?")]
  public bool inputIsToggle = false;
  private Color highlightTint = new Color(0.2f, 0.2f, 0.2f);
  private ViveGrip_ButtonManager button;
  private ViveGrip_TouchDetection touch;
  private ConfigurableJoint joint;
  private GameObject jointObject;
  private bool anchored = false;
  private bool inputPressed = false;
  private GameObject lastTouchedObject;

  void Start() {
    button = GetComponent<ViveGrip_ButtonManager>();
    GameObject gripSphere = InstantiateTouchSphere();
    touch = gripSphere.AddComponent<ViveGrip_TouchDetection>();
    touch.radius = touchRadius;
	}

  void Update() {
    GameObject touchedObject = touch.NearestObject();
    HandleHighlighting(touchedObject);
    HandleGrabbing(touchedObject);
    HandleInteraction(touchedObject);
    HandleFumbling();
    lastTouchedObject = touchedObject;
  }

  void HandleGrabbing(GameObject targetObject) {
    if (targetObject == null) { return; }
    bool shouldConnect = targetObject.GetComponent<ViveGrip_Grabbable>() != null;
    shouldConnect &= !SomethingHeld() && targetObject != null && GrabRequested();
    if (shouldConnect) {
      ViveGrip_Highlight target = GetHighlight(targetObject);
      if (target != null) { target.RemoveHighlighting(); }
      CreateConnectionTo(targetObject.GetComponent<Rigidbody>());
    }
    if (SomethingHeld() && DropRequested()) {
      DestroyConnection();
    }
  }

  bool GrabRequested() {
    return inputIsToggle ? GrabToggleRequested() : button.Pressed("grab");
  }

  bool DropRequested() {
    return inputIsToggle ? GrabToggleRequested() : button.Released("grab");
  }

  bool GrabToggleRequested() {
    bool inputWasPressed = inputPressed;
    inputPressed = button.Holding("grab");
    // TODO: return !inputWasPressed && inputPressed; ?
    if (inputWasPressed) { return false; }
    return inputPressed;
  }

  void HandleInteraction(GameObject targetObject) {
    if (targetObject == null) { return; }
    if (SomethingHeld()) {
      targetObject = joint.connectedBody.gameObject;
    }
    if (targetObject.GetComponent<ViveGrip_Interactable>() == null) { return; }
    if (button.Pressed("interact")) {
      targetObject.SendMessage("OnViveGripInteraction", SomethingHeld(), SendMessageOptions.DontRequireReceiver);
    }
    if (button.Holding("interact")) {
      targetObject.SendMessage("OnViveGripInteractionHeld", SomethingHeld(), SendMessageOptions.DontRequireReceiver);
    }
  }

  void HandleHighlighting(GameObject targetObject) {
    ViveGrip_Highlight last = GetHighlight(lastTouchedObject);
    ViveGrip_Highlight current = GetHighlight(targetObject);
    if (last != null && last != current) {
      last.RemoveHighlighting();
    }
    if (current != null && !SomethingHeld()) {
      current.Highlight(highlightTint);
    }
  }

  ViveGrip_Highlight GetHighlight(GameObject targetObject) {
    if (targetObject == null) { return null; }
    return targetObject.GetComponent<ViveGrip_Highlight>();
  }

  void HandleFumbling() {
    if (SomethingHeld()) {
      Vector3 grabbableAnchorPosition = AnchorWorldPositionOf(joint.connectedBody.gameObject);
      float grabDistance = Vector3.Distance(transform.position, grabbableAnchorPosition);
      bool pulledToMiddle = grabDistance < holdRadius;
      anchored = anchored || pulledToMiddle;
      if (anchored && grabDistance > holdRadius) {
        DestroyConnection();
      }
    }
  }

  void CreateConnectionTo(Rigidbody desiredBody) {
    jointObject = InstantiateJointParent();
    Quaternion desiredRotation = OrientationChangeFor(desiredBody.gameObject);
    Vector3 offset = desiredBody.gameObject.GetComponent<ViveGrip_Grabbable>().anchor;
    joint = ViveGrip_JointFactory.JointToConnect(jointObject, desiredBody, offset, desiredRotation);
  }

  Quaternion OrientationChangeFor(GameObject target) {
    ViveGrip_Grabbable grabbable = target.GetComponent<ViveGrip_Grabbable>();
    if (grabbable.snapToOrientation) {
      // Undo current rotation, apply the orientation, and translate that to controller space (reverse order because Quaternions)
      Quaternion localToController = transform.rotation * Quaternion.Euler(grabbable.localOrientation) * Quaternion.Inverse(target.transform.rotation);
      return localToController;
    }
    return Quaternion.identity;
  }

  void DestroyConnection() {
    Destroy(jointObject);
    anchored = false;
  }

  Vector3 AnchorWorldPositionOf(GameObject grabbableObject) {
    Vector3 anchor = grabbableObject.GetComponent<ViveGrip_Grabbable>().anchor;
    Transform grabbableTransform = grabbableObject.transform;
    return grabbableTransform.position + grabbableTransform.TransformVector(anchor);
  }

  GameObject InstantiateJointParent() {
    jointObject = new GameObject("ViveGrip Joint");
    jointObject.transform.parent = transform;
    jointObject.transform.localPosition = Vector3.zero;
    jointObject.transform.localScale = Vector3.one;
    jointObject.transform.rotation = Quaternion.identity;
    Rigidbody jointRigidbody = jointObject.AddComponent<Rigidbody>();
    jointRigidbody.useGravity = false;
    jointRigidbody.isKinematic = true;
    return jointObject;
  }

  GameObject InstantiateTouchSphere() {
    GameObject gripSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    Renderer sphereRenderer = gripSphere.GetComponent<Renderer>();
    sphereRenderer.enabled = visible;
    if (visible) {
      sphereRenderer.material = new Material(Shader.Find("ViveGrip/TouchSphere"));
      sphereRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      sphereRenderer.receiveShadows = false;
    }
    gripSphere.transform.localScale = Vector3.one * touchRadius;
    gripSphere.transform.position = transform.position;
    gripSphere.transform.SetParent(transform);
    gripSphere.AddComponent<Rigidbody>().isKinematic = true;
    gripSphere.name = "ViveGrip Touch Sphere";
    return gripSphere;
  }

  bool SomethingHeld() {
    return jointObject != null;
  }
}
