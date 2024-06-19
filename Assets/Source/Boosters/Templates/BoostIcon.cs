using Players;
using UnityEngine;
using UnityEngine.UI;

namespace Boosters
{
    public class BoostIcon : AbilityButton
    {
        [SerializeField] private Image _image;

        public BoostIcon Initialize(float coolDown, Sprite sprite)
        {
            Initialize(coolDown);
            _image.sprite = sprite;
            return this;
        }
    }
}