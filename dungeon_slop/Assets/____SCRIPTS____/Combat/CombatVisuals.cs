using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatVisuals : MonoBehaviour
{
    [Header("Character Info")]
    [SerializeField] Image characterPortrait;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI characterHealth;
    [SerializeField] TextMeshProUGUI characterShield;

    [Header("Selected Info")]
    public TextMeshProUGUI infoName;
    public TextMeshProUGUI infoDescription;

    [Header("Abilities Info")]
    [SerializeField] Image abilitie01;
    [SerializeField] Image abilitie02;
    [SerializeField] Image abilitie03;
    [SerializeField] Image abilitie04;


    internal void InitializeCombatVisuals(CombatData combatData)
    {

    }

    internal IEnumerator SetTurnVisuals(TurnState turnState)
    {
        switch (turnState)
        {
            case TurnState.playerTurn:
                return SetPlayerTurnVisuals();

            case TurnState.enemyTurn:
                return SetEnemyTurnVisuals();

            default:
                return null;
        }
    }

    IEnumerator SetPlayerTurnVisuals()
    {
        yield return null;
    }
    IEnumerator SetEnemyTurnVisuals()
    {
        yield return null;

    }

    public void SetSelectedCharacterInfo(CharacterData chData)
    {
        this.characterName.text = chData.characterName.ToString();
        this.characterPortrait.sprite = chData.characterSprite;
        this.characterHealth.text = chData.health.ToString();
        this.characterShield.text = chData.shield.ToString();
    }
}
