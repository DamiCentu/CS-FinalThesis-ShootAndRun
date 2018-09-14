using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingWallsBehaviour : MonoBehaviour {

    public float speed = 1f;
    public Vector3 openPos;
    public Vector3 closePos;

    bool _isClosing = false;
    float _timer = 0f;

    void Update () {

        if (_isClosing) 
            transform.position = Vector3.Lerp(openPos, closePos, _timer * speed);
        else  
            transform.position = Vector3.Lerp(closePos, openPos, _timer * speed);

        _timer += Time.deltaTime;

        //if (Input.GetKeyDown(KeyCode.C))
        //    OnClosingWall();

        //if (Input.GetKeyDown(KeyCode.O))
        //    OnOpeningWall();

        //Debug.Log(openPos + "" + closePos + ""+ _timer + "" + _timer * speed);
    }

    public void OpenWall(bool open) {
        if (open)
            _isClosing = false;
        else
            _isClosing = true;

        _timer = 0f;
    }

    //public void OnClosingWall() {
    //    _isClosing = true;
    //    _timer = 0f; 
    //}

    //public void OnOpeningWall() {
    //    _isClosing = false;
    //    _timer = 0f; 
    //}
}
