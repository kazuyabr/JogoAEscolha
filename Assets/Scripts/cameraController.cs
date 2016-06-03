using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {

	private	playerBehaviour player;
	private	float			posX;



	void Start () {
		player = FindObjectOfType (typeof(playerBehaviour)) as playerBehaviour;
	}


	
	void Update () {

		posX = player.transform.position.x;

		transform.position = new Vector3 (posX, transform.position.y, transform.position.z);
	}

}