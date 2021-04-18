using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	/*
	//체력과 컴포넌트 변수
	public int maxHealth;	
	public int curHealth;
	
	//플레이어 타겟
	public Transform target;
	
	Rigidbody rigid;
	BoxCollider boxCollider;
	
	//플레이어 추적 네비게이션
	NavMeshAgent nav;


	
	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
		nav = GetComponent<NavMeshAgent>();
	}
	
	void Update()
	{
		nav.SetDestination(target.position);		//도착 목표 위치 지정 함수 설정
	}
	
	void FreezeVelocity()
	{
		rigid.velocity = Vector3.zero;				//NavAgent 이동 방해 않도록 로직
		rigid.angularVelocity = Vector3.zero;		
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Bullet") {								// 총알 데미지 현재 체력에서 빼기
			Weapon weapon = other.GetComponent<Bullet>();
			curHealth -= Bullet.damage;
			
			Debug.Log("Range " + curHealth);
		}
	}
	*/
}
