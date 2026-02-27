using UnityEngine;

public enum EPlayerAttackType
{
    Attack1,
    Attack2, 
    Attack3, 

    Count
}

public enum EAttackMode
{
    Sequential,
    Random,
}

public enum EGameState
{
    Ready,
    Game,
    Dead,
}

public enum EDieType
{
    InstantRespawn,
    DelayedRespawn,
    GameOver,
}

public enum EMonsterState
{
    Idle,
    Sleep,
    Patrol,
    Trace,
    Attack,
    Damaged,
    Die,
}
