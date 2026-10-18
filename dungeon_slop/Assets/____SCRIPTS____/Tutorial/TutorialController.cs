using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoSingleton<TutorialController>
{
    [SerializeField] private TutorialData tutorial;

    private List<TutorialPhase> currentPhases;

    private int currentPhaseIndex;
    private TutorialPhase currentPhase;

    [System.Serializable]
    public class TutorialData
    {
        public List<TutorialPhase> phases;
    }

    public void SetTutorial()
    {
        if (currentPhases != null)
        {
            ClearPhaseSubscriptions(currentPhases);
        }

        currentPhases = tutorial.phases;

        InitializePhases();
        StartTutorial();
    }

    private void InitializePhases()
    {
        for (int i = 0; i < currentPhases.Count; i++)
        {
            TutorialPhase phase = currentPhases[i];

            phase.Initialize();
            phase.OnCompleted += OnPhaseCompleted;

            TutorialPhase nextPhase =
                i + 1 < currentPhases.Count
                    ? currentPhases[i + 1]
                    : null;

            phase.SetNextPhase(nextPhase);
        }
    }

    private void StartTutorial()
    {
        currentPhaseIndex = 0;

        StartCurrentPhase();
    }

    private void StartCurrentPhase()
    {
        if (currentPhaseIndex >= currentPhases.Count)
        {
            FinishTutorial();
            return;
        }

        currentPhase = currentPhases[currentPhaseIndex];

        currentPhase.StartPhase();
    }

    private void OnPhaseCompleted(TutorialPhase phase)
    {
        if (phase != currentPhase)
            return;

        phase.EndPhase();

        currentPhaseIndex++;

        StartCurrentPhase();
    }

    private void FinishTutorial()
    {
        currentPhase = null;
        currentPhases = null;

        Debug.Log("Tutorial terminado");
    }

    private void ClearPhaseSubscriptions(List<TutorialPhase> phases)
    {
        foreach (TutorialPhase phase in phases)
        {
            phase.OnCompleted -= OnPhaseCompleted;
        }
    }
}