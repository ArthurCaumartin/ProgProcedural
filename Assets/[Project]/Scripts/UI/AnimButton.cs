using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
public class AnimButton : MonoBehaviour
{
    public GameObject refJump;
    private void Start() 
    {
        transform.DOJump(new Vector3(refJump.transform.position.x, refJump.transform.position.y, refJump.transform.position.z), 5, 3, 1f);
    }
}
