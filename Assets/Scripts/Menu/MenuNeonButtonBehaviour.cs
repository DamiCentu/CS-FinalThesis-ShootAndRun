using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuNeonButtonBehaviour : MonoBehaviour {
    
    public Text[] TextsToChangeMaterial;
    public GameObject[] ParentOfMeshRendererersToChangeMaterial;

    public float maxTiltTime = 0.2f;
    public float minTiltTime = 0.1f;

    public int tiltTimes = 3;

    public float maxTiltTimeForEachElement = 0.35f;
    public float minTiltTimeForEachElement = 0.2f;

    public string id = "";

    public Material SelectedMaterial;
    public Material deactivatedMaterial;
    public Material HoverMaterial;

    public SwitchGOActive[] elementToActiveAndDeactive;

    public bool staySelected = false;

    public UnityEvent OnClickEvent;

    ButtonState _state = ButtonState.Off;
    List<MeshRenderer> _meshRendererToChangeMaterial = new List<MeshRenderer>();

    void Awake()
    {
        if (_meshRendererToChangeMaterial.Count > 0)
            return;
            
        foreach (var go in ParentOfMeshRendererersToChangeMaterial)
        {
            foreach (var mesh in go.GetComponentsInChildren<MeshRenderer>())
            {
                _meshRendererToChangeMaterial.Add(mesh);
            }
        }
    }

    public void HoverState()
    {
        if (_state == ButtonState.Pressed || _state == ButtonState.Selected)
            return;

        SetMaterial(TextsToChangeMaterial, HoverMaterial);
        SetMaterial(_meshRendererToChangeMaterial, HoverMaterial);

        _state = ButtonState.Hover;

        if(elementToActiveAndDeactive.Length > 0)
        {
            foreach (var element in elementToActiveAndDeactive)
            {
                element.SetMaterialActive(true);
            }
        }
    }

    public void ClickAction()
    {
        if (_state == ButtonState.Selected)
            return;

        EventManager.instance.ExecuteEvent(Constants.MENU_BUTTON_CLICKED, new object[] { id });

        _state = ButtonState.Pressed;
        StartCoroutine(ClickedRoutine());
    }

    IEnumerator ClickedRoutine()
    {
        int tiltCount = 0;

        if (staySelected)
        {
            _state = ButtonState.Selected;
        }

        while (tiltCount < tiltTimes)
        {
            bool numberIsPair = tiltCount % 2 == 0;
            SetMaterial(TextsToChangeMaterial, !numberIsPair ? SelectedMaterial : deactivatedMaterial);
            SetMaterial(_meshRendererToChangeMaterial, !numberIsPair ? SelectedMaterial : deactivatedMaterial);
            yield return new WaitForSeconds(Random.Range(minTiltTime, maxTiltTime));

            CheckIfRoutineHasToStop();

            tiltCount++;
        }

        int reverse = Random.Range(0, 2);

        if(reverse == 0)
        {
            SetMaterial(TextsToChangeMaterial, SelectedMaterial);
            yield return new WaitForSeconds(Random.Range(minTiltTimeForEachElement, maxTiltTimeForEachElement));

            CheckIfRoutineHasToStop();

            SetMaterial(_meshRendererToChangeMaterial, SelectedMaterial);
        }
        else
        {
            SetMaterial(_meshRendererToChangeMaterial, SelectedMaterial);
            yield return new WaitForSeconds(Random.Range(minTiltTimeForEachElement, maxTiltTimeForEachElement));

            CheckIfRoutineHasToStop();

            SetMaterial(TextsToChangeMaterial, SelectedMaterial);
        }

        if(_state == ButtonState.Off || !staySelected)
        {
            ForceDisable(); //hacemos esto por si el nieri apreta los botones muy rapido
        }

        if(OnClickEvent != null)
            OnClickEvent.Invoke();
    }

    void CheckIfRoutineHasToStop()
    {
        if (_state == ButtonState.Off)
        {
            StopAllCoroutines();
            ForceDisable(); //hacemos esto por si el nieri apreta los botones muy rapido
        }
    }

    public void SetSelectedState()
    {
        _state = ButtonState.Selected;
        SetMaterial(TextsToChangeMaterial, SelectedMaterial);

        if (_meshRendererToChangeMaterial.Count == 0)
        {
            foreach (var go in ParentOfMeshRendererersToChangeMaterial)
            {
                foreach (var mesh in go.GetComponentsInChildren<MeshRenderer>())
                {
                    _meshRendererToChangeMaterial.Add(mesh);
                }
            }
        }

        SetMaterial(_meshRendererToChangeMaterial, SelectedMaterial);
    }

    public void ForceDisable()
    {
        SetMaterial(TextsToChangeMaterial, deactivatedMaterial);
        SetMaterial(_meshRendererToChangeMaterial, deactivatedMaterial);
        _state = ButtonState.Off;

        if (elementToActiveAndDeactive.Length > 0)
        {
            foreach (var element in elementToActiveAndDeactive)
            {
                element.SetMaterialActive(false);
            }
        }
    }

    public void HoverExitState()
    {
        if (_state == ButtonState.Pressed || _state == ButtonState.Selected)
            return;
        
        SetMaterial(TextsToChangeMaterial, deactivatedMaterial);
        SetMaterial(_meshRendererToChangeMaterial, deactivatedMaterial);
        _state = ButtonState.Off;

        if (elementToActiveAndDeactive.Length > 0)
        {
            foreach (var element in elementToActiveAndDeactive)
            {
                element.SetMaterialActive(false);
            }
        }
    }

    void SetMaterial(Text[] array, Material mat)
    {
        foreach (var text in array)
        {
            text.material = mat;
        }
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

    enum ButtonState
    {
        Pressed,
        Hover,
        Off,
        Selected
    }
}
