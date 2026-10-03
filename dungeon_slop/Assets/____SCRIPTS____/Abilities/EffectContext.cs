using System.Collections.Generic;

public class EffectContext
{
    public EnemyController sourceEnemy;
    public EnemyController enemyTargeted;
    public List<EnemyController> enemiesOnBoard;

    public CharacterController sourceCharacter;
    public CharacterController characterTargeted;
    public List<CharacterController> alliesOnBoard;

    public ManaPoolController poolController;
}