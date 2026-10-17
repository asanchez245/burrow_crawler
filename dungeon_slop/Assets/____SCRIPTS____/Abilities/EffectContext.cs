using System.Collections.Generic;

public class EffectContext
{
    public EnemyController sourceEnemy;
    public EnemyController enemyTargeted;
    public List<EnemyController> enemiesOnBoard;

    public Character_Controller sourceCharacter;
    public Character_Controller characterTargeted;
    public List<Character_Controller> alliesOnBoard;

    public ManaPoolController poolController;
}