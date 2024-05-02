using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Screen = Menu.Screen;
using PlayerPrefs = Agava.YandexGames.Utility.PlayerPrefs;

namespace Guide
{
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(Screen))]
    public class Guide : MonoBehaviour, IGame, ILoader, IGuide
    {
        private Screen _screen;
        private Stopper _stopper;

        private int _pointer;
        private Button[] _nextButtons;
        private Button[] _releaseButtons;
        private Button _pause;

        private void OnDestroy()
        {
            foreach (Button next in _nextButtons)
                next.onClick.RemoveListener(Next);

            foreach (Button release in _releaseButtons)
                release.onClick.RemoveListener(Release);
            
            _pause.onClick.RemoveListener(Pause);
        }

        public void Initialize(Button[] nextButtons, Button[] releaseButtons, Button pause)
        {
            _nextButtons = nextButtons;
            _releaseButtons = releaseButtons;
            _pause = pause;

            _stopper = GetComponent<Stopper>();
            _screen = GetComponent<Screen>();
            _screen.Initialize(_stopper);

            foreach (Button next in _nextButtons)
                next.onClick.AddListener(Next);

            foreach (Button release in _releaseButtons)
                release.onClick.AddListener(Release);
            
            _pause.onClick.AddListener(Pause);

            SetStage(SatietyStage.Exhaustion);
            Next();
        }

        public void SetStage(SatietyStage stage) { }

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
            _screen.SetWindow((int)window);
        }

        public void Load()
        {
            _stopper.Release();
            PlayerPrefs.SetString(nameof(CharacteristicConstants.DidPassGuide),
                nameof(CharacteristicConstants.DidPassGuide));
            SceneManager.LoadScene((int)SceneNames.Game);
        }

        public void Release()
        {
            ChangeWindow(GuideWindows.Main);
        }
        
        private void Next()
        {
            ChangeWindow((GuideWindows)_pointer);
            _pointer++;
        }
        
        private void Pause()
        {
            ChangeWindow(GuideWindows.Pause);
        }
    }
}