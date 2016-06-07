using UnityEngine;
using System.Collections;

public class ViveGripExample_Hand : MonoBehaviour {
  public Mesh rest;
  public Mesh primed;
  private float fadeSpeed = 3f;

	void Start () {}

  void ViveGripHighlightStart() {
    GetComponent<MeshFilter>().mesh = primed;
  }

  void ViveGripHighlightStop() {
    GetComponent<MeshFilter>().mesh = rest;
  }

  void ViveGripGrabStart() {
    StopCoroutine("FadeIn");
    StartCoroutine("FadeOut");
  }

  void ViveGripGrabStop() {
    StopCoroutine("FadeOut");
    StartCoroutine("FadeIn");
  }

  IEnumerator FadeOut() {
    Color color = GetComponent<Renderer>().material.color;
    while (color.a > 0.1f) {
      color.a -= fadeSpeed * Time.deltaTime;
      GetComponent<Renderer>().material.color = color;
      yield return null;
    }
  }

  IEnumerator FadeIn() {
    Color color = GetComponent<Renderer>().material.color;
    while (color.a < 1f) {
      color.a += fadeSpeed * Time.deltaTime;
      GetComponent<Renderer>().material.color = color;
      yield return null;
    }
  }
}
