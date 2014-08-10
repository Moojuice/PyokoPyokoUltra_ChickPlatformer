using UnityEngine;
using System.Collections;

public class Skull : MonoBehaviour {
	void Update() {

	}
	void OnTriggerEnter(Collider c) {

		if (c.CompareTag ("Player")) {
			c.GetComponent<Entity>().TakeDamage(10);
		}
	}

}
