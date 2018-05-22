using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (IsShowing(property))
        {
            return EditorGUIUtility.singleLineHeight;
        }
        else
        {
            return 0;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsShowing(property))
            EditorGUI.PropertyField(position, property, true);
    }

    bool IsShowing(SerializedProperty property)
    {
        ShowIfAttribute sia = attribute as ShowIfAttribute;
        SerializedProperty prop = property.serializedObject.FindProperty(sia.variableName);
        bool show = true;
        if (prop != null)
        {
            if (prop.propertyType == SerializedPropertyType.Enum)
            {
                if (sia.comparison == ShowIfAttribute.Compare.EQUALS)
                    show = (prop.enumValueIndex == sia.compareValue);
                 if (sia.comparison == ShowIfAttribute.Compare.NOT_EQUALS)
                    show = (prop.enumValueIndex != sia.compareValue);
            }
        }
        return show;
    }
}