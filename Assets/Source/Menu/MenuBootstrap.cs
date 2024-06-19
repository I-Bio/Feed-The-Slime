using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(YandexLeaderboard))]
    [RequireComponent(typeof(LeaderboardView))]
    [RequireComponent(typeof(Stopper))]
    public class MenuBootstrap : MonoBehaviour
    {
        [SerializeField] private string _leaderboardName = "Leaderboard";
        [SerializeField] private LocalizedText _anonymous;
        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _template;

        [SerializeField] private ProgressSetup _progress;
        [SerializeField] private LevelBootstrap _bootstrap;
        [SerializeField] private Game _endGame;

        [Space] [Header("Sound")]
        [SerializeField] private Sprite _onIcon;
        [SerializeField] private Sprite _offIcon;
        [SerializeField] private Button _volume;
        [SerializeField] private Image _icon;
        [SerializeField] private List<AudioSource> _sources;
        [SerializeField] private AudioSource _music;

        private YandexLeaderboard _yandexLeaderboard;
        private LeaderboardView _leaderboard;
        private Stopper _stopper;

        private void Awake()
        {
            _yandexLeaderboard = GetComponent<YandexLeaderboard>();
            _leaderboard = GetComponent<LeaderboardView>();
            _stopper = GetComponent<Stopper>();

            _yandexLeaderboard.Initialize(_leaderboard, _leaderboardName, _anonymous, _container, _template);
            _stopper.Initialize(_onIcon, _offIcon, _volume, _icon, _sources, _music);
            _progress.Initialize(_yandexLeaderboard, _bootstrap, _stopper, _endGame);
        }
    }
}