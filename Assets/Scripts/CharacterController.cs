using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		//rigidbody.AddForce(movement);

	}
	
	void OnTriggerEnter(Collider other) 
	{
		rigidbody.velocity = new Vector3 (0.0f,0.0f,0.0f);
		Debug.Log("HE");
	}

}
