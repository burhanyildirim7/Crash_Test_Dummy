using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AracControl : MonoBehaviour
{
    public static AracControl instance;

    [HideInInspector]public Rigidbody rb;

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
	}

    public IEnumerator IncreaseVelocity()
    {
        yield return new WaitForSeconds(.1f);
        rb.velocity = Vector3.forward * 5;
        float velocitySpeed = 5;
        while (transform.childCount > 0)
        {
            velocitySpeed += .4f;
            rb.velocity = Vector3.forward * 5;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("engel"))
		{
            other.GetComponent<Collider>().enabled = false;
            rb.velocity = Vector3.zero;
            PlayerController.instance.transform.parent = null;
            StartCoroutine(PlayerController.instance.ThrowPlayer());
            Debug.Log("engel");
		}
	}
}
