using UnityEngine;

//! conflit Systeme.Random et UnityEngine.Random, mais besion de Systeme pour Action 

public class MineGenerator : MonoBehaviour
{
    [Header("Cells Prafabs : ")]
    [SerializeField] private TerrainCell _stoneCellPrefab;
    [SerializeField] private TerrainCell _dirtCellPrefab;
    [SerializeField] private TerrainCell _hardStoneCellPrefab;
    [SerializeField] private TerrainCell _mineralCellPrefab;
    [SerializeField] private TerrainCell _wallCellPrefab;

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

    public AnimationCurve _perlinCurveTest;

    [SerializeField, Range(0f, 1f)] private float _whiteAreaRangeRatio = 5;
    public float _whiteAreaRange;
    [SerializeField] private Vector2 _whiteAreaSpawnRatio;
    [SerializeField] private Vector2Int _whiteAreaSpawnPosition;
    [SerializeField, Range(0, 50)] int _startAreaSize;

    private TerrainCell[,] _cellArray;


    private void Start()
    {
        _cellArray = new TerrainCell[_size.x, _size.y];

        _whiteAreaRange = Mathf.Lerp(0, Vector2.Distance(_size / 2, _size), _whiteAreaRangeRatio);

        SpawnWallCells();

        GenerateNoiseTextures();

        SpawnStoneCells();
        SpawnHardStoneCells();

        MineralCheck();

        // print(IsInRange(new Vector2Int(-1, -1)) ? "In range" : "Not in Range");
        // print(IsInRange(new Vector2Int(1, 1)) ? "In range" : "Not in Range");
    }

    private void OnValidate()
    {
        // _whiteAreaSpawnRatio = Vector2.ClampMagnitude(_whiteAreaSpawnRatio, 1);

        _whiteAreaSpawnPosition.x = (int)Mathf.Lerp(0, _size.x, _whiteAreaSpawnRatio.x);
        _whiteAreaSpawnPosition.y = (int)Mathf.Lerp(0, _size.y, _whiteAreaSpawnRatio.y);

        GenerateNoiseTextures();
        _whiteAreaRange = Mathf.Lerp(0, Vector2.Distance(_size / 2, _size), _whiteAreaRangeRatio);
    }

    private void GenerateNoiseTextures()
    {
        _hardStoneSeed = Random.Range(0, 500);
        _stoneSeed = Random.Range(0, 500);

        _stoneTexture = new Texture2D(_size.x, _size.y);
        _hardStoneTexture = new Texture2D(_size.x, _size.y);
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                float pixel = 0;

                Vector2 perlinPos = new Vector2(x + _stoneSeed, y + _stoneSeed) * _stoneNoiseFrequency;

                //! Deforme la texture avec un remap horrible (15min a relire et recomprendre ce passage... nik toi Arthur du passé)
                float whiteAreaCenterDistance = Vector2Int.Distance(_whiteAreaSpawnPosition, _size);
                float curentDistance = Vector2Int.Distance(_whiteAreaSpawnPosition, new Vector2Int(x, y));
                perlinPos *= _perlinCurveTest.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, whiteAreaCenterDistance, curentDistance)));

                //! Set la couleur du pixel de la texture
                pixel = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);
                if (Vector2.Distance(_whiteAreaSpawnPosition, new Vector2(x, y)) < _whiteAreaRange)
                    pixel = 0;

                //! Definit la zone de depart
                if (Vector2Int.Distance(new Vector2Int(x, y), Vector2Int.zero) < _startAreaSize)
                    pixel = 0;

                //! Valide les nouvelles data des textures
                _stoneTexture.SetPixel(x, y, new Color(pixel, pixel, pixel));

                perlinPos = new Vector2(x + _hardStoneSeed, y + _hardStoneSeed) * _hardStoneNoiseFrequency;
                pixel = Mathf.PerlinNoise(perlinPos.x, perlinPos.y);
                _hardStoneTexture.SetPixel(x, y, new Color(pixel, pixel, pixel));
            }
        }

        _stoneTexture.Apply();
        _hardStoneTexture.Apply();
    }

    private void SpawnWallCells()
    {
        LoopInCellArray((x, y) =>
        {
            if (x == 0 || x == _size.x - 1 || y == 0 || y == _size.y - 1)
            {
                if (Vector2Int.Distance(new Vector2Int(x, y), Vector2Int.zero) > _startAreaSize)
                    SpawnCell(new Vector2Int(x, y), _wallCellPrefab);
            }
        });
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
            if (Vector2.Distance(_whiteAreaSpawnPosition, new Vector2(x, y)) > _whiteAreaRange)
            {
                if (_hardStoneTexture.GetPixel(x, y).r > _hardStoneSpawnThresold)
                    SpawnCell(new Vector2Int(x, y), _hardStoneCellPrefab);
            }
        });
    }

    private void SpawnCell(Vector2Int position, TerrainCell cellToSpawn)
    {
        if(_cellArray[position.x, position.y] && _cellArray[position.x, position.y].Type == CellType.Wall)
            return;


        if (_cellArray[position.x, position.y] != null)
            Destroy(_cellArray[position.x, position.y].gameObject);

        TerrainCell newCell = Instantiate(cellToSpawn.gameObject, transform).GetComponent<TerrainCell>();
        _cellArray[position.x, position.y] = newCell;

        newCell.transform.localPosition = new Vector3(position.x, 0, position.y);

        if(newCell.Type == CellType.Stone)
            newCell.Initialise(_stoneTexture.GetPixel(position.x, position.y).r);
            
        if(newCell.Type == CellType.HardStone)
            newCell.Initialise(_hardStoneTexture.GetPixel(position.x, position.y).r);
            
        if(newCell.Type == CellType.Wall)
            newCell.Initialise(2);
    
        if(newCell.Type == CellType.Mineral)
            newCell.Initialise(1);
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

        SpawnCell(spawnPos, _mineralCellPrefab);

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
        if (random < .25f)
            toReturn = Vector2Int.up;
        else if (random < .5f)
            toReturn = Vector2Int.down;
        else if (random < .85f)
            toReturn = Vector2Int.left;
        else if (random < 1f)
            toReturn = Vector2Int.right;

        // print(toReturn);
        return toReturn;
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
