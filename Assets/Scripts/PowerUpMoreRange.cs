using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMoreRange : IPowerUp {
    public float multiplierRange;

    public void Start()
    {
        EventManager.instance.SubscribeEvent("ReduceRange", ReduceRange);
    }

    void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            object[] container = new object[1];
            container[0] = 1;
            EventManager.instance.ExecuteEvent("PrimaryWeaponMoreRange", container);
            //Destroy(this.gameObject);
        }
    }

    public void ReduceRange(object[] parameterContainer)
    {
        object[] container = new object[1];
        container[0] =-1;
        EventManager.instance.ExecuteEvent("PrimaryWeaponMoreRange", container);
    }
}
