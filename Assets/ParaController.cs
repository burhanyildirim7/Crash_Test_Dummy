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
			GameController.instance.para+=15;
			GameController.instance.levelPara+=15;
			PlayerPrefs.SetInt("para", GameController.instance.para);
			//UIController.instance.SetParaText();

		}

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("zemin") && GameController.instance.firstCrash)
		{
			GameController.instance.firstCrash = false;	
			AracControl.instance.isAracActive = false;
			PlayerController.instance.zeminde = true;		
			PlayerController.instance.canTap = false;
			PlayerController.instance.OpenGravities();
			Time.timeScale = 1;
			PlayerController.instance.onBoarding.SetActive(false);
			StartCoroutine(ActivateDistanceTime());
		}
		if (collision.transform.CompareTag("zemin"))
		{
			Time.timeScale = 1;
			PlayerController.instance.onBoarding.SetActive(false);
			PlayerController.instance.canTap= false;
		}

	}

	IEnumerator ActivateDistanceTime()
	{
		yield return new WaitForSeconds(1f);
		PlayerController.instance.isDistanceTime = true;
	}
}
