using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthEffect : MonoBehaviour
{
    [SerializeField] private Image splatterImage = null;
    private void Start() 
    {
        GameObject.DontDestroyOnLoad(gameObject);    
    }
    public void RefreshPlayerSplitterUI(int playerHealth)
    {
        Color splatterImageColor = splatterImage.color;
        splatterImageColor.a = 1 - (playerHealth / 100.0f);
        splatterImage.color = splatterImageColor;
    }
}
