#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static DialogSystemEditorHelpers.DrawerHelpers;

[CustomPropertyDrawer(typeof(Choice))]
public class ChoiceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var txtSP   = prop.FindPropertyRelative("choiceText");
        var visSP   = prop.FindPropertyRelative("isVisible");
        var selSP   = prop.FindPropertyRelative("isSelectable");
        var actSP   = prop.FindPropertyRelative("choicesToActivate");
        var brSP    = prop.FindPropertyRelative("branch");

        if (!Foldout(Line(pos, 0), prop, txtSP.stringValue)) return;
        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(Line(pos, 1), txtSP);
        EditorGUI.PropertyField(Line(pos, 2), visSP);
        EditorGUI.PropertyField(Line(pos, 3), selSP);
        EditorGUI.PropertyField(Line(pos, 4), actSP, true);
        EditorGUI.PropertyField(Line(pos, 5), brSP, true);

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (!prop.isExpanded) return h;
        float actH = EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("choicesToActivate"), true);
        float brH  = EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("branch"), true);
        return 6 * (h + v) + actH + brH;
    }
}
#endif
