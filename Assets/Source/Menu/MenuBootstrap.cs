using Lean.Localization;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(YandexLeaderboard))]
    [RequireComponent(typeof(LeaderboardView))]
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(SoundChanger))]
    public class MenuBootstrap : MonoBehaviour
    {
        [SerializeField] private string _leaderboardName = "Leaderboard";
        [SerializeField] private string _anonymousName = "Anonymous";
        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _template;

        [SerializeField] private ProgressSetup _progress;
        [SerializeField] private LevelBootstrap _bootstrap;
        [SerializeField] private Game _endGame;

        [Space, Header("Sound")] 
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Slider _gameVolume;
        [SerializeField] private Slider _musicVolume;
        [SerializeField] private AudioSource _music;

        [Space, Header(nameof(SubWindowSwitcher))] 
        [SerializeField] private SubWindowSwitcher _subSwitcher;
        [SerializeField] private Button[] _subCloseButtons;
        [SerializeField] private Button[] _subVolumeButtons;
        [SerializeField] private Window[] _subParents;

        private YandexLeaderboard _yandexLeaderboard;
        private LeaderboardView _leaderboard;
        private Stopper _stopper;
        private SoundChanger _sound;

        private void Awake()
        {
            _yandexLeaderboard = GetComponent<YandexLeaderboard>();
            _leaderboard = GetComponent<LeaderboardView>();
            _stopper = GetComponent<Stopper>();
            _sound = GetComponent<SoundChanger>();
            
            _sound.Initialize(_mixer, _gameVolume, _musicVolume, _music);
            _stopper.Initialize(_sound.Mute, _sound.Release);
            _progress.Initialize(_yandexLeaderboard, _bootstrap, _stopper, _sound, _endGame);
            _subSwitcher.Initialize(_stopper, _subCloseButtons, _subVolumeButtons, _subParents);
        }

        private void Start()
        {
            _anonymousName = LeanLocalization.GetTranslationText(_anonymousName);
            _yandexLeaderboard.Initialize(_leaderboard, _leaderboardName, _anonymousName, _container, _template);
        }
    }
}