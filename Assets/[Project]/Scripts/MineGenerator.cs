using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! conflit Systeme.Random et UnityEngine.Random, mais besion de Systeme pour Action 

public class MineGenerator : MonoBehaviour
{
    [Header("Cells Prafabs : ")]
    [SerializeField] private GameObject _stoneCellPrefab;
    [SerializeField] private GameObject _dirtCellPrefab;
    [SerializeField] private GameObject _hardStoneCellPrefab;
    [SerializeField] private GameObject _mineralCellPrefab;

    [Header("Terrain : ")]
    [SerializeField] private Vector2Int _size;

    [Header("Stone :")]
    [SerializeField] private float _stoneSeed;
    [SerializeField, Range(0, 1)] private float _stoneNoiseFrequency;
    [SerializeField, Range(0, 1)] private float _stoneSpawnThresold;
    [SerializeField] private Texture2D _stoneTexture;

    [Header("Hard Stone :")]
    [SerializeField] private float _hardStoneSeed;
    [SerializeField, Range(0, 1)] private float _hardStoneNoiseFrequency;
    [SerializeField, Range(0, 1)] private float _hardStoneSpawnThresold;
    [SerializeField] private Texture2D _hardStoneTexture;

    [Header("Mineral :")]
    [SerializeField] private float _mineralSpawnChance = .01f;

    private GameObject[,] _cellArray;


    private void Start()
    {
        _cellArray = new GameObject[_size.x, _size.y];

        GenerateNoiseTextures();

        SpawnStoneCells();
        SpawnHardStoneCells();

        MineralCheck();

        // print(IsInRange(new Vector2Int(-1, -1)) ? "In range" : "Not in Range");
        // print(IsInRange(new Vector2Int(1, 1)) ? "In range" : "Not in Range");
    }

    private void OnValidate()
    {
        GenerateNoiseTextures();
    }

    private void GenerateNoiseTextures()
    {
        _hardStoneSeed = Random.Range(-10000, 10000);
        _stoneSeed = Random.Range(-10000, 10000);

        _stoneTexture = new Texture2D(_size.x, _size.y);
        _hardStoneTexture = new Texture2D(_size.x, _size.y);
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                float pixel = 0;
                Vector2 perlinPos = new Vector2(x + _stoneSeed, y + _stoneSeed) * _stoneNoiseFrequency;
                pixel = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);
                _stoneTexture.SetPixel(x, y, new Color(pixel, pixel, pixel));

                perlinPos = new Vector2(x + _hardStoneSeed, y + _hardStoneSeed) * _hardStoneNoiseFrequency;
                pixel = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);
                _hardStoneTexture.SetPixel(x, y, new Color(pixel, pixel, pixel));
            }
        }

        _stoneTexture.Apply();
        _hardStoneTexture.Apply();
    }

    private void SpawnStoneCells()
    {
        LoopInCellArray((x, y) =>
        {
            if (_stoneTexture.GetPixel(x, y).r > _stoneSpawnThresold)
            {
                SpawnCell(new Vector2Int(x, y), _stoneCellPrefab);
            }
        });
    }

    private void SpawnHardStoneCells()
    {
        LoopInCellArray((x, y) =>
        {
            if (_hardStoneTexture.GetPixel(x, y).r > _hardStoneSpawnThresold)
            {
                SpawnCell(new Vector2Int(x, y), _hardStoneCellPrefab);
            }
        });
    }

    private void SpawnCell(Vector2Int position, GameObject prefab)
    {
        if (_cellArray[position.x, position.y] != null)
            Destroy(_cellArray[position.x, position.y]);

        GameObject newCell = _cellArray[position.x, position.y] = Instantiate(prefab, transform);
        newCell.transform.localPosition = new Vector3(position.x, 0, position.y);
    }

    private void MineralCheck()
    {
        LoopInCellArray((x, y) =>
        {
            if (_cellArray[x, y] && Random.value < _mineralSpawnChance)
            {
                SpawnMineralVein(new Vector2Int(x, y));
            }
        });
    }

    private void SpawnMineralVein(Vector2Int spawnPos)
    {
        int stop = 0;
        Vector2Int currentPos = spawnPos;

        SpawnMineralCell(currentPos);

        while (stop < 5)
        {
            stop++;

            Vector2Int randomDir = GetRandomDirection();
            Vector2Int newPos = currentPos + randomDir;

            if (IsInRange(newPos) && _cellArray[newPos.x, newPos.y] != null)
            {
                SpawnCell(currentPos, _mineralCellPrefab); 
            }
            else
                break;

            currentPos = newPos;
        }
    }

    private Vector2Int GetRandomDirection()
    {
        float random = Random.value;
        Vector2Int toReturn = Vector2Int.up;
        if(random < .25f)
            toReturn = Vector2Int.up;
        else if(random < .5f)
            toReturn = Vector2Int.up;
        else if(random < .85f)
            toReturn = Vector2Int.up;
        else if(random < 1f)
            toReturn = Vector2Int.up;
        
        print(toReturn);
        return toReturn;
    }

    private void SpawnMineralCell(Vector2Int positionToSpawn)
    {
        Destroy(_cellArray[positionToSpawn.x, positionToSpawn.y]);
        GameObject newCell = _cellArray[positionToSpawn.x, positionToSpawn.y] = Instantiate(_mineralCellPrefab, transform);
        newCell.transform.localPosition = new Vector3(positionToSpawn.x, 0, positionToSpawn.y);
    }

    [ContextMenu("Regenerate Mine")]
    public void RegenerateMine()
    {
        DeleteMine();
        SpawnStoneCells();
    }

    [ContextMenu("Delete Mine")]
    private void DeleteMine()
    {
        LoopInCellArray((x, y) =>
        {
            if (_cellArray[x, y])
                Destroy(_cellArray[x, y]);
        });
    }

    private void LoopInCellArray(System.Action<int, int> action)
    {
        for (int x = 0; x < _cellArray.GetLength(0); x++)
        {
            for (int y = 0; y < _cellArray.GetLength(1); y++)
            {
                action.Invoke(x, y);
            }
        }
    }

    private bool IsInRange(Vector2Int posToCheck)
    {
        try
        {
            if (_cellArray[posToCheck.x, posToCheck.y]) { };
        }
        catch (System.Exception)
        {
            return false;
        }
        return true;
    }
}
