using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAction : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    public Vector3 moveDir;
    public Vector3 turnDir;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveDirCoroutine());
    }

    private void Update()
    {
        tr.Translate(moveDir * Time.deltaTime);
        tr.Rotate(turnDir * Time.deltaTime);
    }

    IEnumerator MoveDirCoroutine()
    {
        while (true)
        {
            System.Random rand = new System.Random();

            moveDir = Vector3.forward * (float)rand.NextDouble() * moveSpeed;

            turnDir = new Vector3((float)rand.NextDouble() * 90 - 45, (float)rand.NextDouble() * 180 - 90, (float)rand.NextDouble() * 90 - 45);
            turnDir = turnDir * (float)rand.NextDouble() * turnSpeed;

            // Á¤Áö
            if (moveDir.z < 1)
                moveDir = turnDir = Vector3.zero;

            yield return new WaitForSeconds(3f);
        }
    }
}
