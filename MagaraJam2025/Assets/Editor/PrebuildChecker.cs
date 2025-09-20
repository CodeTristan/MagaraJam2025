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
                "MusicManager'da mute = true. Bu þekilde build almak istediðine emin misin?",
                "Devam Et (Build)", "Ýptal Et");

            if (!proceed)
            {
                throw new BuildFailedException("Build kullanýcý tarafýndan mute açýk olduðu için iptal edildi.");
            }
        }
        else
        {
            Debug.Log("Mute kontrolü geçti.");
        }

        var gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null || gameManager.DebugMod)
        {
            bool proceed = EditorUtility.DisplayDialog(
                "Debug Mod Aktif Olabilir!",
                "GameManager sahnede yok veya Debug mod aktif. Kapatmayý unutma!!",
                "Devam Et (Build)" , "Ýptal Et");

            if (!proceed)
            {
                throw new BuildFailedException("Build kullanýcý tarafýndan GameManager kontrolü nedeniyle iptal edildi.");
            }
        }

        Canvas litterBox = GameObject.Find("LitterBoxCanvas").GetComponent<Canvas>();
        if (litterBox == null || !litterBox.enabled)
        {
            bool proceed = EditorUtility.DisplayDialog(
                "LitterBoxCanvas Bulunamadý!",
                "LitterBoxCanvas sahnede bulunamadý veya aktif deðil. Bu þekilde build almak istediðine emin misin?",
                "Devam Et (Build)", "Ýptal Et");
            if (!proceed)
            {
                throw new BuildFailedException("Build kullanýcý tarafýndan LitterBoxCanvas kontrolü nedeniyle iptal edildi.");
            }
        }
        else
        {
            Debug.Log("LitterBoxCanvas kontrolü geçti.");
        }
    }
}
#endif
