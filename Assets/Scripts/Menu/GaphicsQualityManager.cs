using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaphicsQualityManager : MonoBehaviour {

    public MenuNeonButtonBehaviour[] allButtons;

    void Start() {
        EventManager.instance.SubscribeEvent(Constants.MENU_BUTTON_CLICKED, onMenuButtonCicked);

        foreach (var bt in allButtons)
        {
            bt.ForceDisable();
        }

        var qualityLevel = QualitySettings.GetQualityLevel();

        if(qualityLevel == 0)
            allButtons.Where(x => x.id == "Low").First().SetSelectedState();
        else if(qualityLevel == 2)
            allButtons.Where(x => x.id == "Medium").First().SetSelectedState();
        else if(qualityLevel == 5)
            allButtons.Where(x => x.id == "High").First().SetSelectedState();
    }

    private void onMenuButtonCicked(object[] parameterContainer)
    {
        MenuNeonButtonBehaviour buttonClicked = null;
        foreach (var button in allButtons)
        {
            if (button.id == (string)parameterContainer[0])
                buttonClicked = button;
        }

        if (buttonClicked == null)
            return;

        foreach (var buttonete in allButtons)
        {
            if (buttonete.id != buttonClicked.id)
                buttonete.ForceDisable();
        }
    }

    public void SetLowGraphics()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void SetMediumGraphics()
    {
        QualitySettings.SetQualityLevel(2);
    }

    public void SetHighGraphics()
    {
        QualitySettings.SetQualityLevel(5);
    }
}
