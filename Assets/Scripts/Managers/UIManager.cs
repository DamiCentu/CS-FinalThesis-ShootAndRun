using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour, IPauseable {

    public List<GameObject> dash;
    public SimpleHealthBar bossLife0;
    public GameObject bossLifeBar0;

    public SimpleHealthBar bossLife1;
    public GameObject bossLifeBar1;

    public SimpleHealthBar bossLife2;
    public GameObject bossLifeBar2;

    public List<Image> lifeImages;
    public Text tutorialText;
    public Text pointsText;
    public Text multiplierPointsText;
    public Text notificationPointsText;
    public float timeToShowPointsInfoText = 1.5f;
    public float timeToDisapearTutoText = 4f;
    public GameObject panelOfTutorial;

    public static UIManager instance = null;

    public const string TUTORIAL_MOVE = "Use W,A,S,D to move";
    public const string TUTORIAL_DASH = "Use spacebar to dash";
    public const string TUTORIAL_ULTIMATE = "Use Q to use your ULTIMATE!";
    public const string TUTORIAL_SHOOT = "Use left click to shoot";
    public const string TUTORIAL_SHOOT_SPECIAL = "Use right click to shoot your special weapon";
    public const string TUTORIAL_PICK_POWER_UP = "Pick power ups to upgrade your champion";

    WaitForSeconds _waitToDisapearTutoText;
    HashSet<string> _hashToCompare = new HashSet<string>();
    Queue<String> queueForNotificationPointsText = new Queue<String>();

    bool _paused;
    WaitForSeconds _waitBetweenTextInfoPoints;

    public void OnPauseChange(bool v)
    {
        _paused = v;
    }

    void Start() {
        EventManager.instance.SubscribeEvent(Constants.UPDATE_BOSS_LIFE, UpdateBossLife);
        EventManager.instance.SubscribeEvent(Constants.UI_UPDATE_PLAYER_LIFE, OnUpdatePlayerLife);
        EventManager.instance.SubscribeEvent(Constants.UI_TUTORIAL_RESTART, OnTutorialRestart);
        EventManager.instance.SubscribeEvent(Constants.UI_TUTORIAL_CHANGE, OnTutorialChange);
        EventManager.instance.SubscribeEvent(Constants.UI_TUTORIAL_DEACTIVATED, OnTutorialDeactivated);
        EventManager.instance.SubscribeEvent(Constants.UI_POINTS_UPDATE, OnPointsUpdate);
        EventManager.instance.SubscribeEvent(Constants.UI_CLEAR_MULTIPLIER, OnClearMultiplierText);
        EventManager.instance.SubscribeEvent(Constants.UI_NOTIFICATION_TEXT_UPDATE, OnNotificationTextUpdate);
        if (instance == null) { 
            instance = this;
        }
        pointsText.text = "0";
        tutorialText.text = "";
        multiplierPointsText.text = "";
        multiplierPointsText.transform.parent.gameObject.SetActive(false);
        notificationPointsText.transform.parent.gameObject.SetActive(false);

        _waitToDisapearTutoText = new WaitForSeconds(timeToDisapearTutoText);
        _waitBetweenTextInfoPoints = new WaitForSeconds(timeToShowPointsInfoText);
    }

    private void OnNotificationTextUpdate(object[] param)
    {
        if (!queueForNotificationPointsText.Contains((string)param[0]))
        {
            queueForNotificationPointsText.Enqueue((string)param[0]);
        }

        StartCoroutine(ShowTextFromQueueRoutine(notificationPointsText, queueForNotificationPointsText, _waitBetweenTextInfoPoints));
    }

    private void OnClearMultiplierText(object[] parameterContainer)
    {
        multiplierPointsText.text = "";
        multiplierPointsText.transform.parent.gameObject.SetActive(false);
    }

    private void OnPointsUpdate(object[] param)
    {
        var currentPoints = (int)param[0];
        var currentMultiplier = (float)param[1];

        if(currentMultiplier > 1)
        {
            var text = currentMultiplier.ToString() + "X";
            multiplierPointsText.transform.parent.gameObject.SetActive(true);
            multiplierPointsText.text = text;
        }

        pointsText.text = currentPoints.ToString();
    }

    IEnumerator ShowTextFromQueueRoutine(Text text, Queue<string> queue, WaitForSeconds wait)
    {
        text.transform.parent.gameObject.SetActive(true);

        while(queue.Count > 0)
        {
            text.text = queue.Dequeue();
            yield return wait;
        }

        text.text = "";
        text.transform.parent.gameObject.SetActive(false);
    }

    private void OnTutorialRestart(object[] parameterContainer) {
        _hashToCompare.Clear();
    }

    private void OnTutorialDeactivated(object[] parameterContainer) {
        tutorialText.enabled = false;
        panelOfTutorial.SetActive(false);
    }

    private void OnTutorialChange(object[] parameterContainer) {
        if(_hashToCompare.Contains((string) parameterContainer[0])) {
            return;
        }
        _hashToCompare.Add((string)parameterContainer[0]);
        panelOfTutorial.SetActive(true);
        tutorialText.enabled = true;
        StopAllCoroutines();
        tutorialText.text = (string)parameterContainer[0];
        StartCoroutine(TextDisapearRoutine());
    }

    IEnumerator TextDisapearRoutine() {
        yield return _waitToDisapearTutoText;
        while (_paused)
            yield return null;
        tutorialText.enabled = false;
        panelOfTutorial.SetActive(false);
    }


    public void ActivateBar(bool b, int n) {
        if (n == 0)
        {
            bossLifeBar0.SetActive(b);
        }

        else if (n == 1)
        {
            bossLifeBar1.SetActive(b);

        }
        else {
            bossLifeBar2.SetActive(b);
        }
    }

    private void UpdateBossLife(object[] parameterContainer)
    {
        if (bossLife0 == null)
            return;
        //      bossLife.gameObject.SetActive(true);
        int life = (int)parameterContainer[0];
        int maxLife = (int)parameterContainer[1];
        int numberBoss = (int)parameterContainer[2];

        if (numberBoss == 0)
        {
            UpdateLifeBarBoss(life, maxLife, bossLife0);

        }
        else if (numberBoss == 1)
        {

            UpdateLifeBarBoss(life, maxLife, bossLife1);
        }

        else
        {
            UpdateLifeBarBoss(life, maxLife, bossLife2);

        }
    }

    private void UpdateLifeBarBoss(int life, int maxLife, SimpleHealthBar bossLife)
    {
        if (life > 0)
        {
            bossLife.transform.parent.gameObject.SetActive(true);
            bossLife.UpdateBar(life, maxLife);
            if (life < (float)maxLife / 4)
            {
                bossLife.UpdateColor(Color.red);
            }
            else if (life < (float)maxLife / 2)
            {
                bossLife.UpdateColor(Color.yellow);

            }
            else
            {
                bossLife.UpdateColor(Color.green);
            }
        }
        else
        {
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

        //creditsText.text = "Credits: " + (int)parameterContainer[1]; //creditsRemaining 
    }
}
