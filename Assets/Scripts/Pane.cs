using UnityEngine;
using System.Collections;

public class Pane : MonoBehaviour
{
	//Has a helper method for destroying itself
	//When you call this, it fades out over time and then deletes itself from DotManager.

	public SpriteRenderer spriteRend;

	void Start () 
	{
		spriteRend = this.GetComponent<SpriteRenderer>();
	}
	
	void Update () 
	{
	
	}
}
