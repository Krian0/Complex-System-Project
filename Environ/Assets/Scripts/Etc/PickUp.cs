using UnityEngine;

public class PickUp : MonoBehaviour
{
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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (!holding)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
                if (hit.distance <= 15 && hit.collider.gameObject.tag == "Sphere")
                    lastSeen = hit.collider.gameObject;

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0)) && lastSeen)
            {
                lastSeen.transform.position = holdPos.position;
                Rigidbody rb = lastSeen.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                holding = true;
                anim.SetTrigger("Pickup");
            }

            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
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