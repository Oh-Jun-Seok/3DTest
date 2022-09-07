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

    float hAxis;
    float vAxis;

    bool run;

    public Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        run = Input.GetButton("Run");

        Vector3 forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 right = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;
        moveVec = forward * vAxis + right * hAxis;
        transform.position += moveVec * (run ? runSpeed : nomalSpeed) * Time.deltaTime;

        if (moveVec != Vector3.zero)
            anim.SetFloat("Move", (run ? 1f : 0.5f));
        else
            anim.SetFloat("Move", 0f);

        transform.LookAt(transform.position + moveVec);
    }
    private void LateUpdate()
    {
        if (moveVec == Vector3.forward || moveVec == Vector3.zero)
        {
            Vector3 rotaVec = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotaVec), 1f);
        }
    }
}
