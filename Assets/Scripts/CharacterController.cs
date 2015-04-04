using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	public GameObject plane;
	public float speed;

	private float halfScaleX;
	private float halfScaleY;
	private bool checkX = true;
	private bool checkZ = true;
	
	private float offsetX;
	private float offsetZ;


	// Use this for initialization
	void Start () {
		
		halfScaleX = Mathf.Abs((plane.transform.localScale.x/2) - 0.75f);
		halfScaleY = Mathf.Abs((plane.transform.localScale.y/2) - 0.75f);
		Debug.Log(halfScaleX);
		Debug.Log(halfScaleY);

	}
	
	// Update is called once per frame
	void Update () {
		offsetX = (Mathf.Abs(this.transform.position.x));
		offsetZ = (Mathf.Abs(this.transform.position.z));

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		//rigidbody.velocity = movement * speed;
		//rigidbody.AddForce(movement);

		bool bounced = false;

		if(offsetX < halfScaleX) // inside boundary x
		{
			checkX = true;
		}
		else // outside boundary x
		{
			checkX = false;
			Vector3 temp = this.transform.position;
			if(temp.x < halfScaleX)
			{
				temp.x = -halfScaleX + 0.01f;
			}
			else
			{
				temp.x = halfScaleX - 0.01f;
			}
			this.transform.position = temp;
			bounced = true;
		}
		
		if(offsetZ < halfScaleY) // inside boundary z or y
		{
			checkZ = true;
		}
		else // outside boundary z or y
		{
			checkZ = false;
			Vector3 temp = this.transform.position;
			if(temp.z < halfScaleY)
			{
				temp.z = -halfScaleY + 0.01f;
			}
			else
			{
				temp.z = halfScaleY - 0.01f;
			}
			this.transform.position = temp;
			bounced = true;
		}


		if(bounced == false)
		{
			if(checkX == true && checkZ == true) // can move horizontal and vertical
			{
				Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
				rigidbody.velocity = movement * speed;
			}
			else if(checkX == true && checkZ == false) // can move horizontal but not vertical
			{
				Vector3 movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
				rigidbody.velocity = movement * speed;
			}
			else if(checkX == false && checkZ == true) // can move vertical but not horizontal
			{
				Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical);
				rigidbody.velocity = movement * speed;
			}
			else  // can't move either way
			{
				Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);
				rigidbody.velocity = movement * speed;
			}
		}


	}

}
