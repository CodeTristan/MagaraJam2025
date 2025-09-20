#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DialogSystemEditorHelpers.DrawerHelpers;

[CustomPropertyDrawer(typeof(ChoiceBody))]
public class ChoiceBodyDrawer : PropertyDrawer
{
    private readonly Dictionary<string, ReorderableList> _lists = new();

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var textSP = prop.FindPropertyRelative("DialogText");
        var loopSP = prop.FindPropertyRelative("isLoop");
        var choices = prop.FindPropertyRelative("choices");

        if (!Foldout(Line(pos, 0), prop, textSP.stringValue)) return;
        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(Line(pos, 1), textSP);
        EditorGUI.PropertyField(Line(pos, 2), loopSP);

        // Reorderable choices
        var rl = GetList(choices);
        Rect listRect = new(pos.x, pos.y + 3 * (h + v), pos.width, rl.GetHeight());
        rl.DoList(listRect);

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (!prop.isExpanded) return h;
        var rl = GetList(prop.FindPropertyRelative("choices"));
        return 3 * (h + v) + rl.GetHeight();
    }

    ReorderableList GetList(SerializedProperty sp)
    {
        if (_lists.TryGetValue(sp.propertyPath, out var rl)) return rl;

        rl = new ReorderableList(sp.serializedObject, sp, true, true, true, true);

        rl.drawHeaderCallback = r => EditorGUI.LabelField(r, "Choices");
        rl.elementHeight = h + 2;

        rl.drawElementCallback = (r, i, a, f) =>
        {
            var el = sp.GetArrayElementAtIndex(i);
            string txt = el.FindPropertyRelative("choiceText").stringValue;
            EditorGUI.LabelField(r, $"{i}. {txt}");
        };

        _lists[sp.propertyPath] = rl;
        return rl;
    }
}
#endif
