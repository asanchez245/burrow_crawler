using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyInfo _info { get; private set; }
    EnemyVisuals _visuals;

    public EnemyData enemyData;

    private void Awake()
    {
        InitializeCharacter();
    }

    public void InitializeCharacter()
    {
        _info = GetComponent<EnemyInfo>();
        if (_info != null) _info.InitializeEnemyInfo(enemyData);
        _visuals = GetComponent<EnemyVisuals>();
        if (_visuals != null) _visuals.InitializeEnemyVisuals(enemyData);
    }

    #region Character Stats Management
    public void ModifyCharacterHealth(int amount, bool isFixed)
    {
        int healthAmount;
        if (!isFixed)
        {
            healthAmount = _info.GetCurrentHealth() - amount;
        }
        else
        {
            healthAmount = amount;
        }
        _info.SetCurrentHealth(healthAmount);
    }
    public void SetMaxHealth(int amount) { _info.SetMaxHealth(amount); }

    public void ModifyCharacterShield(int amount, bool isFixed)
    {
        int shieldAmount;
        if (!isFixed)
        {
            shieldAmount = _info.GetCurrentShield() - amount;
        }
        else
        {
            shieldAmount = amount;
        }
        _info.SetCurrentShield(shieldAmount);
    }
    public void SetMaxShield(int amount) { _info.SetMaxShield(amount); }



    #endregion
}
