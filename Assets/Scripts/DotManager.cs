using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DotManager : Singleton<DotManager>
{
	//All of the assets
	public Dictionary<string, GameObject> dotLib;
	public List<string> dotsLoaded;

	public List<GameObject> activeDots;

	public void Init()
	{
		dotLib = new Dictionary<string, GameObject>();
		dotsLoaded = new List<string>();
		activeDots = new List<GameObject>();
	}

	public GameObject CreateDot(string dotName, Vector3 dotPos, bool trackDot = true)
	{
		GameObject dotPrefab = DotManager.Inst.FindOrLoadDot(dotName);

		GameObject dot = (GameObject)GameObject.Instantiate(dotPrefab, dotPos, Quaternion.identity);

		if (trackDot)
		{
			activeDots.Add(dot);
		}

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
				dotsLoaded.Add(dotName);
				dotLib.Add(dotName, dotToAdd);

				//Fluff with a fork & serve to 4-6 hungry players.
				return dotLib[dotName];
			}
		}

		Debug.LogError("[DotManager]\n\tCould not find: " + dotName + " in Dot Lib");
		return null;
	}

	public void Update()
	{
		for (int i = 0; i < activeDots.Count; i++)
		{
			if (activeDots[i] == null)
			{
				activeDots.RemoveAt(i);
				i--;
			}
		}
	}
}
