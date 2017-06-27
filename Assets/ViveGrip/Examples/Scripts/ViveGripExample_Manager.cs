using UnityEngine;
using System.Collections.Generic;

// See EXTENSIONS.md for more information

public class ViveGripExample_Manager : MonoBehaviour {
  public bool disableAllHighlighting = false;
  public float gripStrengthMultiplier = 1f;
  public float gripRotationMultiplier = 1f;

  void Update() {
    SetHighlighting();
    SetGripStrength();
  }

  void SetHighlighting() {
    foreach(ViveGrip_Highlighter highlighter in FindObjectsOfType<ViveGrip_Highlighter>()) {
      highlighter.enabled = !disableAllHighlighting;
    }
  }

  void SetGripStrength() {
    ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER = gripStrengthMultiplier;
    ViveGrip_JointFactory.ANGULAR_DRIVE_MULTIPLIER = gripRotationMultiplier;
  }
}
