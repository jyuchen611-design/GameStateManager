using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Start,
    Prepare,
    Playing,
    Over,
}

public class GameStateManager : MonoBehaviour
{
    //单例类
    public static GameStateManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 状态改变事件
    /// </summary>
    public event Action<GameState,GameState> OnStateChanged;

    //所有可以的转换路径
    private readonly Dictionary<GameState, GameState[]> allAllowedTransition = new Dictionary<GameState, GameState[]> 
    {
        //比如:Start只能转成Prepare
        {GameState.Start , new[]{GameState.Prepare} },
        {GameState.Prepare , new[]{GameState.Playing}  },
        {GameState.Playing , new[]{GameState.Over}  },
    };

    //外部只能读,不能改
    [SerializeField]
    private GameState currentGameState = GameState.Start;
    public GameState CurrentGameState {  get { return currentGameState; } }

    //给外部提供改变游戏状态的方法,并触发状态改变事件
    public void ChangeState(GameState newState)
    {
        if (!IsTransitionAllowed(currentGameState,newState)) { return; }

        GameState originGamestate = currentGameState;
        currentGameState = newState;

        //触发状态改变事件
        OnStateChanged.Invoke(originGamestate, newState);
    }

    //判断转换是否合理
    private bool IsTransitionAllowed(GameState currentGameState, GameState newState)
    {
        GameState[] allowedTransition = allAllowedTransition[currentGameState];

        //IndexOf方法:返回newState在allowedTransition数组的下标位置,若没有则返回-1
        return Array.IndexOf(allowedTransition, newState) >= 0 ;
    }
}
