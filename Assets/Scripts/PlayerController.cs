using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Text distanceTextZemin,bestDistancneTextZemin;
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
    public bool isDistanceTime;
    float tempY;
    public Animator playerAnimator;
    public List<Rigidbody> ragDollsRb = new();
    public GameObject cameraLookAtTarget,hips;
    public bool isForceTime,isForceTime2,isForceTime3,zeminde,havada;
    float lastForce = 2000;
    float tempLastForce;
    public GameObject paralarParenti,birdPrefab,onBoarding;
    public GameObject arac;
    int atisSirasi; // 0 ise ilk extra fýrlatma  1 ise ikinci extra fýrlatma mümkündür
    public GameObject groundTextCanvas,cizgi,distancePanel,bestDistancePanel;
    

    public static PlayerController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start()
    {
        DOTween.Init();
        //PlayerPrefs.DeleteAll();
       // PlayerPrefs.SetInt("distance",100);
        StartingEvents();
        DistanceTextGroundForStart();
        zeminde = true;
    }

	private void Update()
	{
       
        if (zeminde && Mathf.Abs(hips.GetComponent<Rigidbody>().velocity.z) <= .1f && isDistanceTime)
		{
			CalculateDistance();
            isDistanceTime = false;
            Time.timeScale = 1;
            PlayerController.instance.onBoarding.SetActive(false);
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
                    Time.timeScale = 1;
				}
                else if(atisSirasi == 1 && canTap)
				{
                    isForceTime = false;
                    isForceTime2 = false;
                    isForceTime3 = true;
                    canTap = false;
                    atisSirasi++;
                    onBoarding.SetActive(false);
                    Time.timeScale = 1;
                }
			}
        }

        if (isForceTime)
        {
            StartCoroutine(TimeSlow());
            lastForce = 2000;
            isForceTime = false;
            Debug.Log("forse 1");
            foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
            float power = (float)(GameController.instance.power + GameController.instance.height) / 2;
            lastForce = lastForce * 12 + lastForce * power * GameController.instance.firlatmaForce; ;
            tempLastForce = lastForce;
            Trajectory.instance.SimulateTrajectory(hips, hips.transform.position, new Vector3(0, .3f, 1) * lastForce);
            hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1) * lastForce);
            //hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            //hips.transform.DOJump(new Vector3(0, 1, 200), 20, 1, 4).SetEase(Ease.Linear).OnComplete(() =>
            //         hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1)*20000)
            //); ;       
        }
        else if (isForceTime2)
        {
            //StartCoroutine(CloseConstraints());
            isForceTime2 = false;
            foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
            float power = (float)(GameController.instance.power + GameController.instance.height) / 5;
            //lastForce = lastForce * 12 + lastForce * power;
            lastForce = tempLastForce / 1.5f;
            tempLastForce = lastForce;
            Trajectory.instance.SimulateTrajectory(hips, hips.transform.position, new Vector3(0, .3f, 1) * lastForce);
            hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1) * lastForce);

        }
        else if (isForceTime3)
        {
            //StartCoroutine(CloseConstraints());
            isForceTime3 = false;
            foreach (Rigidbody rb in ragDollsRb) rb.velocity = Vector3.zero;
            float power = (float)(GameController.instance.power + GameController.instance.height) / 5;
            //lastForce = lastForce * 12 + lastForce * power;
            lastForce = tempLastForce / 1.5f;
            tempLastForce = lastForce;
            Trajectory.instance.SimulateTrajectory(hips, hips.transform.position, new Vector3(0, .3f, 1) * lastForce);
            hips.GetComponent<Rigidbody>().AddForce(new Vector3(0, .3f, 1) * lastForce);

        }
        if (hips.GetComponent<Rigidbody>().velocity.y < 0 && !zeminde)
        {
            Debug.Log("canTap true");
            lastForce = 2000;
            canTap = true;
        }
        if (hips.transform.position.y > 3 &&  hips.transform.position.y < 30 && hips.GetComponent<Rigidbody>().velocity.y < 0 )
        {
            if (atisSirasi < 2)
            {
                onBoarding.SetActive(true);
                Time.timeScale = .5f;
            }
        }
    }

	private void LateUpdate()
	{
        cameraLookAtTarget.transform.position = new(0, hips.transform.position.y, hips.transform.position.z + 2);
    }

	public IEnumerator TimeSlow()
    {
        Time.timeScale = .1f;
        yield return new WaitForSeconds(.1f);
        Time.timeScale = 1;
       
    }

    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {
        atisSirasi = 0;
        canTap = false;
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
        isForceTime2 = false;
        isForceTime3 = false;
        lastForce = 2000;
        isDistanceTime = false;
        groundTextCanvas.transform.position = new Vector3(0, 0, -30);
    }

	public void CalculateDistance()
	{
        UIController.instance.ActivateWinScreen();
        groundTextCanvas.transform.position = new Vector3(1.2f,10,hips.transform.position.z);
        groundTextCanvas.transform.DOMove(new Vector3(1.2f, 2.2f, hips.transform.position.z), 1f).SetEase(Ease.OutBounce);
        bestDistancePanel.SetActive(false);
        distancePanel.SetActive(true);
        int distance = (int)hips.transform.position.z - (int)engel.transform.position.z;
        distanceTextZemin.text = distance.ToString() + "m";
        distanceUiText.text =  distance.ToString() + "m";
        bestDistance = PlayerPrefs.GetInt("distance");
        if (distance > bestDistance)
		{
            bestDistance = distance;
            PlayerPrefs.SetInt("distance", distance);
            cizgi.transform.position = new Vector3(0, 1.15F, hips.transform.position.z);
            bestDistancneTextZemin.text = distance.ToString() + "m";
            bestDistancePanel.SetActive(true);
            distancePanel.SetActive(false);
            UIController.instance.bestDistanceText.text = "Best Distance : " + distance.ToString();
		}
        
    }

    public void DistanceTextUi()
	{
        int distance = (int)hips.transform.position.z - (int)engel.transform.position.z;
        distanceUiText.text = distance.ToString() + "m";
        if(distance > bestDistance)
		{
            Debug.Log(distance);
            bestDistance = distance;
            PlayerPrefs.SetInt("distance", distance);
            groundTextCanvas.transform.position = new Vector3(.5f,5f,hips.transform.position.z);
        }
	}

    public void DistanceTextGroundForStart()
	{
        bestDistance = PlayerPrefs.GetInt("distance");
        distanceUiText.text =  bestDistance.ToString() + "m";
        cizgi.transform.position = new Vector3(0, 1.15F, engel.transform.position.z + bestDistance);
    }

	#region ESKI SISTEM 
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
	#endregion


	public void ClearParalarParenti()
	{
        int childs = paralarParenti.transform.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.Destroy(paralarParenti.transform.GetChild(i).gameObject);
        }
    }
    

    public void OpenKinematics()
	{
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.isKinematic = true;
        }
    }

    public void CloseKinematics()
	{
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.isKinematic = false;
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

    public IEnumerator CloseConstraints()
	{
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;

        yield return new WaitForSeconds(.2f);
        OpenConstraints();

    }

    public void OpenConstraints()
    {
        foreach (Rigidbody rb in ragDollsRb)
        {
            rb.constraints = RigidbodyConstraints.None;
        }
        hips.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
    }


}
