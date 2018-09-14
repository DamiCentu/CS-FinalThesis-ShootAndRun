using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : IShootable {

    public GameObject prefabMine;
    bool mineSet= false;
    GameObject mine; 


    public override void Shoot(Transform shootPosition, Vector3 forward)
    {
        //print("asd");
        if (mineSet) {
            mine.GetComponent<MineBullet>().Boom();
            mineSet = false;
        }

        else if (this._canShoot)
        {
            _timer = cooldown;
            mine = Instantiate(prefabMine, shootPosition.transform.position, Quaternion.Euler(forward));
            mine.transform.forward = forward;
            mineSet = true;
        }

    }
}
