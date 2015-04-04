using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DotManager : Singleton<DotManager>
{
	//All of the assets
	public Dictionary<string, GameObject> dotLib;
	public List<string> dotsLoaded;

	public GameObject panePrefab;

	//All of the assets
	public Dictionary<string, Material> matLib;

	public Dictionary<string, Material> edenDotLib;
	public Dictionary<string, Material> edenEnvLib;
	public Dictionary<string, Material> hellDotLib;
	public Dictionary<string, Material> hellEnvLib;

	public Dictionary<string, Sprite> edenSprLib;
	public Dictionary<string, Sprite> hellSprLib;
	public List<string> matsLoaded;

	public List<GameObject> activeDots;

	public List<Sprite> backgroundSprites;

	public void Init()
	{
		panePrefab = Resources.Load<GameObject>("PanePrefab");
		dotLib = new Dictionary<string, GameObject>();
		matLib = new Dictionary<string, Material>();
		dotsLoaded = new List<string>();
		matsLoaded = new List<string>();
		activeDots = new List<GameObject>();

		edenDotLib = new Dictionary<string, Material>();
		edenEnvLib = new Dictionary<string, Material>();
		hellDotLib = new Dictionary<string, Material>();
		hellEnvLib = new Dictionary<string, Material>();

		edenSprLib = new Dictionary<string, Sprite>();
		hellSprLib = new Dictionary<string, Sprite>();

		LoadAllMats();
		LoadAllSprites();
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

	public GameObject CreatePane(bool isEden = true)
	{
		GameObject paneGO;

		//Create a sprite at a random position.
		paneGO = (GameObject)GameObject.Instantiate(panePrefab);
		paneGO.transform.position = Vector3.zero;

		Pane pane = paneGO.GetComponent<Pane>();

		paneGO.transform.position = new Vector3(Random.Range(-25.0f, 25.0f), Random.Range(0.001f, .25f), Random.Range(-25.0f, 25.0f));

		float scale = Random.Range(3, 15);

		//Give it an appropriate size.
		pane.transform.localScale = new Vector3(scale, scale, 1);

		//Give it a random texture
		pane.spriteRend.sprite = FindRandomSprite(isEden);

		//Give it a random color.
		pane.spriteRend.material = FindRandomMat(isEden, false);

		return paneGO;
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

	public void LoadAllSprites()
	{
		Sprite[] sprites;

		#region Eden Mat Loading
		sprites = Resources.LoadAll<Sprite>("Shapes/Eden");
		for (int i = 0; i < sprites.Length; i++)
		{
			//matsLoaded.Add(mats[i].name);
			edenSprLib.Add(sprites[i].name, sprites[i]);
		}

		sprites = Resources.LoadAll<Sprite>("Shapes/EndOfDays");
		for (int i = 0; i < sprites.Length; i++)
		{
			//matsLoaded.Add(mats[i].name);
			hellSprLib.Add(sprites[i].name, sprites[i]);
		}
		#endregion

		Debug.Log("Eden Sprites: " + edenSprLib.Count + "\nEnd Sprites: " + hellSprLib.Count + "\n");
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

	public Sprite FindRandomSprite(bool isEden = true)
	{
		if (isEden)
		{
			return FindRandomSpriteFromDictionary(edenSprLib);
			
		}
		else
		{
			return FindRandomSpriteFromDictionary(hellSprLib);
		}
	}

	public Material FindRandomMatFromDictionary(Dictionary<string, Material> dict)
	{
		List<Material> matList = dict.Values.ToList();

		return matList[Random.Range(0, matList.Count)];
	}

	public Sprite FindRandomSpriteFromDictionary(Dictionary<string, Sprite> dict)
	{
		List<Sprite> spriteList = dict.Values.ToList();

		return spriteList[Random.Range(0, spriteList.Count)];
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
