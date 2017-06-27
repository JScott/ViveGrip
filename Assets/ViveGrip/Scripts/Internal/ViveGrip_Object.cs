using UnityEngine;
using ViveGrip.TypeReferences;

[DisallowMultipleComponent]
public class ViveGrip_Object : MonoBehaviour {
  [Tooltip("somethin!")]
  [ClassImplements(typeof(ViveGrip_HighlightEffect))]
  public ClassTypeReference highlightEffect = typeof(ViveGrip_TintEffect); // can be null
  private ViveGrip_Highlighter highlighter;

  public void Start() {
    highlighter = ViveGrip_Highlighter.AddTo(gameObject);
    highlighter.UpdateEffect(highlightEffect.Type);
    // highlighter.UpdateEffect(null);
  }

  public void Update() {
    // highlighter.enabled = !disableHighlight;
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
