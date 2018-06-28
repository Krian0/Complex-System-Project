using Environ.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironUI : MonoBehaviour
{
    public EnvironObject eObject;
    public EnvironOutput eOut;

    public Text currentHealth;
    public Text maxHealth;

    public Text outputName;
    public Text fSourceEO;
    public Text lSourceEO;
    public Text targetEO;

    private static string nullString = "N/A";

    private void Start()
    {
        UpdateData();

    }

    public void UpdateData()
    {
        if (eObject)
        {
            currentHealth.text = eObject.hitPoints.ToString();
            maxHealth.text = eObject.hitPointLimit.ToString();

            if (eOut)
            {
                outputName.text = eOut.name;
                fSourceEO.text = (eOut.firstSource) ? eOut.firstSource.name : nullString;
                lSourceEO.text = (eOut.lastSource) ? eOut.lastSource.name : nullString;
                targetEO.text = (eOut.target) ? eOut.target.name : nullString;
            }

            else
            {
                outputName.text = nullString;
                fSourceEO.text = nullString;
                lSourceEO.text = nullString;
                targetEO.text = nullString;
            }
        }

        else
        {
            currentHealth.text = nullString;
            maxHealth.text = nullString;
            outputName.text = nullString;
            fSourceEO.text = nullString;
            lSourceEO.text = nullString;
            targetEO.text = nullString;
        }
    }
}