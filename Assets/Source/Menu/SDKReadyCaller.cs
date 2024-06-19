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

            YandexGamesSdk.GameReady();
        }
    }
}