using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory_Defeat : MonoBehaviour {

    public void Continue()
    {
        SceneManager.LoadScene("Menu");
    }

    private void Update()
    {
        
    }
}
