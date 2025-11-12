using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SpawnData))]
public class SpawnDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool isBoss = property.FindPropertyRelative("isBoss").boolValue;
        float height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("spawnTime"));
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("spriteType"));
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("health"));
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("speed"));
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("isBoss"));
        if (isBoss)
            height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("bossPattern"));
        return height + 10;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        var spawnTime = property.FindPropertyRelative("spawnTime");
        var spriteType = property.FindPropertyRelative("spriteType");
        var health = property.FindPropertyRelative("health");
        var speed = property.FindPropertyRelative("speed");
        var isBoss = property.FindPropertyRelative("isBoss");
        var bossPattern = property.FindPropertyRelative("bossPattern");

        Rect r = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(r, spawnTime);
        r.y += EditorGUIUtility.singleLineHeight + 2;
        EditorGUI.PropertyField(r, spriteType);
        r.y += EditorGUIUtility.singleLineHeight + 2;
        EditorGUI.PropertyField(r, health);
        r.y += EditorGUIUtility.singleLineHeight + 2;
        EditorGUI.PropertyField(r, speed);
        r.y += EditorGUIUtility.singleLineHeight + 2;
        EditorGUI.PropertyField(r, isBoss);
        r.y += EditorGUIUtility.singleLineHeight + 2;

        if (isBoss.boolValue)
            EditorGUI.PropertyField(r, bossPattern);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
#endif
