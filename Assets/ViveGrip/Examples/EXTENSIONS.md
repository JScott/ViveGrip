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

This is sent whenever object highlighting starts or stops. `ViveGripExample_ExtendHighlight` uses this to highlight the controller as well as the object. It simply captures the base color on starting and lightens it when the controller highlights an object. Note that `ViveGripHighlightStop` is called when the object is grabbed.

Another option would be to manually add `ViveGrip_Highlighter` as it leverages these methods to do the default highlighting for objects. However, it doesn't play nice with the way I fade the example hands in and out. It may be useful to try this in your scene if the conditions are right.

### Touching

- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripTouchStop(ViveGrip_GripPoint gripPoint)`

This is similar to highlighting but doesn't change when objects are grabbed.

### Interacting

- `ViveGripInteractStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripInteractStop(ViveGrip_GripPoint gripPoint)`

Vive Grip doesn't do anything before sending these messages. You have full control over what happens when using the interaction button.
