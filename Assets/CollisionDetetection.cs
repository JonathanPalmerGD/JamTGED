using UnityEngine;
using System.Collections;

public class CollisionDetetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			return;
		}
		other.rigidbody.velocity = new Vector3(0.0f,0.0f,0.0f);
	}

}
