using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class SDKInitializer : MonoBehaviour
    {
        private void Awake()
        {
            YandexGamesSdk.CallbackLogging = true;
        }

        private IEnumerator Start()
        {
            yield return YandexGamesSdk.Initialize(OnInitialize);
        }

        private void OnInitialize()
        {
            if (PlayerPrefs.HasKey(nameof(YandexGamesSdk.GameReady)))
                PlayerPrefs.DeleteKey(nameof(YandexGamesSdk.GameReady));

            SceneManager.LoadScene((int)SceneNames.Game);
        }
    }
}