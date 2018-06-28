using Environ.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironUI : MonoBehaviour
{
    public EnvironOutput output;
    public Text fSourceEO;
    public Text lSourceEO;
    public Text targetEO;

    private void Start()
    {
        UpdateData();

    }

    public void UpdateData()
    {
        if (output)
        {
            fSourceEO.text = (output.firstSource) ? output.firstSource.name : "None";
            lSourceEO.text = (output.lastSource) ? output.lastSource.name : "None";
            targetEO.text = (output.target) ? output.target.name : "None";
        }

        else
        {
            fSourceEO.text = "None";
            lSourceEO.text = "None";
            targetEO.text = "None";
        }
    }
}
