﻿using System.Collections.Generic;
using Agava.WebUtility;
using Boosters;
using Cinemachine;
using Foods;
using Input;
using Menu;
using Players;
using Spawners;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
    [RequireComponent(typeof(FadeCaster))]
    [RequireComponent(typeof(Revival))]
    public class GuideBootstrap : MonoBehaviour
    {
        [SerializeField] private InputSetup _input;
        [SerializeField] private PlayerCharacteristics _characteristics;
        [SerializeField] private PlayerSetup _player;
        [SerializeField] private ThemePreparer _theme;

        [Space] [Header("Sound")]
        [SerializeField] private Sprite _onIcon;
        [SerializeField] private Sprite _offIcon;
        [SerializeField] private Button _volume;
        [SerializeField] private Image _icon;
        [SerializeField] private List<AudioSource> _sources;
        [SerializeField] private AudioSource _music;

        [Space] [Header(nameof(FadeCaster))]
        [SerializeField] private LayerMask _fadeMask;
        [SerializeField] private float _castDelay = 0.01f;
        [SerializeField] private int _hitsCapacity = 10;

        [Space] [Header(nameof(BoosterSpawner))]
        [SerializeField] private BoosterSpawner _boosterSpawner;
        [SerializeField] private Booster _template;
        [SerializeField] private Transform _pointsHolder;
        [SerializeField] private Vector3 _offSet;
        [SerializeField] private float _spawnDelay = 5f;

        [Space] [Header(nameof(BoosterFactory))]
        [SerializeField] private Sprite _speedIcon;
        [SerializeField] private Sprite _scoreIcon;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private float _minLifeTime = 4f;
        [SerializeField] private float[] _scaleValues;
        [SerializeField] private float[] _additionalValues;

        [Space] [Header(nameof(Guide))]
        [SerializeField] private Guide _guide;
        [SerializeField] private Button[] _nextButtons;
        [SerializeField] private Button[] _releaseButtons;
        [SerializeField] private ObjectHighlighter _exampleFood;
        [SerializeField] private float _selectValue = 4f;
        [SerializeField] private Transform _enemy;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private GuideBeacon _beacon;
        [SerializeField] private GuideTrigger _trigger;
        [SerializeField] private Button _close;
        [SerializeField] private Button _pause;
        [SerializeField] private Button[] _loadButtons;
        [SerializeField] private ObjectFiller _filler;

        [Space] [Header("Depend on device")]
        [SerializeField] private GameObject _mobile;
        [SerializeField] private GameObject _desktop;

        private FadeCaster _fadeCaster;
        private IRevival _revival;
        private SaveService _saveService;

        private void Awake()
        {
            _saveService = new SaveService(Launch, _characteristics);
            _saveService.Load();
        }

        private void Launch(IReadOnlyCharacteristics characteristics)
        {
            PrepareComponents();
            InitializeLevel(characteristics);
            InitializeGuide();
        }

        private void PrepareComponents()
        {
            _fadeCaster = GetComponent<FadeCaster>();
            _revival = GetComponent<IRevival>();
        }

        private void InitializeLevel(IReadOnlyCharacteristics characteristics)
        {
            IMovable movable = new Speed(characteristics.Speed);
            ICalculableScore calculableScore = new AdditionalScore(new Score(), characteristics.ScorePerEat);
            IHidden hidden = _player.GetComponent<IHidden>();
            IPlayerVisitor visitor = _player.GetComponent<IPlayerVisitor>();
            PlayerPrefs.SetString(
                nameof(PlayerCharacteristics.IsAllowedSound),
                characteristics.IsAllowedSound.ToString());

            _input.Initialize();
            _player.Initialize(
                movable,
                calculableScore,
                (float)ValueConstants.Zero,
                _guide,
                _revival,
                _theme.Initialize(hidden, visitor, out List<ISelectable> selectables),
                selectables,
                null);
            _boosterSpawner.Initialize(
                new BoosterFactory(
                    _scaleValues,
                    _additionalValues,
                    _speedIcon,
                    _scoreIcon,
                    _maxLifeTime,
                    _minLifeTime,
                    _pointsHolder,
                    _offSet,
                    _boosterSpawner.Pull<Booster>),
                _spawnDelay,
                _template);
        }

        private void InitializeGuide()
        {
            if (Device.IsMobile)
                _mobile.SetActive(true);
            else
                _desktop.SetActive(true);

            _beacon.Initialize(_guide);
            _trigger.Initialize(
                _camera,
                _close,
                _player.transform,
                _enemy,
                () => _guide.ChangeWindow(GuideWindows.Enemy),
                _guide.Release);
            _exampleFood.Initialize((float)ValueConstants.Zero, _selectValue, transform);
            _fadeCaster.Initialize(_fadeMask, _player.transform, _camera.transform, _castDelay, _hitsCapacity);
            _filler.Initialize();
            _exampleFood.SetSelection();
            _guide.Initialize(
                _nextButtons,
                _releaseButtons,
                _loadButtons,
                _pause,
                _filler,
                _onIcon,
                _offIcon,
                _volume,
                _icon,
                _sources,
                _music);
        }
    }
}