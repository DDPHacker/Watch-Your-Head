using UnityEngine;
using System.Collections;

public class HPController : MonoBehaviour {
	public GameObject[] HPImage;

	public void show(int hp){
		for (int i=0;i<6;++i){
			if (hp==i)
				HPImage[i].SetActive(true);
			else HPImage[i].SetActive(false);
		}
	}
}
