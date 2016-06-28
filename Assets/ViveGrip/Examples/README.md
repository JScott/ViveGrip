## Vive Grip Demo Scene

Load up the scene and press play. It'll give you an idea of what Vive Grip can do with little effort and almost no extra code.

Some things you might want to try:

- Trying to push the heavy box with the light box
- Flipping the bubble gun 360 and catching it on the way down
- Move the light box onto the heavy box without grabbing it

### How to create the...

#### Stack of boxes (easy)

- `ViveGrip_Grabbable`
  - `Anchor.enabled`
  - `Rotation.mode`
- `Rigidbody`

The boxes are cubes with `ViveGrip_Grabbable` attached. This automatically adds a `Rigidbody` and makes it so I can grip them with my controller's grip point. I changed the mass of each so that Unity's physics engine would handle the details of the Iight difference.

By playing with `Anchor.enabled` and `Rotation.mode` I can get different effects with the boxes. I enabled the anchor for the small box to make it feel more like a small object I am gripping in my palm. I also set the rotation mode to `Disabled` on the big box to give it a feeling of being unweildy or difficult to hold on to.

#### Slider (easy)

- `ViveGrip_Grabbable`
  - `Rotation.mode`
- `Rigidbody`

The slider is a cube with `ViveGrip_Grabbable` attached. It's `Rigidbody` is constrained to only move on one axis. To prevent it from going too far, I added two invisible objects with colliders at either end of its movement. The track is just for show and has no collider.

I also set the rotation mode to `Disabled` so that it doesn't bother trying to rotate it with the controller. This isn't strictly necessary due to the constraints but is trivial enough that I might as well.

#### Button (intermediate)

- `ViveGrip_Interactable`
- `BoxCollider`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`

The button is a cube with `ViveGrip_Interactable` attached. It doesn't need to be picked up so it doesn't automatically add a `Rigidbody` and I use the default collider. Instead of being grabbable, any `ViveGripInteractionStart` methods in scripts attached to the object will be called when it's interacted with.

In this case, the attached script will move the button in and back out when triggered. We also ignore the `ViveGrip_GripPoint` parameter because we don't need it for this.

#### Dial and light (intermediate)

- `ViveGrip_Grabbable`
- `Rigidbody`

The dial is a cylinder with `ViveGrip_Grabbable` attached. The `Rigidbody` position is frozen but it rotates freely with a hinge joint. I also set some limits to prevent it from rotating all the way around.

The attached script will read the rotation of the dial and use that to set the light's color.

#### Hands (intermediate)

- `ViveGrip_GripPoint`
- `ViveGripTouchStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripTouchStop(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripGrabStop(ViveGrip_GripPoint gripPoint)`

The hands are a grip point and model attached to the controller tracked objects. To start things off I add a hand model sibling and adjust the grip point anchor to roughly match the palm of the hand.

In order to give visual cues to the player, I change the hand mesh when an object is touched and the hand fades when we grab something. The logic is hooked into the methods that get called on the controller and all its children when starting or stopping a touch or grab. There are also a few edge cases that get handled around the hand-at-rest cue.

#### Lever (advanced)

- `ViveGrip_Grabbable`
  - `Anchor.localPosition`
- `Rigidbody`
- `HingeJoint`

The lever is a model with `ViveGrip_Grabbable` attached. To make sure that it gets gripped by the handle, I set the anchor's local position appropriately. When I enable the anchor, a Gizmo in the Scene view gives me a visual cue as to where the anchor will be positioned.

The lever needs to rotate when its pulled, which I achieve with a hinge joint. It behaves as you would expect by configuring the joint's anchor and axis appropriately and setting some `Rigidbody` constraints.

#### Bubble gun (advanced)

- `ViveGrip_Grabbable`
  - `Snap to Orientation`
  - `Rotation.localOrientation`
- `ViveGrip_Interactable`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripInteractionStop(ViveGrip_GripPoint gripPoint)`

The bubbler gun is a mesh `ViveGrip_Grabbable` attached. It needs to be held properly by the controller so I enable the anchor and set the rotation mode to `Apply Grip and Orientation`. I can give the handle a comfortable grip by setting the local orientation to tilt it forward slightly. I also add `ViveGrip_Interactable` because I want to fire it by pulling the trigger.

Any attached scripts will call `ViveGripInteractionStart` while the interaction button is held and `ViveGripInteractionStop` when we let go. I use this to toggle a boolean so that bubbles are made when the trigger is held. I also make sure that `gripPoint.SomethingHeld()` is true from the provided `ViveGrip_GripPoint` script so that it must be held to fire.
