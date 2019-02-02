using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MineBullet : MonoBehaviour {

    public float radius=20;
    public LayerMask hittableLayer;
    public float timeToBoom=2;
    Timer timer;
    public int damage = 3;
    private int maxNumber=5;
    private float waitTime=2;
    private List<AbstractEnemy> enemiesToAffects;
    private float minDistance=3.5f;

    public void Boom()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, hittableLayer, QueryTriggerInteraction.Collide);
        enemiesToAffects = FindClosestEnemies(hitColliders);

        foreach (var e in enemiesToAffects) {
            e.GetComponent<Flocking>().SetTarget(this.gameObject);
        }

        StartCoroutine("StartBoom");
    }

    private IEnumerator StartBoom()
    {

        yield return new WaitForSeconds(waitTime);

        foreach (var e in enemiesToAffects) {
            e.GetComponent<Flocking>().RestoreTarget();
           
        }

        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius, hittableLayer, QueryTriggerInteraction.Collide);

        foreach (var c in hitColliders)
        {
            var h = c.gameObject.GetComponent<IHittable>();
            if (h != null) h.OnHit(damage);
        }

        Destroy(this.gameObject);

    }
    private List<AbstractEnemy> FindClosestEnemies(Collider[] colliders)
    {
        List<AbstractEnemy> enemies = new List<AbstractEnemy>();
        foreach (var collider in colliders) {
            ChargerEnemyBehaviour charge = collider.gameObject.GetComponent<ChargerEnemyBehaviour>();
            NormalEnemyBehaviour normal = collider.gameObject.GetComponent<NormalEnemyBehaviour>();
            CubeEnemyBehaviour cube = collider.gameObject.GetComponent<CubeEnemyBehaviour>();
            if (charge != null || normal != null || cube != null) {
                var enemy = collider.GetComponent<AbstractEnemy>();
                enemies.Add(enemy);
            }
        }
        List<AbstractEnemy> closestEnemies = new List<AbstractEnemy>();
        GameObject player = EnemiesManager.instance.player;
        return enemies.Where(e => Vector3.Distance(e.transform.position, player.transform.position)< radius)
                      .OrderBy(e => Vector3.Distance(e.transform.position, player.transform.position))
                      .Take(maxNumber).ToList();


    }
}
