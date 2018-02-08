using UnityEngine;
using ViveGrip.TypeReferences;

public class ViveGrip_Object : MonoBehaviour {
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

  void OnValidate() {
    if (highlighter == null) { return; }
    highlighter.UpdateEffect(highlightEffect.Type);
  }
}
