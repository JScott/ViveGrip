using UnityEngine;

[DisallowMultipleComponent]
public class ViveGrip_Interactable : ViveGrip_Highlight {
  void Start() {}

  // These are called this on all scripts in the attached object:

  // When touched and the interaction button is pressed
  //   void ViveGripInteractionStart(ViveGrip_GripPoint gripPoint) {}

  // Every frame when touched and the interaction button is held
  //   void ViveGripInteractionStop(ViveGrip_GripPoint gripPoint) {}
}
