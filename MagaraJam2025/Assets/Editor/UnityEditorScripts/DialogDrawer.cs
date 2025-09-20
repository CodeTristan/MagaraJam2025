#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static DialogSystemEditorHelpers.DrawerHelpers;

[CustomPropertyDrawer(typeof(Dialog))]
public class DialogDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var nameSP = prop.FindPropertyRelative("name");
        var exprSP = prop.FindPropertyRelative("Expression");
        var animSP = prop.FindPropertyRelative("Animation");
        var voiceSP = prop.FindPropertyRelative("voiceOverName");
        var sentSP = prop.FindPropertyRelative("sentences");

        string summary = nameSP.stringValue;

        if (!Foldout(Line(pos, 0), prop, summary)) return;
        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(Line(pos, 1), nameSP);
        EditorGUI.PropertyField(Line(pos, 2), exprSP);
        EditorGUI.PropertyField(Line(pos, 3), animSP);
        EditorGUI.PropertyField(Line(pos, 4), voiceSP);
        EditorGUI.PropertyField(Line(pos, 5), sentSP, true);

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (!prop.isExpanded) return h;
        return 6 * (h + v) + EditorGUI.GetPropertyHeight(
                 prop.FindPropertyRelative("sentences"), true);
    }
}
#endif
