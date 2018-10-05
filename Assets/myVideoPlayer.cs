using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class myVideoPlayer : MonoBehaviour {
    public RawImage image;
    public VideoPlayer vp;
    public AudioSource audio;
	// Use this for initialization
	void Start () {
        StartCoroutine(PlayVideo());
	}

    IEnumerator PlayVideo() {
        vp.Prepare();
        WaitForSeconds wait = new WaitForSeconds(1);
        while (!vp.isPrepared) {
            yield return wait;
            break;
        }
        image.texture = vp.texture;
        vp.Play();
    }
}
