using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAction : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private Animator animator;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private int landingInterval;   // 착지 동작 소요 프레임

    [Range(0f, 1f)]
    [SerializeField] private float flyProbability;

    public Vector3 moveDir;
    public Vector3 turnDir;

    private bool isMove, isGrounded;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    private void Update()
    {

        tr.Translate(moveDir * Time.deltaTime);
        tr.Rotate(turnDir * Time.deltaTime);

        Vector3 rotation = tr.rotation.eulerAngles;
        // 새가 너무 기울어지지 않도록
        if (Mathf.Abs(rotation.x - 180) < 160)  // 20<x<340
        {
            turnDir.x = 0;
            StartCoroutine(Balance());
        }
        if (Mathf.Abs(rotation.z - 180) < 160)  // 20<z<340
        {
            turnDir.z = 0;
            StartCoroutine(Balance());
        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            System.Random rand = new System.Random();

            moveDir = Vector3.forward * (float)rand.NextDouble() * moveSpeed;

            if (moveDir.z < 1)  // 일정 속도 이하인 경우
            {
                if (isGrounded) // 지면에 위치해있다면 정지
                    Stop();
                else    // 나는 중이면 속도 높임
                    moveDir += Vector3.forward;
            }
            else
            {
                isMove = true;

                turnDir = new Vector3(0, (float)rand.NextDouble() * 90 - 45, 0);

                bool startFly = rand.NextDouble() < flyProbability ? true : false;
                if (isGrounded && startFly) // 지면에 있으면서 날기 시작
                {
                    rb.useGravity = false;
                    turnDir += Vector3.right * (float)rand.NextDouble() * (-20);    // 상승
                }
                else if (!isGrounded)   // 나는 중
                    turnDir += Vector3.right * (float)rand.NextDouble() * 20;    // 상승 or 하강

                turnDir = turnDir * (float)rand.NextDouble() * turnSpeed;
            }

            Animate();
            yield return new WaitForSeconds(3f);
        }
    }

    // 지면에서 정지
    private void Stop()
    {
        isMove = false;
        isGrounded = rb.useGravity = true;
        moveDir = turnDir = Vector3.zero;
    }

    // 평형 유지 (뒤집히지 않도록)
    private IEnumerator Balance()
    {
        float interval = 1f / landingInterval;
        for (int i = 0; i < landingInterval; i++)
        {
            tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.Euler(new Vector3(0, tr.rotation.eulerAngles.y)), interval * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    // 애니메이션 전환
    private void Animate()
    {
        animator.SetBool("isMove", isMove);
        animator.SetBool("isGrounded", isGrounded);
    }

    // 지면에 착지
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Terrain"))
        {
            Stop();
            StartCoroutine(Balance());
            Animate();
        }
    }

    // 날기 시작
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            isGrounded = false;
            Animate();
        }
    }
}
