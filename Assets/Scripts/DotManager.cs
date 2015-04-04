using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DotManager : Singleton<DotManager>
{
	//All of the assets
	public Dictionary<string, GameObject> dotLib;
	public List<string> dotsLoaded;

	//All of the assets
	public Dictionary<string, Material> matLib;

	public Dictionary<string, Material> edenDotLib;
	public Dictionary<string, Material> edenEnvLib;
	public Dictionary<string, Material> hellDotLib;
	public Dictionary<string, Material> hellEnvLib;
	public List<string> matsLoaded;

	public List<GameObject> activeDots;

	public List<Sprite> backgroundSprites;

	public void Init()
	{
		dotLib = new Dictionary<string, GameObject>();
		matLib = new Dictionary<string, Material>();
		dotsLoaded = new List<string>();
		matsLoaded = new List<string>();
		activeDots = new List<GameObject>();

		edenDotLib = new Dictionary<string, Material>();
		edenEnvLib = new Dictionary<string, Material>();
		hellDotLib = new Dictionary<string, Material>();
		hellEnvLib = new Dictionary<string, Material>();

		LoadAllMats();
	}

	public GameObject CreateDot(string dotName, Vector3 dotPos, bool trackDot = true, bool isEden = true, bool isDot = true)
	{
		GameObject dotPrefab = DotManager.Inst.FindOrLoadDot(dotName);

		GameObject dot = (GameObject)GameObject.Instantiate(dotPrefab, dotPos, Quaternion.identity);

		//dot.renderer.material = matLib[matsLoaded[Random.Range(0, matsLoaded.Count)]];

		if (activeDots.Count < 15)
		{
			dot.renderer.material = FindRandomMat(true, false);

			//Debug.Log("Eden\n");
		}
		else
		{
			//Debug.Log("End\n");
			dot.renderer.material = FindRandomMat(false, false);
		}

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

	public void LoadAllMats()
	{
		Material[] mats;

		#region Eden Mat Loading
		mats = Resources.LoadAll<Material>("Material/Eden/Dots");
		for (int i = 0; i < mats.Length; i++)
		{
			matsLoaded.Add(mats[i].name);
			edenDotLib.Add(mats[i].name, mats[i]);
		}

		mats = Resources.LoadAll<Material>("Material/Eden/Environment");
		for (int i = 0; i < mats.Length; i++)
		{
			matsLoaded.Add(mats[i].name);
			edenEnvLib.Add(mats[i].name, mats[i]);
		}
		#endregion

		#region End of Days Mat Loading
		mats = Resources.LoadAll<Material>("Material/EndOfDays/Dots");
		for (int i = 0; i < mats.Length; i++)
		{
			matsLoaded.Add(mats[i].name);
			hellDotLib.Add(mats[i].name, mats[i]);
		}

		mats = Resources.LoadAll<Material>("Material/EndOfDays/Environment");
		for (int i = 0; i < mats.Length; i++)
		{
			matsLoaded.Add(mats[i].name);
			hellEnvLib.Add(mats[i].name, mats[i]);
		}
		#endregion

		#region Old Code
		/*
		Debug.Log(mats.Length + "\n");
		mats = Resources.LoadAll<Material>("Material/EndOfDays");

		for (int i = 0; i < mats.Length; i++)
		{
			matsLoaded.Add(mats[i].name);
			matLib.Add(mats[i].name, mats[i]);
		}
		Debug.Log(matLib.Count + "\n");*/

		#endregion
	}

	public Material FindOrLoadMat(string matName = "BrightRed", bool isEden = true, bool isDot = true)
	{
		string matLoc = "Materials/";

		matLoc += isEden ? "Eden/" : "EndOfDays";
		matLoc += isDot ? "Dots/" : "Environment";

		Material returnMat = null;

		if (isEden)
		{
			if (isDot)
			{
				returnMat = edenDotLib[matName];
			}
			else
			{
				returnMat = edenEnvLib[matName];
			}
		}
		else
		{
			if (isDot)
			{
				returnMat = hellDotLib[matName];
			}
			else
			{
				returnMat = hellEnvLib[matName];
			}
		}

		if (!returnMat)
		{
			Debug.LogError("[DotManager]\t" + matName + " could not be found\n\t\t" + "Eden? : " + isEden + "\tDot? : " + isDot);
		}

		return returnMat;

		#region Old
		/*
		//If it already exists, serve it up
		if (dotLib.ContainsKey(matName))
		{
			return matLib[matName];
		}
		else
		{
			//Otherwise we need to load it.
			Material matToAdd = Resources.Load<Material>("Materials/" + matName);
			//Debug.Log("Dots/" + dotName + "\n");

			//If we found something
			if (matToAdd != null)
			{
				//Add it and record that it is loaded.
				matsLoaded.Add(matName);
				matLib.Add(matName, matToAdd);

				//Fluff with a fork & serve to 4-6 hungry players.
				return matLib[matName];
			}
		}
		 
		 
		Debug.LogError("[DotManager]\n\tCould not find: " + matName + " in Dot Lib");
		return null;*/
		#endregion
	}

	public Material FindRandomMat(bool isEden = true, bool isDot = true)
	{
		if (isEden)
		{
			if (isDot)
			{
				return FindRandomMatFromDictionary(edenDotLib);
			}
			else
			{
				return FindRandomMatFromDictionary(edenEnvLib);
			}
		}
		else
		{
			if (isDot)
			{
				return FindRandomMatFromDictionary(hellDotLib);
			}
			else
			{
				return FindRandomMatFromDictionary(hellEnvLib);
			}
		}
	}

	public Material FindRandomMatFromDictionary(Dictionary<string, Material> dict)
	{
		List<Material> matList = dict.Values.ToList();

		return matList[Random.Range(0, matList.Count)];
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
