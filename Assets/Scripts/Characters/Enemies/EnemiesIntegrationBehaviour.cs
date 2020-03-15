using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;

public class EnemiesIntegrationBehaviour : MonoBehaviour , IPauseable {
    public float reintegrateMaxValue = 20f;
    public float lerpMaxValue = 5f;

    public GameObject GOToDesactivate;
    public Image hudImage;
    public float speedToRotateImage = 60f;
    public float tiltingTime = 0.05f;
    public ParticleSystem integrationParticles;
    //public Vector3 imageOffset;

    SkinnedMeshRenderer[] _skinnedRends;
    MeshRenderer[] _meshRends;

    Collider _col;

    Light[] _lights;

    float _timeToReintegrate = 0f;
    float _timer = 0f;
    float _tiltingTimer = 0; 

    bool _reintergrate = false;
    bool _paused;

    public void OnPauseChange(bool v) {
        _paused = v; 
    }

    void Update () {
        if (_paused)
            return;

        if (!_reintergrate || _timeToReintegrate == 0)
            return;

        ImageTilting();

        ImageRotation();

        ImageScaling();


        MeshReintegration(); 
	}

    public bool LoadingNotComplete { get { return _reintergrate; } }

    public void SetReintergration(float timeToReintegrate) {

        EnableLightsIfPosible(false);

        if (_skinnedRends == null)
            _skinnedRends = GetComponentsInChildren<SkinnedMeshRenderer>();
       
        if(_skinnedRends == null)
            _meshRends = GetComponentsInChildren<MeshRenderer>();

        _timer = 0f;
        _tiltingTimer = 0f; 

        if (timeToReintegrate == 0) { 
            SetValue(_meshRends, reintegrateMaxValue);
            SetValue(_skinnedRends, reintegrateMaxValue);
            ActivateOrDeactivateIntegration(false);
        }
        else { 
            _timeToReintegrate = timeToReintegrate;
            if (_col == null)
                _col = GetComponent<Collider>();
            _col.enabled = false;
            _reintergrate = true;
            //RaycastHit rh;
            //Physics.Raycast(transform.position, Vector3.down, out rh,10f, Utility.LayerNumberToMask(15));//15 = floor 
            if(hudImage != null) {
                hudImage.enabled = true;
                hudImage.transform.position = /*rh.point +*/ new Vector3(transform.position.x, EnemiesManager.instance.IntegrationImageYOffset, transform.position.z); 
            }
            ActivateOrDeactivateIntegration(true);
        } 
    }

    void MeshReintegration() {
        var v = Mathf.Lerp(0f, lerpMaxValue, _timer / _timeToReintegrate);

        SetValue(_meshRends, v);
        SetValue(_skinnedRends, v);

        _timer += Time.deltaTime;
        if (_timer > _timeToReintegrate) {
            _reintergrate = false;
            SetValue(_meshRends, reintegrateMaxValue);
            SetValue(_skinnedRends, reintegrateMaxValue); 
            _col.enabled = true;

            EnableLightsIfPosible(true);

            StartCoroutine(FinishedReintegrationRoutine());
        }
    }

    void ImageTilting() {
        if (hudImage == null)
            return;

        _tiltingTimer += Time.deltaTime;
        if(_tiltingTimer > tiltingTime) {
            if (hudImage.enabled) {
                hudImage.enabled = false;
            }
            else hudImage.enabled = true;
            _tiltingTimer = 0f;
        }
    }

    void ImageRotation() {
        if (hudImage == null)
            return;
        hudImage.transform.Rotate(Vector3.forward, speedToRotateImage * Time.deltaTime);
    }

    void ImageScaling() {
        var s = Mathf.Lerp(3f, 1f, _timer / _timeToReintegrate); 
        if (hudImage == null)
            return;
        hudImage.transform.localScale = new Vector3(s, s, 1f);
    }

    void OnDisable() { 
        SetValue(_meshRends, 0f);
        SetValue(_skinnedRends, 0f);
    }

    void SetValue(Renderer [] rend, float value) {
        if (rend == null)
            return;

        foreach (var r in rend) {
            r.material.SetFloat("_reintegrateValue", value);
        }
    }

    IEnumerator FinishedReintegrationRoutine() {
        if (integrationParticles != null) { 
            integrationParticles.Stop();
            hudImage.enabled = false;
            yield return new WaitForSeconds(1f);
            while (_paused)
                yield return null;
            ActivateOrDeactivateIntegration(false);
        }

        yield return null;
    }

    void EnableLightsIfPosible(bool value)
    {
        if(_lights == null)
            _lights = GetComponentsInChildren<Light>();

        if (_lights.Length == 0)
            return;

        foreach (var lightInChild in _lights)
        {
            lightInChild.gameObject.SetActive(value);
        }
    }

    void ActivateOrDeactivateIntegration(bool b) { 
        if (GOToDesactivate != null)
            GOToDesactivate.SetActive(b);
    }
}
