using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public CombatData nextCombat;
    public List<CharacterData> playerParty;
    public override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void EnterCombat(CombatData combatData)
    {
        nextCombat = combatData;
        StartCoroutine(Scene_Controller.Instance.ChangeScene(2, 0.5f));
    }
}
