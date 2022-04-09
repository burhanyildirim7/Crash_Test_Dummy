using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracControl : MonoBehaviour
{
    public static AracControl instance;

    [HideInInspector]public Rigidbody rb;

    public List<GameObject> Explossions = new();

	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}


	void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    public IEnumerator DelayAndActivateCar()
	{
        yield return new WaitForSeconds(1f);
        rb.useGravity = true;
        StartCoroutine(IncreaseVelocity());
	}

    public IEnumerator IncreaseVelocity()
    {
        yield return new WaitForSeconds(.001f);
        //rb.velocity = Vector3.forward * 5;
        float velocitySpeed = GameController.instance.power;
		//rb.velocity = transform.forward * velocitySpeed;
		while (transform.childCount > 1)
		{
			velocitySpeed += 50f;
			//rb.velocity = transform.forward * velocitySpeed;
			rb.AddForce(transform.forward * velocitySpeed);
			yield return new WaitForSeconds(.01f);
		}
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.zero);
        rb.constraints = RigidbodyConstraints.FreezePosition;

	}

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("engel"))
		{
            other.GetComponent<Collider>().enabled = false;
            rb.velocity = Vector3.zero;
            PlayerController.instance.transform.parent = null;
            StartCoroutine(PlayerController.instance.ThrowPlayer());
            Instantiate(Explossions[0], other.transform.position + new Vector3(0,1,1), Quaternion.identity);
            Debug.Log("engel");
		}
	}
}
