using System;
using System.Collections.Generic;
using Players;
using Spawners;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Menu
{
    public class ThemeFactory : IThemeFactory
    {
        private readonly SerializedPair<int, ThemePreparer[]>[] ZoneTemplates;
        private readonly SerializedPair<int, CenterPreparer[]>[] CenterTemplates;
        private readonly Renderer Ground;
        private readonly IHidden Hidden;
        private readonly IPlayerVisitor Visitor;
        private readonly int DifficultId;
        private readonly Action<AudioSource> OnAudioFoundCallback;
        
        public ThemeFactory(SerializedPair<int, ThemePreparer[]>[] zoneTemplates,
            SerializedPair<int, CenterPreparer[]>[] centerTemplates, Renderer ground, IHidden hidden,
            IPlayerVisitor visitor, int completedLevels, Action<AudioSource> onAudioFoundCallback)
        {
            ZoneTemplates = zoneTemplates;
            CenterTemplates = centerTemplates;
            Ground = ground;
            Hidden = hidden;
            Visitor = visitor;
            OnAudioFoundCallback = onAudioFoundCallback;
            DifficultId = 0;
            
            for (int i = 0; i < ZoneTemplates.Length; i++)
            {
                if (ZoneTemplates[i].Key > completedLevels)
                    break;

                DifficultId = i;
            }
        }
        
        public List<Contactable> CreateCenter(Transform point, out List<ISelectable> selectables)
        {
            return Object.Instantiate(CenterTemplates[DifficultId].Value.GetRandom(), point)
                .Initialize(Hidden, Visitor, out selectables, Ground);
        }

        public List<Contactable> CreateTheme(Transform point, out List<ISelectable> selectables)
        {
            return Object.Instantiate(ZoneTemplates[DifficultId].Value.GetRandom(), point)
                .Initialize(Hidden, Visitor, out selectables, OnAudioFoundCallback);
        }
    }
}