using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Screen = Menu.Screen;

namespace Guide
{
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(Screen))]
    public class Guide : MonoBehaviour, IGame, ILoader, IGuide
    {
        private ITransferService _transferService;
        private CanvasGroup _fadeBackground;
        private CanvasGroup _mainUi;
        private CanvasGroup _endGame;
        private TextMeshProUGUI[] _rewards;
        private Screen _screen;
        private Stopper _stopper;

        private int _pointer;
        private Button[] _nextButtons;
        private Button _release;
        private Button _win;

        private float _offAlpha;
        private float _onAlpha;

        private void OnDestroy()
        {
            foreach (Button next in _nextButtons)
                next.onClick.RemoveListener(Next);

            _release.onClick.RemoveListener(Release);
            _win.onClick.RemoveListener(Win);
        }

        public void Initialize(ITransferService transferService, CanvasGroup fadeBackground, CanvasGroup endGame,
            CanvasGroup mainUi,
            Button[] nextButtons, Button release, Button win, float offAlpha, float onAlpha, TextMeshProUGUI[] rewards)
        {
            _transferService = transferService;
            _fadeBackground = fadeBackground;
            _endGame = endGame;
            _mainUi = mainUi;
            _nextButtons = nextButtons;
            _release = release;
            _win = win;
            _offAlpha = offAlpha;
            _onAlpha = onAlpha;
            _rewards = rewards;

            _stopper = GetComponent<Stopper>();
            _screen = GetComponent<Screen>();

            foreach (Button next in _nextButtons)
                next.onClick.AddListener(Next);

            _release.onClick.AddListener(Release);
            _win.onClick.AddListener(Win);
            
            SetStage(SatietyStage.Exhaustion);
            Open();
        }

        public void SetStage(SatietyStage stage)
        {
            foreach (TextMeshProUGUI reward in _rewards)
                reward.SetText(_transferService.Reward.ToString());
        }

        public void Win()
        {
            ChangeWindow(GuideWindows.Win);
        }

        public void Lose()
        {
            ChangeWindow(GuideWindows.Lose);
        }

        public void ChangeWindow(GuideWindows window)
        {
            _stopper.Pause();
            _mainUi.alpha = _offAlpha;
            _mainUi.blocksRaycasts = false;
            _screen.SetWindow((int)window);

            if (window != GuideWindows.Lose && window != GuideWindows.Win)
                return;

            _fadeBackground.alpha = _offAlpha;
            _endGame.alpha = _onAlpha;

            _transferService.PassLevel();
        }

        public void Load()
        {
            _stopper.Release();
            _transferService.AllowReceive();
            SceneManager.LoadScene((int)SceneNames.Menu);
        }

        public void Release()
        {
            _fadeBackground.alpha = _offAlpha;
            _screen.Hide();
            _mainUi.alpha = _onAlpha;
            _mainUi.blocksRaycasts = true;
            _stopper.Release();
        }

        public void Open()
        {
            _fadeBackground.alpha = _onAlpha;
            Next();
        }

        private void Next()
        {
            ChangeWindow((GuideWindows)_pointer);
            _pointer++;
        }
    }
}