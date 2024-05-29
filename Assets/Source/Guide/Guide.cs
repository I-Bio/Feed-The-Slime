using System.Collections.Generic;
using Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Screen = Menu.Screen;

namespace Guide
{
    [RequireComponent(typeof(Stopper))]
    [RequireComponent(typeof(Screen))]
    public class Guide : MonoBehaviour, IGame, IGuide
    {
        private Screen _screen;
        private Stopper _stopper;

        private int _pointer;
        private Button[] _nextButtons;
        private Button[] _releaseButtons;
        private Button[] _loadButtons;
        private Button _pause;
        private ObjectFiller _filler;

        private void OnDestroy()
        {
            foreach (Button next in _nextButtons)
                next.onClick.RemoveListener(Next);

            foreach (Button release in _releaseButtons)
                release.onClick.RemoveListener(Release);

            foreach (Button load in _loadButtons)
                load.onClick.RemoveListener(Load);

            _pause.onClick.RemoveListener(Pause);
        }

        public void Initialize(Button[] nextButtons, Button[] releaseButtons, Button[] loadButtons, Button pause,
            ObjectFiller filler, Sprite onIcon, Sprite offIcon, Button volume, Image icon, List<AudioSource> sources, AudioSource music)
        {
            _nextButtons = nextButtons;
            _releaseButtons = releaseButtons;
            _loadButtons = loadButtons;
            _pause = pause;
            _filler = filler;

            _stopper = GetComponent<Stopper>();
            _screen = GetComponent<Screen>();

            _stopper.Initialize(onIcon, offIcon, volume, icon, sources, music);
            _screen.Initialize(_stopper);

            foreach (Button next in _nextButtons)
                next.onClick.AddListener(Next);

            foreach (Button release in _releaseButtons)
                release.onClick.AddListener(Release);

            foreach (Button load in _loadButtons)
                load.onClick.AddListener(Load);

            _pause.onClick.AddListener(Pause);

            SetStage(SatietyStage.Exhaustion);
            Next();
            _filler.EmptyUp();
        }

        public void SetStage(SatietyStage stage) {}

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
            PlayerPrefs.SetString(nameof(CharacteristicConstants.DidPassGuide),
                nameof(CharacteristicConstants.DidPassGuide));
            _filler.FillUp(() => SceneManager.LoadScene((int)SceneNames.Game));
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