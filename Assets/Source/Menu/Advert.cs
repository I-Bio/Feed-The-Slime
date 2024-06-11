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
            InterstitialAd.Show(() => Stopper.FocusPause(true),
                _ => Stopper.FocusRelease(true));
        }
        public void ShowReward(Action onReward, Action onClose)
        {
            VideoAd.Show((() => Stopper.FocusPause(true)), onReward, onClose);
        }
    }
}