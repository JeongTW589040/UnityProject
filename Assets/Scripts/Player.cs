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

    bool wDown;  //Walk
    bool jDown;  //Jump
    bool iDown;  //Interation
    bool sDown1; //Swap1
    bool sDown2; //Swap2
    bool isJump;
    bool isDodge;
    bool isSwap;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;
    GameObject nearObject;
    GameObject equipWeapon;



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
        Interation();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
    }

    //////// ĳ���� ////////
    void Move()
    {
        //ĳ���� �̵�
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (isSwap)
            moveVec = Vector3.zero;

        if (wDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;

        //�̵� �ִϸ��̼�
        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        //ĳ���� ����
        if (jDown && !isJump && !isDodge && !isSwap && moveVec == Vector3.zero)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            //���� �ִϸ��̼�
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            isJump = true;
        }

        //ĳ���� ȸ��
        if (jDown && !isJump && !isDodge && !isSwap && moveVec != Vector3.zero)
        {
            dodgeVec = moveVec;
            speed *= 2;

            //���� �ִϸ��̼�
            anim.SetTrigger("doDodge");

            isDodge = true;

            //���� ������
            Invoke("DodgeOut", dodgeDelay);
        }

        //ĳ���� ����
        transform.LookAt(transform.position + moveVec);
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Floor �±׿� �浹 ��
        if (collision.gameObject.tag == "Floor") 
        {
            anim.SetBool("isJump", false);

            isJump = false;
        }
    }

    //////// ������ ////////
    void Swap()
    {
        //���Ⱑ ���ų� �� ���⸦ ��� ���� ��
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;

        //�⺻��
        int weaponIndex = -1;

        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;

        //���� ��ü
        if ((sDown1 || sDown2) && !isJump && !isDodge)
        {
            if(equipWeapon != null)
                equipWeapon.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            //��ü �ִϸ��̼�
            anim.SetTrigger("doSwap");

            isSwap = true;

            //��ü ������
            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interation()
    {
        //�ʵ忡 �����۰� ��ȣ�ۿ�
        if(iDown && nearObject != null && !isJump)
        {
            //Weapos �±׿� �浹 ��
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
