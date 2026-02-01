using TMPro;
using UnityEngine;
using System.Collections;

public class WaveUIManager : MonoBehaviour
{
    public static WaveUIManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 0.4f;
    [SerializeField] float showDuration = 1.6f;
    [SerializeField] float popScale = 1.12f;
    [SerializeField] float baseScale = 0.9f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (waveText == null) waveText = GetComponentInChildren<TextMeshProUGUI>();
        if (canvasGroup == null) canvasGroup = GetComponentInChildren<CanvasGroup>();

        if (canvasGroup == null)
        {
            if (waveText != null) canvasGroup = waveText.gameObject.GetComponent<CanvasGroup>() ?? waveText.gameObject.AddComponent<CanvasGroup>();
            else canvasGroup = gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        }

        // Asegurar estado inicial
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    public void ShowWave(int waveNumber)
    {
        if (Instance == null) return;
        StopAllCoroutines();
        StartCoroutine(ShowWaveCoroutine(waveNumber));
    }

    IEnumerator ShowWaveCoroutine(int waveNumber)

    {   if (waveText == null) waveText = GetComponentInChildren<TextMeshProUGUI>();
        if (canvasGroup == null) canvasGroup = GetComponentInChildren<CanvasGroup>() ?? (waveText != null ? waveText.gameObject.AddComponent<CanvasGroup>() : gameObject.AddComponent<CanvasGroup>());


        waveText.text = $"WAVE {waveNumber}";
        RectTransform rt = waveText.rectTransform;
        rt.localScale = Vector3.one * baseScale;

        // Fade in + pop
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / fadeDuration);
            canvasGroup.alpha = lerp;
            rt.localScale = Vector3.one * Mathf.Lerp(baseScale, popScale, Mathf.SmoothStep(0f, 1f, lerp));
            yield return null;
        }

        canvasGroup.alpha = 1f;
        rt.localScale = Vector3.one * popScale;

        // Mantener visible
        float timer = 0f;
        while (timer < showDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = 1f - Mathf.Clamp01(t / fadeDuration);
            canvasGroup.alpha = lerp;
            rt.localScale = Vector3.one * Mathf.Lerp(baseScale, popScale, Mathf.SmoothStep(0f, 1f, lerp));
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}