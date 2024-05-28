using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectFader : MonoBehaviour
{
    private const string SrcBlend = "_SrcBlend";
    private const string DstBlend = "_DstBlend";
    private const string ZWrite = "_ZWrite";
    private const string PropertyColor = "_Color";
    private const string AlphaBlend = "_ALPHABLEND_ON";

    private Material[] _materials;

    [SerializeField] private float _fadedAlpha = 0.33f;
    [SerializeField] private float _fadeDelay = 0.03f;

    private Renderer _renderer;
    private float _initialAlpha;
    private Coroutine _fadeInRoutine;
    private Coroutine _fadeOutRoutine;

    public void Initialize()
    {
        _renderer = GetComponent<Renderer>();
        _materials = _renderer.materials;
        _initialAlpha = _materials[0].color.a;
    }

    public void FadeIn()
    {
        if (_fadeInRoutine != null)
            StopCoroutine(_fadeInRoutine);

        _fadeInRoutine = StartCoroutine(FadeObjectIn());
    }

    public void FadeOut()
    {
        if (_fadeOutRoutine != null)
            StopCoroutine(_fadeOutRoutine);

        _fadeOutRoutine = StartCoroutine(FadeObjectOut());
    }

    private IEnumerator FadeObjectIn()
    {
        WaitForSeconds wait = new WaitForSeconds(_fadeDelay);

        if (_materials.First().HasProperty(PropertyColor) == true)
        {
            while (_materials.First().color.a < _initialAlpha)
            {
                ChangeMaterialColor(_initialAlpha);
                yield return wait;
            }
        }

        foreach (var material in _materials)
        {
            material.DisableKeyword(AlphaBlend);
            ChangeMaterialProperties(material, BlendMode.One, BlendMode.Zero, ValueConstants.One, RenderQueue.Geometry);
        }
    }

    private IEnumerator FadeObjectOut()
    {
        WaitForSeconds wait = new WaitForSeconds(_fadeDelay);

        foreach (var material in _materials)
        {
            ChangeMaterialProperties(material, BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha, ValueConstants.Zero, RenderQueue.Transparent);
            material.EnableKeyword(AlphaBlend);
        }

        if (_materials.First().HasProperty(PropertyColor) == false)
            yield break;

        while (_materials.First().color.a > _fadedAlpha)
        {
            ChangeMaterialColor(_fadedAlpha);
            yield return wait;
        }
    }

    private void ChangeMaterialColor(float alpha)
    {
        foreach (var material in _materials.Where(material => material.HasProperty(PropertyColor)))
        {
            material.color = new Color(
                material.color.r,
                material.color.g,
                material.color.b,
                alpha
            );
        }
    }

    private void ChangeMaterialProperties(Material material, BlendMode srcBlend, BlendMode dstBlend,
        ValueConstants zWrite, RenderQueue renderQueue)
    {
        material.SetInt(SrcBlend, (int)srcBlend);
        material.SetInt(DstBlend, (int)dstBlend);
        material.SetInt(ZWrite, (int)zWrite);
        material.renderQueue = (int)renderQueue;
    }
}