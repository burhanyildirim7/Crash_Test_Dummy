using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TextMeshPro distanceText;
    public Text distanceUiText;
    [SerializeField] float bestDistance;
    public bool distanceTextTime;
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
    public bool isForceTime,isForceTime2,isForceTime3,zeminde,havada;
    float lastForce = 2000;
    public GameObject paralarParenti,birdPrefab,onBoarding;
    public GameObject arac;
    int atisSirasi; // 0 ise ilk extra fýrlatma  1 ise ikinci extra fýrlatma mümkündür

    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        PlayerPrefs.SetFloat("distance",100);
        StartingEvents();
        DistanceTextGroundForStart();
        zeminde = true;
    }

	private void Update()
	{

		if (distanceTextTime)
		{
            CalculateDistance();
		}
        if (Input.GetMouseButtonDown(0))
		{
			if (havada)
			{
                if(atisSirasi == 0 && canTap)
				{
                    isForceTime = false;
                    isForceTime2 = true;
                    isForceTime3 = false;
                    canTap = false;
                    atisSirasi++;
                    onBoarding.SetActive(false);
				}
                else if(atisSirasi == 1 && canTap)
				{
                    isForceTime = false;
                    isForceTime2 = false;
                    isForceTime3 = true;
                    canTap = false;
                    atisSirasi++;
                    onBoarding.SetActive(false);
                }
			}
			//if (onBoarding.activeInHierarchy) {
   //             lastForce = 2000;
   //             isForceTime3 = true;
   //         }
            
            //if (status == 0)
            //{
            //	isStatus1 = true;
            //	status = 1;
            //	canTap = false;
            //	StartCoroutine(Tap1());
            //             onBoarding.SetActive(false);
            //             Time.timeScale = 1f;
            //         }
            //else if (status == 1)
            //{
            //	isStatus2 = true;
            //	status = 2;
            //	canTap = false;
            //	StartCoroutine(Tap2());
            //             onBoarding.SetActive(false);
            //             Time.timeScale = 1f;
            //         }
        }
	}


	private void FixedUpdate()
	{
		if (isForceTime)
		{
            Debug.Log("forse 1");
            foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
            float power = (float)(GameController.instance.power + GameController.instance.height) / 200;
            Debug.Log("p"+power);
            lastForce = lastForce*4 +  lastForce * 100 * power;
            Debug.Log(lastForce);
            foreach (Rigidbody rb in ragDollsRb) rb.AddForce(new Vector3(0, .7f, 1) * lastForce);
            isForceTime = false;
        }
		else if (isForceTime2)
		{
            Debug.Log("forse 2");
            foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
            float power = (float)(GameController.instance.power + GameController.instance.height) / 200;
            lastForce = lastForce * 4 + lastForce * 100 * power;
            foreach (Rigidbody rb in ragDollsRb) rb.AddForce(new Vector3(0, .7f, 1) * lastForce);
            isForceTime2 = false;
        }
		else if (isForceTime3)
		{
            Debug.Log("forse 3");
            foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
            float power = (float)(GameController.instance.power + GameController.instance.height) / 200;
            lastForce = lastForce * 4 + lastForce * 100 * power;
            foreach (Rigidbody rb in ragDollsRb) rb.AddForce(new Vector3(0, .7f, 1) * lastForce);
            isForceTime3 = false;
        }
		if (hips.GetComponent<Rigidbody>().velocity.y < 0  && !zeminde)
		{
            if(atisSirasi < 2)onBoarding.SetActive(true);
            Debug.Log("canTap true");
            lastForce = 2000;
            canTap = true;
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
        atisSirasi = 0;
        UIController.instance.bestDistanceText.text = "";
        distanceTextTime = false;
        playerAnimator.enabled = true;
        transform.parent = arac.transform;
        GameController.instance.SetAracSpeedAndRotate();
        GameController.instance.SetVehicleType();
        engel.GetComponent<Collider>().enabled = true;
        AracControl.instance.current = 0;
        status = 0;
        isStatus1 = isStatus2 = false;
        CloseColliders(); // titrememesi için bu silme bunu
        onBoarding.SetActive(false);
		tempY = 0;
        isForceTime = false;
        lastForce = 2000;
    }

	public void CalculateDistance()
	{
        float distance = hips.transform.position.z - engel.transform.position.z;
        distanceText.text = distance.ToString("#.00");
        distanceUiText.text = "Distance : " + distance.ToString("#.00");
        if (distance > bestDistance)
		{
            bestDistance = distance;
            PlayerPrefs.SetFloat("distance", distance + 5);
            distanceText.gameObject.transform.position = hips.transform.position + new Vector3(2.5f, 0, 0);
            UIController.instance.bestDistanceText.text = "Best Distance : " + distance.ToString("#.00");
		}           
	}

    public void DistanceTextUi()
	{
		float distance = hips.transform.position.z - engel.transform.position.z;
		distanceUiText.text = "Distance : " + distance.ToString("#.00");
        if(distance > bestDistance)
		{
            Debug.Log(distance);
            bestDistance = distance;
            PlayerPrefs.SetFloat("distance", distance);
            distanceText.gameObject.transform.position = new Vector3(.5f,1.15f,bestDistance);
        }
	}

    public void DistanceTextGroundForStart()
	{
        bestDistance = PlayerPrefs.GetFloat("distance");
        distanceUiText.text = "Distance : " + bestDistance.ToString("#.00");
        distanceText.gameObject.transform.position = new Vector3(0.5f,1.15f,bestDistance);
    }

    public IEnumerator ThrowPlayer()
	{
        OpenColliders();
        onBoarding.SetActive(false);
        StartCoroutine(CalculateCoins());
        playerAnimator.enabled = false;
        float power = GameController.instance.power + GameController.instance.height;
        float distance = 5 + power * GameController.instance.firlatmaForce;
        float time = 0;
        float aci = 0;
        Vector3 pos = transform.position;
        while (time < 1 && !isStatus1)
		{
            if (isStatus1) onBoarding.SetActive(false);
            yield return new WaitForSeconds(.005f);
            if (tempY > transform.position.y )
            {
                canTap = true;
                if (power < 4) { 
                    onBoarding.SetActive(true);
                    Time.timeScale = .5f;
                }
            }
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
            if (isStatus1) onBoarding.SetActive(false);
            DistanceTextUi();
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
        onBoarding.SetActive(false);
        yield return new WaitForSeconds(.02f);
        tempY = 0;
        while (time < 1 && !isStatus2)
        {
            if (isStatus2) onBoarding.SetActive(false);
            yield return new WaitForSeconds(.005f);
            canTap = false;
            if (tempY > transform.position.y ) { 
                canTap = true;
                if (power < 4)
                {
                    onBoarding.SetActive(true);
                    Time.timeScale = .5f;
                }
            }
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
            if (isStatus2) onBoarding.SetActive(false);
            DistanceTextUi();
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
        onBoarding.SetActive(false);
        while (time < 1 )
        {
            yield return new WaitForSeconds(.005f);
            canTap = false;
            if (tempY > transform.position.y)
            {
                canTap = true;
			}
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
            DistanceTextUi();
        }
        Time.timeScale = 1;
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
            int rnd = Random.Range(0, 400);
            if (rnd < 7)
            {
                GameObject coin = Instantiate(coinPrefab,pos, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }
            else if (rnd == 8)
            {
                GameObject bird = Instantiate(birdPrefab, pos, Quaternion.identity);
                bird.transform.tag = "kus";
                bird.transform.parent = paralarParenti.transform;
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
            int rnd = Random.Range(0, 400);
            if (rnd < 7)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }
            else if (rnd == 8)
            {
                GameObject bird = Instantiate(birdPrefab, pos, Quaternion.identity);
                bird.transform.tag = "kus";
                bird.transform.parent = paralarParenti.transform;
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
            int rnd = Random.Range(0, 400);
            if (rnd < 7)
            {
                GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);
                coin.transform.tag = "para";
                coin.transform.parent = paralarParenti.transform;
            }
            else if (rnd == 8)
			{
                GameObject bird = Instantiate(birdPrefab, pos, Quaternion.identity);
                bird.transform.tag = "kus";
                bird.transform.parent = paralarParenti.transform;
            }
            
        }
        yield return new WaitForSeconds(.001f);
    }

    public void ClearParalarParenti()
	{
        int childs = paralarParenti.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(paralarParenti.transform.GetChild(i).gameObject);
        }
    }
    


    public void OpenRagDolsRb()
	{
        onBoarding.SetActive(false);
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        ClearParalarParenti();
        foreach(Rigidbody rb in ragDollsRb)
		{
            rb.useGravity = true;
		}
	}

    public void CloseRagDolsRb()
    {
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.useGravity = false;
        }
    }

    public void CloseColliders()
	{
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.gameObject.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
        }
    }

    public void OpenColliders()
	{
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.gameObject.GetComponent<Collider>().enabled = true;
            rb.isKinematic = false;
        }
    }


}
