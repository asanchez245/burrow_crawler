public enum GameEventType
{
    OnTriggerAbility,
    OnEnemyAttack,
    OnEnemyHeal,
    OnEnemyDefense
    //OnDraw,
    //OnTurnStart,
    //OnLockBoard,
    //OnResolve,
    //OnAttack,
    //OnReveal,
    //OnHit,
    //OnDeath
}

public class GameEvent
{
    public GameEventType eventType;
    public EffectContext ctxt;

    public GameEvent(GameEventType type, EffectContext ctxt)
    {
        eventType = type;
        this.ctxt = ctxt;
    }
}
