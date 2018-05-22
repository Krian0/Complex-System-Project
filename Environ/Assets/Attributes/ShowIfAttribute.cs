using UnityEngine;

public class ShowIfAttribute : PropertyAttribute
{
    public string variableName;
    public int compareValue;
    public Compare comparison;

    public enum Compare
    {
        EQUALS = 0,
        NOT_EQUALS
    }

    public ShowIfAttribute(string varName, Compare comp, int value)
    {
        variableName = varName;
        compareValue = value;
        comparison = comp;
    }
}