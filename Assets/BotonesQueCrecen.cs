using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class ButtonScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Escalas")]
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = Vector3.one * 1.2f;
    [Header("Duración animación (s)")]
    public float duration = 0.15f;

    void Awake()
    {
        // Asegurarnos de partir siempre de la escala normal
        transform.localScale = normalScale;
        Debug.Log($"[{name}] Awake: escala inicial {transform.localScale}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"[{name}] OnPointerEnter recibido");
        StopAllCoroutines();
        StartCoroutine(ScaleTo(hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"[{name}] OnPointerExit recibido");
        StopAllCoroutines();
        StartCoroutine(ScaleTo(normalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        Debug.Log($"[{name}] ScaleTo terminó en {targetScale}");
    }
}
