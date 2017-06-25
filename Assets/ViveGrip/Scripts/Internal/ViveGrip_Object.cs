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

  void OnDisable() {
    ViveGrip_Highlighter highlighter = GetComponent<ViveGrip_Highlighter>();
    if (highlighter == null) { return; }
    highlighter.RemoveHighlight();
  }

  void OnEnable() {
    ViveGrip_Highlighter highlighter = GetComponent<ViveGrip_Highlighter>();
    if (highlighter == null) { return; }
    highlighter.Highlight();
  }
}
