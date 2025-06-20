using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Scroll : MonoBehaviour
{
    [Header("Scroll Settings")]
    [Tooltip("Velocidad inicial de desplazamiento hacia la izquierda")]
    [SerializeField]
    private float initialSpeed = 8f;

    [Header("Speed Ramp Settings")]
    [Tooltip("Velocidad máxima a alcanzar")]
    [SerializeField]
    private float maxSpeed = 12f;

    [Tooltip("Tiempo en segundos para alcanzar la velocidad máxima")]
    [SerializeField]
    private float rampDuration = 40f;

    // Tiempo transcurrido desde el inicio
    private float elapsedTime = 0f;

    void Update()
    {
        // Acumula tiempo cada frame
        elapsedTime += Time.deltaTime;

        // Factor entre 0 (inicio) y 1 (cuando elapsedTime >= rampDuration)
        float t = Mathf.Clamp01(elapsedTime / rampDuration);

        // Interpola la velocidad actual entre initialSpeed y maxSpeed
        float currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, t);

        // Mueve el objeto hacia la izquierda
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
    }
}
