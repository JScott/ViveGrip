using UnityEngine;
using System.Collections.Generic;

// See EXTENSIONS.md for more information

public class ViveGripExample_Manager : MonoBehaviour {
  public bool disableAllHighlighting = false;
  public float gripStrengthMultiplier = 1f;
  public float gripRotationMultiplier = 1f;

  void Update() {
    SetHighlighting(disableAllHighlighting);
    ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER = gripStrengthMultiplier;
    ViveGrip_JointFactory.ANGULAR_DRIVE_MULTIPLIER = gripRotationMultiplier;
  }

  void SetHighlighting(bool value) {
    foreach(ViveGrip_Grabbable grabbable in FindObjectsOfType<ViveGrip_Grabbable>()) {
      grabbable.disableHighlight = value;
    }
    foreach(ViveGrip_Interactable interactable in FindObjectsOfType<ViveGrip_Interactable>()) {
      interactable.disableHighlight = value;
    }
  }
}
