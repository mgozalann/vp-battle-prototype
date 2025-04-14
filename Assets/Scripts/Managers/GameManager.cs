using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    
    public event Action<GameState> OnGameStateChanged;
    
    public GameState CurrentState { get; private set; }

    private void Awake() => I = this;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void SetState(GameState state)
    { 
        CurrentState = state;
        
        OnGameStateChanged?.Invoke(state);
    }
}

[Serializable]
public enum GameState
{
    Preparation,
    InBattle,
    Victory,
    Defeat
}