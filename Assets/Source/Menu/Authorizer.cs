using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Authorizer : MonoBehaviour
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
            SceneManager.LoadScene((int)SceneNames.Menu);
            YandexGamesSdk.GameReady();
        }
    }
}