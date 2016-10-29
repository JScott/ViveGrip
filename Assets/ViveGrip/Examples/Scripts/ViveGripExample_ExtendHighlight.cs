using UnityEngine;
using System.Collections.Generic;

// See EXTENSIONS.md for more information

public class ViveGripExample_ExtendHighlight : MonoBehaviour {
  Color highlightColor = Color.blue;
  private Queue<Color> oldColors = new Queue<Color>();

  void Start() {}

  void Update() {
    DisableDefaultHighlighter();
  }

  void ViveGripHighlightStart() {
    if (!this.enabled) { return; }
    Highlight();
  }

  void ViveGripHighlightStop() {
    if (!this.enabled) { return; }
    RemoveHighlight();
  }

  void DisableDefaultHighlighter() {
    ViveGrip_Highlighter highlighter = GetComponent<ViveGrip_Highlighter>();
    if (highlighter != null) highlighter.enabled = false;
  }

  void Highlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      Color currentColor = material.color;
      oldColors.Enqueue(currentColor);
      material.color = highlightColor;
    }
  }

  void RemoveHighlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      if (oldColors.Count == 0) { break; }
      material.color = oldColors.Dequeue();
    }
    oldColors.Clear();
  }
}
