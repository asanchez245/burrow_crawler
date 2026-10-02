using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CharacterController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterInfo _info { get; private set; }
    public CharacterVisuals _visuals { get; private set; }

    public CharacterData characterData;


    public void InitializeCharacter()
    {
        _info = GetComponent<CharacterInfo>();
        if (_info != null) _info.InitializeCharacterInfo(characterData);
        _visuals = GetComponent<CharacterVisuals>();
        if (_visuals != null) _visuals.InitializeCharacterVisuals(characterData, this);

        SelectCharacter();
    }

    #region Character Stats Management
    public void ModifyCharacterHealth(int amount, bool isFixed)
    {
        int healthAmount;
        if (!isFixed)
        {
            healthAmount = _info.GetCurrentHealth() + amount;
        }
        else
        {
            healthAmount = amount;
        }
        if (amount < 0)
        {
            _visuals.CharGetDamageVisuals();
        }
        _info.SetCurrentHealth(healthAmount);
        _visuals.UpdateHealthText(healthAmount);
    }
    public void SetMaxHealth(int amount) { _info.SetMaxHealth(amount); }
    public int GetCurrentHealth() {  return _info.GetCurrentHealth(); }

    public void ModifyCharacterShield(int amount, bool isFixed)
    {
        int shieldAmount;
        if (!isFixed)
        {
            shieldAmount = _info.GetCurrentShield() + amount;
        }
        else
        {
            shieldAmount = amount;
        }
        shieldAmount = Mathf.Max(0,shieldAmount);
        _info.SetCurrentShield(shieldAmount);
        _visuals?.UpdateShieldText(shieldAmount);
    }
    public void SetMaxShield(int amount) { _info.SetMaxShield(amount); }
    public int GetCurrentShield() {  return _info.GetCurrentShield(); }



    #endregion



    public void InitializeCharacterAbilities(CharacterData chData)
    {
        int ab = 0;
        foreach(var ability in _info._characterAbilities)
        {
            if(chData.characterAbilities[ab] != null)
            {
                ability.Initialize(chData.characterAbilities[ab]); 
                ability.gameObject.SetActive(true);

            }
            else
            {
                ability.gameObject.SetActive(false);
            }
            ab++;
        }
    }

    public void ChangePortrait(CharacterData chData)
    {
        _visuals.SetCharacterPortrait(chData);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectCharacter();
    }

    public void SelectCharacter()
    {
        CombatController.Instance.SetSelectedCharacter(this);
        InitializeCharacterAbilities(characterData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _visuals.PointerEnterCharacter(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _visuals.PointerEnterCharacter(false);
    }

    public Transform GetCharacterPos() { return _info.chartacterPosition; }
}
