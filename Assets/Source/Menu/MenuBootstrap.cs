using System;
using System.Linq;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(YandexLeaderboard))]
    [RequireComponent(typeof(LeaderboardView))]
    public class MenuBootstrap : MonoBehaviour
    {
        [Header("Leaderboard")]
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private string _leaderboardName = "Leaderboard";
        [SerializeField] private string _anonymousName = "Anonymous";
        [SerializeField] private Transform _container;

        [SerializeField] private ProgressSetup _progress;

        private YandexLeaderboard _yandexLeaderboard;
        private LeaderboardView _leaderboard;

        private void Awake()
        {
            _yandexLeaderboard = GetComponent<YandexLeaderboard>();
            _leaderboard = GetComponent<LeaderboardView>();
            _progress.Initialize(_yandexLeaderboard);
        }

        private void Start()
        {
            _anonymousName = LeanLocalization.GetTranslationText(_anonymousName);
            _yandexLeaderboard.Initialize(_leaderboard, _name, _leaderboardName, _anonymousName, _container);
        }
    }
}