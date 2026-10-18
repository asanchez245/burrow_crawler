using System;
using UnityEngine;

public abstract class TutorialPhase : MonoBehaviour
{
    public Action<TutorialPhase> OnCompleted;

    public TutorialPhase NextPhase { get; private set; }

    public void SetNextPhase(TutorialPhase nextPhase)
    {
        NextPhase = nextPhase;
    }

    public abstract void Initialize();

    public abstract void StartPhase();

    public virtual void EndPhase()
    {
    }

    protected void CompletePhase()
    {
        OnCompleted?.Invoke(this);
    }
}