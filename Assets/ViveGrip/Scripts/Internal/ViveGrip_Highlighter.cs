using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_Highlighter : MonoBehaviour {
  Color tintColor = Color.white;
  float tintRatio = 0.2f;
  private Queue<Color> oldColors = new Queue<Color>();

  void Start () {}

  public void Highlight(Color color) {
    RemoveHighlight();
    foreach (Material material in GetComponent<Renderer>().materials) {
      Color currentColor = material.color;
      oldColors.Enqueue(currentColor);
      material.color = Color.Lerp(currentColor, color, tintRatio);
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
