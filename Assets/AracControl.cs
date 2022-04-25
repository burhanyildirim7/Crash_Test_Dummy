using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using Cinemachine;

public class AracControl : MonoBehaviour
{
    public CinemachineVirtualCamera cmVcam;
    public PathCreator pathCreator;
    public RoadMeshCreator roadMeshCreator;
    public static AracControl instance;
    public float speed = 5;
    [HideInInspector]public float distanceTravelled;
    // way point ile movement
    public Transform[] waypoints;
    public int current = 0;
    public bool isAracActive = false;
    public GameObject kirikCamlarPrefab;
    [HideInInspector]public float speedMultiplier = 0;
    bool azaltma, hizlanma;
    [HideInInspector]public bool dokunmatik,yokus;
    public float anlikYakit;




    public List<GameObject> Explossions = new();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}
	void Start()
    {
        distanceTravelled = 8;
        UpdateRoad();
    }

	private void Update()
	{
        if (Input.GetMouseButton(0) && isAracActive && dokunmatik)
        {
            azaltma = false;
            if(speedMultiplier < 1)speedMultiplier += Time.deltaTime;
            if (anlikYakit > 0) anlikYakit -= Time.deltaTime*3;
            
            UIController.instance.SetSpeedSlider();
        }

        if (azaltma && isAracActive && !yokus)
        {
            if (speedMultiplier > 0) speedMultiplier -= Time.deltaTime;

            if (speedMultiplier <= 0)
            {
                speedMultiplier = 0;
                azaltma = false;
            }
        }

        if (Input.GetMouseButtonUp(0) && isAracActive && dokunmatik)
        {
            azaltma = true;
        }

        if (transform.position.y < 2.9f) hizlanma = false;

        if(hizlanma && isAracActive)
		{
            speedMultiplier += Time.deltaTime*0.1f;
            azaltma = false;
		}
		//else if( transform.position.y < 2.9f && GameController.instance.height >= 2 && isAracActive)
		//{
  //          if (speedMultiplier > .4f) speedMultiplier -= Time.deltaTime * 0.05f;

  //          if (speedMultiplier <= .4f)
  //          {
  //              speedMultiplier = .4f;
  //              azaltma = false;
  //          }
  //      }
        
    }



	private void FixedUpdate()
	{
        Debug.Log(speedMultiplier);
        if (isAracActive)
		{
            if (speedMultiplier > 1) speedMultiplier = 1;
            speed = 8 + (GameController.instance.power + GameController.instance.height) / 10;
            if (speed > 30) speed = 30;

            distanceTravelled += speed * Time.deltaTime * speedMultiplier;
            if (distanceTravelled == 0) distanceTravelled = 8; 
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            PlayerController.instance.cameraLookAtTarget.transform.position = transform.position + new Vector3 (0,5,0);
            transform.rotation = Quaternion.Euler(pathCreator.path.GetRotationAtDistance(distanceTravelled).eulerAngles.x, pathCreator.path.GetRotationAtDistance(distanceTravelled).eulerAngles.y, 0);          
        }
	}

    public void UpdateRoad()
	{
        if (waypoints.Length > 0)
        {
            BezierPath bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
            pathCreator.bezierPath = bezierPath;
        }
        pathCreator.TriggerPathUpdate();
        roadMeshCreator.UpdateRoad();

		

    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("engel"))
		{
            yokus = false;
            hizlanma = false;
            UIController.instance.yakitSliderPanel.SetActive(false);
            StartCoroutine(PlayerController.instance.TimeSlow());
            cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
            cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
            cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
            cmVcam.GetCinemachineComponent<CinemachineTransposer>().m_YawDamping = 0;
            isAracActive = false;
            other.GetComponent<Collider>().enabled = false;
            PlayerController.instance.transform.parent = null;
            PlayerController.instance.playerAnimator.enabled = false;
            PlayerController.instance.OpenColliders();
            //PlayerController.instance.isForceTime = true;
            PlayerController.instance.zeminde = false;
            PlayerController.instance.havada = true;
            PlayerController.instance.distanceTextTime = true;
            //StartCoroutine( PlayerController.instance.ThrowPlayer());
            PlayerController.instance.Jump1();
            distanceTravelled = 0;
            if(GameController.instance.power < 32)Instantiate(Explossions[0], other.transform.position + new Vector3(0,2,0), Quaternion.identity);
            else Instantiate(Explossions[1], other.transform.position + new Vector3(0,2,1), Quaternion.identity);
			if (GameController.instance.type >= 8)
			{
                GameObject kirikCamlar = Instantiate(kirikCamlarPrefab, PlayerController.instance.hips.transform.position, Quaternion.identity);
                for (int i = 0; i < kirikCamlar.transform.childCount; i++)
                {
                    Vector3 forcePower = new(Random.Range(-10, 10), Random.Range(0, 20), Random.Range(10, 30));
                    kirikCamlar.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(forcePower * 300);
                }
                DestroyMe(kirikCamlar);
            }
            
		}
        else if (other.CompareTag("timeslow"))
		{
            PlayerController.instance.CalculateCoins1();
            Time.timeScale = .4f;
            azaltma = false;
            hizlanma = false;
            dokunmatik = false;
           
		}else if (other.CompareTag("hizlanma"))
		{
            hizlanma = true;
            Debug.Log(hizlanma);
            yokus = true;
		}

	}

    public IEnumerator DestroyMe(GameObject obj)
	{
        yield return new WaitForSeconds(3f);
        Destroy(obj);
	}

    public IEnumerator OpenRagdols()
    {
        yield return new WaitForSeconds(2f);
        PlayerController.instance.playerAnimator.enabled = false;
    }


 

}
