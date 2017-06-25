using UnityEngine;

[DisallowMultipleComponent]
public class ViveGrip_Object : MonoBehaviour {
  [Tooltip("Should the highlighting on this object be turned off?")]
  public bool disableHighlight = false;
  private ViveGrip_Highlighter highlighter;

  public void Start() {
    highlighter = ViveGrip_Highlighter.AddTo(gameObject);
  }

  public void Update() {
    highlighter.enabled = !disableHighlight;
  }
}
