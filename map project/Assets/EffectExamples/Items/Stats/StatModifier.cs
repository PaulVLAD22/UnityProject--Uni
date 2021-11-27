using System;

public enum StatModType
{
    Flat,
    Percent
}

[Serializable]
public class StatModifier
{
    public float Value;
    public StatModType Type;

    public StatModifier() { }

    public StatModifier(float value, StatModType type)
    {
        Value = value;
        Type = type;
    }
}