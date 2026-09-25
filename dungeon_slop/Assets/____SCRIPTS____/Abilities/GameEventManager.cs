using System;
using System.Collections.Generic;

public static class GameEventManager
{
    private static Dictionary<GameEventType, Action<GameEvent>> listeners =
        new Dictionary<GameEventType, Action<GameEvent>>();

    public static void Subscribe(GameEventType evt, Action<GameEvent> callback)
    {
        if (!listeners.ContainsKey(evt))
            listeners[evt] = delegate { };
        listeners[evt] += callback;
    }

    public static void Unsubscribe(GameEventType evt, Action<GameEvent> callback)
    {
        if (listeners.ContainsKey(evt))
            listeners[evt] -= callback;
    }

    public static void Raise(GameEvent evt)
    {
        if (listeners.ContainsKey(evt.eventType))
            listeners[evt.eventType].Invoke(evt);
    }
}
