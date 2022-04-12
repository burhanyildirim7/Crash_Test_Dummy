using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaController : MonoBehaviour
{
	public GameObject coinEfecti, tuyEfecti;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("para"))
		{
			Instantiate(coinEfecti, transform.position, Quaternion.identity);
			Destroy(other.gameObject);
			GameController.instance.para++;
			PlayerPrefs.SetInt("para", GameController.instance.para);
			UIController.instance.SetParaText();
		}
		else if (other.CompareTag("kus"))
		{
			Instantiate(tuyEfecti, transform.position, Quaternion.identity);
			Destroy(other.gameObject);
		}
	}
}
