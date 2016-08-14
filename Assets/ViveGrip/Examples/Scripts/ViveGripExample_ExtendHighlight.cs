using UnityEngine;
using System.Collections;

// See EXTENSIONS.md for more information

public class ViveGripExample_ExtendHighlight : MonoBehaviour {
  private Renderer objectRenderer;
  private Color baseColor;

  void Start() {
    objectRenderer = GameObject.Find("Right Hand").GetComponent<Renderer>();
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
