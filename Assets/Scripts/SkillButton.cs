using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour {
    public float timerToDrop;
    float _timer;
    bool OnTheButton;
    public DropUlt ult;
    public DropSpecial special;
    public Transform newSkillPosition;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OnTheButton) {
            _timer -= Time.deltaTime;
            if (_timer <= 0) {
                float prob = UnityEngine.Random.Range(0, 1);
                if (prob < 0.1)
                {
                    ult.transform.position = newSkillPosition.position;
                    ult.gameObject.SetActive(true);
                }
                else {
                    special.transform.position = newSkillPosition.position;
                    special.gameObject.SetActive(true);
                }
                //print("ya ta!!");
                this.gameObject.SetActive(false);
            }
        }
	}


    private void OnTriggerEnter(Collider other)
    {
        //print("chau");
        if (other.gameObject.layer == 8) //player
        {
            OnTheButton = true;
            _timer = timerToDrop;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print("hola");
        if (other.gameObject.layer == 8) //player
        {
            OnTheButton = false;
        }
    }
}
