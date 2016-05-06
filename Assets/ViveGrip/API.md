# API

## `ViveGrip_GripPoint`

This script is used to mark the point that grabs objects and all the details regarding that.

### Attached Device

The `SteamVR_TrackedObject` that this grip is attached to.

### Grab Radius

The distance at which this grip can grab objects.

### Hold Radius

The distance at which objects will be dropped when forced away.

### Outline Shader

The shader used to outline objects. It will default to `ViveGrip/Outline` if none provided.

### Outline Color

The colour passed to the Outline Shader as `_OutlineColor`.

### Visible

Should the grip radius be visible? This can be very helpful for troubleshooting and development.

### Input

Which Vive controller input should trigger grabbing?

### Input is Toggle

Should the grip be a toggle instead of held?

## `ViveGrip_Grabbable`

### Anchor

The position in local space that a `ViveGrip_GripPoint` will grab.

### Snap to Orientation

Should the object snap to a given orientation when grabbed?

### Orientation

The orientation that will be snapped to if `Snap to Orientation` is set.
