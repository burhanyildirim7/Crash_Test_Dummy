using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracControl : MonoBehaviour
{
    public static AracControl instance;

    // way point ile movement
    public GameObject[] wayPoints;
    public int current = 0;
    float WPradius = 1;
    public bool isAracActive = false;
    public GameObject kirikCamlarPrefab;


    public List<GameObject> Explossions = new();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}
	void Start()
    {
       // isAracActive = true;
        transform.position = wayPoints[0].transform.position;
    }

	private void Update()
	{
		if (isAracActive)
		{
            if (Vector3.Distance(wayPoints[current].transform.position, transform.position) < WPradius)
            {
                current++;

                if (current >= wayPoints.Length)
                {
                    current = 0;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[current].transform.position, Time.deltaTime * GameController.instance.aracSpeed);
            Vector3 relativePos = wayPoints[current].transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, GameController.instance.aracrRotSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("engel"))
		{
            isAracActive = false;
            other.GetComponent<Collider>().enabled = false;
            PlayerController.instance.transform.parent = null;
            StartCoroutine(PlayerController.instance.ThrowPlayer());
            Instantiate(Explossions[0], other.transform.position + new Vector3(0,1,1), Quaternion.identity);
            Debug.Log("engel");
			if (GameController.instance.type >= 8)
			{
                GameObject kirikCamlar = Instantiate(kirikCamlarPrefab, PlayerController.instance.hips.transform.position, Quaternion.identity);
                for (int i = 0; i < kirikCamlar.transform.childCount; i++)
                {
                    Vector3 forcePower = new(Random.Range(-10, 10), Random.Range(0, 20), Random.Range(10, 30));
                    kirikCamlar.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(forcePower * 100);
                }
                DestroyMe(kirikCamlar);
            }
            
		}
	}

    public IEnumerator DestroyMe(GameObject obj)
	{
        yield return new WaitForSeconds(3f);
        Destroy(obj);
	}


}
