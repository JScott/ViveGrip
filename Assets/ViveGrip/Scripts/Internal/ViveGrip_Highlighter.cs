using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_Highlighter : MonoBehaviour {
  private Queue<Color> oldColors = new Queue<Color>();

  void Start () {}

  public void Highlight(Color color) {
    RemoveHighlighting();
    foreach (Material material in GetComponent<Renderer>().materials) {
      Color currentColor = material.color;
      oldColors.Enqueue(currentColor);
      Color tintedColor = currentColor + color;
      material.color = tintedColor;
    }
  }

  public void RemoveHighlighting() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      if (oldColors.Count == 0) { break; }
      material.color = oldColors.Dequeue();
    }
    oldColors.Clear();
  }

  void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {
    Highlight(gripPoint.tintColor);
  }

  void ViveGripHighlightStop() {
    RemoveHighlighting();
  }

  void OnDestroy() {
    RemoveHighlighting();
  }
}
