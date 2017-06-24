using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class ViveGrip_Object : MonoBehaviour {
  [Tooltip("Should the highlighting on this object be turned off?")]
  public bool disableHighlight = false;
  private ViveGrip_Highlighter highlighter;
  private HashSet<ViveGrip_TouchDetection> touching = new HashSet<ViveGrip_TouchDetection>();

  public void Start() {
    highlighter = ViveGrip_Highlighter.AddTo(gameObject);
  }

  public void Update() {
    highlighter.enabled = !disableHighlight;
  }

  public bool NotTouched() {
    return touching.Count == 0;
  }

  public void Remember(ViveGrip_TouchDetection touch) { touching.Add(touch); }
  public void Forget(ViveGrip_TouchDetection touch) { touching.Remove(touch); }
}
