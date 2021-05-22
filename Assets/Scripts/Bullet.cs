using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage;
	public bool isMelee;


	void OnCollisionEnter(Collision collision)		//바닥에 총알 닿으면 오브젝트 파괴
	{
		if(collision.gameObject.tag == "Floor")
		{
			Destroy(gameObject, 3);
		}
		/*
		else if (collision.gameObject.tag == "Player")
		{
			Destroy(gameObject);
		}
		*/
	}

    void OnTriggerEnter(Collider other)
    {
		if (!isMelee && other.gameObject.tag == "Wall")
		{
			Destroy(gameObject);
		}
	}
}