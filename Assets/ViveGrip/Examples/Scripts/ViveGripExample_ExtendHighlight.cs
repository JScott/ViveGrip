using UnityEngine;
using System.Collections.Generic;

public class ViveGripExample_NewHighlight : ViveGrip_HighlightEffect {
  public Texture highlightTexture;
  private Queue<Texture> oldTextures = new Queue<Texture>();

  public void Start(GameObject gameObject) {
    Renderer renderer = gameObject.GetComponent<Renderer>();
    if (renderer == null) { return; }
    Stop(gameObject);
    foreach (Material material in renderer.materials) {
      Texture currentTexture = material.mainTexture;
      oldTextures.Enqueue(currentTexture);
      material.mainTexture = highlightTexture;
    }
  }

  public void Stop(GameObject gameObject) {
    Renderer renderer = gameObject.GetComponent<Renderer>();
    if (renderer == null) { return; }
    foreach (Material material in renderer.materials) {
      if (oldTextures.Count == 0) { break; }
      material.mainTexture = oldTextures.Dequeue();
    }
    oldTextures.Clear();
  }
}

public class ViveGripExample_ExtendHighlight : MonoBehaviour {
  public Texture highlightTexture;

  void Start() {
    ViveGrip_Highlighter highlighter = GetComponent<ViveGrip_Highlighter>();
    ViveGripExample_NewHighlight effect = highlighter.UpdateEffect(typeof(ViveGripExample_NewHighlight)) as ViveGripExample_NewHighlight;
    effect.highlightTexture = highlightTexture;
  }
}
