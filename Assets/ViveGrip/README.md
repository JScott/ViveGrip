# Vive Grip

## Install

1. Import the [SteamVR Plugin](http://u3d.as/cjo)
2. Import [Vive Grip](http://u3d.as/t55)

## Quick Start

1. Add SteamVR's `[CameraRig]` object to your Scene
2. Place the Vive Grip's `Grip Point` prefab as children of the controller objects
3. Set the prefab's `Tracked Object` in the Inspector to the respective `SteamVR_TrackedObject`
4. (optional) Change the local position of the `Grip Point` to your desired grip
5. Add `ViveGrip_Grabbable` to your objects

Look at the README in the Examples directory for more detail and to see it in action.

If your controllers don't show up in-game then you may have to add `SteamVR_UpdatePoses` to `Camera (eye)`.
It's a known issue with Unity 5.6 and the SteamVR Plugin:
http://answers.unity3d.com/questions/1299567/vive-controllers-not-tracking-in-unity-560b3.html

## SteamVR's Interaction System

Vive Grip also works with the Interaction System. Simply place the `Grip Point`
prefab as a child of the hand objects. Leave the `Tracked Object` variable blank
in the Inspector and Vive Grip will handle setting the rest up for you.

## Tutorials

[The playlist](https://www.youtube.com/playlist?list=PLwU6y1S7ew46AkeqEMwalgkvjXTKNYUQh)

- [Basic grabbing](https://youtu.be/NyKWBeC_pSI)
- [Interaction scripting](https://youtu.be/kKnO8BSdpZQ)
- [Custom hands](https://youtu.be/peq1WFkxyus)
