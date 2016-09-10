# Vive Grip

## Install

1. Import the [SteamVR Plugin](https://www.assetstore.unity3d.com/en/#!/content/32647)
2. Import [Vive Grip](https://www.assetstore.unity3d.com/en/#!/content/62324)

## Quick Start

1. Add SteamVR's `[CameraRig]` object to your Scene
2. Place the Vive Grip's `Grip Point` prefab as children of the controller objects
3. Set the prefab's `Tracked Object` in the Inspector to the respective `SteamVR_TrackedObject`
4. Move the `Grip Point` to the local position of your grip
5. Add `ViveGrip_Grabbable` to your objects

Look at the README in the Examples directory for more detail and to see it in action.

## Tutorials

[Basic grabbing](https://www.youtube.com/watch?v=NyKWBeC_pSI)

[Interaction scripting](https://www.youtube.com/watch?v=kKnO8BSdpZQ)
