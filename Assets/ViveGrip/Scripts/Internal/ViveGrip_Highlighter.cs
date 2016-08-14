using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_Highlighter : MonoBehaviour {
  private Color tintColor = new Color(0.2f, 0.2f, 0.2f);
  private Queue<Color> oldColors = new Queue<Color>();

  void Start () {}

  public void Highlight(Color color) {
    RemoveHighlight();
    foreach (Material material in GetComponent<Renderer>().materials) {
      Color currentColor = material.color;
      oldColors.Enqueue(currentColor);
      Color tintedColor = currentColor + color;
      material.color = tintedColor;
    }
  }

  public void RemoveHighlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      if (oldColors.Count == 0) { break; }
      material.color = oldColors.Dequeue();
    }
    oldColors.Clear();
  }

  public static void AddTo(GameObject gameObject) {
    if (gameObject.GetComponent<Renderer>() == null) { return; }
    if (gameObject.GetComponent<ViveGrip_Highlighter>() == null) {
      gameObject.AddComponent<ViveGrip_Highlighter>();
    }
  }

  void ViveGripHighlightStart() {
    Highlight(tintColor);
  }

  void ViveGripHighlightStop() {
    RemoveHighlight();
  }

  void OnDestroy() {
    RemoveHighlight();
  }
}
