using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	
	public float speed = 2;
	public float amountToMove = 10;
	private Vector3 startingPosition;
	private bool goingRight;
	
	void Start() {
		startingPosition = transform.position;
		goingRight = true;
	}
	// Update is called once per frame
	void Update () {
		if(goingRight) {
			transform.Translate (Vector3.right * speed * Time.deltaTime);
			if (Mathf.Abs (transform.position.x - startingPosition.x) >= amountToMove) {
				goingRight = false;
			}
		}
		else {
			transform.Translate (Vector3.left * speed * Time.deltaTime);
			if (transform.position.x <= startingPosition.x) {
				goingRight = true;
			}
		}
	}
}
