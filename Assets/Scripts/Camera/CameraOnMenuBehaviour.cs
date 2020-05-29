using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnMenuBehaviour : MonoBehaviour {

    public Waypoint MainMenuWaypoint;
    public float movementSpeed = 1f;
    public float rotationSpeed = 2f;

    Waypoint _currentWP;

    Vector3 _lastWaypointPosition;
    Vector3 _bezierStart;
    Vector3 _bezierEnd;
    float _bezierT;
    float _bezierSpeed;

    Vector3 _handle1;
    Vector3 _handle2;
    
    private float _startTime;
    private float _journeyLength;
    private bool _traveling;

    bool _firstTimeTransitionDone = false;

    Stack<Waypoint> _alreadyTraveledWaypoints = new Stack<Waypoint>();

    void Start () {
        transform.position = MainMenuWaypoint.transform.position;
        transform.rotation = MainMenuWaypoint.transform.rotation;
        _lastWaypointPosition = transform.position;

        EventManager.instance.SubscribeEvent(Constants.MENU_CAMERA_NAVIGATE, OnNavigateTo);
    }

    private void OnNavigateTo(object[] parameterContainer)
    {
        var waypoint = (Waypoint)parameterContainer[0];
        if (waypoint == null || _traveling)
            return;

        _traveling = true;

        _currentWP = waypoint;

        if(!_firstTimeTransitionDone)
        {
            _firstTimeTransitionDone = true;
            UpdateBezierParams();
        }

        _startTime = Time.time;

        _journeyLength = GetJourneyLength();
    }

    void LateUpdate () {
        if (_currentWP == null)
            return;

        float distCovered = (Time.time - _startTime) * movementSpeed * rotationSpeed;
        float fractionOfJourney = distCovered / _journeyLength;

        var movement = _currentWP.IsNear(transform.position) ?
            BezierMovement() : RegularMovement();
        
        transform.position += movement;

        transform.rotation = Quaternion.Slerp(transform.rotation, LastWaypointInPath().transform.rotation, fractionOfJourney);

        if (_bezierT >= 1.0f)
        {
            _alreadyTraveledWaypoints.Push(_currentWP);

            if(_currentWP.next != null)
            {
                _lastWaypointPosition = _currentWP.transform.position;
                _currentWP = _currentWP.next;
                UpdateBezierParams();
            }
            else
            {
                _currentWP = null;
                _traveling = false;
            }
        }
    }

    float GetJourneyLength()
    {
        float lenght = 0;

        Waypoint last = _currentWP;

        while (last.next != null)
        {
            lenght += Vector3.Distance(last.transform.position, last.next.transform.position);
            last = last.next;
        }

        return lenght;

        
    }

    Waypoint LastWaypointInPath ()
    {
        Waypoint last = _currentWP;

        while (last.next != null)
            last = last.next;

        return last;
    }

    int AmountOfWaypointInPath()
    {
        Waypoint last = _currentWP;
        int amount = 1;

        while (last.next != null)
        {
            last = last.next;
            amount++;
        }

        return amount;
    }

    Vector3 BezierMovement()
    {
        _bezierT += Mathf.Min(1.0f, _bezierSpeed * Time.deltaTime);

        _handle1 = Vector3.Lerp(_bezierStart, _currentWP.transform.position, _bezierT);
        _handle2 = Vector3.Lerp(_currentWP.transform.position, _bezierEnd, _bezierT);
        var bezierPoint = Vector3.Lerp(_handle1, _handle2, _bezierT);

        return bezierPoint - transform.position;
    }

    Vector3 RegularMovement()
    {
        var toWaypoint = _currentWP.transform.position - transform.position;
        var direction = toWaypoint.normalized;
        var movementDelta = direction * movementSpeed * Time.deltaTime;

        return movementDelta.sqrMagnitude > toWaypoint.sqrMagnitude ? toWaypoint : movementDelta;
    }

    void UpdateBezierParams()
    {
        var currWaypointPosition = _currentWP.transform.position;

        _bezierT = 0;
        _bezierStart = currWaypointPosition + (_lastWaypointPosition - currWaypointPosition).normalized * _currentWP.radius;

        if (_currentWP.next == null)
        {
            _bezierEnd = currWaypointPosition;
            _bezierSpeed = movementSpeed / _currentWP.radius;
        }
        else
        {
            var nextWaypointPosition = _currentWP.next.transform.position;
            _bezierEnd = currWaypointPosition + (nextWaypointPosition - currWaypointPosition).normalized * _currentWP.radius;
            _bezierSpeed = movementSpeed / _currentWP.radius * 0.5f;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_lastWaypointPosition, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_bezierStart, 0.5f);
        Gizmos.DrawWireSphere(_bezierEnd, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_handle1, 0.25f);
        Gizmos.DrawWireSphere(_handle2, 0.25f);
    }
}
