using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyInputManager : MonoBehaviour
{
    public enum Control
    {
        Keyboard,
        Joystick,
        Any
    }
    public static MyInputManager instance = null;
    public bool useJoystick = true;
    public float maxAngleRotation=45;

    public KeyCode dashJoystick;
    public KeyCode dashKeyBoard;
    public KeyCode ultKeyBoard;
    public KeyCode ultJoystick;
    public LayerMask mouseLayerDetection;
    public Texture2D cursorTexture;
    public Vector2 hotSpot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            useJoystick = !useJoystick;
            if (useJoystick)
            {
                EventManager.instance.ExecuteEvent("usingJoystick");
            }
            else
            {
                EventManager.instance.ExecuteEvent("usingKeyboard");
            }
        }
    }


    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    public void GetPlayerRotation(Transform t, Control control)
    {
        if (control== Control.Joystick )
        {
            float x = Input.GetAxis("RightStickX");
            float y = Input.GetAxis("RightStickY");
            Vector3 direction = new Vector3(x, 0, -y);
          

            if (direction != Vector3.zero) {
                float rotationAngle= Vector3.SignedAngle(t.forward, direction, Vector3.up );

                if (rotationAngle > maxAngleRotation ) {
                    rotationAngle = maxAngleRotation;
                }
                if (rotationAngle < -maxAngleRotation)
                {
                    rotationAngle = -maxAngleRotation;
                }
                t.Rotate(Vector3.up, rotationAngle);
            }
        }
        else if (control == Control.Keyboard)
        {

            Vector3 asd = Input.mousePosition;
            asd.z = 100; // no puede ser 0 xq sino devuelve simepre el mismo valor y tiene que ser grande para que funcione
            Ray ray = Camera.main.ScreenPointToRay(asd);

            RaycastHit info;
            bool match = Physics.Raycast(ray, out info, 1000, mouseLayerDetection);

            if (match)
            {
                Vector3 toRotation = new Vector3(info.point.x, t.position.y, info.point.z);
                t.rotation = Quaternion.LookRotation(toRotation - t.transform.position, Vector3.up);

            }

        }
    }

    internal bool Ult(Control control )
    {
        if (Input.GetKeyDown(ultKeyBoard) &&(  control == Control.Keyboard))
        {
            return true;
        }

        else if (Input.GetKeyDown(ultJoystick) && ( control == Control.Joystick)) {
            return true;
        }
        return false;
    }

    internal bool ShootSpecial(Control control )
    {


        if (Input.GetMouseButton(1) && control == Control.Keyboard)
        {
            return true;
        }

        else if (Input.GetAxis("LeftTrigger") != 0 && control == Control.Joystick) {
            return true;
        }
        return false;
    }

    public bool Dash(Control control )
    {
        if (Input.GetKeyDown(dashKeyBoard) && control == Control.Keyboard)
        {
            return true;
        }
        if (Input.GetKeyDown(dashJoystick) && control == Control.Joystick) {
            return true;
        }
        return false;
    }

    internal Vector2 Move(Control control)
    {
        Vector2 vel= new Vector2();

        if (control == Control.Keyboard) {
            vel.y = Input.GetAxis("VerticalMouse");
            vel.x = Input.GetAxis("HorizontalMouse");

        }
        else if (control == Control.Joystick) {
            vel.y = Input.GetAxis("VerticalJoystick");
            vel.x = Input.GetAxis("HorizontalJoystick");
        }
        return vel;

    }
    public bool Shoot(Control control)
    {

        if (Input.GetMouseButton(0)&& control==Control.Keyboard)
        {
            return true;
        }

        float x = Input.GetAxis("RightStickX"); // si esta apuntando
        float y = Input.GetAxis("RightStickY"); // si esta apuntando
        if (control == Control.Joystick &&( x!=0 || y!=0) )
            {
            return true;
        }
        return false;
    }

    
    private float GetAxisFunction(string keyName, Func<string, float> fun)
    {
        return fun(keyName);
    }

    private bool GetButtonFunction(string keyName, Func<string, bool> fun)
    {
        return fun(keyName);
    }

    void OnDrawGizmos()
    {

    
        Vector3 asd = Input.mousePosition;
        asd.z = 100; // no puede ser 0 xq sino devuelve simepre el mismo valor y tiene que ser grande para que funcione
        Ray ray = Camera.main.ScreenPointToRay(asd);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(ray.origin, ray.origin + ray.direction*50000);


        RaycastHit info;
        bool match = Physics.Raycast(ray, out info, 1000, mouseLayerDetection);

        if (match)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(info.point, 0.55f);

        }

        return;


        Vector3 positionInWorld = Camera.main.ScreenToWorldPoint(asd);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(positionInWorld, 10);
        print(positionInWorld);
    }

}

