using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clicky : MonoBehaviour
{
	List<string> names;

	void Start()
	{
		names = new List<string>();
		names.Add("Cyan Dot");
		names.Add("Green Dot");
		names.Add("Pink Dot");
	}

	void Update ()
	{
		if(Input.GetKey(KeyCode.Q))
		{
			for (int i = 0; i < 20; i++)
			{
				/*GameObject dot = DotManager.Inst.FindOrLoadDot(names[Random.Range(0, names.Count)]);

				Vector3 pos = new Vector3(Random.Range(-8, 8), Random.Range(-7, 7), Random.Range(-1, -50));
				GameObject.Destroy(GameObject.Instantiate(dot, pos, Quaternion.identity), 1);
				*/
				Vector3 pos = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-7.0f, 7.0f), Random.Range(-1.0f, -5.0f));
				GameObject.Destroy(DotManager.Inst.CreateDot(names[Random.Range(0, names.Count)], pos), 1);
			}
		}
	}
}
