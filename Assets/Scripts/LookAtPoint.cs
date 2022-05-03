using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour
{
  

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.parent.transform.position.x, 1, transform.parent.transform.position.z);
    }
}
