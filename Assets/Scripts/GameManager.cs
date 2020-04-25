using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public bool isGameOver = false;
    public Text timerText;
    public Text levelText;

    private void Awake()
    {
        
    }
}
