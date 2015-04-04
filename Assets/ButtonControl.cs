using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class ButtonControl : MonoBehaviour {
	
	//public Texture aTexture;
	public GameObject start;
	public EventSystem theEvents;
	
	
	// Use this for initialization
	void Start () {
		//OnGUI();
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	
	public void LoadLevel()
	{
		Application.LoadLevel("ControllerBranch");
	}
	
	public void LoadMenu()
	{
		Application.LoadLevel("Menu");
	}

	
}
