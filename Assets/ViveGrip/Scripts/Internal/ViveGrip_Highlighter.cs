using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_Highlighter : MonoBehaviour {
  private Color tintColor = new Color(0.2f, 0.2f, 0.2f);
  private Queue<Color> oldColors = new Queue<Color>();

  void Start () {}

  public void Highlight(Color color) {
    if (gameObject.GetComponent<Renderer>() == null) { return; }
    RemoveHighlight();
    foreach (Material material in GetComponent<Renderer>().materials) {
      Color currentColor = material.color;
      oldColors.Enqueue(currentColor);
      Color tintedColor = currentColor + color;
      material.color = tintedColor;
    }
  }

  public void RemoveHighlight() {
    if (gameObject.GetComponent<Renderer>() == null) { return; }
    foreach (Material material in GetComponent<Renderer>().materials) {
      if (oldColors.Count == 0) { break; }
      material.color = oldColors.Dequeue();
    }
    oldColors.Clear();
  }

  public static void AddTo(GameObject gameObject) {
    if (gameObject.GetComponent<ViveGrip_Highlighter>() == null) {
      gameObject.AddComponent<ViveGrip_Highlighter>();
    }
  }

  void ViveGripHighlightStart() {
    if (!this.enabled) { return; }
    Highlight(tintColor);
  }

  void ViveGripHighlightStop() {
    if (!this.enabled) { return; }
    RemoveHighlight();
  }

  void OnDestroy() {
    RemoveHighlight();
  }
}
