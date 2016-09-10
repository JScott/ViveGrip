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
  [HideInInspector]
  public ViveGrip_Grabber grabber;
  private ViveGrip_TouchDetection touch;
  private const string GRIP_SPHERE_NAME = "ViveGrip Touch Sphere";
  private bool firmlyGrabbed = false;
  private bool externalGrabTriggered = false;
  private GameObject lastTouchedObject;
  private GameObject lastInteractedObject;

  void Start() {
    controller = GetComponent<ViveGrip_ControllerHandler>();
    grabber = gameObject.AddComponent<ViveGrip_Grabber>();
    GameObject gripSphere = InstantiateTouchSphere();
    touch = gripSphere.AddComponent<ViveGrip_TouchDetection>();
    touch.radius = touchRadius;
  }

  void Update() {
    GameObject touchedObject = TouchedObject();
    HandleTouching(touchedObject);
    HandleGrabbing(touchedObject);
    HandleInteraction(touchedObject);
    HandleFumbling();
    lastTouchedObject = touchedObject;
  }

  void HandleGrabbing(GameObject givenObject) {
    if (!GrabTriggered() && !externalGrabTriggered) { return; }
    externalGrabTriggered = false;
    if (grabber.HoldingSomething()) {
      if (givenObject != null) {
        Message("ViveGripHighlightStart", givenObject);
      }
      DestroyConnection();
    }
    else if (givenObject != null && givenObject.GetComponent<ViveGrip_Grabbable>() != null) {
      Message("ViveGripHighlightStop", givenObject);
      Message("ViveGripGrabStart", givenObject);
    }
  }

  bool GrabTriggered() {
    if (controller == null) { return false; }
    if (inputIsToggle) {
      return controller.Pressed("grab");
    }
    return grabber.HoldingSomething() ? controller.Released("grab") : controller.Pressed("grab");
  }

  void DestroyConnection() {
    firmlyGrabbed = false;
    Message("ViveGripGrabStop", HeldObject());
  }

  void HandleFumbling() {
    if (grabber.HoldingSomething()) {
      float grabDistance = CalculateGrabDistance();
      bool withinRadius = grabDistance <= holdRadius;
      firmlyGrabbed = firmlyGrabbed || withinRadius;
      if (firmlyGrabbed && !withinRadius) {
        DestroyConnection();
      }
    }
  }

  float CalculateGrabDistance() {
    ViveGrip_Grabbable grabbable = grabber.ConnectedGameObject().GetComponent<ViveGrip_Grabbable>();
    Vector3 grabbedAnchorPosition = grabbable.WorldAnchorPosition();
    return Vector3.Distance(transform.position, grabbedAnchorPosition);
  }

  void HandleInteraction(GameObject givenObject) {
    if (grabber.HoldingSomething()) {
      givenObject = grabber.ConnectedGameObject();
    }
    if (givenObject == null || givenObject.GetComponent<ViveGrip_Interactable>() == null) { return; }
    if (controller.Pressed("interact")) {
      lastInteractedObject = givenObject;
      Message("ViveGripInteractionStart", givenObject);
    }
    if (controller.Released("interact")) {
      Message("ViveGripInteractionStop", lastInteractedObject);
      lastInteractedObject = null;
    }
  }

  void HandleTouching(GameObject givenObject) {
    if (GameObjectsEqual(lastTouchedObject, givenObject)) { return; }
    if (lastTouchedObject != null) {
      Message("ViveGripHighlightStop", lastTouchedObject);
      Message("ViveGripTouchStop", lastTouchedObject);
    }
    if (grabber.HoldingSomething()) { return; }
    if (givenObject != null) {
      Message("ViveGripHighlightStart", givenObject);
      Message("ViveGripTouchStart", givenObject);
    }
  }

  bool GameObjectsEqual(GameObject first, GameObject second) {
    if (first == null && second == null) { return true; }
    if (first == null || second == null) { return false; }
    return first.GetInstanceID() == second.GetInstanceID();
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

  public bool TouchingSomething() {
    return TouchedObject() != null;
  }

  public GameObject TouchedObject() {
    return touch.NearestObject();
  }

  public bool HoldingSomething() {
    return grabber.HoldingSomething();
  }

  public GameObject HeldObject() {
    if (!grabber.HoldingSomething()) { return null; }
    return grabber.ConnectedGameObject();
  }

  public void ToggleGrab() {
    externalGrabTriggered = true;
  }

  public GameObject TrackedObject() {
    return controller.trackedObject.gameObject;
  }

  void Message(string name, GameObject objectToMessage) {
    TrackedObject().BroadcastMessage(name, this, SendMessageOptions.DontRequireReceiver);
    if (objectToMessage == null) { return; }
    objectToMessage.SendMessage(name, this, SendMessageOptions.DontRequireReceiver);
  }
}
