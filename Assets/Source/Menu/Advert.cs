using System;
using Agava.YandexGames;

namespace Menu
{
    public class Advert
    {
        private readonly Stopper Stopper;

        public Advert(Stopper stopper)
        {
            Stopper = stopper;
        }
        public void ShowInter()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            InterstitialAd.Show(() => Stopper.FocusPause(true), _ => Stopper.FocusRelease(true));
#endif      
        }
        public void ShowReward(Action onReward, Action onClose)
        {
#if UNITY_EDITOR
            onReward?.Invoke();
            onClose?.Invoke();
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            VideoAd.Show((() => Stopper.FocusPause(true)), onReward, onClose);
#endif
        }
    }
}