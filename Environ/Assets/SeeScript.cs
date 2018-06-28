using Environ.Main;
using UnityEngine;

public class SeeScript : MonoBehaviour
{
    private RaycastHit rayHit;
    public EnvironUI EUI;


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Physics.Raycast(ray, out rayHit);

        if (Input.GetKeyDown(KeyCode.P) && EUI)
        {
            EUI.eObject = rayHit.collider.gameObject.GetComponent<EnvironObject>();

            if (rayHit.distance <= 15 && EUI.eObject)
            {
                if (EUI.eObject.effects.inputList.Count > 0)
                    EUI.eOut = EUI.eObject.effects.inputList[0];
                else
                    EUI.eOut = null;
            }
        }

        EUI.UpdateData();
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
    }

    public RaycastHit GetRayHit()
    {
        return rayHit;
    }
}
