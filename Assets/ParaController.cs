using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaController : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("para"))
		{
			Destroy(other.gameObject);
			GameController.instance.para++;
			PlayerPrefs.SetInt("para", GameController.instance.para);
			UIController.instance.SetParaText();
		}
	}
}
