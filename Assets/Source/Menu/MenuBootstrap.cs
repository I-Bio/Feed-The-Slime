using Lean.Localization;
using Spawners;
using TMPro;
using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(YandexLeaderboard))]
    [RequireComponent(typeof(LeaderboardView))]
    [RequireComponent(typeof(Stopper))]
    public class MenuBootstrap : MonoBehaviour
    {
        [SerializeField] private string _leaderboardName = "Leaderboard";
        [SerializeField] private string _anonymousName = "Anonymous";
        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _template;

        [SerializeField] private ProgressSetup _progress;
        [SerializeField] private LevelBootstrap _bootstrap;

        private YandexLeaderboard _yandexLeaderboard;
        private LeaderboardView _leaderboard;
        private Stopper _stopper;

        private void Awake()
        {
            _yandexLeaderboard = GetComponent<YandexLeaderboard>();
            _leaderboard = GetComponent<LeaderboardView>();
            _stopper = GetComponent<Stopper>();
            _progress.Initialize(_yandexLeaderboard, _bootstrap, _stopper);
        }

        private void Start()
        {
            _anonymousName = LeanLocalization.GetTranslationText(_anonymousName);
            _yandexLeaderboard.Initialize(_leaderboard, _leaderboardName, _anonymousName, _container, _template);
        }
    }
}