using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterUlt : MonoBehaviour {
    public float timer=4;
    public int amount=10;
    public LineRenderer line;
    public Transform player;
    public GameObject ball;
    public float minRange=0;
    public float maxRange = 10;
    public LayerMask layerToInstance;
    public LayerMask layerToHit;
    public static ScatterUlt instance;
    public List<GameObject> balls= new List<GameObject>();
        List<Vector3> posiciones = new List<Vector3>();
    private void Start()
    {
        instance = this;
    }
    public void StarUlt() {

        for (int i = 0; i < amount; i++)
        {
            float distance = Random.Range(minRange, maxRange);
            Vector3 pos = Utility.RandomVector3InRadiusCountingBoundariesInAnyDirection(player.position, distance, layerToInstance);
            posiciones.Add(pos);
            GameObject b= Instantiate(ball, pos, this.transform.rotation);
            balls.Add(b);
        }

        StartCoroutine("SpawnRays");


    }
    IEnumerator SpawnRays()
    {
        yield return new WaitForSeconds(1f);
        line.positionCount = amount;
        line.SetPositions(posiciones.ToArray());
        for (int i = 0; i < amount - 1; i++)
        {
            Vector3 dir = balls[i + 1].transform.position - balls[i].transform.position;
            float dis = Vector3.Distance(balls[i].transform.position, balls[i+1].transform.position);
            Debug.Log(dir);
            Debug.Log(dis);
            RaycastHit[] rhs = Physics.SphereCastAll(posiciones[i], 4, dir, dis, layerToHit);
            foreach (RaycastHit rh in rhs)
            {
                IHittable ihittable = rh.collider.gameObject.GetComponent<IHittable>();
                Debug.Log(rh.collider.gameObject.name);
                if (ihittable != null)
                {
                    ihittable.OnHit(5);
                    //Debug.Log("hit");
                }

            }
        }
        yield return new WaitForSeconds(1f);
        ResetBalls();
    }

    private void ResetBalls()
    {
        foreach (var b in balls)
        {
            Destroy(b.gameObject);
        }
        line.positionCount = 0;
        balls = new List<GameObject>();
        posiciones = new List<Vector3>();
    }

    internal void Stop()
    {
        ResetBalls();
    }
}
