using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float dodgeDelay;
    public GameObject[] weapons;
    public bool[] hasWeapons;

    float hAxis;
    float vAxis;
    int equipWeaponIndex = -1;
    float fireDelay;

    bool wDown;  //Walk
    bool jDown;  //Jump
    bool fDown;  //Fire;
    bool iDown;  //Interation
    bool sDown1; //Swap1
    bool sDown2; //Swap2
    bool sDown3; //Swap3
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady = true;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    GameObject nearObject;
    Weapon equipWeapon;



    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();
        Move();
        Swap();
        Attack();
        Interation();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    //////// 캐릭터 ////////
    void Move()
    {
        //캐릭터 이동
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge) //닷지 중일 때
            moveVec = dodgeVec;

        if (isSwap || !isFireReady) //교체, 공격 중일 때
            moveVec = Vector3.zero;

        if (wDown) //점프
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;

        //이동 애니메이션
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        //캐릭터 점프
        if (jDown && !isJump && !isDodge && !isSwap && moveVec == Vector3.zero)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //점프 애니메이션
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }

        //캐릭터 회피
        if (jDown && !isJump && !isDodge && !isSwap && moveVec != Vector3.zero)
        {
            dodgeVec = moveVec;
            speed *= 2;

            //닷지 애니메이션
            anim.SetTrigger("doDodge");

            isDodge = true;

            //닷지 딜레이
            Invoke("DodgeOut", dodgeDelay);
        }

        //캐릭터 시점
        transform.LookAt(transform.position + moveVec);
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Floor 태그와 충돌 시
        if (collision.gameObject.tag == "Floor") 
        {
            anim.SetBool("isJump", false);

            isJump = false;
        }
    }

    //////// 공격 ////////
    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;

        isFireReady = equipWeapon.rate < fireDelay;

        if (fDown && isFireReady && !isDodge && !isSwap)
        {
            equipWeapon.Use();

            anim.SetTrigger("doSwing");
            fireDelay = 0;
        }
    }

    //////// 아이템 ////////
    void Swap()
    {
        //무기가 없거나 그 무기를 들고 있을 때
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        //기본값
        int weaponIndex = -1;

        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        //무기 교체
        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if(equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            //교체 애니메이션
            anim.SetTrigger("doSwap");

            isSwap = true;

            //교체 딜레이
            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        //필드에 아이템과 상호작용
        if(iDown && nearObject != null && !isJump)
        {
            //Weapos 태그와 충돌 시
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }

    
}
