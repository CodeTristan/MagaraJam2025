#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreBuildChecker : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Sahneyi kontrol et
        var musicManager = Object.FindFirstObjectByType<MusicManager>();
        if (musicManager != null && musicManager.isMuted)
        {
            bool proceed = EditorUtility.DisplayDialog(
                "Mute Aktif!",
                "MusicManager'da mute = true. Bu �ekilde build almak istedi�ine emin misin?",
                "Devam Et (Build)", "�ptal Et");

            if (!proceed)
            {
                throw new BuildFailedException("Build kullan�c� taraf�ndan mute a��k oldu�u i�in iptal edildi.");
            }
        }
        else
        {
            Debug.Log("Mute kontrol� ge�ti.");
        }

        var gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null || gameManager.DebugMod)
        {
            bool proceed = EditorUtility.DisplayDialog(
                "Debug Mod Aktif Olabilir!",
                "GameManager sahnede yok veya Debug mod aktif. Kapatmay� unutma!!",
                "Devam Et (Build)" , "�ptal Et");

            if (!proceed)
            {
                throw new BuildFailedException("Build kullan�c� taraf�ndan GameManager kontrol� nedeniyle iptal edildi.");
            }
        }

        Canvas litterBox = GameObject.Find("LitterBoxCanvas").GetComponent<Canvas>();
        if (litterBox == null || !litterBox.enabled)
        {
            bool proceed = EditorUtility.DisplayDialog(
                "LitterBoxCanvas Bulunamad�!",
                "LitterBoxCanvas sahnede bulunamad� veya aktif de�il. Bu �ekilde build almak istedi�ine emin misin?",
                "Devam Et (Build)", "�ptal Et");
            if (!proceed)
            {
                throw new BuildFailedException("Build kullan�c� taraf�ndan LitterBoxCanvas kontrol� nedeniyle iptal edildi.");
            }
        }
        else
        {
            Debug.Log("LitterBoxCanvas kontrol� ge�ti.");
        }
    }
}
#endif
