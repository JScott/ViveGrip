# Vive Grip Extensions

Event messaging is a powerful tool in Vive Grip that lets you extend and modify fundamental aspects of Vive Grip. In fact, everything that Vive Grip does from grabbing to highlighting occurs through these methods.

The methods are called on the acting controller, its children, and the scene objects involved. The scripts mentioned here are on the right controller's Grip Point in the demo scene. You can enable them in the Inspector to try them out with that controller.

## How do I extend...

### Grabbing

- `ViveGripGrabStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStop(ViveGrip_GripPoint gripPoint)`

This is sent whenever an object is grabbed or released. `ViveGripExample_ExtendGrab` uses this to enable a toggling grab. If the grip is started and released within the threshold, I manually toggle the grab so that the object isn't dropped.

### Highlighting

- `ViveGripHighlightStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripHighlightStop(ViveGrip_GripPoint gripPoint)`

This is sent whenever object highlighting starts or stops. Note that `ViveGripHighlightStop` is called when the object is grabbed.

If you want to do custom highlighting effects, look at `ViveGrip_HighlightEffect`. By implementing this, as `ViveGripExample_ExtendHighlight` does, you can choose it as a highlight effect on a grabbable or interactable. Be sure to make your implementation `public` so that the Inspector dropdown can use it.

### Touching

- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripTouchStop(ViveGrip_GripPoint gripPoint)`

This is similar to highlighting but doesn't change when objects are grabbed.

### Interacting

- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripInteractionStop(ViveGrip_GripPoint gripPoint)`

Vive Grip doesn't do anything before sending these messages. You have full control over what happens when using the interaction button.
