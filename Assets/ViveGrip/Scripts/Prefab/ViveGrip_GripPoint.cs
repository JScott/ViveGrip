using UnityEngine;
using Valve.VR;

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
  [HideInInspector]
  public ViveGrip_ControllerHandler controller;
  public const string GRIP_SPHERE_NAME = "ViveGrip Touch Sphere";
  private ViveGrip_TouchDetection touch;
  private Color highlightTint = new Color(0.2f, 0.2f, 0.2f);
  private ConfigurableJoint joint;
  private GameObject jointObject;
  private bool firmlyGrabbed = false;
  private bool externalGrabTriggered = false;
  private Vector3 grabbedAt;
  private GameObject lastTouchedObject;
  private GameObject lastInteractedObject;

  void Start() {
    controller = GetComponent<ViveGrip_ControllerHandler>();
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

  void HandleGrabbing(GameObject touchedObject) {
    if (!GrabTriggered() && !externalGrabTriggered) { return; }
    externalGrabTriggered = false;
    if (HoldingSomething()) {
      if (touchedObject != null) {
        GetHighlight(touchedObject).Highlight(highlightTint);
      }
      DestroyConnection();
    }
    else if (touchedObject != null && touchedObject.GetComponent<ViveGrip_Grabbable>() != null) {
      GetHighlight(touchedObject).RemoveHighlighting();
      CreateConnectionTo(touchedObject.GetComponent<Rigidbody>());
    }
  }

  bool GrabTriggered() {
    if (controller == null) { return false; }
    if (inputIsToggle) {
      return controller.Pressed("grab");
    }
    return HoldingSomething() ? controller.Released("grab") : controller.Pressed("grab");
  }

  void HandleInteraction(GameObject touchedObject) {
    if (HoldingSomething()) {
      touchedObject = joint.connectedBody.gameObject;
    }
    if (touchedObject != null) {
      if (touchedObject.GetComponent<ViveGrip_Interactable>() == null) { return; }
      if (controller.Pressed("interact")) {
        lastInteractedObject = touchedObject;
        Message("ViveGripInteractionStart");
      }
    }
    if (controller.Released("interact")) {
      Message("ViveGripInteractionStop", lastInteractedObject);
      lastInteractedObject = null;
    }
  }

  void HandleHighlighting(GameObject touchedObject) {
    ViveGrip_Highlight last = GetHighlight(lastTouchedObject);
    ViveGrip_Highlight current = GetHighlight(touchedObject);
    if (last != current) {
      if (last != null) {
        last.RemoveHighlighting();
        Message("ViveGripTouchStop", last.gameObject);
      }
      if (current != null && !HoldingSomething()) {
        current.Highlight(highlightTint);
        Message("ViveGripTouchStart");
      }
    }
  }

  ViveGrip_Highlight GetHighlight(GameObject touchedObject) {
    if (touchedObject == null) { return null; }
    return touchedObject.GetComponent<ViveGrip_Highlight>();
  }

  void HandleFumbling() {
    if (HoldingSomething()) {
      float grabDistance = CalculateGrabDistance();
      bool withinRadius = grabDistance <= holdRadius;
      firmlyGrabbed = firmlyGrabbed || withinRadius;
      if (firmlyGrabbed && !withinRadius) {
        DestroyConnection();
      }
    }
  }

  float CalculateGrabDistance() {
    ViveGrip_Grabbable grabbable = joint.connectedBody.gameObject.GetComponent<ViveGrip_Grabbable>();
    Vector3 grabbedAnchorPosition = grabbable.WorldAnchorPosition();
    return Vector3.Distance(transform.position, grabbedAnchorPosition);
  }

  void CreateConnectionTo(Rigidbody desiredBody) {
    jointObject = InstantiateJointParent();
    desiredBody.gameObject.GetComponent<ViveGrip_Grabbable>().GrabFrom(transform.position);
    joint = ViveGrip_JointFactory.JointToConnect(jointObject, desiredBody, transform.rotation);
    Message("ViveGripGrabStart");
  }

  void DestroyConnection() {
    GameObject lastObject = HeldObject();
    Destroy(jointObject);
    firmlyGrabbed = false;
    Message("ViveGripGrabStop", lastObject);
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
    gripSphere.layer = gameObject.layer;
    gripSphere.name = GRIP_SPHERE_NAME;
    return gripSphere;
  }

  public bool HoldingSomething() {
    return jointObject != null;
  }

  public bool TouchingSomething() {
    return touch.NearestObject() != null;
  }

  public GameObject HeldObject() {
    if (!HoldingSomething()) { return null; }
    return jointObject.GetComponent<ConfigurableJoint>().connectedBody.gameObject;
  }

  public void ToggleGrab() {
    externalGrabTriggered = true;
  }

  GameObject TrackedObject() {
    return controller.trackedObject.gameObject;
  }

  void Message(string name, GameObject touchedObject = null) {
    TrackedObject().BroadcastMessage(name, this, SendMessageOptions.DontRequireReceiver);
    touchedObject = touchedObject ?? touch.NearestObject();
    if (touchedObject == null) { return; }
    touchedObject.SendMessage(name, this, SendMessageOptions.DontRequireReceiver);
  }
}
