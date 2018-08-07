using System;
using UnityEngine;

public class ColorButtonTriggerEventArgs : EventArgs
{
    public Color Color { get; private set; }

    public ColorButtonTriggerEventArgs(Color color)
    {
        Color = color;
    }
}
