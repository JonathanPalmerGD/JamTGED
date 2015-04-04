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
    private float happiness;
    private int numDotsIntitial;
    private int numDotsCurrent;
    private bool destroyed;
    private DotManager dManager;
    //public GameObject pane;


	void Start () 
	{
		spriteRend = this.GetComponent<SpriteRenderer>();
        happiness = 100.0f;
        destroyed = false;
        numDotsIntitial = DotManager.Inst.activeDots.Count;
	}
	
	void Update () 
	{

        numDotsCurrent = DotManager.Inst.activeDots.Count;

        int temp = numDotsCurrent - numDotsIntitial;

        if (temp >= 50 && destroyed == false)
            DestroySelf();






	}

    void DestroySelf()
    {
        //Color tempColor = this.renderer.material.color;
        //tempColor.a = Mathf.MoveTowards(0, 1, (Time.deltaTime * 5));
        //this.renderer.material.color = tempColor;

        spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, Mathf.Lerp(spriteRend.color.a, 0, Time.deltaTime));
        Debug.Log(spriteRend.color.a);
        if (spriteRend.color.a <= .02f)
        {
            Destroy(gameObject);
            destroyed = true;
            Debug.Log("destroyed");
        }
    }




}
