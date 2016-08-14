using UnityEngine;
using System.Collections;

public class ViveGripExample_ExtendHighlight : MonoBehaviour {
  public Transform objectToHighlight;

  void Start () {
    objectToHighlight.gameObject.AddComponent<ViveGrip_Highlighter>();
  }

	void ViveGripHighlightStart(ViveGrip_GripPoint gripPoint) {
    GameObject touchedObject = gripPoint.TouchedObject();
    touchedObject.GetComponent<ViveGrip_Highlighter>().RemoveHighlighting();
    objectToHighlight.gameObject.SendMessage("ViveGripHighlightStart", gripPoint);
  }

  void ViveGripHighlightStop(ViveGrip_GripPoint gripPoint) {
    objectToHighlight.gameObject.SendMessage("ViveGripHighlightStop", gripPoint);
  }
}
