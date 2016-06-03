using UnityEngine;
using System.Collections;

public class playerBehaviour : MonoBehaviour {

	private Rigidbody2D	playerRb;
	private	Animator	playerAn;

	private bool		walk, olharD;
	public float		speed, speedBase, speedAdd, pulo;



	void Start () {
		playerRb = GetComponent<Rigidbody2D> ();
		playerAn = GetComponent<Animator> ();
		olharD = true;
		speed = speedBase;
	}
	


	void Update () {
		float h = Input.GetAxisRaw ("Horizontal");
		playerRb.velocity = new Vector2 (h * speed, playerRb.velocity.y);

		if (h > 0 && !olharD)
			flip ();
		else if (h < 0 && olharD)
			flip ();

		if(h != 0)
			walk = true;
		else
			walk = false;

		if (Input.GetButtonDown ("Fire3"))
			speed = speed + speedAdd;
		else if (Input.GetButtonUp ("Fire3"))
			speed = speedBase;

		if (Input.GetButtonDown ("Jump"))
			playerRb.AddForce (new Vector2 (0, pulo));

		playerAn.SetBool ("walk", walk);
		playerAn.SetFloat ("velocidade", speed);
	}



	void flip(){
		olharD = !olharD;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}
}