using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {
    protected Pool<NormalBullet> _bulletPool;
    public int numberPoolBullet = 10;
    public NormalBullet bulletPrefab;
    public static BulletManager instance = null;
    private float  range;
    private int index=0;
    public List<float> ranges;

    GameObject _bulletContainer;

    private void Awake() {
        if (instance == null) 
            instance = this; 
        else 
            Destroy(this.gameObject);

        _bulletContainer = new GameObject("PlayerBulletContainer");
    } 

    void Start() {
        EventManager.instance.SubscribeEvent( Constants.EVENT_BULLET_RETURN_TO_POOL, ReturnBulletToPool);
        _bulletPool = new Pool<NormalBullet>(numberPoolBullet, BulletFactory, null, null, true);
        EventManager.instance.SubscribeEvent("PrimaryWeaponMoreRange", ChangeRange);
        if(ranges.Count > 0)
            range = ranges[index];
    }

    void ChangeRange(object[] parameterContainer) {
        index += (int)parameterContainer[0];
        range = ranges[index];
    } 

    NormalBullet BulletFactory() {
        NormalBullet b = Instantiate<NormalBullet>(bulletPrefab, _bulletContainer.transform);
        b.gameObject.SetActive(false);
        return b;
    }

    void ReturnBulletToPool(object[] parameterContainer) {
        //print("asd");
        NormalBullet bullet = (NormalBullet)parameterContainer[0];
        bullet.UnableBullet();
        _bulletPool.DisablePoolObject(bullet);
    } 

    public NormalBullet GetBulletFromPool() { 
        NormalBullet b =  _bulletPool.GetObjectFromPool();
        b.EnableBullet();
        return b;
    }

    public void SetBullet(NormalBullet b, Vector3 position, Vector3 forward) {
        b.transform.position = position;
        b.transform.forward = forward;
        b.GetComponent<TrailRenderer>().enabled = true;
        b.timer = b.timer * range;
    }
}
