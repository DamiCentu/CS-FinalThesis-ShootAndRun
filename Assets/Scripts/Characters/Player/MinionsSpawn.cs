using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsSpawn: MonoBehaviour
{

    public float timer = 4;
    public int amount = 10;
    public LineRenderer line;
    public Transform player;
    public GameObject minion;
    public float minRange = 0;
    public float maxRange = 10;
    public LayerMask layerToInstance;
    public LayerMask layerToHit;
    public static MinionsSpawn instance;
    public List<GameObject> minions = new List<GameObject>();
  //  List<Vector3> posiciones = new List<Vector3>();

    private void Start()
    {
        instance = this;
    }
    public void StarUlt()
    {

        for (int i = 0; i < amount; i++)
        {
            float distance = UnityEngine.Random.Range(minRange, maxRange);
            Vector3 pos = Utility.RandomVector3InRadiusCountingBoundaries(player.position, distance, layerToInstance);
           // posiciones.Add(pos);
            GameObject b = Instantiate(minion, pos, this.transform.rotation);
            minions.Add(b);
        }
    }

}