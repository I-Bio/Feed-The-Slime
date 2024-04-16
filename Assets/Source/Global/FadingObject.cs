using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class FadingObject : MonoBehaviour
{
    private const string SrcBlend = "_SrcBlend";
    private const string DstBlend = "_DstBlend";
    private const string ZWrite = "_ZWrite";
    private const string PropertyColor = "_Color";
    private const string Alphablend = "_ALPHABLEND_ON";

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

        if (_materials[0].HasProperty(PropertyColor) == true)
        {
            while (_materials[0].color.a < _initialAlpha)
            {
                foreach (var material in _materials.Where(material => material.HasProperty(PropertyColor)))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        _initialAlpha
                    );
                }

                yield return wait;
            }
        }

        foreach (var material in _materials)
        {
            material.DisableKeyword(Alphablend);

            material.SetInt(SrcBlend, (int)BlendMode.One);
            material.SetInt(DstBlend, (int)BlendMode.Zero);
            material.SetInt(ZWrite, 1);
            material.renderQueue = (int)RenderQueue.Geometry;
        }
    }

    private IEnumerator FadeObjectOut()
    {
        WaitForSeconds wait = new WaitForSeconds(_fadeDelay);

        foreach (var material in _materials)
        {
            material.SetInt(SrcBlend, (int)BlendMode.SrcAlpha);
            material.SetInt(DstBlend, (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt(ZWrite, 0);

            material.EnableKeyword(Alphablend);
            material.renderQueue = (int)RenderQueue.Transparent;
        }

        if (_materials[0].HasProperty(PropertyColor) == false)
            yield break;

        while (_materials[0].color.a > _fadedAlpha)
        {
            foreach (var material in _materials.Where(material => material.HasProperty(PropertyColor)))
            {
                material.color = new Color(
                    material.color.r,
                    material.color.g,
                    material.color.b,
                    _fadedAlpha
                );
            }

            yield return wait;
        }
    }
}