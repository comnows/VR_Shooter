using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangingEffect : MonoBehaviour
{
    [SerializeField] private GameObject blackBG;
    
    public void ActiveBlackBG()
    {
        blackBG.SetActive(true);
        StartCoroutine(DeActivateBlackBG());
    } 

    IEnumerator DeActivateBlackBG()
    {
        yield return new WaitForSeconds(7);
        blackBG.SetActive(false);
    }
}
