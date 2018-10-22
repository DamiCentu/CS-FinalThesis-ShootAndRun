using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsSpawn: MonoBehaviour
{

    public float timer = 4;
    public int amount = 4;
    public Transform player;
    public GameObject minion;
    public static MinionsSpawn instance;
     List<GameObject> minions = new List<GameObject>();
    public float lifeTime=5f;
    public float distance = 3f;
    float currentAngle;

    private void Start()
    {
        instance = this;
    }
    public void StarUlt()
    {
        currentAngle = 0;
        for (int i = 0; i < amount; i++)
        {
            currentAngle += 360/amount;
            //print(currentAngle);

            var angleRadians = Mathf.Deg2Rad * currentAngle;

            float x = player.transform.position.x + (float)Math.Cos(angleRadians) *distance;
            float z = player.transform.position.z + (float)Math.Sin(angleRadians) *distance;

            Vector3 position = new Vector3(x, player.transform.position.y, z);

            GameObject b = Instantiate(minion, position, this.transform.rotation);
            b.transform.SetParent(player);
            Destroy(b, lifeTime);
            minions.Add(b);
        }
    }

    internal void Stop()
    {
        foreach (var item in minions)
        {
            Destroy(item);
        }
    }
}