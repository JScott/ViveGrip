using UnityEngine;
using ViveGrip.TypeReferences;

[DisallowMultipleComponent]
public class ViveGrip_Object : MonoBehaviour {
  // [Tooltip("The highlight effect used by the object's highlighter. Use ViveGrip_Highlighter.UpdateEffect to update this from code.")]
  [ClassImplements(typeof(ViveGrip_HighlightEffect))]
  public ClassTypeReference highlightEffect = typeof(ViveGrip_TintEffect);
  private ViveGrip_Highlighter highlighter;

  public void Awake() {
    highlighter = GetComponent<ViveGrip_Highlighter>();
    if (highlighter == null) {
      highlighter = gameObject.AddComponent<ViveGrip_Highlighter>();
    }
    highlighter.UpdateEffect(highlightEffect.Type);
  }

  void OnDisable() {
    if (highlighter == null) { return; }
    highlighter.RemoveHighlight();
  }

  void OnEnable() {
    if (highlighter == null) { return; }
    highlighter.Highlight();
  }

  void OnValidate() {
    if (highlighter == null) { return; }
    highlighter.UpdateEffect(highlightEffect.Type);
  }
}
