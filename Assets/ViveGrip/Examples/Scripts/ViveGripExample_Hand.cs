using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class ViveGripExample_Hand : MonoBehaviour {
  public Mesh rest;
  public Mesh primed;
  private float fadeSpeed = 3f;

  void Start () {
    bool shadow = transform.childCount == 0;
    if (UnityEngine.XR.XRDevice.model.Contains("Rift") && !shadow) {
      // Oculus Touch feels more natural with a tilt
      transform.Rotate(40f, 0, 0);
      transform.Translate(0f, -0.05f, -0.03f, Space.World);
    }
  }

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
