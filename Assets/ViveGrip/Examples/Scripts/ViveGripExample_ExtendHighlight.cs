using UnityEngine;
using System.Collections.Generic;

class ViveGripExample_NewHighlight : ViveGrip_HighlightEffect {
  public Texture highlightTexture;
  private Queue<Texture> oldTextures = new Queue<Texture>();

  public void Start(ViveGrip_Highlighter highlighter) {
    Renderer renderer = highlighter.GetComponent<Renderer>();
    if (renderer == null) { return; }
    Stop(highlighter);
    foreach (Material material in renderer.materials) {
      Texture currentTexture = material.mainTexture;
      oldTextures.Enqueue(currentTexture);
      material.mainTexture = highlightTexture;
    }
  }

  public void Stop(ViveGrip_Highlighter highlighter) {
    Renderer renderer = highlighter.GetComponent<Renderer>();
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

  void Start() {}

  void Update() {
    // GetComponent<ViveGrip_Object>().disableHighlight = true;
    // AddNewHighlighter();
  }

  void AddNewHighlighter() {
    // if (GetComponent<ViveGripExample_NewHighlight>() != null) { return; }
    // ViveGripExample_NewHighlight newHighlighter = gameObject.AddComponent<ViveGripExample_NewHighlight>();
    // newHighlighter.highlightTexture = highlightTexture;
  }
}
