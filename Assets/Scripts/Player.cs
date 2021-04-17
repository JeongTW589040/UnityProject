using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //캐릭터 이동
    public float speed;
    public float jumpPower;
    public float dodgeDelay;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        //캐릭터 이동
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if (isDodge)
            moveVec = dodgeVec;

        if (wDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;

        //이동 애니메이션
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        //캐릭터 점프
        if (jDown && !isJump && !isDodge && moveVec == Vector3.zero)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }

        //캐릭터 회피
        if (jDown && !isJump && !isDodge && moveVec != Vector3.zero)
        {
            dodgeVec = moveVec;
            speed *= 2;

            anim.SetTrigger("doDodge");

            isDodge = true;
           
            Invoke("DodgeOut", dodgeDelay);
        }

        //캐릭터 시점
        transform.LookAt(transform.position + moveVec);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //캐릭터 점프
        if(collision.gameObject.tag == "Floor") //Floor 태그와 충돌 시
        {
            anim.SetBool("isJump", false);

            isJump = false;
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
}
