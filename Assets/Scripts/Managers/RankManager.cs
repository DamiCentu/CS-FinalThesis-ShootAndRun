using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

    public GameObject rankPanel;
    public GameObject scorePanel;
    public GameObject setNameForScorePanel;
    public Text scoreInSetNamePanel; //el score para mostrar mientras se settea el nombre
    public Text[] nameCharsInPanel; //3

    public Text[] scoreTexts; //los 10 textos de los scores
    public Text[] numberRankTexts; // los 10 textos del numero en posision del ranking
    public Text[] aliasTexts; //los 10 textos de las 3 letras de los duenios del score

    public float timeToChangeCharInPanel = 1f;

    [Header("esto es para saber cuantos textos hay que poner en el editor")]
    public int maxScoresToShow = 10;

    bool _canChangeName = false;
    bool _canGoToWinScreen = false;

    int _currentIndexSelected = 0;
    float _timerToChangeCharInPanel = 15f;
    Data _actualData;
    Data[] _allRanks;

    void Start ()
    {
        EventManager.instance.SubscribeEvent(Constants.WIN_LEVEL, OnWinLevel);
        _actualData = new Data();
	}

    private void OnWinLevel(object[] parameterContainer)
    {
        rankPanel.SetActive(true);
        EventManager.instance.ExecuteEvent(Constants.PAUSE_OR_UNPAUSE);
        DoShit();
    }

    void DoShit () //lo que hago aca es chequear si hay que poner el panel con todos los scores guardados
    {
        var points = FindObjectOfType<PointsManager>().CurrentPoints;
        _allRanks = SavingAndLoading.LoadRanks(SceneManager.GetActiveScene().name);
        if(_allRanks != null && _allRanks.Length > 0) //preguntamos si esta vacio la data cargada, significa que no hay scores guardados
        {
            if(_allRanks.Length >= maxScoresToShow) //preguntamos si esta lleno el array para mostrar de data
            {
                bool hasToShowScoreWithoutChangeAnything = true;
                foreach (var data in _allRanks)
                {
                    if(data.score < points) //como esta lleno nos fijamos si hay alguno menor
                    {
                        hasToShowScoreWithoutChangeAnything = false;
                        SetPanelForSetNameOnScore(points);
                        break;
                    }
                }

                if(hasToShowScoreWithoutChangeAnything)
                {
                    SetPanelForScores(); //si no hay ninguno menor mostramos el score tal cual esta cargado
                }
            }
            else
            {
                SetPanelForSetNameOnScore(points); //si le falta algun lugar para llegar a la maxima cantidad de scores se muestra igual el panel
            }
        }
        else
        {
            SetPanelForSetNameOnScore(points); //se meustra porque esta vacio
        }
    }

    void SetPanelForSetNameOnScore(int score) // esto es cuando un score falta para llegar a los 10 que se muestran o cuando hay un score que no es mayor al recien hecho
    {
        setNameForScorePanel.SetActive(true);
        scoreInSetNamePanel.text = score.ToString();
        foreach (var t in nameCharsInPanel)
        {
            t.text = "a";
        }
        _canChangeName = true;
        _actualData.score = score;
    }

    void SetPanelForScores(Data actualData = null) //aca es para mostrar una vez que se seteo el score nuevo o cuando el score no es mayor a cualquiera de los guardados
    {
        var listToSave = new List<Data>();

        if (_allRanks != null)
            listToSave = _allRanks.ToList();

        if (actualData != null)
        {
            listToSave.Add(actualData);
        }

        listToSave = listToSave.OrderByDescending(x => x.score).ToList();

        while(listToSave.Count > maxScoresToShow)
        {
            listToSave.RemoveAt(listToSave.Count - 1);
        }

        SavingAndLoading.SaveRank(listToSave.ToArray(), SceneManager.GetActiveScene().name);

        scorePanel.SetActive(true);

        for (int i = 0; i < maxScoresToShow; i++)
        {
            if(i >= listToSave.Count)
            {
                scoreTexts[i].text = "";
                aliasTexts[i].text = "";
                numberRankTexts[i].text = "";
            }
            else
            {
                scoreTexts[i].text = listToSave[i].score.ToString();
                aliasTexts[i].text = listToSave[i].name;
                numberRankTexts[i].text = (i + 1).ToString();
            }
        }

        _canGoToWinScreen = true;
    }

    void Update()
    {
        if(_canGoToWinScreen)
        {
            if(SceneManager.GetActiveScene().name == Constants.LEVEL_2_NAME)
            {
                if(Input.anyKeyDown)
                {
                    EventManager.instance.ExecuteEvent(Constants.GO_TO_GAME_COMPLETE_SCENE);
                }
            }
            else
            {
                if (Input.anyKeyDown)
                {
                    EventManager.instance.ExecuteEvent(Constants.GO_TO_LEVEL_COMPLETE_SCENE);
                }
            }
        }

        if (!_canChangeName)
            return;

        _timerToChangeCharInPanel += Time.deltaTime;

        if (_timerToChangeCharInPanel < timeToChangeCharInPanel)
            return;

        var inputVer = Input.GetAxisRaw("VerticalMouse");
        var inputHor = Input.GetAxisRaw("HorizontalMouse");
       
        if (inputVer != 0)
        {
            if (inputVer > 0)
            {
                nameCharsInPanel[_currentIndexSelected].text = incrementCharacter(nameCharsInPanel[_currentIndexSelected].text[0], true).ToString();
            }
            else
            {
                nameCharsInPanel[_currentIndexSelected].text = incrementCharacter(nameCharsInPanel[_currentIndexSelected].text[0], false).ToString();
            }
            _timerToChangeCharInPanel = 0;
        }
        else if (inputHor != 0)
        {
            if (inputHor > 0)
            {
                _currentIndexSelected++;
                if (_currentIndexSelected >= nameCharsInPanel.Length)
                {
                    _currentIndexSelected = nameCharsInPanel.Length - 1;
                }
            }
            else
            {
                _currentIndexSelected--;
                if (_currentIndexSelected < 0)
                {
                    _currentIndexSelected = 0;
                }
            }
            _timerToChangeCharInPanel = 0;
        }

        for (int i = 0; i < nameCharsInPanel.Length; i++)
        {
            if (i == _currentIndexSelected)
                nameCharsInPanel[i].color = Color.yellow;
            else
                nameCharsInPanel[i].color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _currentIndexSelected = 0;
            _canChangeName = false;
            setNameForScorePanel.SetActive(false);
            _actualData.name = "";
            foreach (var c in nameCharsInPanel)
            {
                _actualData.name = _actualData.name + c.text;
            }
            SetPanelForScores(_actualData);
        }
    }

    char incrementCharacter(char input, bool increment)
    {
        if(increment)
            return (input == 'z' ? 'a' : (char)(input + 1));
        else
            return (input == 'a' ? 'z' : (char)(input - 1));
    }
}
