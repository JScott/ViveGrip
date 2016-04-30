using UnityEngine;
using System.Collections;

public class ViveGrip_Highlighter {
  public GameObject highlightedObject;
  public bool disabled = false;
  private Shader outlineShader;
  private Color outlineColor;
  private Shader oldShader;

  public ViveGrip_Highlighter(Shader outlineShader, Color outlineColor) {
    this.outlineShader = outlineShader;
    this.outlineColor = outlineColor;
  }

  public void UpdateFor(GameObject gameObject) {
    if (gameObject == highlightedObject) { return; }
    if (highlightedObject != null) { RemoveHighlighting(); }
    if (!disabled && gameObject != null) { Highlight(gameObject); }
  }

  private void RemoveHighlighting() {
    if (oldShader != null) {
      highlightedObject.GetComponent<Renderer>().material.shader = oldShader;
    }
    highlightedObject = null;
  }

  private void Highlight(GameObject gameObject) {
    highlightedObject = gameObject;
    Shader currentShader = highlightedObject.GetComponent<Renderer>().material.shader;
    if (currentShader != outlineShader) {
      oldShader = currentShader;
    }
    highlightedObject.GetComponent<Renderer>().material.shader = outlineShader;
    highlightedObject.GetComponent<Renderer>().material.SetFloat("_Outline", 0.0005f);
    highlightedObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", outlineColor);
  }
}
