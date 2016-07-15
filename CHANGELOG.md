# Change Log
All notable changes to this project will be documented in this file.
This project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased]
### Added
- Grip spheres now inherit the layer of the grip point that spawns them, enabling better collision control
- `gripPoint.HeldObject()` to get the held GameObject
- `gripPoint.ToggleGrab()` to grab and release in script
- A tar ball example to show off grab toggling

### Fixed
- Clean up a few grip point variables

## [v2.1.1] - 2016-07-08
### Fixed
- Build no longer breaks on `import UnityEditor`

## [v2.1.0] - 2016-07-03
### Added
- Door example

### Fixed
- Cleaned up repeated `ViveGrip_JointFactory` code

### Changed
- Decreased the grip position and rotation strength by half (**update your object mass!**)

## [v2.0.0] - 2016-06-28
### Fixed
- Objects will highlight when letting go of them
- Example hand will animate properly during edge cases

### Changed
- `ViveGripHighlight*` methods renamed to `ViveGripTouch*` (**update your scripts!**)
- Increased the grip position and rotation strength by x10 (**update your object mass!**)
- Reworked the variables and defaults for `ViveGrip_Grabbable` to be clearer and more useful (**update your grabbables!**)
- Rewrote and improved large parts of documentation

## [v1.2.0] - 2016-06-11
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

## [v1.1.0] - 2016-06-03
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

[Unreleased]: https://github.com/JScott/ViveGrip/compare/v2.1.1...HEAD
[2.1.1]: https://github.com/JScott/ViveGrip/compare/v2.1.0...v2.1.1
[2.1.0]: https://github.com/JScott/ViveGrip/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/JScott/ViveGrip/compare/v1.2.0...v2.0.0
[1.2.0]: https://github.com/JScott/ViveGrip/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/JScott/ViveGrip/compare/v1.0.2...v1.1.0
[1.0.2]: https://github.com/JScott/ViveGrip/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/JScott/ViveGrip/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/JScott/ViveGrip/compare/initial...v1.0.0
