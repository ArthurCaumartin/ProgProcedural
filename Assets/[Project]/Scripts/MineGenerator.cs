using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _stoneCellPrefab;
    [SerializeField] private GameObject _mineralCellPrefab;
    
    [Header("Terrain : ")]
    [SerializeField] private Vector2Int _size;
    [SerializeField] private float _noiseFrequency;
    [SerializeField] private float _seed;
    [SerializeField] private Texture2D _noiseTexture;
    [SerializeField] private GameObject[,] _cellArray;


    void Start()
    {
        SpawnNewTerrain();
    }

    void OnValidate()
    {
        GenerateNoiseTexture();
    }

    private void GenerateNoiseTexture()
    {
        _seed = Random.Range(-10000, 10000);
        _noiseTexture = new Texture2D(_size.x, _size.y);
        for (int x = 0; x < _noiseTexture.width; x++)
        {
            for (int y = 0; y < _noiseTexture.height; y++)
            {
                float pixel = Mathf.PerlinNoise((x + _seed) * _noiseFrequency, (y + _seed) * _noiseFrequency);
                _noiseTexture.SetPixel(x, y, new Color(pixel, pixel, pixel));
            }
        }

        _noiseTexture.Apply();
    }

    private void SpawnNewTerrain()
    {
        GenerateNoiseTexture();

        _cellArray = new GameObject[_size.x, _size.y];

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if (_noiseTexture.GetPixel(x, y).r > .5f)
                {
                    GameObject newCell = _cellArray[x, y] = Instantiate(_stoneCellPrefab, transform);
                    newCell.transform.localPosition = new Vector3(x, 0, y);
                }
            }
        }
    }

    [ContextMenu("Regenerate Mine")]
    public void RegenerateMine()
    {
        DeleteMine();
        SpawnNewTerrain();
    }

    private void DeleteMine()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if (_cellArray[x, y])
                    Destroy(_cellArray[x, y]);
            }
        }
    }
}
