using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage;



	void OnCollisionEnter(Collision collision)		//바닥에 총알 닿으면 오브젝트 파괴
	{
		if(collision.gameObject.tag == "Floor")
		{
			Destroy(gameObject, 3);
		}
		else if (collision.gameObject.tag == "Wall")
		{
			Destroy(gameObject);
		}
	}
	
	/*
	void OnTriggerEnter(Collision other)			//벽에 총알 닿으면 오브젝트 파괴
	{
		if(other.gameObject.tag == "Wall")
		{
			Destroy(gameObject);
		}
		else if(other.gameObject.tag == "Player")
		{
			Destroy(gameObject);
		}
	}
	*/
}