using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchGOActive : MonoBehaviour {

    Text _text;
    List<MeshRenderer> _allMeshes = new List<MeshRenderer>();
    Image _image;

    public Material deactivatedMaterial;
    public Material activeMaterial;

    private void Awake()
    {
        _text = GetComponent<Text>();

        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {
            _allMeshes.Add(item);
        }

        _image = GetComponent<Image>();

        SetMaterialActive(false);
    }

    public void SetMaterialActive(bool value)
    {
        if(_text != null)
        {
            SetMaterial(_text, value ? activeMaterial : deactivatedMaterial);
        }

        if (_image != null)
        {
            SetMaterial(_image, value ? activeMaterial : deactivatedMaterial);
        }

        if (_allMeshes != null && _allMeshes.Count > 0)
        {
            SetMaterial(_allMeshes, value ? activeMaterial : deactivatedMaterial);
        }
    }

    void SetMaterial(Text text, Material mat)
    {
        text.material = mat;
    }

    void SetMaterial(Image image, Material mat)
    {
        image.material = mat;
    }

    void SetMaterial(List<MeshRenderer> list, Material mat)
    {
        foreach (var mesh in list)
        {
            var allMats = mesh.materials;
            allMats[0] = mat;
            mesh.materials = allMats;
        }
    }
}
