using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveGripExample_Teleporter : MonoBehaviour {
	public Transform player;
	private bool teleported = false;

	void Start () {}

	void ViveGripInteractionStart() {
		HashSet<Transform> subjects = new HashSet<Transform>();
		subjects.Add(player);
		subjects.Add(transform);
		foreach(ViveGrip_GripPoint gripPoint in GripPoints()) {
			if (gripPoint.HoldingSomething()) {
				subjects.Add(gripPoint.HeldObject().transform);
			}
		}
		foreach(Transform subject in subjects) {
			subject.Translate(Distance(), Space.World);
		}
		teleported = !teleported;
	}

	Vector3 Distance() {
		int direction = teleported ? -1 : 1;
		return new Vector3(0f, 0f, -6f * direction);
	}

	ViveGrip_GripPoint[] GripPoints() {
		return player.GetComponentsInChildren<ViveGrip_GripPoint>();
	}
}
