using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Spawner : MonoBehaviour
{
    [Header("Prefabs de piezas")]
    public GameObject[] piecePrefabs;

    [Header("Punto inicial de spawn (opcional)")]
    public Transform initialSpawnPoint;

    [Header("Transform por defecto si no hay Initial Spawn Point")]
    public Vector3 defaultSpawnPosition = new Vector3(4.4f, -8.76f, 0f);
    public Vector3 defaultSpawnScale = new Vector3(3.944204f, 3.944204f, 1f);

    [Header("Intervalo de spawn (segundos)")]
    public float spawnInterval = 2f;

    [Header("Umbral de destrucción")]
    [Tooltip("Cuando una pieza cruce esta X (eje negativo), será destruida")]
    public float destroyXThreshold = -50f;

    // --- Interno ---
    private Vector3 nextSpawnPos;
    private Vector3 nextSpawnScale;
    private List<GameObject> spawnedPieces = new List<GameObject>();
    // ---------------

    private void Start()
    {
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

        InvokeRepeating(nameof(SpawnPiece), spawnInterval, spawnInterval);
    }

    private void Update()
    {
        // Recorremos al revés para poder borrar sin problemas
        for (int i = spawnedPieces.Count - 1; i >= 0; i--)
        {
            GameObject piece = spawnedPieces[i];
            // Si ya fue destruida de otra forma o cruza el umbral:
            if (piece == null || piece.transform.position.x <= destroyXThreshold)
            {
                if (piece != null)
                    Destroy(piece);
                spawnedPieces.RemoveAt(i);
            }
        }
    }

    private void SpawnPiece()
    {
        if (piecePrefabs == null || piecePrefabs.Length == 0)
        {
            Debug.LogWarning("PieceSpawner: no hay prefabs asignados.");
            return;
        }

        // Instanciamos la pieza
        GameObject prefab = piecePrefabs[Random.Range(0, piecePrefabs.Length)];
        GameObject inst = Instantiate(prefab, nextSpawnPos, Quaternion.identity);
        inst.transform.localScale = nextSpawnScale;

        // La guardamos para destruirla luego
        spawnedPieces.Add(inst);

        // Buscamos el próximo punto de spawn dentro del prefab
        Transform spawnChild = FindChildByNameRecursive(inst.transform, "SpawnSiguientePieza");
        if (spawnChild != null)
        {
            nextSpawnPos = spawnChild.position;
            nextSpawnScale = spawnChild.lossyScale;
        }
        else
        {
            Debug.LogWarning($"PieceSpawner: '{prefab.name}' no tiene un child 'SpawnSiguientePieza'.");
        }
    }

    private Transform FindChildByNameRecursive(Transform parent, string childName)
    {
        if (parent.name == childName) return parent;
        foreach (Transform c in parent)
        {
            Transform r = FindChildByNameRecursive(c, childName);
            if (r != null) return r;
        }
        return null;
    }

    public void StopSpawning() => CancelInvoke(nameof(SpawnPiece));
}
