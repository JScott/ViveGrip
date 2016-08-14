# Vive Grip Extensions

Event messaging is a powerful tool in Vive Grip that lets you extend and modify fundamental aspects of Vive Grip. Several events will trigger a message being sent to the scripts on the acting controller, its children, and the scene object involved. Methods with the appropriate name will be triggered automatically.

The scripts mentioned here are on the right controller's Grip Point. You can enable them in the Inspector to try them out with that controller.

## How do I extend...

### Grabbing

- `ViveGripGrabStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStop(ViveGrip_GripPoint gripPoint)`

This is sent whenever an object is grabbed or released. `ViveGripExample_ExtendGrab` uses this to enable a toggling grab. If the grip is started and released within the threshold, we manually toggle the grab so that the object isn't dropped.

### Highlighting

- `ViveGripHighlightStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripHighlightStop(ViveGrip_GripPoint gripPoint)`

This is sent whenever object highlighting starts or stops. `ViveGripExample_ExtendHighlight` uses this to highlight the controller instead of the object. It adds the highlight script to a given object and passes on the highlight messages to it. It also disables the default highlighting.

### Touching

- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripTouchStop(ViveGrip_GripPoint gripPoint)`

This is different than highlighting because it doesn't toggle when an object is grabbed.

### Interacting

- `ViveGripInteractStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripInteractStop(ViveGrip_GripPoint gripPoint)`

Vive Grip doesn't do anything before sending these messages. You have full control over what happens when using the interaction button.
