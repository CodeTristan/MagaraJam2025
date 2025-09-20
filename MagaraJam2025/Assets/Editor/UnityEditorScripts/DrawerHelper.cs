#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DialogSystemEditorHelpers
{
    internal static class DrawerHelpers
    {
        internal const float h = 18;
        internal const float v = 10;

        internal static Rect Line(Rect baseRect, int i)
            => new(baseRect.x,
                   baseRect.y + i * (h + v),
                   baseRect.width,
                   h);

        /// <summary>Basit “fold-out title (özet)” satýrý.</summary>
        internal static bool Foldout(Rect pos, SerializedProperty prop, string summary)
        {
            string title = $"{ObjectNames.NicifyVariableName(prop.type)}  ({summary})";
            return prop.isExpanded = EditorGUI.Foldout(pos, prop.isExpanded, title, true);
        }


    }



/// <summary>Null dönen FindPropertyRelative çaðrýlarýný güvenli hâle getirir.</summary>
internal static class SafeProperty
    {
        /// Unity nesnesi gerektiði için basit ScriptableObject türevi
        private class DummySO : ScriptableObject { }

        private static readonly SerializedObject _dummySO =
            new SerializedObject(ScriptableObject.CreateInstance<DummySO>());

        /// Boþ dönen property yerine “zararsýz” bir SerializedProperty döndürür.
        internal static SerializedProperty Find(SerializedProperty parent, string name)
        {
            var sp = parent.FindPropertyRelative(name);
            if (sp != null) return sp;

            /* -----------------------------------------------------------
             * Fallback:  dummy ScriptableObject’taki “m_Name” alaný.
             *  - Her zaman mevcuttur   (Unity'nin dahili ismi)
             *  - .stringValue => ""    (boþ ama güvenli)
             *  - .intValue    => 0     vb.
             * Böylece .stringValue / .intValue okunsa bile NullRef atmaz.
             * ----------------------------------------------------------*/
            return _dummySO.FindProperty("m_Name");
        }
    }


}
#endif
