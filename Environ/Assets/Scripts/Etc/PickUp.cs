using Environ.Main;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public SeeScript seeing;
    public Transform holdPos;
    private GameObject lastSeen;
    private bool holding;

    Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update ()
    {
        if (!seeing)
            return;

        RaycastHit hit = seeing.GetRayHit();
        if (hit.collider)
            if (hit.distance <= 15 && hit.collider.gameObject.tag == "Sphere")
                lastSeen = hit.collider.gameObject;


        if (!holding)
        {
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) && lastSeen)
            {
                lastSeen.transform.position = holdPos.position;
                Rigidbody rb = lastSeen.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                holding = true;
                anim.SetTrigger("Pickup");
            }
        }

        else
        {
            lastSeen.transform.position = holdPos.position;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                holding = false;
                Rigidbody rb = lastSeen.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.AddForce(transform.forward * 200);
                lastSeen = null;
                anim.SetTrigger("Throw");
            }
        }
    }
}