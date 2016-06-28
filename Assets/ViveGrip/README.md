# Vive Grip

Vive Grip helps you to highlight, grab, and interact with game objects using the HTC Vive. It leverages Unity's physics engine with a simple interface that abstracts the powerful ConfigurableJoint component. Examples are included for creating weighted objects, levers, dials, guns, and more.

## Install

1. Import the [SteamVR Plugin](https://www.assetstore.unity3d.com/en/#!/content/32647) and place their prefab rig in your scene
2. Import the latest [Vive Grip release](https://github.com/JScott/ViveGrip/releases/latest)

## Quick Start

1. Add SteamVR's `[CameraRig]` object to your Scene
2. Place the Vive Grip's `Grip Point` prefab as children of the controller objects
3. Set the prefab's `Tracked Object` in the Inspector to the respective `SteamVR_TrackedObject`
4. Move the `Grip Point` to the local position of your grip
5. Add `ViveGrip_Grabbable` to your objects

Look at the README in the [Examples directory](Examples) for more detail and to see it in action.

## Why use Vive Grip?

- Fully supports Unity's physics systems for realistic interactions
- Smooth grab pathing prevents disruptive movements
- Configurable options support your unique interaction needs
- Supports both grabbable objects and interaction hooks
