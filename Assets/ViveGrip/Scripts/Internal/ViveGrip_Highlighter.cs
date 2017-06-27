using UnityEngine;
using System.Collections.Generic;
using ViveGrip.TypeReferences;
using System.Reflection;

public interface ViveGrip_HighlightEffect {
  void Start(ViveGrip_Highlighter highlighter);
  void Stop(ViveGrip_Highlighter highlighter);
}

public class ViveGrip_Highlighter : MonoBehaviour {
  public bool disabled = false;
  private ViveGrip_HighlightEffect highlight = null;
  private bool highlighted = false;
  private HashSet<ViveGrip_GripPoint> grips = new HashSet<ViveGrip_GripPoint>();

  void Start() {}

  void Update() {
    if (highlighted && grips.Count == 0) {
      RemoveHighlight();
    }
    if (!highlighted && grips.Count != 0) {
      Highlight();
    }
  }

  public void RemoveHighlight() {
    if (highlight == null) { return; }
    highlight.Stop(this);
    highlighted = false;
  }

  public void Highlight() {
    if (highlight == null) { return; }
    highlight.Start(this);
    highlighted = true;
  }

  public ViveGrip_HighlightEffect UpdateEffect(System.Type effectType) {
    if (effectType == null) {
      if (highlight != null) { highlight.Stop(this); }
      highlight = null;
    } else if (typeof(ViveGrip_HighlightEffect).IsAssignableFrom(effectType)) {
      if (highlight != null) { highlight.Stop(this); }
      highlight = System.Activator.CreateInstance(effectType) as ViveGrip_HighlightEffect;
    } else {
      Debug.LogError(effectType + " does not implement the ViveGrip_HighlightEffect interface");
    }
    return highlight;
  }

  public ViveGrip_HighlightEffect CurrentEffect() {
    return highlight;
  }

  void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    grips.Add(gripPoint);
  }

  void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {
    if (!this.enabled) { return; }
    grips.Remove(gripPoint);
  }

  void OnDisable() {
    RemoveHighlight();
  }
}

// Core highlight effects
// ======================

public class ViveGrip_TintEffect : ViveGrip_HighlightEffect {
  private Color tintColor = new Color(0.2f, 0.2f, 0.2f);
  private Queue<Color> oldColors = new Queue<Color>();

  public void Start(ViveGrip_Highlighter highlighter) {
    Renderer renderer = highlighter.GetComponent<Renderer>();
    if (renderer == null) { return; }
    Stop(highlighter);
    foreach (Material material in Materials(renderer)) {
      StashColor(material);
    }
  }

  public void Stop(ViveGrip_Highlighter highlighter) {
    Renderer renderer = highlighter.GetComponent<Renderer>();
    if (renderer == null) { return; }
    foreach (Material material in Materials(renderer)) {
      if (oldColors.Count == 0) { break; }
      PopColor(material);
    }
    oldColors.Clear();
  }

  void StashColor(Material material) {
    oldColors.Enqueue(material.color);
    material.color = material.color + tintColor;
  }

  void PopColor(Material material) {
    material.color = oldColors.Dequeue();
  }

  protected virtual Material[] Materials(Renderer renderer) {
    return renderer.materials;
  }
}

public class ViveGrip_RecursiveTintEffect : ViveGrip_TintEffect {
  protected override Material[] Materials(Renderer renderer) {
    // NOTE: GetComponentsInChildren is guaranteed to always return in the same order
    Renderer[] renderers = renderer.gameObject.GetComponentsInChildren<Renderer>();
    List<Material> materials = new List<Material>();
    foreach (Renderer family in renderers) {
      materials.AddRange(family.materials);
    }
    return materials.ToArray();
  }
}
