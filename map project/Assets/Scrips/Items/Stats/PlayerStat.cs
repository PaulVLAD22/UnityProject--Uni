using System;
using System.Collections.Generic;

[Serializable]
public class PlayerStat 
{

    public float BaseValue;

    public float Value
    {
        get
        {
            if (!isCalculated)
            {
                isCalculated = true;
                _value = CalculateFinalValue();
            }
            return _value;
        }
    }
    private bool isCalculated = false;
    private float _value;

    private readonly List<StatModifier> statModifiers;


    public PlayerStat(float baseValue)
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
    }

    public void AddModifier(StatModifier mod)
    {
        isCalculated = false;
        statModifiers.Add(mod);
    }

    public void RemoveModifier(StatModifier mod)
    {
        statModifiers.Remove(mod);
        isCalculated = false;
    }

    private float CalculateFinalValue()
    {
        if(isCalculated)
            statModifiers.Sort((x, y) => {

                return x.Type - y.Type;
                } );
        float finalValue = BaseValue;

       foreach(var statMod in statModifiers)
        {
            switch (statMod.Type)
            {
                case StatModType.Flat:
                    finalValue += statMod.Value;
                    break;
                case StatModType.Percent:
                    finalValue *= 1 + statMod.Value / 100;
                    break;
                default:
                    break;
            };
        }


        return (float)Math.Round(finalValue, 4);
    }
}
