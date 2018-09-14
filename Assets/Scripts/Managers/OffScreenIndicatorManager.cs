using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenIndicatorManager : MonoBehaviour {

    public float timeSplicingQuoteForUpdate = 0.001f;
    public GameObject arrowPrefab;
    public Camera main;
    public Transform parentOfArrows;

    List<Tuple< IOffScreen, GameObject>> _allOffScreen = new List<Tuple<IOffScreen, GameObject>>();

    Pool<GameObject> _poolArrows;

    public static OffScreenIndicatorManager instance { get; private set; }

    void Awake() {
        _poolArrows = new Pool<GameObject>(15, ArrowFactoryMethod, null, null, true);
    }

    void Start () {
        Debug.Assert(FindObjectsOfType<OffScreenIndicatorManager>().Length == 1);
        if (instance == null)
            instance = this;

        StartCoroutine(IndicatorUpdate());
    } 

    IEnumerator IndicatorUpdate() {
        var wait = new WaitForEndOfFrame();
        var start = Time.realtimeSinceStartup;
        while (true) { 

            for (int i = 0; i < _allOffScreen.Count; i++) { 
                var tuple = _allOffScreen[i];
                if (tuple == null)
                    continue;

                Vector3 screenPos = main.WorldToScreenPoint(tuple.Item1.GetPosition);

                if (screenPos.z > 0
                    && screenPos.x > 0 && screenPos.x < Screen.width
                    && screenPos.y > 0 && screenPos.y < Screen.height) { 
                    tuple.Item2.SetActive(false);
                    continue;
                }
                else { 
                    if (screenPos.z < 0)
                        screenPos *= -1;

                    Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0f) / 2;
                    screenPos -= screenCenter;
                    float angle = Mathf.Atan2(screenPos.y, screenPos.x);
                    angle -= 90 * Mathf.Deg2Rad;
                    float cos = Mathf.Cos(angle);
                    float sin = -Mathf.Sin(angle);

                    screenPos = screenCenter + new Vector3(sin * 150, cos * 150, 0f);

                    float m = cos / sin;
                    Vector3 screenBounds = screenCenter * 0.9f;

                    if (cos > 0) {
                        screenPos = new Vector3(screenBounds.y / m, screenBounds.y, 0f);
                    }
                    else {
                        screenPos = new Vector3(-screenBounds.y / m, -screenBounds.y, 0);
                    }

                    if (screenPos.x > screenBounds.x) {
                        screenPos = new Vector3(screenBounds.x, screenBounds.x * m, 0);
                    }
                    else if (screenPos.x < -screenBounds.x) {
                        screenPos = new Vector3(-screenBounds.x, -screenBounds.x * m, 0);
                    }

                    screenPos += screenCenter;

                    tuple.Item2.SetActive(true);
                    tuple.Item2.transform.position = screenPos;
                    tuple.Item2.transform.rotation = Quaternion.Euler(0f,0f,angle* Mathf.Rad2Deg + 180);
                }

                if (Time.realtimeSinceStartup - start > timeSplicingQuoteForUpdate) {
                    yield return wait;
                    start = Time.realtimeSinceStartup;
                } 
            }

            yield return null; 
        }
    }

    public void SubscribeIOffScreen(IOffScreen elem) {
        _allOffScreen.Add(Tuple.Create(elem, GiveMeArrow()));
    }

    public void UnsubscribeIOffScreen(IOffScreen elem) { 
        foreach (var tuple in _allOffScreen) { 
            if(tuple.Item1 == elem) { 
                _allOffScreen.Remove(tuple);
                ReturnArrowToPool(tuple.Item2);
                tuple.Item2.SetActive(false); 
                break;
            }
        }
    }

    public void CleanIndicators() {
        foreach (var tuple in _allOffScreen) { 
            ReturnArrowToPool(tuple.Item2);
            tuple.Item2.SetActive(false);
        }
        _allOffScreen.Clear();
    }

    #region POOL METHODS
    GameObject ArrowFactoryMethod() {
        var a = Instantiate(arrowPrefab, parentOfArrows);
        a.gameObject.SetActive(false);
        return a;
    }

    GameObject GiveMeArrow() {
        return _poolArrows.GetObjectFromPool();
    }

    void ReturnArrowToPool(GameObject arrow) {
        _poolArrows.DisablePoolObject(arrow);
    }
    #endregion
}
