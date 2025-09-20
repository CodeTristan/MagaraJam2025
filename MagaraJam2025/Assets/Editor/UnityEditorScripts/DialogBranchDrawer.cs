#if UNITY_EDITOR
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static DialogSystemEditorHelpers.DrawerHelpers;

[CustomPropertyDrawer(typeof(DialogBranch))]
public class DialogBranchDrawer : PropertyDrawer
{
    // Runtime’da list yeniden yaratýlmasýn diye cache tutuyoruz
    private readonly Dictionary<string, ReorderableList> _lists = new();

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        var nameSP = prop.FindPropertyRelative("name");
        var idCounterSP = prop.FindPropertyRelative("IDCounter");
        var clusterSP = prop.FindPropertyRelative("ClusterName");
        var startSP = prop.FindPropertyRelative("IsStartBranch");
        var instrListSP = prop.FindPropertyRelative("instructions");

        string summary = $"{nameSP.stringValue}  ·  {instrListSP.arraySize} ins";

        if (!Foldout(Line(pos, 0), prop, summary)) return;

        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(Line(pos, 1), nameSP);
        EditorGUI.PropertyField(Line(pos, 2), idCounterSP);
        EditorGUI.PropertyField(Line(pos, 3), clusterSP);
        EditorGUI.PropertyField(Line(pos, 4), startSP);

        // --- ReorderableList ---
        var list = GetList(instrListSP);
        float listHeight = list.GetHeight();
        Rect listRect = new(pos.x, pos.y + 5 * (h + v), pos.width, listHeight);
        list.DoList(listRect);

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        if (!prop.isExpanded) return h;
        var list = GetList(prop.FindPropertyRelative("instructions"));
        return 5 * (h + v) + list.GetHeight();
    }

    // ---------------------------------------------------
    private ReorderableList GetList(SerializedProperty sp)
    {
        if (_lists.TryGetValue(sp.propertyPath, out var cached)) return cached;

        var rl = new ReorderableList(sp.serializedObject, sp, true, true, true, true);

        rl.drawElementCallback = (rect, idx, act, foc) =>
        {
            var el = sp.GetArrayElementAtIndex(idx);
            var instRef = el.managedReferenceValue as Instruction;
            if (instRef == null)
            {
                EditorGUI.LabelField(rect, "<null>");
                return;
            }

            // Fold-out at the start of the row
            el.isExpanded = EditorGUI.Foldout(
                new Rect(rect.x, rect.y, 18, EditorGUIUtility.singleLineHeight),
                el.isExpanded, GUIContent.none, false);

            // Summary label right next to the fold-out arrow
            EditorGUI.LabelField(
                new Rect(rect.x + 18, rect.y, rect.width - 18, EditorGUIUtility.singleLineHeight),
                $"{instRef.type} — {instRef.name}");

            // Draw the full property *below* the summary when expanded
            if (el.isExpanded)
            {
                var body = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + 2,
                                    rect.width, rect.height - EditorGUIUtility.singleLineHeight - 2);
                EditorGUI.PropertyField(body, el, GUIContent.none, true);
            }
        };

        rl.elementHeightCallback = i =>
        {
            var el = sp.GetArrayElementAtIndex(i);
            float baseH = EditorGUIUtility.singleLineHeight + 2;
            if (!el.isExpanded) return baseH;
            return baseH + EditorGUI.GetPropertyHeight(el, true);
        };

        rl.onAddDropdownCallback = (btnRect, l) =>
        {
            var menu = new GenericMenu();
            foreach (InstructionType t in Enum.GetValues(typeof(InstructionType)))
            {
                menu.AddItem(new GUIContent(t.ToString()), false, () =>
                {
                    int newIdx = sp.arraySize;                // 1) boþ slot aç
                    sp.InsertArrayElementAtIndex(newIdx);

                    var element = sp.GetArrayElementAtIndex(newIdx);

                    // 2) Seçilen tipe göre gerçek nesne oluþtur
                    element.managedReferenceValue = CreateInstanceOf(t);

                    // 3) Ýsteðe baðlý: baþlýk / ID vs. doldur
                    var obj = (Instruction)element.managedReferenceValue;
                    obj.name = t.ToString();
                    obj.type = t;                             // alanlar temel sýnýfta tanýmlý :contentReference[oaicite:0]{index=0}

                    sp.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        };

        sp.serializedObject.ApplyModifiedProperties();
        _lists[sp.propertyPath] = rl;
        return rl;
    }

    private static Instruction CreateInstanceOf(InstructionType type)
    {
        switch (type)
        {
            case InstructionType.Dialog: return new Dialog(0);
            case InstructionType.DialogBranch: return new DialogBranch(0);
            case InstructionType.ChoiceBody: return new ChoiceBody(0);
            case InstructionType.Background: return new BackgroundInstruction();
            case InstructionType.Music:
            case InstructionType.Minigame: return new Miscellaneous();
            case InstructionType.ConditionSwitchInstruction: return new ConditionInstruction(0);
            case InstructionType.ConditionVariableInstruction: return new ConditionInstruction(0);
            case InstructionType.WaitInstruction: return new WaitInstruction(0);
            case InstructionType.ConditionalBranchInstruction: return new ConditionalBranchInstruction(0);
            case InstructionType.JumpBranchInstruction: return new JumpBranchInstruction(0);
            case InstructionType.MissionInstruction: return new MissionInstruction(0);
            default: return new Instruction(); // güvenlik
        }
    }
}
#endif
