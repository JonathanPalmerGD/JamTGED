using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotManager : Singleton<DotManager>
{
	//All of the assets
	public Dictionary<string, GameObject> dotLib;
	public List<string> spritesLoaded;

	public void Init()
	{
		dotLib = new Dictionary<string, GameObject>();
		spritesLoaded = new List<string>();
	}

	public GameObject CreateDot(string dotName, Vector3 dotPos)
	{
		GameObject dotPrefab = DotManager.Inst.FindOrLoadDot(dotName);

		GameObject dot = (GameObject)GameObject.Instantiate(dot, dotPos, Quaternion.identity);

		return dot;
	}

	public GameObject FindOrLoadDot(string dotName)
	{
		//If it already exists, serve it up
		if (dotLib.ContainsKey(dotName))
		{
			return dotLib[dotName];
		}
		else
		{
			//Otherwise we need to load it.
			//All Dots are inside of Resources/Dots/<contents>
			GameObject dotToAdd = Resources.Load<GameObject>("Dots/" + dotName);
			//Debug.Log("Dots/" + dotName + "\n");

			//If we found something
			if (dotToAdd != null)
			{
				//Add it and record that it is loaded.
				spritesLoaded.Add(dotName);
				dotLib.Add(dotName, dotToAdd);

				//Fluff with a fork & serve to 4-6 hungry players.
				return dotLib[dotName];
			}
		}

		Debug.LogError("[DotManager]\n\tCould not find: " + dotName + " in Dot Lib");
		return null;
	}
}
