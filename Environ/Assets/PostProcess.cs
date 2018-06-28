using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour {

    public Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat)
            Graphics.Blit(source, destination, mat);
        else
            Graphics.Blit(source, destination);
    }


    public void ClearMaterial()
    {
        mat = null;
    }

    public void SetMaterial(Material material)
    {
        mat = material;   
    }
}
