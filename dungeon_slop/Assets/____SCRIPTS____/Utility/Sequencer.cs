using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoSingleton<Sequencer>
{
    class SequenceEntry
    {
        public IEnumerator sequence;
        public Object owner;
        public bool hasOwner;

        public SequenceEntry(IEnumerator sequence, Object owner = null)
        {
            this.sequence = sequence;
            this.owner = owner;
            hasOwner = owner != null;
        }
    }

    Queue<SequenceEntry> sequences = new Queue<SequenceEntry>();
    public bool isResolving { get; private set; } = false;

    public void TriggerSequence()
    {
        if (!isResolving)
        {
            StartCoroutine(ResolveSequences());
        }
    }

    public void TriggerSequenceList(List<IEnumerator> sequencesList)
    {
        foreach (var sequence in sequencesList)
        {
            AddSequence(sequence);
        }

        TriggerSequence();
    }
    public void AddSequence(IEnumerator sequence)
    {
        if (sequence == null) return;
        sequences.Enqueue(new SequenceEntry(sequence));
    }
    public void AddSequence(IEnumerator sequence, Object owner)
    {
        if (sequence == null || owner == null) return;
        sequences.Enqueue(new SequenceEntry(sequence, owner));
    }
    public void RemoveSequence(IEnumerator sequence)
    {
        if (sequence == null)
        {
            return;
        }
        Queue<SequenceEntry> reorderedActions = new Queue<SequenceEntry>();
        while (sequences.Count > 0)
        {
            var currentSequence = sequences.Dequeue();
            if (currentSequence.sequence != sequence)
            {
                reorderedActions.Enqueue(currentSequence);
            }
        }
        sequences = reorderedActions;
    }
    public void RemoveSequencesByOwner(Object owner)
    {
        if (owner == null) return;

        Queue<SequenceEntry> reorderedActions = new Queue<SequenceEntry>();
        while (sequences.Count > 0)
        {
            var currentSequence = sequences.Dequeue();
            if (currentSequence.owner != owner)
            {
                reorderedActions.Enqueue(currentSequence);
            }
        }
        sequences = reorderedActions;
    }
    public void InsertSequence(IEnumerator sequence)
    {
        if (sequence == null)
        {
            return;
        }

        Queue<SequenceEntry> reorderedActions = new Queue<SequenceEntry>();
        reorderedActions.Enqueue(new SequenceEntry(sequence));

        while (sequences.Count > 0)
        {
            reorderedActions.Enqueue(sequences.Dequeue());
        }

        sequences = reorderedActions;

        if (!isResolving)
        {
            StartCoroutine(ResolveSequences());
        }
    }

    public IEnumerator ResolveSequences()
    {
        isResolving = true;

        while (sequences.Count > 0)
        {
            SequenceEntry sequenceEntry = sequences.Dequeue();

            if (sequenceEntry == null || sequenceEntry.sequence == null) continue;

            if (sequenceEntry.hasOwner && !sequenceEntry.owner) continue;

            yield return sequenceEntry.sequence;
        }
        isResolving = false;
    }
}