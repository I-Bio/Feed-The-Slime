using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public interface IThemeFactory
    {
        public List<Contactable> CreateCenter(Transform point, out List<ISelectable> selectables);
        public List<Contactable> CreateTheme(Transform point, out List<ISelectable> selectables);
    }
}