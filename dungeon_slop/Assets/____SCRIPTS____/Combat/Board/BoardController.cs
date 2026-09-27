using System.Collections;
using UnityEngine;

public class BoardController : MonoSingleton<BoardController>
{
    public BoardInfo boardInfo { get; private set; }
    public BoardVisuals boardVisuals { get; private set; }

    public void Initialize()
    {
        boardInfo = GetComponent<BoardInfo>();
        if (boardInfo != null) boardInfo.Initialize();
        boardVisuals = GetComponent<BoardVisuals>();
        if (boardVisuals != null) boardVisuals.Initialize();
    }

    public IEnumerator GenerateManaTurn()
    {
        foreach (var character in boardInfo.characters)
        {
            foreach(var mana in character._info._manaGenerationPerTurn)
            {
                ManaPoolController.Instance.GenerateMana(mana.manaType,character._info._chartacterPosition.position,mana.manaQuantity);
            }
            yield return new WaitForSeconds(boardVisuals.manaGenerationDelay);
        }
    }
}
