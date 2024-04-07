using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        PreGame,
        Active,
        GameOver
    }

    private State _state = State.Active;
    public State GameState { get { return _state; } }

    public event EventHandler OnGameOver;

    public void SetGameOver()
    {
        _state = State.GameOver;
        OnGameOver?.Invoke(this, EventArgs.Empty);
        Debug.Log("Game over");
    }
}