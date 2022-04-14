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
			Destroy(other.gameObject);
			Instantiate(coinEfecti, transform.position, Quaternion.identity);
			GameController.instance.para++;
			GameController.instance.levelPara++;
			PlayerPrefs.SetInt("para", GameController.instance.para);
			UIController.instance.SetParaText();

		}
		else if (other.CompareTag("kus"))
		{
			Instantiate(tuyEfecti, transform.position, Quaternion.identity);
			Destroy(other.gameObject);
		}

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("zemin") && GameController.instance.firstCrash)
		{
			GameController.instance.firstCrash = false;
			UIController.instance.ActivateWinScreen();
			AracControl.instance.isAracActive = false;
			PlayerController.instance.isForceTime2 = false;
			PlayerController.instance.zeminde = true;
		}
		else if (collision.transform.CompareTag("zemin"))
		{
			PlayerController.instance.distanceTextTime = true;
		}
	}
}
