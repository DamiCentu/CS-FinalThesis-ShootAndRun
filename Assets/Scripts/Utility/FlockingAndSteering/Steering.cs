using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Steering : MonoBehaviour, ISteerable {
    public Transform target;

    [Header("Forces")]
    public float velocityLimit = 5f;
	public float forceLimit = 10f;

    [Header("PursuitAndArrival")]
    public float arrivalRadius = 5f;
	public float pursuitPeriod = 2f;
    public float leaderDistanceBehind = 2f;
    [Header("Wander")]
    public float wanderDistanceAhead = 5f;
	public float wanderRandomRadius = 5f;
	public float wanderPeriod;
	public float wanderRandomStrength = 5f;

    [Header("Avoidance")]
    public float raycastLength = 3f; 
    public float rhAngle = 20f;
    public LayerMask raycastLayers;
    public float avoidanceMultiplier = 1f;

    Vector3 _velocity;
	private Vector3 _steerForce;

	Vector3 _wander;
	float _nextWander; 

	public Vector3 position { get { return transform.position; } }
	public Vector3 velocity { get { return _velocity; } }
	public float mass { get { return 1f; } }
	public float maxVelocity { get { return velocityLimit; } }

    //public bool wander = false;

	virtual protected void Start () { 
    }

	protected Vector3 Seek(Vector3 targetPosition) {
		var deltaPos = targetPosition - transform.position;
		var desiredVel = deltaPos.normalized * maxVelocity;
		return desiredVel - _velocity;
	}

	protected Vector3 Flee(Vector3 targetPosition) {
		var deltaPos = targetPosition - transform.position;
		var desiredVel = -deltaPos.normalized * maxVelocity;		//La velocidad deseada es la OPUESTA a seek
		return desiredVel - _velocity;
	}

	protected Vector3 Arrival(Vector3 targetPosition, float arrivalRadius) {
		var deltaPos = targetPosition - transform.position;
		var distance = deltaPos.magnitude;
		Vector3 desiredVel;
		if(distance < arrivalRadius)
            desiredVel = deltaPos * maxVelocity / arrivalRadius;
		else
            desiredVel = deltaPos / distance * maxVelocity;

		return desiredVel - _velocity;
	}

	protected Vector3 ArrivalOptimized(Vector3 targetPosition, float arrivalRadius) {
		var deltaPos = targetPosition - transform.position;
		var desiredVel = Utility.TruncateFromSteering(deltaPos * maxVelocity/arrivalRadius, maxVelocity);
		return desiredVel - _velocity;
	}

	protected Vector3 WanderRandomPos() {
		wanderDistanceAhead = 0f;	//HACK: Seteamos a 0 para que los gizmos de wander no muestren cualquier cosa

		if(Time.time > _nextWander) {
			_nextWander = Time.time + wanderPeriod;
			_wander = Utility.RandomDirection() * wanderRandomStrength;
		}
		return Seek(_wander);
	}

	protected Vector3 WanderTwitchy() {
		var desiredVel = Utility.RandomDirection() * maxVelocity;
		return desiredVel - _velocity;
	}

	protected Vector3 WanderWithState(float distanceAhead, float randomRadius, float randomStrength) {
		_wander = Utility.TruncateFromSteering(_wander + Utility.RandomDirection() * randomStrength, randomRadius);
		var aheadPosition = transform.position + _velocity.normalized * distanceAhead + _wander;
		return Seek(aheadPosition);
	}

	protected Vector3 WanderWithStateTimed(float distanceAhead, float randomRadius, float randomStrength) {
		if(Time.time > _nextWander) {
			_nextWander = Time.time + wanderPeriod;
			_wander = Utility.TruncateFromSteering(_wander + Utility.RandomDirection() * randomStrength, randomRadius);
		}
		var aheadPosition = transform.position + _velocity.normalized * distanceAhead + _wander;
		return Seek(aheadPosition);
	}

    //Pursuit: Seek a proyección futura
    protected Vector3 Pursuit(ISteerable who, float periodAhead) {
        var deltaPos = who.position - transform.position;
	    var targetPosition = who.position + who.velocity * deltaPos.magnitude/who.maxVelocity;
		return Seek(targetPosition);
	}

	//Evade: Flee a proyección futura
	protected Vector3 Evade(ISteerable who, float periodAhead) {
		var deltaPos = who.position - transform.position;
		var targetPosition = who.position + who.velocity * deltaPos.magnitude/who.maxVelocity;
		return Flee(targetPosition);
	}

	//ALUM: Falta Containment/Avoidance

    protected Vector3 Avoidance(Vector3 position, Vector3 targetPosition) {
        var dynamicLength = _velocity.magnitude / velocityLimit;
        var forward = position + _velocity.normalized * dynamicLength;
        var avoidanceForce = forward - targetPosition;
        avoidanceForce = avoidanceForce.normalized * avoidanceMultiplier;//forceLimit;
        return avoidanceForce;
    }

    protected Vector3 leaderPointToPersuit(ISteerable leader) {
        var tv = leader.velocity * -1;
        tv = tv.normalized * leaderDistanceBehind;
        var behind = leader.position + tv;
        Debug.DrawLine(leader.position, behind, Color.blue, Time.fixedDeltaTime);
        return (behind);
    }

	//Reinicia las fuerzas
	protected void ResetForces() {
		_steerForce = Vector3.zero;
	}

	// Agrega fuerzas
	protected void AddForce(Vector3 force) {
		_steerForce += force;
	}

	// Aplica la integración: fuerza (aceleración) a velocidad -y- velocidad a posición.
	protected void ApplyForces() {
		//Euler integration
		var dt = Time.fixedDeltaTime;
		_steerForce.y = 0f;
		_steerForce = Utility.TruncateFromSteering(_steerForce, forceLimit);
		_velocity = Utility.TruncateFromSteering(_velocity + _steerForce * dt, maxVelocity);
        //agregado para que no se muevan en el eje Y
        _velocity.y = 0f;
		transform.position += _velocity * dt * SectionManager.instance.EnemiesMultiplicator;
		transform.forward = Vector3.Slerp(transform.forward, _velocity, 0.1f);
	}

    public void resetVelocity(bool hitWall) {
        if (target != null) {
            if (hitWall) {
                _velocity = (target.transform.position - transform.position).normalized * (maxVelocity - (maxVelocity / 4));
            }
            else {
                _velocity = (target.transform.position - transform.position).normalized;
            }
        } 
    }

	virtual protected void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, transform.position+_velocity);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position+_velocity, transform.position+_velocity+_steerForce);
	}
}
