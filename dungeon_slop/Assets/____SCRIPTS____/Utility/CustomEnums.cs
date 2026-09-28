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
public enum TurnState
{
    state_none = -1, 
    playerTurn,
    enemyTurn,
}
public enum ManaType
{
    NeutralMana,
    YellowMana,
    BlueMana,
    RedMana,
}