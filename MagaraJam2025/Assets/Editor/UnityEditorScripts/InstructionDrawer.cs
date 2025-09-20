#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using static DialogSystemEditorHelpers.DrawerHelpers;

//// ---------------- ConditionInstruction -----------------
//[CustomPropertyDrawer(typeof(ConditionInstruction))]
//public class ConditionInstructionDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        var condSP = prop.FindPropertyRelative("condition");
//        var opSP = prop.FindPropertyRelative("operation");
//        var opdSP = prop.FindPropertyRelative("operand");
//        var varSP = prop.FindPropertyRelative("operand_VariableName");
//        var rStart = prop.FindPropertyRelative("randomValueStart");
//        var rEnd = prop.FindPropertyRelative("randomValueEnd");

//        if (!Foldout(Line(pos, 0), prop, condSP.FindPropertyRelative("conditionName").stringValue)) return;
//        EditorGUI.indentLevel++;

//        EditorGUI.PropertyField(Line(pos, 1), condSP, true);
//        EditorGUI.PropertyField(Line(pos, 2), opSP);
//        EditorGUI.PropertyField(Line(pos, 3), opdSP);
//        EditorGUI.PropertyField(Line(pos, 4), varSP);
//        EditorGUI.PropertyField(Line(pos, 5), rStart);
//        EditorGUI.PropertyField(Line(pos, 6), rEnd);

//        EditorGUI.indentLevel--;
//    }

//    public override float GetPropertyHeight(SerializedProperty p, GUIContent l)
//        => p.isExpanded ? 7 * (h + v) +
//            EditorGUI.GetPropertyHeight(p.FindPropertyRelative("condition"), true) : h;
//}

//// ---------------- WaitInstruction -----------------
//[CustomPropertyDrawer(typeof(WaitInstruction))]
//public class WaitInstructionDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        var wt = prop.FindPropertyRelative("waitTime");
//        label.text += $"  ({wt.floatValue:F1}s)";
//        EditorGUI.PropertyField(pos, wt, label);
//    }
//    public override float GetPropertyHeight(SerializedProperty p, GUIContent l) => h;
//}

//// ---------------- ConditionalBranchInstruction -----------------
//[CustomPropertyDrawer(typeof(ConditionalBranchInstruction))]
//public class ConditionalBranchDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        var txtSP = prop.FindPropertyRelative("conditionText");
//        if (!Foldout(Line(pos, 0), prop, txtSP.stringValue)) return;
//        EditorGUI.indentLevel++;
//        EditorGUI.PropertyField(Line(pos, 1), txtSP);
//        EditorGUI.PropertyField(Line(pos, 2), prop.FindPropertyRelative("ElseBranchCreated"));
//        EditorGUI.PropertyField(Line(pos, 3), prop.FindPropertyRelative("conditions"), true);
//        EditorGUI.PropertyField(Line(pos, 4), prop.FindPropertyRelative("logics"), true);
//        EditorGUI.PropertyField(Line(pos, 5), prop.FindPropertyRelative("branch"), true);
//        EditorGUI.PropertyField(Line(pos, 6), prop.FindPropertyRelative("elseBranch"), true);
//        EditorGUI.indentLevel--;
//    }
//    public override float GetPropertyHeight(SerializedProperty p, GUIContent l)
//    {
//        if (!p.isExpanded) return h;
//        float add = EditorGUI.GetPropertyHeight(p.FindPropertyRelative("conditions"), true)
//                   + EditorGUI.GetPropertyHeight(p.FindPropertyRelative("logics"), true)
//                   + EditorGUI.GetPropertyHeight(p.FindPropertyRelative("branch"), true)
//                   + EditorGUI.GetPropertyHeight(p.FindPropertyRelative("elseBranch"), true);
//        return 7 * (h + v) + add;
//    }
//}

//// ---------------- JumpBranchInstruction -----------------
//[CustomPropertyDrawer(typeof(JumpBranchInstruction))]
//public class JumpBranchDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        var file = prop.FindPropertyRelative("FileName");
//        label.text += $" -> {file.stringValue}";
//        if (!Foldout(Line(pos, 0), prop, file.stringValue)) return;
//        EditorGUI.indentLevel++;
//        EditorGUI.PropertyField(Line(pos, 1), file);
//        EditorGUI.PropertyField(Line(pos, 2), prop.FindPropertyRelative("BranchName"));
//        EditorGUI.PropertyField(Line(pos, 3), prop.FindPropertyRelative("Return"));
//        EditorGUI.indentLevel--;
//    }
//    public override float GetPropertyHeight(SerializedProperty p, GUIContent l)
//        => p.isExpanded ? 4 * (h + v) : h;
//}

//// ---------------- MissionInstruction -----------------
//[CustomPropertyDrawer(typeof(MissionInstruction))]
//public class MissionInstructionDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        var mn = prop.FindPropertyRelative("MissionName");
//        if (!Foldout(Line(pos, 0), prop, mn.stringValue)) return;
//        EditorGUI.indentLevel++;
//        EditorGUI.PropertyField(Line(pos, 1), mn);
//        EditorGUI.PropertyField(Line(pos, 2), prop.FindPropertyRelative("missionInstructionType"));
//        EditorGUI.PropertyField(Line(pos, 3), prop.FindPropertyRelative("ObjectiveName"));
//        EditorGUI.indentLevel--;
//    }
//    public override float GetPropertyHeight(SerializedProperty p, GUIContent l)
//        => p.isExpanded ? 4 * (h + v) : h;
//}

//// ---------------- BackgroundInstruction -----------------
//[CustomPropertyDrawer(typeof(BackgroundInstruction))]
//public class BackgroundInstructionDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//    {
//        var chk = prop.FindPropertyRelative("checkDialogs");
//        label.text += chk.boolValue ? " (wait all dialogs)" : "";
//        EditorGUI.PropertyField(pos, chk, label);
//    }
//    public override float GetPropertyHeight(SerializedProperty p, GUIContent l) => h;
//}

[CustomPropertyDrawer(typeof(Miscellaneous), true)]
public class MiscInstructionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        // ---------- 1) Özet baþlýk  ----------
        string summary = GetSummary(prop);
        prop.isExpanded = EditorGUI.Foldout(Line(pos, 0), prop.isExpanded, summary, true);
        if (!prop.isExpanded) return;

        // ---------- 2) Tüm alt alanlarý otomatik çiz ----------
        EditorGUI.indentLevel++;
        int line = 1;

        SerializedProperty iterator = prop.Copy();
        SerializedProperty end = iterator.GetEndProperty();

        // .NextVisible(true) ? ilk child
        if (iterator.NextVisible(true))
        {
            do
            {
                // Skip Unity internals
                if (iterator.name == "m_Script" ||
                    iterator.propertyPath.EndsWith("managedReferenceFullTypename"))
                    continue;

                float hProp = EditorGUI.GetPropertyHeight(iterator, true);
                EditorGUI.PropertyField(
                    new Rect(pos.x, pos.y + line * (h + v), pos.width, hProp),
                    iterator, true);

                line += Mathf.CeilToInt(hProp / (h + v));
            }
            while (iterator.NextVisible(false) && !SerializedProperty.EqualContents(iterator, end));
        }
        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (!prop.isExpanded) return h;

        float height = h + v;                   // fold-out satýrý
        SerializedProperty it = prop.Copy();
        SerializedProperty end = it.GetEndProperty();

        if (it.NextVisible(true))
        {
            do
            {
                if (it.name == "m_Script" || it.propertyPath.EndsWith("managedReferenceFullTypename"))
                    continue;

                height += EditorGUI.GetPropertyHeight(it, true) + v;
            }
            while (it.NextVisible(false) && !SerializedProperty.EqualContents(it, end));
        }
        return height;
    }

    // --------------------------------------------------
    /// <summary>Inspector’da görünen “tipik” özet metni üretir.</summary>
    private static string GetSummary(SerializedProperty prop)
    {
        var nameSP = prop.FindPropertyRelative("name");
        string s = string.IsNullOrEmpty(nameSP?.stringValue) ? "<unnamed>" : nameSP.stringValue;

        // Alt sýnýfa özel “preview” eklemek için istediðin alaný keþfet:
        if (prop.FindPropertyRelative("checkDialogs") != null)
        {
            bool wait = prop.FindPropertyRelative("checkDialogs").boolValue;
            s += wait ? "  (wait-all dialogs)" : "";
        }
        else if (prop.FindPropertyRelative("waitTime") != null)
        {
            float t = prop.FindPropertyRelative("waitTime").floatValue;
            s += $"  ({t:0.#} s)";
        }
        else if (prop.FindPropertyRelative("MissionName") != null)
        {
            s += $"  [{prop.FindPropertyRelative("MissionName").stringValue}]";
        }
        else if (prop.FindPropertyRelative("FileName") != null)
        {
            s += $"  [{prop.FindPropertyRelative("FileName").stringValue}]";
        }

        return s;
    }
}

#endif
