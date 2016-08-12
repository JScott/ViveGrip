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
  [HideInInspector]
  public Color tintColor = new Color(0.2f, 0.2f, 0.2f);
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
    HandleHighlighting(touchedObject);
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
        Message("ViveGripHighlightStart");
      }
      DestroyConnection();
    }
    else if (givenObject != null && givenObject.GetComponent<ViveGrip_Grabbable>() != null) {
      Message("ViveGripHighlightStop");
      Message("ViveGripGrabStart");
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
    Message("ViveGripGrabStop", HeldObject()); // TODO: what if I don't specify an object here?
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
      Message("ViveGripInteractionStart");
    }
    if (controller.Released("interact")) {
      Message("ViveGripInteractionStop", lastInteractedObject);
      lastInteractedObject = null;
    }
  }

  void HandleHighlighting(GameObject givenObject) {
    if (GameObjectsEqual(lastTouchedObject, givenObject)) { return; }
    Message("ViveGripHighlightStop", lastTouchedObject);
    Message("ViveGripTouchStop", lastTouchedObject);
    if (grabber.HoldingSomething()) { return; }
    Message("ViveGripHighlightStart");
    Message("ViveGripTouchStart");
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

  void Message(string name, GameObject objectToMessage = null) {
    // Debug.Log(name + " -- " + TrackedObject());
    TrackedObject().BroadcastMessage(name, this, SendMessageOptions.DontRequireReceiver); // TODO: can I get away with Send here for something like the hands?
    objectToMessage = objectToMessage ?? TouchedObject();
    if (objectToMessage == null) { return; }
    // Debug.Log(Time.time + "\t-- " + name + "\t-- " + objectToMessage);
    objectToMessage.SendMessage(name, this, SendMessageOptions.DontRequireReceiver);
  }
}
