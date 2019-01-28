using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueBehaviour : MonoBehaviour {
    public int creditTime = 15;
    public GameObject panel;
    public Text timeText;
    public Text creditText;

    int _currentTime = 0;

	void Start ()
    {
        EventManager.instance.SubscribeEvent(Constants.CREDIT_LOSED, OnCreditLosed);
    }

    private void OnCreditLosed(object[] param)
    {
        EventManager.instance.ExecuteEvent(Constants.PAUSE_OR_UNPAUSE, new object[] { true });
        StartCoroutine(CountdownRoutine((int)param[0]));
    }

    IEnumerator CountdownRoutine(int currentCredits)
    {
        panel.SetActive(true);
        _currentTime = creditTime;
        creditText.text = "Credits: " + currentCredits.ToString();
        while (_currentTime > 0)
        {
            timeText.text = _currentTime.ToString();
            yield return new WaitForSeconds(1.0f);
            _currentTime--;
        }
    }
    
    void Update ()
    {
		if(Input.anyKeyDown && panel.activeSelf)
        {
            panel.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.PAUSE_OR_UNPAUSE);
        }
	}
}
