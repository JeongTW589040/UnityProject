using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	public int maxHealth;
	public int curHealth;
	public Transform target;

	public bool isChase;

	Rigidbody rigid;
	BoxCollider boxCollider;
	Material mat;
	NavMeshAgent nav;
	Animator anim;



	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		boxCollider = GetComponent<BoxCollider>();
		mat = GetComponentInChildren<MeshRenderer>().material;
		nav = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator>();

		Invoke("ChaseStart", 2);
	}

	void Update()
	{
		if (isChase)
			nav.SetDestination(target.position); 
	}

	public void HitByGrenades(Vector3 explosionPos)
    {
		curHealth -= 100;
		Vector3 reactVec = transform.position - explosionPos;
		StartCoroutine(OnDamage(reactVec, true));
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Melee")
		{
			Weapon weapon = other.GetComponent<Weapon>();
			curHealth -= weapon.damage;

			Vector3 reactVec = transform.position - other.transform.position;

			StartCoroutine(OnDamage(reactVec, false));
		}
		else if (other.tag == "Bullet")
		{
			Bullet bullet = other.GetComponent<Bullet>();
			curHealth -= bullet.damage;

			Vector3 reactVec = transform.position - other.transform.position;

			Destroy(other.gameObject);

			StartCoroutine(OnDamage(reactVec, false));
		}
	}

	IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
	{
		mat.color = Color.red;
		yield return new WaitForSeconds(0.1f);

		if(curHealth > 0)
        {
			mat.color = Color.white;
        }
        else
        {
			mat.color = Color.gray;
			gameObject.layer = 12;  //EnemyDead
			isChase = false;
			nav.enabled = false;
			anim.SetTrigger("doDie");

            if (isGrenade)
            {
				//넉백
				reactVec = reactVec.normalized;
				reactVec += Vector3.up * 3;
				rigid.freezeRotation = false;
				rigid.AddForce(reactVec * 5, ForceMode.Impulse);
				rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
			}
            else
            {
				//넉백
				reactVec = reactVec.normalized;
				reactVec += Vector3.up;
				rigid.AddForce(reactVec * 5, ForceMode.Impulse);
			}
			Destroy(gameObject, 2);
		}
    }

	void FixedUpdate()
	{
		FreezeVelocity();
	}

	void FreezeVelocity()
	{
		if (isChase)
		{
			rigid.velocity = Vector3.zero;
			rigid.angularVelocity = Vector3.zero;
		}
	}

	void ChaseStart()
	{
		isChase = true;
		anim.SetBool("isWalk", true);
	}
}