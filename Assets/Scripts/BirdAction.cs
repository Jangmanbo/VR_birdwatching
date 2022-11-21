using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAction : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private float moveSpeed = 5;

    private void Awake()
    {
        tr = gameObject.GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity=new Vector3(1, 0, 1);
    }
}
