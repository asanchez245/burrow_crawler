using System;

[Serializable]
public enum AbilityTiers 
{ 
    Common, 
    Rare, 
    Epic 
}

public enum PlayType
{
    Attack,
    Defense,
    Heal,
    Utility
}
public enum AbilityTag
{
    Neutral = 0,
    Esoteric = 1,
    GunPowder = 2,
    Steel = 3,
    Heal = 4,
}

public enum TurnState
{
    state_none = -1, 
    playerTurn,
    enemyTurn,
}
public enum SpeedCheckType
{
    Faster,
    Equal,
    Slower
}

public enum EntityAnims
{
    Idle,
    Spawn,
    Attack,
    Hurt,
    Death
}
public enum EntityState 
{
    neutral,
    aggressive,
    defensive,
    supportive
}

public enum PlayerState
{
    none,
    combat,
    map,
}