using UnityEngine;
using System.Collections.Generic;

// See EXTENSIONS.md for more information

public class ViveGripExample_ExtendHighlight : MonoBehaviour {
  public Texture highlightTexture;
  private Queue<Texture> oldTextures = new Queue<Texture>();

  void Start() {}

  void Update() {
    GetComponent<ViveGrip_Object>().disableHighlight = true;
  }

  void ViveGripHighlightStart() {
    if (!this.enabled) { return; }
    Highlight();
  }

  void ViveGripHighlightStop() {
    if (!this.enabled) { return; }
    RemoveHighlight();
  }

  void Highlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      Texture currentTexture = material.mainTexture;
      oldTextures.Enqueue(currentTexture);
      material.mainTexture = highlightTexture;
    }
  }

  void RemoveHighlight() {
    foreach (Material material in GetComponent<Renderer>().materials) {
      if (oldTextures.Count == 0) { break; }
      material.mainTexture = oldTextures.Dequeue();
    }
    oldTextures.Clear();
  }
}
