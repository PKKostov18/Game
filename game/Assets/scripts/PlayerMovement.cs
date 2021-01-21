using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    public Rigidbody2D RB;

    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Vector2 mouseInput;

    public float sens = 1f;

    public Camera viewCam;

    public GameObject bulletImpact; 
    public int ammo;

    public Animator gunAnim;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //script for the movement
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x;
        Vector3 moveVertical = transform.right * moveInput.y;

        RB.velocity = (moveHorizontal + moveVertical) * moveSpeed;
        //


        //script for the view
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * sens;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x);

        viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));
        //


        //script for shooting
        if (Input.GetMouseButtonDown(0))
        {
            if  (ammo > 0)
            {
            Ray ray = viewCam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
	            Instantiate(bulletImpact, hit.point, transform.rotation);

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                }
            }
            else
            {
                Debug.Log("I am looking at nothing!");
            }
            ammo--; 
            gunAnim.SetTrigger("Shoot");
         }
        }
    }
}
