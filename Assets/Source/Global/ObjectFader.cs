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

    [SerializeField] private float _alpha = 0.33f;
    [SerializeField] private float _delay = 0.03f;

    private Material[] _materials;

    private Renderer _renderer;
    private float _initialAlpha;
    private Coroutine _fadeInRoutine;
    private Coroutine _fadeOutRoutine;
    private WaitForSeconds _wait;

    public void Initialize()
    {
        _renderer = GetComponent<Renderer>();
        _materials = _renderer.materials;
        _initialAlpha = _materials[0].color.a;
        _wait = new WaitForSeconds(_delay);
    }

    public void Appear()
    {
        if (_fadeInRoutine != null)
            StopCoroutine(_fadeInRoutine);

        _fadeInRoutine = StartCoroutine(AppearRoutine());
    }

    public void Disappear()
    {
        if (_fadeOutRoutine != null)
            StopCoroutine(_fadeOutRoutine);

        _fadeOutRoutine = StartCoroutine(DisappearRoutine());
    }

    private IEnumerator AppearRoutine()
    {
        if (_materials.First().HasProperty(PropertyColor) == true)
        {
            while (_materials.First().color.a < _initialAlpha)
            {
                ChangeMaterialColor(_initialAlpha);
                yield return _wait;
            }
        }

        foreach (var material in _materials)
        {
            material.DisableKeyword(AlphaBlend);
            ChangeMaterialProperties(material, BlendMode.One, BlendMode.Zero,
                ValueConstants.One, RenderQueue.Geometry);
        }
    }

    private IEnumerator DisappearRoutine()
    {
        foreach (var material in _materials)
        {
            ChangeMaterialProperties(material, BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha,
                ValueConstants.Zero, RenderQueue.Transparent);
            material.EnableKeyword(AlphaBlend);
        }

        if (_materials.First().HasProperty(PropertyColor) == false)
            yield break;

        while (_materials.First().color.a > _alpha)
        {
            ChangeMaterialColor(_alpha);
            yield return _wait;
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