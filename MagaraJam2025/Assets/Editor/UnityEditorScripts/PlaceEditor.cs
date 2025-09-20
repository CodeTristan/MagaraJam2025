using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Place))]
public class PlaceEditor : Editor
{
    private SerializedProperty dialogTriggersProp;
    private ReorderableList dialogTriggerList;

    private void OnEnable()
    {
        dialogTriggersProp = serializedObject.FindProperty("dialogTriggers");

        dialogTriggerList = new ReorderableList(serializedObject, dialogTriggersProp, true, true, true, true);
        dialogTriggerList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Dialog Triggers");

        dialogTriggerList.elementHeightCallback = index =>
        {
            return EditorGUIUtility.singleLineHeight * 12;
        };

        dialogTriggerList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            SerializedProperty element = dialogTriggersProp.GetArrayElementAtIndex(index);

            var y = rect.y;
            float h = EditorGUIUtility.singleLineHeight + 5;

            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, h), element.FindPropertyRelative("branchName"));
            y += h;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width, h), element.FindPropertyRelative("fileName"));
            y += h;
            EditorGUI.PropertyField(new Rect(rect.x, y, rect.width / 2 - 5, h), element.FindPropertyRelative("isRepeatable"));
            EditorGUI.PropertyField(new Rect(rect.x + rect.width / 2 + 5, y, rect.width / 2 - 5, h), element.FindPropertyRelative("isRandom"));
            y += h;

            EditorGUI.LabelField(new Rect(rect.x, y, rect.width, h), "Required Conditions");
            y += h;
            DrawCheckConditionList(element.FindPropertyRelative("RequiredConditions"), rect, ref y);

            EditorGUI.LabelField(new Rect(rect.x, y, rect.width, h), "Affected Conditions");
            y += h;
            DrawCheckConditionList(element.FindPropertyRelative("AffectedConditions"), rect, ref y);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "dialogTriggers");

        EditorGUILayout.Space(10);
        dialogTriggerList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawCheckConditionList(SerializedProperty list, Rect rect, ref float y)
    {
        for (int i = 0; i < list.arraySize; i++)
        {
            var element = list.GetArrayElementAtIndex(i);
            float h = EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(new Rect(rect.x + 10, y, rect.width - 20, h), element.FindPropertyRelative("enumConditionName"));
            y += h;
            GUI.enabled = false;
            EditorGUI.PropertyField(new Rect(rect.x + 10, y, rect.width - 20, h), element.FindPropertyRelative("conditionName"));
            GUI.enabled = true;
            y += h;
            EditorGUI.PropertyField(new Rect(rect.x + 10, y, rect.width - 20, h), element.FindPropertyRelative("status"));
            y += h;
            EditorGUI.PropertyField(new Rect(rect.x + 10, y, rect.width - 20, h), element.FindPropertyRelative("type"));
            y += h;
            EditorGUI.PropertyField(new Rect(rect.x + 10, y, rect.width - 20, h), element.FindPropertyRelative("CheckSign"));
            y += h;

            //if (GUI.Button(new Rect(rect.x + 10, y, rect.width - 20, h), "Sync conditionName with enumConditionName"))
            //{
            //    var enumProp = element.FindPropertyRelative("enumConditionName");
            //    var strProp = element.FindPropertyRelative("conditionName");
            //    strProp.stringValue = enumProp.enumNames[enumProp.enumValueIndex];
            //}
            y += h;

            if (GUI.Button(new Rect(rect.x + 10, y, rect.width - 20, h), "Remove Condition"))
            {
                list.DeleteArrayElementAtIndex(i);
                break;
            }

            y += h + 4;
            EditorGUI.indentLevel--;
        }

        if (GUI.Button(new Rect(rect.x + 10, y, rect.width - 20, EditorGUIUtility.singleLineHeight), "Add Condition"))
        {
            list.InsertArrayElementAtIndex(list.arraySize);
        }

        y += EditorGUIUtility.singleLineHeight + 10;
    }
}
