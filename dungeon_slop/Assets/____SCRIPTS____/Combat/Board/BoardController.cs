using System.Collections;
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
        return boardInfo.characters[0];
    }

    public IEnumerator GenerateManaTurn()
    {
        if(boardInfo.characters.Count <= 0 || boardInfo.characters == null)
        {
            Debug.LogWarning("no se han encontrado personajes en boardinfo.characters");
            yield break;
        }
        foreach (var character in boardInfo.characters)
        {
            foreach(var mana in character._info._manaGenerationPerTurn)
            {
                ManaPoolController.Instance.GenerateMana(mana.manaType,character._info.chartacterPosition.position,mana.manaQuantity);
            }
            yield return new WaitForSeconds(boardVisuals.manaGenerationDelay);
        }
    }
}
