## Vive Grip Demo Scene

Load up the scene and press play. It'll give you an idea of what Vive Grip can do with little effort and almost no extra code.

Some things you might want to try:

- Trying to push the heavy box with the light box
- Tossing the bubble gun 360 and catching it on the way down
- Move the light box onto the heavy box without grabbing it

### How to create the...

#### Stack of boxes (easy)

- `ViveGrip_Grabbable`
  - `Snap to Anchor`
  - `Apply Grip Rotation`
- `Rigidbody`

The boxes are cubes with `ViveGrip_Grabbable` attached. This automatically adds a `Rigidbody` and makes it so I can grip them with my controller's grip point. I changed the mass of each so that Unity's physics engine would handle the details of the Iight difference.

I also wanted the heavy box to be unwieldy so I turned off `Apply Grip Rotation` and `Snap to Anchor`. This makes it hang off your grip wherever you grab it instead of snapping the position and rotation to the controller.

#### Slider (easy)

- `ViveGrip_Grabbable`
  - `Apply Grip Rotation`
- `Rigidbody`

The slider is a cube with `ViveGrip_Grabbable` attached. It's `Rigidbody` is constrained to only move on one axis. `Apply Grip Rotation` is turned off because I don't want it to try to rotate at all. To prevent it from going too far, I added two invisible objects with colliders at either end of its movement.

The track is just for show and has no collider. You'll get some weird behaviour if the slider is colliding with the wall, however.

#### Button (intermediate)

- `ViveGrip_Interactable`
- `BoxCollider`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`

The button is a cube with `ViveGrip_Interactable` attached. It doesn't need to be picked up so it doesn't automatically add a `Rigidbody` and I use the default collider. Instead of being grabbable, any `ViveGripInteractionStart` methods in scripts attached to the object will be called when its interacted with.

In this case, the attached script will move the button in and back out when triggered. We also ignore the provided `ViveGrip_GripPoint` object because we don't need it for this.

#### Dial and light (intermediate)

- `ViveGrip_Grabbable`
- `Rigidbody`

The dial is a cylinder with `ViveGrip_Grabbable` attached. The `Rigidbody` position is frozen but it rotates freely with a hinge joint. I also set some limits to prevent it from rotating all the way around.

The attached script will read the rotation of the dial and use that to set the light's color.

#### Lever (advanced)

- `ViveGrip_Grabbable`
  - `Anchor`
- `Rigidbody`
- `HingeJoint`

The lever is a model with `ViveGrip_Grabbable` attached. To make sure that it gets gripped by the handle, I set the local anchor point's Y value. An easy way to find the position you want is to parent an empty object, move it, and then copy its relative position.

The lever needs to rotate when its pulled, which I achieve with a hinge joint. I set the joint's anchor to the bottom of the mesh, set the axis of rotation, and configure a few motors to get it moving properly. I also set a few `Rigidbody` constraints so that it behaves as you would expect.

The holder for the lever is just for show and has no colliders.

#### Bubble gun (advanced)

- `ViveGrip_Grabbable`
  - `Snap to Orientation`
  - `Local Orientation`
- `ViveGrip_Interactable`
- `ViveGripInteractionStart(ViveGrip_GripPoint gripPoint)`
- `ViveGripInteractionStop(ViveGrip_GripPoint gripPoint)`

The bubbler gun is a mesh `ViveGrip_Grabbable` attached. It needs to be held properly by the controller so I enable `Snap To Orienatation` and give it a `Local Orientation` that tilts it forward slightly for comfort. I also add `ViveGrip_Interactable` because I want to fire it by interacting with it.

Any attached scripts will call `ViveGripInteractionStart` while the interaction button is held and `ViveGripInteractionStop` when we let go. I use this to toggle a boolean so that bubbles are made when I hold the trigger down. I also make sure that `gripPoint.SomethingHeld()` is true from the provided `ViveGrip_GripPoint` script so that it can't be fired from the ground.
