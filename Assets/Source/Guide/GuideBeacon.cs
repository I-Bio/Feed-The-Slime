using UnityEngine;

namespace Guide
{
    public class GuideBeacon : MonoBehaviour
    {
        private IGuide _guide;
        
        private void OnDestroy()
        {
            _guide?.Open();
        }

        public void Initialize(IGuide guide)
        {
            _guide = guide;
        }
    }
}