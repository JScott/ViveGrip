using UnityEngine;
using System.Collections;

public class ViveGripExample_ExtendHighlight : MonoBehaviour {
  public Transform objectToHighlight;
  private Color baseColor;
  private Renderer objectRenderer;

  void Start() {
    objectRenderer = objectToHighlight.GetComponent<Renderer>();
    baseColor = objectRenderer.material.color;
  }

  void ViveGripHighlightStart() {
    if (!this.enabled) { return; }
    objectRenderer.material.color = Color.Lerp(baseColor, Color.white, 0.5f);
  }

  void ViveGripHighlightStop() {
    if (!this.enabled) { return; }
    objectRenderer.material.color = baseColor;
  }
}
