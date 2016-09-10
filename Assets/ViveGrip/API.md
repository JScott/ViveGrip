# Core APIs

```
ViveGrip_GripPoint
ViveGrip_GripPoint.touchRadius
ViveGrip_GripPoint.holdRadius
ViveGrip_GripPoint.visible
ViveGrip_GripPoint.inputIsToggle
ViveGrip_GripPoint.TouchingSomething()
ViveGrip_GripPoint.TouchedObject()
ViveGrip_GripPoint.HoldingSomething()
ViveGrip_GripPoint.HeldObject()
ViveGrip_GripPoint.ToggleGrab()
ViveGrip_GripPoint.TrackedObject()
```

```
ViveGrip_GripPoint.controller
ViveGrip_GripPoint.controller.trackedObject
ViveGrip_GripPoint.controller.Device()
ViveGrip_GripPoint.controller.Pressed(["grab"|"interact"])
ViveGrip_GripPoint.controller.Released(["grab"|"interact"])
ViveGrip_GripPoint.controller.Holding(["grab"|"interact"])
ViveGrip_GripPoint.controller.Vibrate(float milliseconds, float strength)
```

# Event message calling

These methods get called if they're in the scripts of the attached object or children of the triggering grip point's tracked controller.

## Grabbable only

```
void ViveGripGrabStart(ViveGrip_GripPoint gripPoint) {}
void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {}
```

## Interactable only

```
void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint) {}
void ViveGripInteractionStop(ViveGrip_GripPoint gripPoint) {}
```

## Both

```
void ViveGripTouchStart(ViveGrip_GripPoint gripPoint) {}
void ViveGripTouchStop(ViveGrip_GripPoint gripPoint) {}

void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {}
void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {}
```

# Internal APIs

Mostly used by other scripts. Many are either easier to access from core APIs or prone to unexpected behaviour. I put an asterisk (*) next to the ones that might be interesting to you.

```
ViveGrip_Highlighter
ViveGrip_Highlighter.Highlight(Color color)
ViveGrip_Highlighter.RemoveHighlight()
ViveGrip_Highlighter.AddTo(GameObject gameObject) *
```

```
ViveGrip_JointFactory
ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER *
ViveGrip_JointFactory.ANGULAR_DRIVE_MULTIPLIER *
ViveGrip_JointFactory.JointToConnect(GameObject jointObject, Rigidbody desiredObject, Quaternion controllerRotation)
```

```
ViveGrip_GripPoint.grabber
ViveGrip_GripPoint.grabber.jointObject
ViveGrip_GripPoint.grabber.joint
ViveGrip_GripPoint.grabber.ConnectedGameObject()
ViveGrip_GripPoint.grabber.HoldingSomething()
```

```
ViveGrip_TouchDetection
ViveGrip_TouchDetection.radius
ViveGrip_TouchDetection.NearestObject()
```
