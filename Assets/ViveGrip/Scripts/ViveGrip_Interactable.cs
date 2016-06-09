using UnityEngine;

[DisallowMultipleComponent]
public class ViveGrip_Interactable : ViveGrip_Highlight {
  void Start() {}

  // These are called this on all scripts in the attached object:

  // When touched and the interaction button is pressed and released, respectively
  //   void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint) {}
  //   void ViveGripInteractionStop(ViveGrip_GripPoint gripPoint) {}
}
