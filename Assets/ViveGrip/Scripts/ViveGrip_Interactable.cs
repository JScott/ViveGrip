using UnityEngine;

[DisallowMultipleComponent]
public class ViveGrip_Interactable : ViveGrip_Object {

  // These are called this on the scripts of the attached object and children of the controller:

  // Called when touched or grabbed and the interaction button is pressed and released, respectively
  //   void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripInteractionStop(ViveGrip_GripPoint gripPoint) {}

  // Called when highlighting changes
  //   void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {}
}
