using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.right * 30 * Time.deltaTime);
    }
}
