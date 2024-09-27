using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerWinGame : MonoBehaviour
{
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject blackBG;


    public void DeleyPlayerWinGame()
    {
        Invoke(nameof(PlayerWinGameEventStart),3);
    }

    private void PlayerWinGameEventStart()
    {
        gameObject.GetComponent<UIScoreBoard>().UpdateScoreBoard();
        winText.SetActive(true);
        blackBG.SetActive(true);
    }
}
