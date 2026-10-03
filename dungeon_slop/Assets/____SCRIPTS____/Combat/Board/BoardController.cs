using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardController : MonoSingleton<BoardController>
{
    public BoardInfo boardInfo { get; private set; }
    public BoardVisuals boardVisuals { get; private set; }

    public override void Awake()
    {
        base.Awake();
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        boardInfo = GetComponent<BoardInfo>();
        if (boardInfo != null) boardInfo.Initialize();
        boardVisuals = GetComponent<BoardVisuals>();
        if (boardVisuals != null) boardVisuals.Initialize();
    }

    public CharacterController GetCharacter()
    {
        return boardInfo.charactersInCombat[0];
    }

    public IEnumerator GenerateManaTurn()
    {
        if(boardInfo.charactersInCombat.Count <= 0 || boardInfo.charactersInCombat == null)
        {
            Debug.LogWarning("no se han encontrado personajes en boardinfo.charactersInCombat");
            yield break;
        }
        foreach (var character in boardInfo.charactersInCombat)
        {
            foreach(var mana in character._info._manaGenerationPerTurn)
            {
                ManaPoolController.Instance.GenerateMana(mana.manaType,character._info.chartacterPosition.position,mana.manaQuantity);
            }
            yield return new WaitForSeconds(boardVisuals.manaGenerationDelay);
        }
    }

    public void RemoveEnemyFromCombat(EnemyController enemy)
    {
        if (boardInfo.enemiesInCombat.Contains(enemy))
        {
            boardInfo.enemiesInCombat.Remove(enemy);
        }
        CombatController.Instance.CheckCombatEnd(true);
    }

    internal void AddEnemyToCombat(EnemyController enemyController)
    {
        boardInfo.enemiesInCombat.Add(enemyController);
    }

    public List<EnemyController> GetEnemiesInCombat()
    {
        return boardInfo.enemiesInCombat;
    }


    public void RemoveCharacterFromCombat(CharacterController characterController)
    {
        if (boardInfo.charactersInCombat.Contains(characterController))
        {
            boardInfo.charactersInCombat.Remove(characterController);
        }
        CombatController.Instance.CheckCombatEnd(false);
    }
    public void AddCharacterToCombat(CharacterController characterController)
    {
        boardInfo.AddCharacter(characterController);
    }
    public List<CharacterController> GetCharactersInCombat()
    {
        return boardInfo.charactersInCombat;
    }
}
