using UnityEngine;
using System.Collections.Generic;

public class ViveGrip_Highlighter : MonoBehaviour {
  private bool highlighted = false;
  private Color tintColor = new Color(0.2f, 0.2f, 0.2f);
  private Queue<Color> oldColors = new Queue<Color>();
  private HashSet<ViveGrip_GripPoint> grips = new HashSet<ViveGrip_GripPoint>();

  void Start() {}

  void Update() {
    if (highlighted && grips.Count == 0) {
      RemoveHighlight();
      highlighted = false;
    }
    if (!highlighted && grips.Count != 0) {
      Highlight();
      highlighted = true;
    }
  }

  public virtual void Highlight() {
    if (gameObject.GetComponent<Renderer>() == null) { return; }
    RemoveHighlight();
    foreach (Material material in GetComponent<Renderer>().materials) {
      Color currentColor = material.color;
      oldColors.Enqueue(currentColor);
      Color tintedColor = currentColor + tintColor;
      material.color = tintedColor;
    }
  }

  public virtual void RemoveHighlight() {
    if (gameObject.GetComponent<Renderer>() == null) { return; }
    foreach (Material material in GetComponent<Renderer>().materials) {
      if (oldColors.Count == 0) { break; }
      material.color = oldColors.Dequeue();
    }
    oldColors.Clear();
  }

  public static ViveGrip_Highlighter AddTo(GameObject gameObject) {
    if (gameObject.GetComponent<ViveGrip_Highlighter>() == null) {
      gameObject.AddComponent<ViveGrip_Highlighter>();
    }
    return gameObject.GetComponent<ViveGrip_Highlighter>();
  }

  // void ViveGripHighlightStart() {
  //   if (!this.enabled) { return; }
  //   Highlight(tintColor);
  // }
  void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    grips.Add(gripPoint);
  }

  // void ViveGripHighlightStop() {
  //   if (!this.enabled) { return; }
  //   RemoveHighlight();
  // }

  void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    grips.Remove(gripPoint);
    // }
  }

  // void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
  //   if (!this.enabled) { return; }
  //   if (!gripPoint.TouchingSomething()) {
  //     grips.Remove(gripPoint);
  //   }
  // }

  void OnDisable() {
    RemoveHighlight();
  }
}
