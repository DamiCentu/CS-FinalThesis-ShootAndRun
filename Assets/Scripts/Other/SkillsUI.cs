using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillsUI : MonoBehaviour {
    
    public Image specialImage;
    public Image specialImageInPlayer;

    public Image ultimateActivated;
    public Image ultimateUsing;

    public List<Image> dashes;
    public List<Image> unactiveDashes;
    public List<Image> dashesInPlayer;
    public List<Image> unactiveDashesInPlayer;

    public static SkillsUI instance;

    public Player.ID id;
    public GameObject gm;

    void Start () {
        EventManager.instance.SubscribeEvent(Constants.ULTIMATE_TIME, RefreshUltimate);
        EventManager.instance.SubscribeEvent(Constants.SPECIAL_TIME, RefreshSpecial);
        EventManager.instance.SubscribeEvent(Constants.ULTIMATE_CHANGE, ChangeUltimate);
        EventManager.instance.SubscribeEvent(Constants.SPECIAL_CHANGE, ChangeSpecial);
        EventManager.instance.SubscribeEvent(Constants.UPDATE_DASH, UpdateDash);
        EventManager.instance.SubscribeEvent(Constants.SHOW_SKILL_UI, ShowSkills );
        EventManager.instance.SubscribeEvent(Constants.LEFT_TIME_ULTING, TimeLeftUlting);

    }


    private void ShowSkills(object[] parameterContainer) {
        if(gm != null)
            gm.SetActive((bool)parameterContainer[0]);
    }

    private void UpdateDash(object[] parameterContainer)
    {
        int maxDash = (int)parameterContainer[1];
        int loadedDash = (int)parameterContainer[0]; 
        for (int i = 0; i < dashes.Count; i++)
        {
            if (i < maxDash) {
                if (i < loadedDash)
                {
                    SetDashesActivity(i,true,false);
                }
                else {
                    SetDashesActivity(i, false, true);
                }
            }
            else
            {
                SetDashesActivity(i, false, false);
            }
        }
    }

    private void SetDashesActivity(int i, bool b1, bool b2)
    {
        dashes[i].gameObject.SetActive(b1);
        unactiveDashes[i].gameObject.SetActive(b2);
        dashesInPlayer[i].gameObject.SetActive(b1);
        unactiveDashesInPlayer[i].gameObject.SetActive(b2);
    }

    private void RefreshUltimate(object[] parameterContainer)
{
        if ((Player.ID)parameterContainer[2] != id)
        {
            return;
        }

        float timer = (float)parameterContainer[0];
        float maxTime = (float)parameterContainer[1];

        if (timer <= 0)
        {
           
        }
        else {
            ultimateActivated.enabled = true;
            RefreshImage(timer, maxTime, ultimateActivated);
        }
    }

    private void TimeLeftUlting(object[] parameterContainer)
    { 
        if ((Player.ID)parameterContainer[2] != id)
        {
            return;
        }
        float timer = (float)parameterContainer[0];
        float maxTime = (float)parameterContainer[1];

        ultimateUsing.enabled = true;
        ultimateActivated.enabled = false;
        RefreshImage(timer, maxTime, ultimateUsing);

    }

    void RefreshImage(float timer, float maxTime, Image image) {
        if (timer < 0)
        {
            image.fillAmount = 1;
        }
        else if (timer < maxTime)
        {
            float percentage = 1 - (timer / maxTime);
            image.fillAmount = percentage;
        }
    }

    void RefreshImage(float timer, float maxTime)
    {
        if (timer < maxTime)
        {
            float percentage = 1 - (timer / maxTime);
        }
    }

    private void ChangeUltimate(object[] parameterContainer)
    {
        if ((Player.ID)parameterContainer[0] != id)
        {
            return;
        }
        Image newImage = (Image)parameterContainer[1];
        string newName = (string)parameterContainer[2];
        //ultimateName.text = newName;
    }
    
    private void RefreshSpecial(object[] parameterContainer)
    {
        if ((Player.ID)parameterContainer[2] != id)
        {
            return;
        }

        float timer = (float)parameterContainer[0];
        float maxTime = (float)parameterContainer[1];

        RefreshImage(timer, maxTime, specialImage);
        RefreshImage(timer, maxTime, specialImageInPlayer);

    }
    
    private void ChangeSpecial(object[] parameterContainer)
    {
        if ((Player.ID)parameterContainer[0] != id)
        {
            return;
        }
        Image newImage = (Image)parameterContainer[1];
        string newName = (string)parameterContainer[2];
        specialImage.sprite = newImage.sprite;
        //specialName.text = newName;
    } 
}
