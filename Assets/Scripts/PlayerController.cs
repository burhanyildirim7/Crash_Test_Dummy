using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    
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
    public GameObject cameraLookAtTarget,hips;
    private bool isForceTime;
    float lastForce = 2000;

    public static PlayerController instance;
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

        cameraLookAtTarget.transform.position = new(0,hips.transform.position.y,hips.transform.position.z+2);
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

	private void FixedUpdate()
	{
		if (isForceTime)
		{
            foreach (Rigidbody rb in ragDollsRb) rb.AddForce(Vector3.forward * lastForce);
            lastForce -= 50f;
            if (lastForce <= 0) isForceTime = false;
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
            yield return new WaitForSeconds(.02f);
            if (tempY > transform.position.y) canTap = true;
			tempY = transform.position.y;
            time += 1 / (distance*5);
            if(aci < 180) aci = 180 *time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci)/(100/power);
            pos.z += .5f;
            transform.position = pos;        
        }
        if (!isStatus1)
        {
            canTap = false;
            OpenRagDolsRb();
            isForceTime = true;       
        }
       
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
            yield return new WaitForSeconds(.02f);
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            transform.position = pos;
        }
        if (!isStatus2)
        {
            canTap = false;
            OpenRagDolsRb();
            isForceTime = true;

        }
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
            yield return new WaitForSeconds(.02f);
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 5);
            if (aci < 180) aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            transform.position = pos;      
        }
        canTap = false;
        OpenRagDolsRb();
        isForceTime = true;

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
            int rnd = Random.Range(0, 10);
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
            aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 10);
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
            aci = 180 * time;
            pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            pos.z += .5f;
            int rnd = Random.Range(0, 10);
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
