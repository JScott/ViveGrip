using UnityEngine;
using System.Collections.Generic;
using ViveGrip.TypeReferences;

// See EXTENSIONS.md for more information

public class ViveGripExample_Manager : MonoBehaviour {
  public bool disableAllHighlighting = false;
  [ClassImplements(typeof(ViveGrip_HighlightEffect))]
  public ClassTypeReference highlightEffectForEverything = null;
  public bool changeHighlightsAtStart = false;
  public float gripStrengthMultiplier = 1f;
  public float gripRotationMultiplier = 1f;

  void Start() {
    if (changeHighlightsAtStart) {
      ChangeAllHighlightEffects();
    }
  }

  void Update() {
    SetHighlighting();
    SetGripStrength();
  }

  void SetHighlighting() {
    foreach(ViveGrip_Highlighter highlighter in AllHighlighters()) {
      highlighter.enabled = !disableAllHighlighting;
    }
  }

  void SetGripStrength() {
    ViveGrip_JointFactory.LINEAR_DRIVE_MULTIPLIER = gripStrengthMultiplier;
    ViveGrip_JointFactory.ANGULAR_DRIVE_MULTIPLIER = gripRotationMultiplier;
  }

  void OnValidate() {
    ChangeAllHighlightEffects();
  }

  void ChangeAllHighlightEffects() {
    if (AllHighlighters().Length == 0) { return; }
    Debug.Log("Changing all highlight effects to: " + highlightEffectForEverything.ToString());
    foreach(ViveGrip_Highlighter highlighter in AllHighlighters()) {
      highlighter.UpdateEffect(highlightEffectForEverything.Type);
    }
  }

  ViveGrip_Highlighter[] AllHighlighters() {
    return FindObjectsOfType<ViveGrip_Highlighter>();
  }
}
