using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoTwin : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 posTween;
    public GameObject refPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        posTween = refPos.transform.position;
        transform.position = posTween;
        BounceEffect();
    }

    // Update is called once per frame
    public void BounceEffect()
    {
        transform.DOMoveY(startPos.y, 0.8f).OnComplete(()=>{

            transform.DOShakePosition(5.0f, strength: new Vector3(0, 1, 0), vibrato: 5, randomness: 1, snapping: false, fadeOut: true);
        });
    }
}
