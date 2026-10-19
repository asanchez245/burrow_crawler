using DG.Tweening;
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
    public TextMeshProUGUI infoCost;

    [Header("_combatInfoAbilities Info")]
    [SerializeField] Image abilitie01;
    [SerializeField] Image abilitie02;
    [SerializeField] Image abilitie03;
    [SerializeField] Image abilitie04;

    [Header("Combat End")]
    [SerializeField] GameObject combatDiePanel;
    [SerializeField] GameObject combatWinPanel;

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
        this.characterPortrait.sprite = chData.characterPortrait;
        this.characterHealth.text = GameManager.Instance.playerParty[0].hp.ToString();
        this.characterShield.text = chData.shield.ToString();
    }

    public void ShowDieVisuals(bool show)
    {
        if(show)
        {
            combatDiePanel.SetActive(true);
            combatDiePanel.GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() =>
            {
                combatDiePanel.SetActive(true);
            });
        }
        else
        {
            combatDiePanel.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
            {
                combatDiePanel.SetActive(false);
            });
        }
    }

    public void ShowWinVisuals(bool show)
    {
        if (show)
        {
            combatWinPanel.SetActive(true);
            combatWinPanel.GetComponent<CanvasGroup>().DOFade(1, 1f).OnComplete(() =>
            {
                combatWinPanel.SetActive(true);
            });
        }
        else
        {
            combatWinPanel.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
            {
                combatWinPanel.SetActive(false);
            });
        }
    }

    internal void UpdateHealthText(int newHealth)
    {
        characterHealth.DOKill();

        characterHealth.color = Color.red;
        characterHealth.text = newHealth.ToString();
        characterHealth.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1, 1).OnComplete(() =>
        {
            characterHealth.transform.localScale = Vector3.one;
            characterHealth.DOColor(Color.white, 0.2f);
        });
    }

    internal void UpdateShieldText(int newShield)
    {
        characterShield.DOKill();

        characterShield.color = Color.red;
        characterShield.text = newShield.ToString();
        characterShield.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 1, 1).OnComplete(() =>
        {
            characterShield.transform.localScale = Vector3.one;
            characterShield.DOColor(Color.white, 0.2f);
        });
    }
}
