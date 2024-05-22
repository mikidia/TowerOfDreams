using UnityEditor;

[CustomEditor(typeof(MobType))]
public class MobTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var mobType = (MobType)target;

        var isRegularMobProperty = serializedObject.FindProperty("_isRegularMob");
        var isEliteMobProperty = serializedObject.FindProperty("_isEliteMob");
        var isBossMobProperty = serializedObject.FindProperty("_isBossMob");

        EditorGUILayout.PropertyField(isRegularMobProperty);
        EditorGUILayout.PropertyField(isEliteMobProperty);
        EditorGUILayout.PropertyField(isBossMobProperty);

        if (isRegularMobProperty.boolValue)
        {
            isEliteMobProperty.boolValue = false;
            isBossMobProperty.boolValue = false;
        }
        else if (isEliteMobProperty.boolValue)
        {
            isRegularMobProperty.boolValue = false;
            isBossMobProperty.boolValue = false;
        }
        else if (isBossMobProperty.boolValue)
        {
            isRegularMobProperty.boolValue = false;
            isEliteMobProperty.boolValue = false;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

