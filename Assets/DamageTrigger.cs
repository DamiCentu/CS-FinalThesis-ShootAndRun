using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour {
    public LayerMask layerThatDontAffect;

    public void OnTriggerStay(Collider other)
    {
        if (layerThatDontAffect != (layerThatDontAffect | (1 << other.gameObject.layer)))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                p.OnHit(1);
            }
            print("me choque");
     //       EventManager.instance.ExecuteEvent(Constants.CHARGER_CRUSH);

        }
    }
}

