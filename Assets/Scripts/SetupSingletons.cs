using UnityEngine;
using System.Collections;

public class SetupSingletons : MonoBehaviour
{
	void Awake()
	{
		DotManager.Inst.Init();
	}
}
