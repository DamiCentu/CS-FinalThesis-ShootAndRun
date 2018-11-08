using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public abstract class IBullet : MonoBehaviour {
    public int damage;
    public float speed;
    public float lifeTime = 1;
    public float timer;


    public void Awake()
    {
        timer = lifeTime;
    }

    
    public virtual void DestroyBullet()
    {
        //print("se destruye");
         object[] content = new object[1];
        content[0] = this;
        //print(1);
        EventManager.instance.ExecuteEvent(Constants.EVENT_BULLET_RETURN_TO_POOL,content);
    }


    public void EnableBullet()
    {
        this.gameObject.SetActive(true);
        this.GetComponent<TrailRenderer>().enabled = true;
        timer = lifeTime;
    }
    public void UnableBullet()
    {
        this.gameObject.SetActive(false);
        this.GetComponent<TrailRenderer>().Clear();
        this.GetComponent<TrailRenderer>().enabled = false;
    }
}
