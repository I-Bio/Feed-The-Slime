using Agava.YandexGames;
using UnityEngine;

namespace Menu
{
    public class SDKReadyCaller
    {
        public void CallGameReady()
        {
            if (PlayerPrefs.HasKey(nameof(YandexGamesSdk.GameReady)))
                return;
            
            Debug.Log("GAME READY");
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif
        }
    }
}