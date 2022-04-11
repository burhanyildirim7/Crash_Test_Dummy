using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AracControl : MonoBehaviour
{
    public static AracControl instance;

    // way point ile movement
    public GameObject[] wayPoints;
    int current = 0;
    float rotSpeed;
    public float speed;
    float WPradius = 1;


    [HideInInspector]public Rigidbody rb;
    [HideInInspector]public bool canForce;
    public Transform startingTarget, startingTarget2, startingTarget3;

    public List<GameObject> Explossions = new();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}
	void Start()
    {
        DOTween.Init();
        canForce = true;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        //UpdateDoTweenWayPoints();
    }



    public IEnumerator DelayAndActivateCar()
	{
        yield return new WaitForSeconds(1f);
        //rb.useGravity = true;
        //StartCoroutine(IncreaseVelocity());
	}

    public IEnumerator IncreaseVelocity()
    {
       yield return new WaitForSeconds(.001f);
  //      float velocitySpeed = GameController.instance.power;
		//while (canForce)
		//{
		//	velocitySpeed += 50f;
		//	rb.AddForce(transform.forward * velocitySpeed);
		//	yield return new WaitForSeconds(.01f);
		//}      
	}

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("engel"))
		{
            
            canForce = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.zero);
            rb.constraints = RigidbodyConstraints.FreezePosition;
            StopCoroutine(IncreaseVelocity());
            other.GetComponent<Collider>().enabled = false;
            PlayerController.instance.transform.parent = null;
            StartCoroutine(PlayerController.instance.ThrowPlayer());
            Instantiate(Explossions[0], other.transform.position + new Vector3(0,1,1), Quaternion.identity);
            Debug.Log("engel");
		}
	}

 //   public void UpdateDoTweenWayPoints()
	//{
 //       GetComponent<DOTweenPath>().path.wps[2] = startingTarget2.transform.position;
 //       GetComponent<DOTweenPath>().path.wps[1] = startingTarget.transform.position;
 //       GetComponent<DOTweenPath>().path.wps[3] = startingTarget3.transform.position;
 //       Debug.Log(GetComponent<DOTweenPath>().path.wps.Length);
 //   }
}
