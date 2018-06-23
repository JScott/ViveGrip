# Change Log
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]

## [2.12.1] - 2018-06-23
### Fixed
- `ViveGrip_ControllerHandler` now finds `Device()` after `Start`

## [2.12.0] - 2018-05-10
### Added
- `ViveGrip_Grabbable.AttachedGripPoints()`
- `ViveGrip_Grabber.RebuildJoint()`

### Changed
- Updated documentation (SteamVR supports WinMR now)

## [2.11.1] - 2018-02-08
### Added
- Support for Unity 2017.3
- Support for SteamVR v1.2.3

### Removed
- `ViveGrip_Object` highlighting `OnEnable()`

### Fixed
- Handling `Destroy()` interference in `ViveGrip_TouchDetection` loop
- Repeat objects in `ViveGrip_TouchDetection`

## [2.11.0] - 2017-06-28
### Added
- `ViveGrip_HighlightEffect` for custom highlight effects
- `ViveGrip_TintEffect` and `ViveGrip_TintChildrenEffect`
- Composite Toy in the demo scene
- Teleporter in the demo scene

### Changed
- Overhauled `ViveGrip_Highlighter` and adjacent code
- Grabbables and Interactables now subclass a `ViveGrip_Object` class
- `ViveGripExample_Manager` lets you change the highlight effects scene-wide at runtime

### Fixed
- Highlighting no longer fall apart when there's no `Renderer` on an Object
- Highlighting now accounts for multiple controllers being used at once
- Tint highlighting works with transparency

## [2.10.0] - 2017-06-10
### Added
- Support for SteamVR's Interaction System
- A variable to disable the highlight on a grabbable or interactable from the Inspector
- API documentation for grabbables and interactables
- An example Manager class

### Changed
- `TrackedObject()` was moved to `ViveGrip_ControllerHandler`
- `TrackedObject()` on `ViveGrip_GripPoint` will stop working in the next version
- A more natural example hand grip for Oculus Touch

## [2.9.1] - 2017-06-01
### Fixed
- ViveGrip_ControllerHandler.Pressed now only triggers once for ViveInput.Both

## [2.9.0] - 2017-05-31
### Changed
- "grab" and "interact" are now proper enums instead of strings
- Exposed ViveGrip_GripPoint.GRIP_SPHERE_NAME
- ViveGrip_ControllerHandler grab and interact variable names and options

## [2.8.1] - 2017-04-22
### Fixed
- Controllers not showing up because of a SteamVR bug

### Changed
- Exposed the `ViveGrip_JointFactory` drive multipliers as public

## [2.8.0] - 2017-03-12
### Added
- [Compound Colliders](https://docs.unity3d.com/Manual/class-Rigidbody.html) are supported (PR 15)

## [2.7.0] - 2017-02-20
### Changed
- `ViveGrip_GripPoint.controller.Vibrate(int milliseconds, float strength [0.0-1.0])`
- Minor code improvements
### Added
- `ViveGrip_GripPoint.UpdateRadius(float touch, float hold)`

## [2.6.1] - 2017-01-10
### Fixed
- `ViveGrip_ControllerHandler.Device()` will return null when the device isn't ready

## [2.6.0] - 2016-11-11
### Added
- `ViveGrip_EventBridge` lets you connect methods to Vive Grip events through the Inspector
- The Switch example to show off the Event Bridge
- The Extension Cube example to show off extending grabbing and highlighting in code
### Fixed
- Core Vive Grip scripts now obey Unity's `enabled` flags
### Changed
- Moved releases to their own directory
- Tweaked the documentation

## [2.5.0] - 2016-09-10
### Added
- API.md to try to map out all the bits whether they have examples or not
- Links to the tutorial videos in the README

### Changed
- Made `gripPoint.controller.Device()` public for ease of use

### Fixed
- Converted errant tabs into spaces

## [2.4.0] - 2016-08-14
### Changed
- `ViveGrip_GripPoint` is now solely responsible for messaging instead of acting on state change
- Moved the tint color to `ViveGrip_Highlighter`

### Added
- `ViveGripHighlightStart` and `ViveGripHighlightStop`
- `ViveGrip_Grabber` now does the grabbing functionality that the grip point used to do
- EXTENSIONS.md and example extensions to better explain how event messaging works

### Fixed
- Don't add a highlighter if there's no renderer to work with

## [2.3.0] - 2016-07-16
### Added
- A floating capsule example to show off snap zone functionality

## [2.2.0] - 2016-07-14
### Added
- Grip spheres now inherit the layer of the grip point that spawns them, enabling better collision control
- `gripPoint.HeldObject()` to get the held GameObject
- `gripPoint.ToggleGrab()` to grab and release in script
- A tar ball example to show off grab toggling

### Fixed
- Clean up a few grip point variables

## [2.1.1] - 2016-07-08
### Fixed
- Build no longer breaks on `import UnityEditor`

## [2.1.0] - 2016-07-03
### Added
- Door example

### Fixed
- Cleaned up repeated `ViveGrip_JointFactory` code

### Changed
- Decreased the grip position and rotation strength by half (**update your object mass!**)

## [2.0.0] - 2016-06-28
### Fixed
- Objects will highlight when letting go of them
- Example hand will animate properly during edge cases

### Changed
- `ViveGripHighlight*` methods renamed to `ViveGripTouch*` (**update your scripts!**)
- Increased the grip position and rotation strength by x10 (**update your object mass!**)
- Reworked the variables and defaults for `ViveGrip_Grabbable` to be clearer and more useful (**update your grabbables!**)
- Rewrote and improved large parts of documentation

## [1.2.0] - 2016-06-11
### Changed
- `ViveGripInteractionHeld` is now `ViveGripInteractionStop` (**update your scripts!**)
- All `ViveGripXStart` and `ViveGripXStop` scripts pass in the calling ViveGrip_GripPoint object

### Added
- Messages to the tracked object and all children:
  - `ViveGripHighlightStart` and `ViveGripHighlightStop`
  - `ViveGripGrabStart` and `ViveGripGrabStop`
- Example hands and documentation
- "None" button option that maps to nothing
- `gripPoint.controller` and `ViveGrip_ControllerHandler.Vibrate(milliseconds, strength)`

### Fixed
- Honouring the `.enabled` flag on grabbable and interactable scripts

## [1.1.0] - 2016-06-03
### Fixed
- Anchors were secretly kind of broken
- Letting go of the grip will no longer grab
- General code improvements

### Changed
- Updated to SteamVR Plugin v1.1.0

### Added
- Trackpad to ViveGrip_ButtonManager inputs
- Snapping to a grabbable's anchor can now be toggled
- Added a Gizmo icon to grabbable anchors

## [1.0.2] - 2016-05-11
### Fixed
- Namespaced the Example scripts so they don't conflict with your stuff

## [1.0.1] - 2016-05-07
### Fixed
- "Sticky grip" when releasing outside the touch radius

## [1.0.0] - 2016-04-31
### Added
- Grabbables
- Interactables
- Demo scene
- Documentation

[Unreleased]: https://github.com/JScott/ViveGrip/compare/v2.12.1...HEAD
[2.12.1]: https://github.com/JScott/ViveGrip/compare/v2.12.0...v2.12.1
[2.12.0]: https://github.com/JScott/ViveGrip/compare/v2.11.1...v2.12.0
[2.11.1]: https://github.com/JScott/ViveGrip/compare/v2.11.0...v2.11.1
[2.11.0]: https://github.com/JScott/ViveGrip/compare/v2.10.1...v2.11.0
[2.10.0]: https://github.com/JScott/ViveGrip/compare/v2.9.1...v2.10.0
[2.9.1]: https://github.com/JScott/ViveGrip/compare/v2.9.0...v2.9.1
[2.9.0]: https://github.com/JScott/ViveGrip/compare/v2.8.1...v2.9.0
[2.8.1]: https://github.com/JScott/ViveGrip/compare/v2.8.0...v2.8.1
[2.8.0]: https://github.com/JScott/ViveGrip/compare/v2.7.0...v2.8.0
[2.7.0]: https://github.com/JScott/ViveGrip/compare/v2.6.1...v2.7.0
[2.6.1]: https://github.com/JScott/ViveGrip/compare/v2.6.0...v2.6.1
[2.6.0]: https://github.com/JScott/ViveGrip/compare/v2.5.0...v2.6.0
[2.5.0]: https://github.com/JScott/ViveGrip/compare/v2.4.0...v2.5.0
[2.4.0]: https://github.com/JScott/ViveGrip/compare/v2.3.0...v2.4.0
[2.3.0]: https://github.com/JScott/ViveGrip/compare/v2.2.0...v2.3.0
[2.2.0]: https://github.com/JScott/ViveGrip/compare/v2.1.1...v2.2.0
[2.1.1]: https://github.com/JScott/ViveGrip/compare/v2.1.0...v2.1.1
[2.1.0]: https://github.com/JScott/ViveGrip/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/JScott/ViveGrip/compare/v1.2.0...v2.0.0
[1.2.0]: https://github.com/JScott/ViveGrip/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/JScott/ViveGrip/compare/v1.0.2...v1.1.0
[1.0.2]: https://github.com/JScott/ViveGrip/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/JScott/ViveGrip/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/JScott/ViveGrip/compare/initial...v1.0.0
