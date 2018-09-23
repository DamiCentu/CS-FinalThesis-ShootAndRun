using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public  class Utility {

    //vectores de movimiento aquí 
    public static int[] xMoveVector8 = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
    public static int[] yMoveVector8 = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };

    //Trunca un vector a un largo máximo
    public static Vector3 Truncate(Vector3 vec, float maxMag) {
		var mag = vec.magnitude;
		if(mag < float.Epsilon) return vec;
		else return vec * Mathf.Min(1f, maxMag/mag);
	}

    public static Vector3 SetYInVector3(Vector3 vec, float y) {
        return new Vector3(vec.x, y, vec.z);
    } 

    //Baraja de elementos de un array dinámico
    public static void KnuthShuffle<T>(List<T> array) {
		for(int i = 0; i<array.Count-1; i++) {
			var j = UnityEngine.Random.Range(i, array.Count);
			if(i != j) {
				var temp = array[j];
				array[j] = array[i];
				array[i] = temp;
			}
		}
	}

    public static Vector3 TruncateFromSteering(Vector3 vec, float maxMagnitude) {
        var magnitude = vec.magnitude;
        return vec * Mathf.Min(1f, maxMagnitude / magnitude);
    }

    public static Vector3 RandomDirection() {
		var theta = UnityEngine.Random.Range(0f, 2f*Mathf.PI);
		var phi = UnityEngine.Random.Range(0f, Mathf.PI);
		var u = Mathf.Cos(phi);
		return new Vector3(Mathf.Sqrt(1-u*u)*Mathf.Cos(theta), Mathf.Sqrt(1-u*u)*Mathf.Sin(theta), u);
	}

	//Dibuja una flecha gizmo con dirección (en vez de solo una linea)
	public static void GizmoArrow(Vector3 from, Vector3 to, float scale = 0.25f, float gap = 0.15f) {
		var dir = to - from;
		to -= dir.normalized * gap;
		var offset = Vector3.Cross(dir.normalized, Vector3.up) * scale;
		var arrowLeft = to - dir.normalized * scale + offset;
		var arrowRight = to - dir.normalized * scale - offset;

		Gizmos.DrawLine(from, to);
		Gizmos.DrawLine(to, arrowLeft);
		Gizmos.DrawLine(to, arrowRight);
	}

    public static bool InRange(Vector3 position,Vector3 target, float distance) {
        return Vector3.Distance(position, target) < distance;
    }

    public static LayerMask LayerNumberToMask(int layerNum) {
       return 1 << layerNum;
    }

    public static void RemoveFromListGeneric<T> (List<T> list , T obj) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i].Equals(obj)) { 
                list.Remove(obj);
                return;
            }
        }
    }

    public static int CalcPercentage(float actual, float total) {
        return (int) (actual / total * 100);
    }


    public static bool InRangeSquared(Vector3 destiny, Vector3 origen, float distance) { 
        return (destiny - origen).sqrMagnitude < distance * distance;
    }

    //public static bool NotInSight(Vector3 pos,Vector3 targerPos, LayerMask mask) {
    //    var dir = targerPos - pos;
    //    Debug.DrawLine(pos, pos += pos.normalized * dir.magnitude, Color.yellow, Time.deltaTime);
    //    return Physics.Raycast(pos, dir, dir.magnitude, mask);
    //}

    public static void DestroyAllInAndClearList<T>(List<T> list) where T : MonoBehaviour {
        foreach (var n in list) { 
            UnityEngine.Object.Destroy(n.gameObject);
        }
        list.Clear();
    }

    public static void DestroyAllInAndClearList(List<GameObject> list) {
        foreach (var n in list) { 
            UnityEngine.Object.Destroy(n);
        }
        list.Clear();
    }

    public static void DeactivateList <T> (List<T> list) where T : MonoBehaviour {
        foreach (var n in list) {
            if (n is NormalEnemyBehaviour)
                EnemiesManager.instance.ReturnNormalEnemyToPool(n as NormalEnemyBehaviour);
            else if (n is ChargerEnemyBehaviour)
                EnemiesManager.instance.ReturnChargerEnemyToPool(n as ChargerEnemyBehaviour);
            else if (n is EnemyTurretBehaviour) {
                //(n as EnemyTurretBehaviour).DeactivateEverithing();
                EnemiesManager.instance.ReturnTurretEnemyToPool((n as EnemyTurretBehaviour).DeactivateEverithing());
            }
            else if (n is PowerUpChaserEnemy)
                EnemiesManager.instance.ReturnChaserEnemyToPool(n as PowerUpChaserEnemy);

            n.gameObject.SetActive(false);
        }
    }

    public static Vector3 RandomPointInBox(Vector3 center, Vector3 size, float constantY) {
        var r = center + new Vector3((UnityEngine.Random.value - 0.5f) * size.x, 0f, (UnityEngine.Random.value - 0.5f) * size.z);
        r.y = constantY;
        return r;
    }

    // devuelve una posicion dentro de un radio RANDOM, tirando raycast detectando algo de la mascara , vaes a buscar siempre la posicion mas alejada de los objetos a los que detecto
    public static Vector3 RandomVector3InRadiusCountingBoundaries(Vector3 actualPos, float maxDistanceRadius, LayerMask maskToDetect) {

        var pos = actualPos;
        float randomX = UnityEngine.Random.Range(-1f, 1f);
        float randomZ = UnityEngine.Random.Range(-1f, 1f);
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ);
        float maxRange = 0f;

        var crossRamdomDirection = new Vector3(randomDirection.z,0,randomDirection.x);

        RaycastHit rh;

        var dir = new Vector3();

        if(Physics.Raycast(pos, randomDirection, out rh, maxDistanceRadius, maskToDetect)) {
            maxRange = rh.distance;
            dir = randomDirection;
            if(Physics.Raycast(pos, -randomDirection, out rh, maxDistanceRadius, maskToDetect)) {
                if(maxRange < rh.distance) {
                    dir = -randomDirection;
                    maxRange = rh.distance;
                }
                if(Physics.Raycast(pos, crossRamdomDirection, out rh, maxDistanceRadius, maskToDetect)) {
                    if(maxRange < rh.distance) {
                        dir = crossRamdomDirection;
                        maxRange = rh.distance;
                    }
                    if(Physics.Raycast(pos, -crossRamdomDirection, out rh, maxDistanceRadius, maskToDetect)) {
                        if(maxRange < rh.distance) {
                            dir = -crossRamdomDirection;
                            maxRange = rh.distance;
                        }

                        pos += dir * maxRange;
                    }
                    else pos += -crossRamdomDirection * maxDistanceRadius;
                }
                else pos += crossRamdomDirection * maxDistanceRadius;
            }
            else  pos += -randomDirection * maxDistanceRadius;
        }
        else pos += randomDirection * maxDistanceRadius;

        return pos;
    }

    internal static void RandomVector3InRadiusCountingBoundaries(Vector3 position, int v, object misileEnemyLayerMask)
    {
        throw new NotImplementedException();
    }
}
