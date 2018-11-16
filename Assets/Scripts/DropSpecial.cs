using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropSpecial : MonoBehaviour , IPauseable {
    public Mine mine;
    public SpecialBombGun bombGun;
    public List<Image> SpecialImages;
    Image currentImage;
    public List<string> SpecialName;
    string currentName;
    public Text currentText;
    public float timeToChangeSpecial;
    int index = 0;
    Timer timer;

    bool _paused;
    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start()
    {
        timer = new Timer(timeToChangeSpecial, ChangeSpecial);
        timer = new Timer(timeToChangeSpecial, ChangeSpecial);
        currentImage = SpecialImages[index];
        currentText.text = SpecialName[index];
        currentText.gameObject.SetActive(true);
        currentImage.gameObject.SetActive(true);
    }

    private void ChangeSpecial()
    {
        currentImage.gameObject.SetActive(false);
        index++;
        if (index >= SpecialImages.Count)
        {
            index = 0;
        }
        currentImage = SpecialImages[index];
        currentImage.gameObject.SetActive(true);
        currentText.text = SpecialName[index];
        timer = new Timer(timeToChangeSpecial, ChangeSpecial);
    }

    void Update()
    {
        if (_paused)
            return;
        timer.CheckAndRun();
    }


    private void OnTriggerEnter(Collider other)
    {
        //print("chau");
        if (other.gameObject.layer == 8) //player
        {
            IShootable Special;
            if (index == 0)
            {
                Special = mine;
            }
            else {
                Special = bombGun;
            }

            Player p = other.GetComponent<Player>();
            p.ChangeSpecial(Special);

            object[] container = new object[3];
            container[0] = p.id;
            container[1] = SpecialImages[index];
            container[2] = SpecialName[index];
            EventManager.instance.ExecuteEvent(Constants.SPECIAL_CHANGE, container);


            gameObject.SetActive(false);


        }
    }
}
