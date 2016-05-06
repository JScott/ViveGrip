# Vive Grip

The HTC Vive is an amazing platform that enables you to make amazing content. However, [SteamVR for Unity](https://www.assetstore.unity3d.com/en/#!/content/32647) leaves controller interaction up to you and has sparse documentation. Compounding this, Unity's [ConfigurableJoint](http://docs.unity3d.com/Manual/class-ConfigurableJoint.html) is the best way to simulate object grabbing but also lacks documentation for how complex it is.

Vive Grip handles these for you. A few simple scripts let you can create immersive interactions with game objects. It lets you worry about your awesome game logic instead of boilerplate interaction code.

## Install

1. Import the [SteamVR Plugin](https://www.assetstore.unity3d.com/en/#!/content/32647) and place their prefab rig in your scene
2. Import the latest [Vive Grip release]()

## Quick Start

1. Add SteamVR's `[CameraRig]` object to your Scene
2. Place the Vive Grip's `Grip Point` prefab as a child of the controller objects
3. Set the prefab's `Tracked Object` in the Inspector to the respective `SteamVR_TrackedObject`
4. Move the `Grip Point` to the local position of your grip
5. Add `ViveGrip_Grabbable` and `ViveGrip_Interactable` to your objects

Check out the [Demo Scene](Examples) for more detail and to see it in action.

## Why use Vive Grip?

- Fully supports Unity's physics systems for realistic interactions
- Smooth grab pathing prevents disruptive movements
- Configurable options support your unique interaction needs
- Supports both grabbable objects and interaction hooks