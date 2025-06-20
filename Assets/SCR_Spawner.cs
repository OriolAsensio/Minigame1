using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Spawner : MonoBehaviour
{
    [Header("Prefabs de piezas")]
    public GameObject[] piecePrefabs;

    [Header("Spawn Point inicial (opcional)")]
    public Transform initialSpawnPoint;

    [Header("Transform por defecto si no hay Initial Spawn Point")]
    public Vector3 defaultSpawnPosition = new Vector3(4.4f, -8.76f, 0f);
    public Vector3 defaultSpawnScale = new Vector3(3.944204f, 3.944204f, 1f);

    [Header("Velocidad de spawn")]
    [Tooltip("Tiempo en segundos al inicio entre cada pieza")]
    public float initialSpawnInterval = 2f;
    [Tooltip("Mínimo tiempo entre piezas (máxima velocidad)")]
    public float minSpawnInterval = 0.5f;
    [Tooltip("Tiempo en segundos que tarda de inicial a mínimo")]
    public float spawnRampDuration = 120f;

    [Header("Umbral de destrucción X")]
    public float destroyXThreshold = -50f;

    // Estado interno
    private Vector3 nextSpawnPos;
    private Vector3 nextSpawnScale;
    private float elapsedSinceStart = 0f;
    private float elapsedSinceLastSpawn = 0f;
    private List<GameObject> spawnedPieces = new List<GameObject>();

    void Start()
    {
        // Inicializamos punto/escala
        if (initialSpawnPoint != null)
        {
            nextSpawnPos = initialSpawnPoint.position;
            nextSpawnScale = initialSpawnPoint.lossyScale;
        }
        else
        {
            nextSpawnPos = defaultSpawnPosition;
            nextSpawnScale = defaultSpawnScale;
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;
        elapsedSinceStart += dt;
        elapsedSinceLastSpawn += dt;

        // Calculamos el intervalo actual entre spawns
        float t = Mathf.Clamp01(elapsedSinceStart / spawnRampDuration);
        float currentInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);

        // ¿Toca spawnear?
        if (elapsedSinceLastSpawn >= currentInterval)
        {
            SpawnPiece();
            elapsedSinceLastSpawn = 0f;
        }

        // Destrucción de piezas que crucen el umbral X
        for (int i = spawnedPieces.Count - 1; i >= 0; i--)
        {
            var piece = spawnedPieces[i];
            if (piece == null || piece.transform.position.x <= destroyXThreshold)
            {
                if (piece != null) Destroy(piece);
                spawnedPieces.RemoveAt(i);
            }
        }
    }

    private void SpawnPiece()
    {
        if (piecePrefabs.Length == 0) return;

        // Instancio la pieza
        var prefab = piecePrefabs[Random.Range(0, piecePrefabs.Length)];
        var inst = Instantiate(prefab, nextSpawnPos, Quaternion.identity);
        inst.transform.localScale = nextSpawnScale;

        spawnedPieces.Add(inst);

        // Busco el child de referencia
        var spawnChild = FindChildByNameRecursive(inst.transform, "SpawnSiguientePieza");
        if (spawnChild != null)
        {
            nextSpawnPos = spawnChild.position;
            nextSpawnScale = spawnChild.lossyScale;
        }
        else
        {
            Debug.LogWarning($"PieceSpawner: '{prefab.name}' sin 'SpawnSiguientePieza'.");
        }
    }

    private Transform FindChildByNameRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform c in parent)
        {
            var r = FindChildByNameRecursive(c, name);
            if (r != null) return r;
        }
        return null;
    }

    public void StopSpawning()
    {
        enabled = false;
    }
}
