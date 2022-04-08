using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    [SerializeField]private GameObject coinPrefab;
    [SerializeField]private GameObject engel;
    int status = 0;
    bool canTap, isStatus1,isStatus2;
    float tempY;
    public Animator playerAnimator;
    public List<Rigidbody> ragDollsRb = new();


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        StartingEvents();
        StartCoroutine(CalculateCoins());
    }

	private void Update()
	{


		if (Input.GetMouseButtonDown(0) && canTap)
		{
			if (status == 0)
			{
				isStatus1 = true;
				status = 1;
				canTap = false;
				StartCoroutine(Tap1());
			}
			else if (status == 1)
			{
				isStatus2 = true;
				status = 2;
				canTap = false;
				StartCoroutine(Tap2());
			}
		}
	}


    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {

        //transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        //transform.parent.transform.position = Vector3.zero;
        //GameController.instance.isContinue = false;
        //GameController.instance.score = 0;
        //transform.position = new Vector3(0, transform.position.y, 0);
        //GetComponent<Collider>().enabled = true;

    }



    public IEnumerator ThrowPlayer()
	{
        playerAnimator.enabled = false;
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power/2;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus1)
		{
            if (tempY > transform.position.y) canTap = true;
			tempY = transform.position.y;
            time += 1 / (distance*5);
            if(aci < 180) aci = 180 *time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci)/(100/power);
            pos.z += .5f;
            transform.position = pos;
            yield return new WaitForSeconds(.015f);
        }
        OpenRagDolsRb();
        canTap = false;
    }

    public IEnumerator Tap1()
    {
        CloseRagDolsRb();
        StopCoroutine(ThrowPlayer());
        StartCoroutine(CalculateCoins2());
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power/3;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus2)
        {
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            transform.position = pos;
            yield return new WaitForSeconds(.015f);
        }
        OpenRagDolsRb();
        canTap = false;
    }

    public IEnumerator Tap2()
    {
        CloseRagDolsRb();
        StopCoroutine(Tap1());
        StartCoroutine(CalculateCoins3());
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power/4;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 )
        {
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            transform.position = pos;
            yield return new WaitForSeconds(.015f);
        }
        OpenRagDolsRb();
        canTap = false;
    }

    public IEnumerator CalculateCoins()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power / 2;
        float time = 0;
        float aci = 0;
        Vector3 pos = engel.transform.position;
        while (time < 1 )
        {
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 15);
            if (rnd == 0)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CalculateCoins2()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power / 3;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1)
        {
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 15);
            if (rnd == 0)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator CalculateCoins3()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 10 + power / 4;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1)
        {
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 15);
            if (rnd == 0)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void OpenRagDolsRb()
	{
        foreach(Rigidbody rb in ragDollsRb)
		{
            rb.useGravity = true;
		}
	}

    void CloseRagDolsRb()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = false;
        }
    }
}
