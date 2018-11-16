using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreadorMapaModular : MonoBehaviour , IPauseable {
    public GameObject prefab;
    public bool instance=false;
    public int x;
    public int z;
    public Transform initialPos;
    public Transform parent;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_paused)
            return;

        if (instance == true)
        {
            instance = false;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < z; j++)
                {

                    GameObject p=Instantiate(prefab,new Vector3(i,0,j)+ initialPos.position, initialPos.transform.rotation);
                    p.transform.parent = parent;
                }
            }

        }
	}
}
