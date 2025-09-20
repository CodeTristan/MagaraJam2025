//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;

//[CustomEditor(typeof(AllPlacesSO))]
//public class AllPlacesSOEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        // Varsayýlan Inspector
//        DrawDefaultInspector();

//        EditorGUILayout.Space();

//        AllPlacesSO allPlaces = (AllPlacesSO)target;

//        if (GUILayout.Button("Fill all PlaceSO's automatically"))
//        {
//            string[] guids = AssetDatabase.FindAssets("t:PlaceSO");
//            allPlaces.places = new List<PlaceSO>();

//            foreach (var guid in guids)
//            {
//                string path = AssetDatabase.GUIDToAssetPath(guid);
//                PlaceSO place = AssetDatabase.LoadAssetAtPath<PlaceSO>(path);
//                if (place != null)
//                    allPlaces.places.Add(place);
//            }

//            EditorUtility.SetDirty(allPlaces);
//            AssetDatabase.SaveAssets();

//            Debug.Log("All PlaceSO's have been filled: " + allPlaces.places.Count);
            
//            Repaint();
//        }
//    }
//}
