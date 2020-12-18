using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MyPostProcessingManager : MonoBehaviour {

    public Material BerserkMaterial;
    public Material NoShaderMaterial;
    public Material ScatterMaterial;
    Material _material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
            _material = NoShaderMaterial;

        Graphics.Blit(source, destination, NoShaderMaterial);
    }


}
