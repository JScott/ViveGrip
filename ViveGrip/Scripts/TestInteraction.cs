using UnityEngine;
using System.Collections;

public class TestInteraction : MonoBehaviour {
	void Start () {}

  void OnInteraction(bool held) {
    Debug.Log("Hey!");
    if (held) {
      Debug.Log("Let go of me!");
    }
    else {
      Debug.Log("Shoo!");
    }
  }
}
