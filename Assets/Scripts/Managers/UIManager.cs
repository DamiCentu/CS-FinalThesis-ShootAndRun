using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    public List<GameObject> dash;
    public SimpleHealthBar bossLife;
    public List<Image> lifeImages;
    public Text creditsText;

    public static UIManager instance = null;

    private void Start()
    {
        EventManager.instance.SubscribeEvent(Constants.UPDATE_BOSS_LIFE, UpdateBossLife);
        EventManager.instance.SubscribeEvent(Constants.UPDATE_PLAYER_LIFE, OnUpdatePlayerLife);
        if (instance == null) 
            instance = this;
   }



    private void UpdateBossLife(object[] parameterContainer) {
        if (bossLife == null)
            return;

        int life = (int)parameterContainer[0];
        int maxLife = (int)parameterContainer[1];

        if (life > 0) {
            bossLife.transform.parent.gameObject.SetActive(true);
            bossLife.UpdateBar(life, maxLife);
            if (life < (float)maxLife / 4) {
                bossLife.UpdateColor(Color.red);
            }
            else if (life < (float)maxLife / 2) {
                bossLife.UpdateColor(Color.yellow);

            }
            else {
                bossLife.UpdateColor(Color.green);
            }
        }
        else {
            bossLife.transform.parent.gameObject.SetActive(false);
        } 
    }

    public void OnUpdatePlayerLife(object[] parameterContainer) { 
        int lifeCount = (int)parameterContainer[0];
        
        if (lifeCount <= 0) {
            foreach (var i in lifeImages) { 
                i.gameObject.SetActive(false);
            } 
            return;
        }

        for (int i = 0; i < lifeCount; i++) 
            lifeImages[i].gameObject.SetActive(true); 

        for (int i = lifeCount; i < lifeImages.Count; i++)
            lifeImages[i].gameObject.SetActive(false);

        creditsText.text = "Credits: " + (int)parameterContainer[1]; //creditsRemaining 
    }
}
