# Core APIs

```
ViveGrip_GripPoint
ViveGrip_GripPoint.touchRadius
ViveGrip_GripPoint.holdRadius
ViveGrip_GripPoint.visible
ViveGrip_GripPoint.inputIsToggle
ViveGrip_GripPoint.GRIP_SPHERE_NAME
ViveGrip_GripPoint.TouchingSomething()
ViveGrip_GripPoint.TouchedObject()
ViveGrip_GripPoint.HoldingSomething()
ViveGrip_GripPoint.HeldObject()
ViveGrip_GripPoint.ToggleGrab()
ViveGrip_GripPoint.UpdateRadius(float touchRadius, float holdRadius)
```

```
ViveGrip_GripPoint.controller
ViveGrip_GripPoint.controller.trackedObject
ViveGrip_GripPoint.controller.Device()
ViveGrip_GripPoint.controller.Pressed(ViveInput)
ViveGrip_GripPoint.controller.Released(ViveInput)
ViveGrip_GripPoint.controller.Holding(ViveInput)
ViveGrip_GripPoint.controller.Vibrate(int milliseconds, float strength [0.0-1.0])
ViveGrip_GripPoint.controller.TrackedObject()
```

```
ViveGrip_Grabbable.anchor.enabled
ViveGrip_Grabbable.anchor.localPosition
ViveGrip_Grabbable.rotation.mode
ViveGrip_Grabbable.rotation.localOrientation
ViveGrip_Grabbable.AttachedGripPoints()
```

```
ViveGrip_Highlighter
ViveGrip_Highlighter.effect
ViveGrip_Highlighter.Highlight()
ViveGrip_Highlighter.RemoveHighlight()
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

# Other APIs

Mostly used by other scripts. Many are either easier to access from core APIs or prone to unexpected behaviour. I put an asterisk (*) next to the ones that might be interesting to you.

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
ViveGrip_GripPoint.grabber.RebuildJoint() *
```

```
ViveGrip_TouchDetection
ViveGrip_TouchDetection.radius
ViveGrip_TouchDetection.NearestObject()
```
