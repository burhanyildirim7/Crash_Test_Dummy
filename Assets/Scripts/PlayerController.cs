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
    public GameObject paralarParenti;

    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        StartingEvents();
       
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


	private void FixedUpdate()
	{
		if (isForceTime)
		{
            foreach (Rigidbody rb in ragDollsRb) rb.AddForce(Vector3.forward * lastForce);
            lastForce -= 50f;
            if (lastForce <= 0) isForceTime = false;
        }     
    }

	private void LateUpdate()
	{
        cameraLookAtTarget.transform.position = new(0, hips.transform.position.y, hips.transform.position.z + 2);
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
        StartCoroutine(CalculateCoins());
        playerAnimator.enabled = false;
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 5 + power * GameController.instance.firlatmaForce;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus1)
		{
            yield return new WaitForSeconds(.01f);
            if (tempY > transform.position.y) canTap = true;
			tempY = transform.position.y;
            time += 1 / (distance*50);
            if(aci < 180) aci = 180 *time;
            if (power < 10) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            else if (power < 20) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (200 / power);
            else if (power < 30) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (350 / power);
            else if (power < 50) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (500 / power);
            else if (power < 70) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (700 / power);
            else if (power < 100) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1000 / power);
            else if (power < 150) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1250 / power);
            else if (power < 200) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1500 / power);
            pos.z += .2f;
            transform.position = pos;        
            //transform.position = new Vector3(pos.x,pos.y +5,pos.z);        
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
        float distance = 3 + power / GameController.instance.firlatmaAzaltma1;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus2)
        {
            yield return new WaitForSeconds(.01f);
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 50);
            if (aci < 180) aci = 180 * time;
            if (power < 10) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            else if (power < 20) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (200 / power);
            else if (power < 30) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (350 / power);
            else if (power < 50) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (500 / power);
            else if (power < 70) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (700 / power);
            else if (power < 100) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1000 / power);
            else if (power < 150) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1250 / power);
            else if (power < 200) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1500 / power);
            pos.z += .2f;
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
        float distance = 1 + power / GameController.instance.firlatmaAzaltma2;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 )
        {
            yield return new WaitForSeconds(.01f);
            if (tempY > transform.position.y) canTap = true;
            tempY = transform.position.y;
            time += 1 / (distance * 50);
            if (aci < 180) aci = 180 * time;
            if (power < 10) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            else if (power < 20) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (200 / power);
            else if (power < 30) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (350 / power);
            else if (power < 50) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (500 / power);
            else if (power < 70) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (700 / power);
            else if (power < 100) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1000 / power);
            else if (power < 150) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1250 / power);
            else if (power < 200) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1500 / power);
            pos.z += .2f;
            transform.position = pos;
           
        }
        canTap = false;
        OpenRagDolsRb();
        isForceTime = true;

    }

    public IEnumerator CalculateCoins()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 5 + power * GameController.instance.firlatmaForce ;
        float time = 0;
        float aci = 0;
        Vector3 pos = hips.transform.position; 
        while (time < 1 )
        {
            time += 1 / (distance * 50);
            if (aci < 180) aci = 180 * time;
            if (power < 10) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            else if (power < 20) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (200 / power);
            else if (power < 30) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (350 / power);
            else if (power < 50) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (500 / power);
            else if (power < 70) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (700 / power);
            else if (power < 100) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1000 / power);
            else if (power < 150) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1250 / power);
            else if (power < 200) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1500 / power);
            pos.z += .2f;
            int rnd = Random.Range(0, 150);
            if (rnd < 3)
            {
                GameObject coin = Instantiate(coinPrefab,pos, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }
        }
        yield return new WaitForSeconds(.001f);
    }

    public IEnumerator CalculateCoins2()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 3 + power / GameController.instance.firlatmaAzaltma1 ;
        float time = 0;
        float aci = 0;
        Vector3 pos = hips.transform.position;
        while (time < 1)
        {
            time += 1 / (distance * 50);
            aci = 180 * time;
            if (power < 10) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            else if (power < 20) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (200 / power);
            else if (power < 30) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (350 / power);
            else if (power < 50) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (500 / power);
            else if (power < 70) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (700 / power);
            else if (power < 100) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1000 / power);
            else if (power < 150) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1250 / power);
            else if (power < 200) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1500 / power);
            pos.z += .2f;
            int rnd = Random.Range(0, 150);
            if (rnd < 3)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }
          
        }
        yield return new WaitForSeconds(.001f);
    }

    public IEnumerator CalculateCoins3()
    {
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 1 + power / GameController.instance.firlatmaAzaltma2;
        float time = 0;
        float aci = 0;
        Vector3 pos = hips.transform.position;
        while (time < 1)
        {
            time += 1 / (distance * 50);
            aci = 180 * time;
            if (power < 10) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (100 / power);
            else if (power < 20) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (200 / power);
            else if (power < 30) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (350 / power);
            else if (power < 50) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (500 / power);
            else if (power < 70) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (700 / power);
            else if (power < 100) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1000 / power);
            else if (power < 150) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1250 / power);
            else if (power < 200) pos.y += Mathf.Cos(Mathf.Deg2Rad * aci) / (1500 / power);
            pos.z += .2f;
            int rnd = Random.Range(0, 150);
            if (rnd < 3)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }
            
        }
        yield return new WaitForSeconds(.001f);
    }

    public void ClearParalarParenti()
	{
		while (paralarParenti.transform.childCount > 0) { Destroy(paralarParenti.transform.GetChild(0).gameObject); }
	}
    


    void OpenRagDolsRb()
	{
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        ClearParalarParenti();
        foreach(Rigidbody rb in ragDollsRb)
		{
            rb.useGravity = true;
		}
	}

    void CloseRagDolsRb()
    {
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = false;
        }
    }
}
