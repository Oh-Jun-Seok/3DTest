using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float nomalSpeed;
    public float runSpeed;
    public bool visible;
    public Animator anim;
    public GameObject cam;
    public GameObject mesh;
    public GameObject touchWall;
    public GameManager manager;
    public Rigidbody rigi;

    float hAxis;
    float vAxis;
    float cooldown;

    bool run;
    bool num1;
    bool num2;
    bool num3;

    public Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
        visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        ColorChange();
    }
    private void LateUpdate()
    {
        if (moveVec == Vector3.forward || moveVec == Vector3.zero)
        {
            Vector3 rotaVec = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotaVec), 1f);
        }
    }
    private void PlayerMove()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        run = Input.GetButton("Run");

        Vector3 forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 right = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        moveVec = forward * vAxis + right * hAxis;
        transform.position += moveVec * (run ? runSpeed : nomalSpeed) * Time.deltaTime;

        if (moveVec != Vector3.zero)
        {
            anim.SetFloat("Move", (run ? 1f : 0.5f));
            visible = true;
        }
        else
        {
            anim.SetFloat("Move", 0f);
            if (touchWall != null)
            {
                if (touchWall.GetComponent<MeshRenderer>().material.color == mesh.GetComponent<SkinnedMeshRenderer>().material.color)
                    visible = false;
                else
                    visible = true;
            }
        }

        transform.LookAt(transform.position + moveVec);
    }
    private void ColorChange()
    {
        num1 = Input.GetButtonDown("Num1");
        num2 = Input.GetButtonDown("Num2");
        num3 = Input.GetButtonDown("Num3");
        if (cooldown <= 0 && (num1 || num2 || num3))
        {
            if (num1)
                mesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
            if (num2)
                mesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.green;
            if (num3)
                mesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.blue;
            cooldown = 3f;
        }
        else
            cooldown -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Wall")
            touchWall = collision.gameObject;
        if (collision.transform.tag == "Enemy")
            manager.PlayerDie();
    }
    private void OnCollisionExit(Collision collision)
    {
        touchWall = null;
    }
}
