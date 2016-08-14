using UnityEngine;
using System.Collections;

public class ViveGripExample_Hand : MonoBehaviour {
  public Mesh rest;
  public Mesh primed;
  private float fadeSpeed = 3f;

	void Start () {}

  void ViveGripTouchStart() {
    GetComponent<MeshFilter>().mesh = primed;
  }

  void ViveGripTouchStop(ViveGrip_GripPoint gripPoint) {
    // We might move out of touch range but still be holding something
    if (!gripPoint.HoldingSomething()) {
      GetComponent<MeshFilter>().mesh = rest;
    }
  }

  void ViveGripGrabStart() {
    StopCoroutine("FadeIn");
    StartCoroutine("FadeOut");
  }

  void ViveGripGrabStop(ViveGrip_GripPoint gripPoint) {
    StopCoroutine("FadeOut");
    StartCoroutine("FadeIn");
    // We're not always touching something when we stop grabbing
    if (!gripPoint.TouchingSomething()) {
      GetComponent<MeshFilter>().mesh = rest;
    }
  }

  IEnumerator FadeOut() {
    if(GetComponent<Renderer>() != null) {
      Color color = GetComponent<Renderer>().material.color;
      while (color.a > 0.1f) {
        color.a -= fadeSpeed * Time.deltaTime;
        GetComponent<Renderer>().material.color = color;
        yield return null;
      }
    }
  }

  IEnumerator FadeIn() {
    if(GetComponent<Renderer>() != null) {
      Color color = GetComponent<Renderer>().material.color;
      while (color.a < 1f) {
        color.a += fadeSpeed * Time.deltaTime;
        GetComponent<Renderer>().material.color = color;
        yield return null;
      }
    }
  }
}
