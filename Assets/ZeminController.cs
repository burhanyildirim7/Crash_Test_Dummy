using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeminController : MonoBehaviour
{
	public GameObject tozPrefab;
	public int collisionCount = 0;


	public static ZeminController instance;
	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(this);
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("ragdolkemik") && collisionCount < 10)
		{
			Instantiate(tozPrefab, collision.transform.position, Quaternion.identity);
			collisionCount++;
		}
	}
}
