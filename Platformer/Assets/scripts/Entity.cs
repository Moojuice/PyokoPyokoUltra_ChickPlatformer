using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;
	public bool dead;
	
	
	public void TakeDamage(float dmg) {
		health -= dmg;
		if (health <= 0) {
			Die();
		}
	}
	void Die() {
		dead = true;
		if (this.tag == "Player") {
			Destroy(this.gameObject, .5f);
		}
		else {
			Destroy(this.gameObject, .1f);
		}
	}
	IEnumerator Wait(float time) {
		yield return new WaitForSeconds(time);
	}
	         
}
