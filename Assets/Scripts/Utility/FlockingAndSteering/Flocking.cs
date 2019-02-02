using UnityEngine;
using System.Collections;
using System;

public class Flocking : Steering , IPauseable {


    [Header("Flocking")]
    int f = 0;
    private Transform oldTarget;
	public float neighborhoodRadius = 10f;
	public float separationRadius = 2f;
	public float alignmentMult = 1f;
	public float cohesionMult = 1f;
	public float separationMult = 1f; 

	public bool drawFlockingGizmos = false;

    public bool flockingEnabled = true;

	Vector3 _alignment, _cohesion, _separation;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    public void OnFixedUpdate () {
        if (_paused)
            return;

		ResetForces();

        if (flockingEnabled) { 

            var hits = Physics.OverlapSphere(transform.position, neighborhoodRadius);

		    var sumV = Vector3.zero;			//Suma de velocidades
		    var sumP = Vector3.zero;			//Suma de posiciones
		    var sumSepForce = Vector3.zero;		//Suma de fuerzas de separación (deltas / distancia)

		    int nHits = 0;
		    foreach(var hit in hits) {
			    if(hit.gameObject == gameObject)
				    continue;

                var other = hit.GetComponent<Steering>();
			    if(other == null)
				    continue;

                var deltaP = transform.position - other.position;	//from other to self
			    var distSqr = deltaP.sqrMagnitude;
			    if(distSqr > 0f && distSqr < separationRadius * separationRadius) {
				    sumSepForce += deltaP / distSqr;
			    }

			    nHits++;
			    sumV += other.velocity;
			    sumP += other.position;
		    }

		    if(nHits > 0) {
			    _alignment = sumV.normalized * maxVelocity - velocity;		//Promedio de "direcciones"
			    _cohesion = Seek(sumP / nHits);								//Seguir promedio de posiciones
			    _separation = sumSepForce == Vector3.zero ? Vector3.zero : sumSepForce.normalized * maxVelocity - velocity;	//Prmoedio de fuerzas

			    AddForce(_alignment * alignmentMult);
			    AddForce(_cohesion * cohesionMult);
			    AddForce(_separation * separationMult);
		    }
        }

        //obtacleAvoidance
        var rLeft =  Quaternion.AngleAxis(rhAngle, transform.up) * velocity.normalized ;
        var rRight = Quaternion.AngleAxis(-rhAngle, transform.up) * velocity.normalized;

        Debug.DrawLine(transform.position, transform.position + rLeft.normalized * raycastLength, Color.magenta, Time.fixedDeltaTime);
        Debug.DrawLine(transform.position, transform.position + rRight.normalized * raycastLength, Color.magenta, Time.fixedDeltaTime);

        RaycastHit rh;
        if(Physics.Raycast(transform.position,velocity, out rh, raycastLength, raycastLayers) ||
           Physics.Raycast(transform.position, rLeft, out rh, raycastLength, raycastLayers) ||
           Physics.Raycast(transform.position, rRight, out rh, raycastLength, raycastLayers)) {
            AddForce(Avoidance(transform.position, rh.transform.position)); //* avoidanceMultiplier);
        }

        //Pavear en grupo
        //if (gameObject.layer == (int)Utility.layers.Leader) AddForce(ArrivalOptimized(Target, arrivalRadius));
        //else AddForce(ArrivalOptimized(leaderPointToPersuit(Leader), arrivalRadius));
         
          
        if (target != null) //&& !wander)
            AddForce(Seek(target.position));
            //else
                //AddForce(WanderRandomPos()); 

        ApplyForces();
	}

    internal void SetTarget(GameObject gameObject)
    {

        if (target != null) {
            oldTarget = target;
        }
        target = gameObject.transform;
        print("player pos" + oldTarget.position);
        print("target pos" + target.position);
    }

    internal void RestoreTarget()
    {
        ResetForces();
        target = oldTarget;
    }

    override protected void OnDrawGizmos() {
		base.OnDrawGizmos();

		if(!drawFlockingGizmos)
			return;

		//if(++f % 50 == 0) {
			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(transform.position, neighborhoodRadius);
		//}

		Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position, transform.position+_alignment*alignmentMult);
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(transform.position, transform.position+_cohesion*cohesionMult);
		Gizmos.color = Color.magenta;
		Gizmos.DrawLine(transform.position, transform.position+_separation*separationMult);
		Gizmos.DrawWireSphere(transform.position, separationRadius);
	}
}
