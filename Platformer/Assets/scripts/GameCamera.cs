using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Transform target;
	private float trackSpeed = 10; 

	public void SetTarget(Transform t) {
		target = t;
		
		if (farAway()) {
			trackSpeed = 100;
		}
		
		
	}
	
	void LateUpdate() {
		if (target) {
			if (!farAway ()) {
				trackSpeed = 10;
			}
			float x = IncrementTowards (transform.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards (transform.position.y, target.position.y, trackSpeed);
			transform.position = new Vector3(x, y, transform.position.z);
		}
	}
	
	// Increase n towards a target by speed 
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) { //if current speed == target, return speed
			return n;
		}
		else {
			float dir = Mathf.Sign (target-n); //must n be incremented or decremented to get close to target 
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target-n))? n: target; //if n has now passed target then return target, otherwise return n 
		}
	}
	
	bool farAway() {
		return Mathf.Abs (transform.position.x - target.position.x) > 0 || Mathf.Abs (transform.position.y - target.position.y) > 0;
	}
}
