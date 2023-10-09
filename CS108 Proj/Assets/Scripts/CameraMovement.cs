using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private float camDistance = 10f;

    [SerializeField]
    //private float followSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -10f);
       
    }

}
