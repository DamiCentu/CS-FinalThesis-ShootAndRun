using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullets : EnemyBulletBehaviour
{
    public float followIntensity=0.2f;
    public float lifeTime = 3;
    public float timer;


    public void Awake()
    {
        timer = lifeTime;
    }

    public override void OnTriggerEnter(Collider c)
    {

    }
    public override void Update()
    {
        timer -= Time.deltaTime;
        base.Update();
        Transform player = EnemiesManager.instance.player.transform;
        /*float rotX = Mathf.Lerp(this.transform.position.x, player.transform.position.x, followIntensity);
        float rotz = Mathf.Lerp(this.transform.position.z, player.transform.position.z, followIntensity);

          this.transform.rotation = Quaternion.Euler(new Vector3(rotX, 0, rotz));
        Vector3 rot =Quaternion.LookRotation(player.position - this.transform.position, Vector3.up).eulerAngles;

        float rotX = Mathf.Lerp(this.transform.rotation.eu.x, player.transform.position.x, followIntensity);
        float rotz = Mathf.Lerp(this.transform.position.z, player.transform.position.z, followIntensity);*/

        Vector3 auxDir = player.position - this.transform.position;
        this.transform.forward = Vector3.RotateTowards(transform.forward, auxDir, followIntensity, 0.0f);
        this.transform.forward = new Vector3(this.transform.forward.x, 0f, this.transform.forward.z);
        //  this.transform.LookAt(player);
        if (timer <= 0) {
            Destroy(this.gameObject);
        }
    }

}
