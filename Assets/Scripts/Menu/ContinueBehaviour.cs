using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueBehaviour : MonoBehaviour {
    public int creditTime = 15;
    public float waitOnCountdownTime = 1f;
    public GameObject panel;
    public Text timeText;
    public Text creditText;

    int _currentTime = 0;

    bool _canPressAnyKey = false;
    WaitForSeconds _waitOnCountdown;

	void Start ()
    {
        EventManager.instance.SubscribeEvent(Constants.CREDIT_LOSED, OnCreditLosed);
        _waitOnCountdown = new WaitForSeconds(waitOnCountdownTime);
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
        timeText.text = _currentTime.ToString();
        yield return _waitOnCountdown;
        _canPressAnyKey = true;
        while (_currentTime > 0)
        {
            timeText.text = _currentTime.ToString();
            yield return _waitOnCountdown;
            _currentTime--;
        }
    }
    
    void Update ()
    {
		if(_canPressAnyKey && Input.anyKeyDown && panel.activeSelf)
        {
            _canPressAnyKey = false;
            panel.SetActive(false);
            EventManager.instance.ExecuteEvent(Constants.PAUSE_OR_UNPAUSE);
        }
	}
}
