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

### Composite Toy (easy)

- `ViveGrip_Grabbable`
- `ViveGrip_Highlighter`
- `ViveGrip_TintChildrenEffect`

Sometimes objects need to be made of multiple renderers or colliders as children. Grabbable will handle "Compound Colliders" by looking in a Collider's parent for a grabbable or interactable if it can't find one in its own object. However, I need to tell Vive Grip to highlight the children of the object as well. I do this by selecting the `ViveGrip_TintChildrenEffect` highlight effect on the grabbable from the Inspector.

### Slider (easy)

- `ViveGrip_Grabbable`
  - `Rotation.mode`
- `Rigidbody`
- `Vibrate`

The slider is a cube with `ViveGrip_Grabbable` attached. It's `Rigidbody` is constrained to only move on one axis. To prevent it from going too far, I added two invisible objects with colliders at either end of its movement. The track is just for show and has no collider.

I also set the rotation mode to `Disabled` so that it doesn't bother trying to rotate it with the controller. This isn't strictly necessary due to the constraints but is trivial enough that I might as well.

To give some sense of weight to the slider as it moves, I leverage the `Vibrate` method on the grip point's controller. By providing the duration in milliseconds at the strength of the vibration (from 0 to 1), I provide some feedback based on how far the slider was moved.

### Switch (easy)

- `ViveGrip_Interactable`
- `BoxCollider`
- `ViveGrip_EventBridge`

The switch leverages the power of Vive Grip events from the comfort of Unity's Inspector. To keep things simple, I'm simply going to trigger the `Flip()` method on the switch to toggle its rotation.

By adding the `ViveGrip_Interactable` script, I allow the object to be highlighted and interacted with. It doesn't need to be picked up so a `Rigidbody` won't be automatically added. Instead, I use the default collider.

Adding the `ViveGrip_EventBridge` script let's me bridge Vive Grip events to custom methods through the Inspector. In this case I select the objects and methods to flip the switch and attach it to an "Interaction Start". You can read EXTENSIONS.md for more detailed information on the events available.

The Button and Bubble Gun examples show when you may want to create an interaction in code instead of through Unity's Inspector.

### Manager (intermediate)

- `ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER`
- `ViveGrip_JointFactory.ANGULAR_DRIVE_MULTIPLIER`
- `ViveGrip_Object.highlightEffect`
- `ViveGrip_Highlighter.enabled`
- `ViveGrip_Highlighter.UpdateEffect`

Vive Grip exposes a few variables that you may want to take advantage of for the whole scene instead of an object-to-object basis. For example, here I change the grip strength and object highlighting.

The grip strength is divided into the "linear" and "angular" drive. In simpler terms, these are the positional and rotational strength of all your grips. By changing these variables, I change the strength of each subsequent grab. The defaults are what have been found to be useful in most situations but there can be many benefits to changing this for your unique needs.

Disabling a highlight on an per-object basis can be done easily by changing the `ViveGrip_Object.highlightEffect`. However, because I need to re-enable it to what it was I simply disable the highlighter script. To be able to select a new highlighter for objects in realtime, I call `ViveGrip_Highlighter.UpdateEffect` when Unity validates the dropdown. I may or may not want to do this when the scene starts so I add a boolean to decide.

### Button (intermediate)

- `ViveGrip_Interactable`
- `BoxCollider`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`
- `Vibrate`

The button is a cube with `ViveGrip_Interactable` attached. As with the Switch, I use the default collider instead of a `Rigidbody`.

The attached script will move the button in and out when triggered. Since it's done in code, I can use the `ViveGrip_GripPoint` parameter to give immediate haptic feedback to the correct controller with `gripPoint.Vibrate(duration, strength)`.

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
- `BoxCollider`

The lever is a model with `ViveGrip_Grabbable` attached. To make sure that it gets gripped by the handle, I set the anchor's local position appropriately. When I enable the anchor, a Gizmo in the Scene view gives me a visual cue as to where the anchor will be positioned.

The lever needs to rotate when its pulled, which I achieve with a hinge joint. It behaves as you would expect by configuring the joint's anchor and axis appropriately and setting some `Rigidbody` constraints.

A script also provides a sense of weight using vibrations, similar to the use in the Slider.

Unlike the other examples with a mesh, this uses Compound Colliders (see: https://docs.unity3d.com/Manual/class-Rigidbody.html) to create the collisions from primitive meshes. Everything is handled for you when the Vive Grip scripts are in the parent of the collider game objects.

### Tar ball (intermediate)

- `ViveGrip_Grabbable`
- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGrip_GripPoint.ToggleGrab()`
- `ViveGrip_GripPoint.HoldingSomething()`
- `ViveGrip_GripPoint.enabled`

Sometimes you want an object that gets grabbed or dropped forcefully. The tar ball shows this off by sticking to any unoccupied grip point on touch and disabling it until it gets shaken off.

When the touch method is triggered, I trigger `ToggleGrab()` which I know will grab the tar ball. To prevent the player from dropping it manually and ending up in a weird state, I store the grip point and disable it. During the update method I check if the speed reaches a certain threshold after being gripped. At that point I enable the grip point again and toggle the grab to drop the tar ball.

### Teleporter (intermediate)

- `ViveGrip_GripPoint.HoldingSomething()`
- `ViveGrip_GripPoint.HeldObject()`

Often people need to teleport while holding an object. This can get complicated, depending on how you teleport, but the idea will be the same as this simple example. `ViveGripExample_Teleporter` will use grip point methods to find held objects and move them when the player moves.

I also make sure that I always teleport the teleporter for the sake of the demo so that the player doesn't lose it by accident.

### Extension Cube (advanced)

- `ViveGripExample_ExtendGrab`
- `ViveGripExample_ExtendHighlight`
- `ViveGrip_GripPoint.ToggleGrab()`
- `ViveGrip_HighlightEffect`

As the extension documentation explains, you can adjust core functionality with scripts that hook into Vive Grip events. This cube has scripts that use these events to transform the default grab setting into a toggle for the cube and change how the cube is highlighted.

For the grab, I simply toggle the grab when the object is released, similar to the tar ball. There's also a small countdown to make sure that simply releasing the grab after some time won't toggle it.

I create a new highlight effect by implementing `ViveGrip_HighlightEffect`. The implementation will have methods to take the highlighted `GameObject` to `Start` and `Stop` the effect. This could be selected from the Inspector but I replace it in code here. The `ViveGrip_Highlighter` handles the rest.

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
