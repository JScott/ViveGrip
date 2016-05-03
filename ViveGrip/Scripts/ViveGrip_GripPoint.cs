using UnityEngine;
using System.Collections;

public class ViveGrip_GripPoint : MonoBehaviour {
  public float grabRadius = 0.2f;
  public float holdRadius = 0.3f;
  public bool visible = false;
  public bool inputIsToggle = false;
  private Color highlightTint = new Color(0.2f, 0.2f, 0.2f);
  private ViveGrip_ButtonManager button;
  private ViveGrip_TouchDetection touch;
  private ConfigurableJoint joint;
  private GameObject jointObject;
  private bool anchored = false;
  private bool inputPressed = false;
  private GameObject lastTouchedObject;
  private bool grabbedObjectHadGravity = false;

  void Start() {
    button = GetComponent<ViveGrip_ButtonManager>();
    GameObject gripSphere = InstantiateTouchSphere();
    touch = gripSphere.AddComponent<ViveGrip_TouchDetection>();
    touch.radius = grabRadius;
	}

  void Update() {
    GameObject touchedObject = touch.NearestObject();
    HandleGrabbing(touchedObject);
    HandleInteraction(touchedObject);
    HandleHighlighting(touchedObject);
    HandleFumbling();
    lastTouchedObject = touchedObject;
  }

  void HandleGrabbing(GameObject targetObject) {
    bool shouldConnect = !SomethingHeld() && targetObject != null && GrabRequested();
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
    if (inputIsToggle) { return GrabToggleRequested(); }
    else { return button.Pressed("grab"); }
  }

  bool DropRequested() {
    if (inputIsToggle) { return GrabToggleRequested(); }
    else { return button.Released("grab"); }
  }

  bool GrabToggleRequested() {
    bool inputWasPressed = inputPressed;
    inputPressed = button.Holding("grab");
    if (inputWasPressed) { return false; }
    else { return inputPressed; }
  }

  void HandleInteraction(GameObject targetObject) {
    if (targetObject == null || !button.Pressed("interact")) { return; }
    if (SomethingHeld()) {
      targetObject = joint.connectedBody.gameObject;
    }
    targetObject.SendMessage("OnInteraction", SomethingHeld(), SendMessageOptions.DontRequireReceiver);
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
    grabbedObjectHadGravity = desiredBody.useGravity;
    desiredBody.useGravity = false;
    jointObject = InstantiateJointParent();
    Quaternion desiredRotation = DesiredLocalOrientationFor(desiredBody.gameObject);
    // Vector3 offset = AnchorOffsetOf(desiredBody);
    Vector3 offset = desiredBody.gameObject.GetComponent<ViveGrip_Grabbable>().anchor;
    // joint = ViveGrip_JointFactory.JointToConnect(jointObject, desiredBody, offset, desiredRotation);
    joint = new ViveGrip_JointConnection(jointObject, desiredBody, offset, desiredRotation).Joint();
    //jointObject.transform.position += AnchorOffsetOf(desiredBody);
  }

  Quaternion DesiredLocalOrientationFor(GameObject target) {
    ViveGrip_Grabbable grabbable = target.GetComponent<ViveGrip_Grabbable>();
    if (grabbable.snapToOrientation) {
      target.transform.rotation = transform.rotation; // Rotations are hard so we cheat
      return target.transform.localRotation * Quaternion.Euler(grabbable.orientation);
    }
    else {
      return target.transform.localRotation;
    }
  }

  Vector3 AnchorOffsetOf(Rigidbody rigidbody) {
    return transform.position - AnchorWorldPositionOf(rigidbody.gameObject);
  }

  void DestroyConnection() {
    joint.connectedBody.useGravity = grabbedObjectHadGravity;
    Destroy(jointObject);
    anchored = false;
  }

  Vector3 AnchorWorldPositionOf(GameObject grabbableObject) {
    Vector3 anchor = grabbableObject.GetComponent<ViveGrip_Grabbable>().anchor;
    Transform transform = grabbableObject.transform;
    return transform.position + transform.TransformVector(anchor);
  }

  GameObject InstantiateJointParent() {
    jointObject = new GameObject("ViveGrip Joint");
    jointObject.transform.parent = transform;
    jointObject.transform.localPosition = Vector3.zero;
    jointObject.transform.localScale = Vector3.one;
    jointObject.transform.rotation = transform.rotation;
    Rigidbody rigidbody = jointObject.AddComponent<Rigidbody>();
    rigidbody.useGravity = false;
    rigidbody.isKinematic = true;
    return jointObject;
  }

  GameObject InstantiateTouchSphere() {
    GameObject gripSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    Renderer renderer = gripSphere.GetComponent<Renderer>();
    renderer.enabled = visible;
    if (visible) {
      renderer.material = new Material(Shader.Find("ViveGrip/TouchSphere"));
      renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
      renderer.receiveShadows = false;
    }
    gripSphere.transform.localScale = Vector3.one * grabRadius;
    gripSphere.transform.position = transform.position;
    gripSphere.transform.SetParent(transform);
    gripSphere.name = "ViveGrip Touch Sphere";
    return gripSphere;
  }

  bool SomethingHeld() {
    return jointObject != null;
  }
}
