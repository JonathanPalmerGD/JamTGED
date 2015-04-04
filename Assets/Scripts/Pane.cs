using UnityEngine;
using System.Collections;

public class Pane : MonoBehaviour
{
	//Has a helper method for destroying itself
	//When you call this, it fades out over time and then deletes itself from DotManager.

	//They track the DotManager.Inst.activeDots.Count
	//When happiness is beyond a certain point, begin destruction

	//

	public SpriteRenderer spriteRend;

	void Start () 
	{
		spriteRend = this.GetComponent<SpriteRenderer>();
	}
	
	void Update () 
	{
	
	}
}
