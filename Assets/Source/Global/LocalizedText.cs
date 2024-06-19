using Lean.Localization;
using UnityEngine;

public class LocalizedText : LeanLocalizedBehaviour
{
    [SerializeField] private string _label;

    public string Label => _label;

    public override void UpdateTranslation(LeanTranslation translation)
    {
        if (translation == null)
            return;

        if (translation.Data is string == false)
            return;

        _label = translation.Data as string;
    }
}