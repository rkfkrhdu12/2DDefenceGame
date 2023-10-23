
#if UNITY_EDITOR
using UnityEngine;

using UnityEditor;


/// <summary>
/// GameObject's Active가 false가 되지 않도록 하십시오. string - Update시킬 함수
/// </summary>
public class RealTimeUpdateAttribute : PropertyAttribute
{
    public readonly string _functionName;
    public RealTimeUpdateAttribute(string functionName)
    {
        _functionName = functionName;
    }
}

[CustomPropertyDrawer(typeof(RealTimeUpdateAttribute))]
public class RealTimeUpdateAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        if (EditorApplication.isPlaying)
        {
            var targetClass = (UIInputComponent)property.serializedObject.targetObject;
            if (targetClass == null) return;

            if (attribute is not RealTimeUpdateAttribute attr) return;

            targetClass.SendMessage(attr?._functionName);
        }

    }
}

// EnumFlags
public class EnumFlagsAttribute : PropertyAttribute
{
}

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}

// ReadOnly
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

#endif