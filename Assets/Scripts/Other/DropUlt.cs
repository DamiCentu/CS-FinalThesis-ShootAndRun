using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropUlt : MonoBehaviour , IPauseable {
    public List<Image> ultImages;
    Image currentImage;
    public List<string> ultName;
    string currentName;
    public Text currentText;
    public float timeToChangeUlt;
     int index = 0;
    Timer timer;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start () {
        timer = new Timer(timeToChangeUlt, ChangeUlt);
        currentText.text = ultName[index];
        currentImage = ultImages[index];
        currentText.gameObject.SetActive(true);
        currentImage.gameObject.SetActive(true);
    }

    private void ChangeUlt()
    {
        currentImage.gameObject.SetActive(false);
        index++;
        if (index >= ultImages.Count) {
            index = 0;
        }
        currentImage = ultImages[index];
        currentImage.gameObject.SetActive(true);
        currentText.text = ultName[index];
        timer = new Timer(timeToChangeUlt, ChangeUlt);
    }

    void Update () {
        if (_paused)
            return;

        timer.CheckAndRun();
	}


    private void OnTriggerEnter(Collider other)
    {
        print("chau");
        if (other.gameObject.layer== 8) //player
        {
            Player.Ults newUlt;

            if (index == 0) {
                newUlt = Player.Ults.Berserker;

            }
            else {
                newUlt = Player.Ults.SlowTime;
            }

            Player p=other.GetComponent<Player>();
            p.ChangeUlt(newUlt);


            object[] container = new object[3];
            container[0] = p.id;
            container[1] = ultImages[index];
            container[2] = ultName[index];
            EventManager.instance.ExecuteEvent(Constants.ULTIMATE_CHANGE, container);

            gameObject.SetActive(false);

        }
    }
}
