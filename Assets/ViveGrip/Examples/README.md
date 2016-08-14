# Vive Grip Demo Scene

Load up the scene and press play. It'll give you an idea of what Vive Grip can do with little effort and almost no extra code.

Some things you might want to try:

- Trying to push the heavy box with the light box
- Flipping the bubble gun 360 and catching it on the way down
- Move the light box onto the heavy box without grabbing it

See EXTENSIONS.md for more examples on how to use code hooks to modify the behaviour of Vive Grip.

## How to create the...

### Stack of boxes (easy)

- `ViveGrip_Grabbable`
  - `Anchor.enabled`
  - `Rotation.mode`
- `Rigidbody`

The boxes are cubes with `ViveGrip_Grabbable` attached. This automatically adds a `Rigidbody` and makes it so I can grip them with my controller's grip point. I changed the mass of each so that Unity's physics engine would handle the details of the Iight difference.

By playing with `Anchor.enabled` and `Rotation.mode` I can get different effects with the boxes. I enabled the anchor for the small box to make it feel more like a small object I am gripping in my palm. I also set the rotation mode to `Disabled` on the big box to give it a feeling of being unweildy or difficult to hold on to.

### Slider (easy)

- `ViveGrip_Grabbable`
  - `Rotation.mode`
- `Rigidbody`
- `Vibrate`

The slider is a cube with `ViveGrip_Grabbable` attached. It's `Rigidbody` is constrained to only move on one axis. To prevent it from going too far, I added two invisible objects with colliders at either end of its movement. The track is just for show and has no collider.

I also set the rotation mode to `Disabled` so that it doesn't bother trying to rotate it with the controller. This isn't strictly necessary due to the constraints but is trivial enough that I might as well.

To give some sense of weight to the slider as it moves, I leverage the `Vibrate` method on the grip point's controller. By providing the duration in milliseconds at the strength of the vibration (from 0 to 1), I provide some feedback based on how far the slider was moved.

### Button (intermediate)

- `ViveGrip_Interactable`
- `BoxCollider`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`
- `Vibrate`

The button is a cube with `ViveGrip_Interactable` attached. It doesn't need to be picked up so it doesn't automatically add a `Rigidbody` and I use the default collider. Instead of being grabbable, any `ViveGripInteractionStart` methods in scripts attached to the object will be called when it's interacted with.

In this case, the attached script will move the button in and back out when triggered. I use the `ViveGrip_GripPoint` parameter to do give immediate haptic feedback.

### Dial and light (intermediate)

- `ViveGrip_Grabbable`
- `Rigidbody`

The dial is a cylinder with `ViveGrip_Grabbable` attached. The `Rigidbody` position is frozen but it rotates freely with a hinge joint. I also set some limits to prevent it from rotating all the way around.

The attached script will read the rotation of the dial and use that to set the light's color.

### Hands (intermediate)

- `ViveGrip_GripPoint`
- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripTouchStop(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStop(ViveGrip_GripPoint gripPoint)`

The hands are a grip point and model attached to the controller tracked objects. To start things off I add a hand model sibling and adjust the grip point anchor to roughly match the palm of the hand.

In order to give visual cues to the player, I change the hand mesh when an object is touched and the hand fades when something is grabbed. The logic is hooked into the methods that get called on the controller and all its children when starting or stopping a touch or grab. There are also a few edge cases that get handled around the hand-at-rest cue.

### Lever (intermediate)

- `ViveGrip_Grabbable`
  - `Anchor.localPosition`
- `Rigidbody`
- `HingeJoint`
- `Vibrate`

The lever is a model with `ViveGrip_Grabbable` attached. To make sure that it gets gripped by the handle, I set the anchor's local position appropriately. When I enable the anchor, a Gizmo in the Scene view gives me a visual cue as to where the anchor will be positioned.

The lever needs to rotate when its pulled, which I achieve with a hinge joint. It behaves as you would expect by configuring the joint's anchor and axis appropriately and setting some `Rigidbody` constraints.

A script also provides a sense of weight using vibrations, similar to the use in the Slider.

### Tar ball (intermediate)

- `ViveGrip_Grabbable`
- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGrip_GripPoint.ToggleGrab()`
- `ViveGrip_GripPoint.HoldingSomething()`
- `ViveGrip_GripPoint.enabled`

Sometimes you want an object that gets grabbed or dropped forcefully. The tar ball shows this off by sticking to any unoccupied grip point on touch and disabling it until it gets shaken off.

When the touch method is triggered, I trigger `ToggleGrab()` which I know will grab the tar ball. To prevent the player from dropping it manually and ending up in a weird state, I store the grip point and disable it. During the update method I check if the speed reaches a certain threshold after being gripped. At that point I enable the grip point again and toggle the grab to drop the tar ball.

### Door (advanced)

- `ViveGrip_Grabbable`
- `HingeJoint`
- `FixedJoint`
- `ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER`

The door is very similar to the Lever with some important distinctions. Instead of the whole object being the grabbable, the handle is grabbable and connected to the hinged body with a fixed joint.

You may encounter slight jittering due to grip strength as the handle tries to go to your grip but stay attached to the door. A script handles the rotation by setting it manually based on the door's hinge rotation. Depending on your application, you might also adjust the positional strength of all grips with `ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER` to your preference.

### Floating Capsule (advanced)

- `ViveGrip_Grabbable`
- `ViveGripGrabStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStop(ViveGrip_GripPoint gripPoint)`

With just the grab messaging I create drop zones that objects snap to when dropped, similar to what you see throughout Job Simulator. If you have particular placement of objects like a key in a lock or CD in a tray then this design technique can make a smooth experience.

Two game objects are needed: the grabbable and it's zone. The grabbable object is the same as the most simple usage except variables to know if it's seated and in the zone. These are `seated`, used internally to float when seated, and `inZone`, modified externally to know if it should seat itself when dropped. The zone object exists to set `inZone` to true if the capsule isn't seated and is touching the zone. I also use its Mesh Renderer to visually indicate that dropping the capsule will seat it.

### Bubble gun (advanced)

- `ViveGrip_Grabbable`
  - `Snap to Orientation`
  - `Rotation.localOrientation`
- `ViveGrip_Interactable`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripInteractionStop(ViveGrip_GripPoint gripPoint)`
- `Vibrate`

The bubbler gun is a mesh `ViveGrip_Grabbable` attached. It needs to be held properly by the controller so I enable the anchor and set the rotation mode to `Apply Grip and Orientation`. I can give the handle a comfortable grip by setting the local orientation to tilt it forward slightly. I also add `ViveGrip_Interactable` because I want to fire it by pulling the trigger.

Any attached scripts will call `ViveGripInteractionStart` while the interaction button is held and `ViveGripInteractionStop` when let go. I use this to toggle a boolean so that bubbles are made when the trigger is held. I also make sure that `gripPoint.SomethingHeld()` is true from the provided `ViveGrip_GripPoint` script so that it must be held to fire. Haptic feedback is also triggered for each bubble fired.
