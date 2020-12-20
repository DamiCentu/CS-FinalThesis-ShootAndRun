using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementsUI : MonoBehaviour {
    public Text text;
    public Text textTitle;
    public Image panel;
    public float fadeTime;
    public float showTime;
    public Coroutine corutine;

    private void Start()
    {
        text.text = "";
        panel.color = FadeIn(0,panel.color);
        text.color = FadeIn(0, text.color);
        textTitle.color = FadeIn(0, textTitle.color);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.U)) {
            ShowEvent("textooo");
        }
	}

    public void ShowEvent(string eventName) {
        if(corutine!=null)
            StopCoroutine(corutine);
        corutine= StartCoroutine(ShowEventCorutine(eventName));
    }

    IEnumerator ShowEventCorutine(string eventName)
    {
        text.text = eventName;
        float time = 0;
        while (time <= fadeTime) {
            float percentage= time/fadeTime;
            panel.color = FadeIn(percentage,panel.color);
            text.color = FadeIn(percentage, text.color);
            textTitle.color = FadeIn(percentage, textTitle.color);
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }

        while (time <= fadeTime+showTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }

        while (time <= fadeTime + showTime + showTime)
        {
            float auxTime = time - fadeTime - showTime;
            float percentage = auxTime / fadeTime;
            panel.color = FadeOut(percentage, panel.color);
            text.color = FadeOut(percentage, text.color);
            textTitle.color = FadeOut(percentage, textTitle.color);
            yield return new WaitForSeconds(Time.deltaTime);
            time += Time.deltaTime;
        }
        text.text = "";
    }

    public Color FadeIn(float percentage, Color color) {
        var alpha = Mathf.Lerp(0.0f, 1.0f, percentage);
        return new Color(color.r, color.g, color.b, alpha);
    }


    public Color FadeOut(float percentage, Color color)
    {
        var alpha = Mathf.Lerp(1.0f, 0.0f, percentage);
        return new Color(color.r, color.g, color.b, alpha);
    }

}
