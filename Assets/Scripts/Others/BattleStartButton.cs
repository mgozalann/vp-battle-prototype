using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartButton : MonoBehaviour
{
    
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnBattleButtonPressed);
    }

    private void OnDisable()
    {
         _button.onClick.RemoveListener(OnBattleButtonPressed);
    }
    private void OnBattleButtonPressed()
    {
        GameManager.I.SetState(GameState.InBattle);
        
        _button.gameObject.SetActive(false);
    }
}