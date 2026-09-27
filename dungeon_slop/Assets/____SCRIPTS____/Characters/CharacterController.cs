using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public CharacterInfo _info { get; private set; }
    CharacterVisuals _visuals;

    [SerializeField] CharacterData _characterData;

    private void Awake()
    {
        InitializeCharacter();
    }

    public void InitializeCharacter()
    {
        _info = GetComponent<CharacterInfo>();
        if (_info != null) _info.InitializeCharacterInfo(_characterData);
        _visuals = new CharacterVisuals();
        if (_visuals != null) _visuals.InitializeCharacterVisuals(_characterData);
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
