using UnityEngine;
using System.Collections;

public class ViveGripExample_DoorHandle : MonoBehaviour {
	void Start () {
    Physics.IgnoreCollision(GameObject.Find("Door").GetComponent<Collider>(), GetComponent<Collider>());
	}
}
