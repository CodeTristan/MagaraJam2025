// Assets/Editor/CheckConditionDrawer.cs
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CheckCondition))]
public class CheckConditionDrawer : PropertyDrawer
{
    readonly float h = EditorGUIUtility.singleLineHeight;
    readonly float v = EditorGUIUtility.standardVerticalSpacing;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        // Kisa ozet
        label.text += $" ({GetSummary(prop)})";

        // Katlanabilir baslik
        prop.isExpanded = EditorGUI.Foldout(Line(pos, 0), prop.isExpanded, label, true);
        if (!prop.isExpanded) return;
        EditorGUI.indentLevel++;

        // Alanlar
        var groupProp = prop.FindPropertyRelative("group");
        var idProp = prop.FindPropertyRelative("id");
        var status = prop.FindPropertyRelative("status");
        var checkSign = prop.FindPropertyRelative("checkSign");

        // 1) Grup dropdown
        EditorGUI.PropertyField(Line(pos, 1), groupProp);

        // 2) Gruba gore enum tipi
        ConditionNameGroup grpVal = (ConditionNameGroup)groupProp.intValue;
        Type enumType = GetEnumType(grpVal);
        if (enumType == null)
        {
            EditorGUI.HelpBox(Line(pos, 2), "Enum type not found!", MessageType.Error);
        }
        else
        {
            // Mevcut id degerini enum nesnesine cevir
            Enum current = Enum.IsDefined(enumType, idProp.intValue)
                           ? (Enum)Enum.ToObject(enumType, idProp.intValue)
                           : (Enum)Enum.GetValues(enumType).GetValue(0);

            // Enum popup
            Enum newVal = EditorGUI.EnumPopup(Line(pos, 2), new GUIContent("Condition"), current);
            idProp.intValue = Convert.ToInt32(newVal);
        }

        // 3) Diger alanlar
        EditorGUI.PropertyField(Line(pos, 3), status);
        EditorGUI.PropertyField(Line(pos, 4), checkSign);

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        => prop.isExpanded ? 5 * h + 4 * v : h;

    // --- Yardimci metodlar ---
    string GetSummary(SerializedProperty p)
    {
        var grp = (ConditionNameGroup)p.FindPropertyRelative("group").intValue;
        var id = p.FindPropertyRelative("id").intValue;
        var type = GetEnumType((ConditionNameGroup)grp);
        var st = p.FindPropertyRelative("status").intValue;
        var sign = p.FindPropertyRelative("checkSign");

        string name = (type != null && Enum.IsDefined(type, id))
                      ? Enum.GetName(type, id)
                      : "-";

        return $"{((ConditionNameGroup)grp)} : {name} [{sign.enumDisplayNames[sign.enumValueIndex]}]  {st}";
    }

    Type GetEnumType(ConditionNameGroup grp)
        => grp switch
        {
            ConditionNameGroup.Game => typeof(GameConditionName),
            _ => null
        };

    Rect Line(Rect baseR, int i)
        => new Rect(baseR.x, baseR.y + i * (h + v), baseR.width, h);
}
