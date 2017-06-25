using UnityEngine;
using System.Collections.Generic;

class ViveGripExample_NewHighlighter : ViveGrip_Highlighter {
  public Texture highlightTexture;
  private Queue<Texture> oldTextures = new Queue<Texture>();

  public override void Highlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      Texture currentTexture = material.mainTexture;
      oldTextures.Enqueue(currentTexture);
      material.mainTexture = highlightTexture;
    }
  }

  public override void RemoveHighlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
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
    GetComponent<ViveGrip_Object>().disableHighlight = true;
    AddNewHighlighter();
  }

  void AddNewHighlighter() {
    if (GetComponent<ViveGripExample_NewHighlighter>() != null) { return; }
    ViveGripExample_NewHighlighter newHighlighter = gameObject.AddComponent<ViveGripExample_NewHighlighter>();
    newHighlighter.highlightTexture = highlightTexture;
  }
}
