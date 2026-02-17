using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Engine.Grid.CellTypes;
using Assets.Engine.Grid;
using Assets.Editor;
using Assets.Editor.Utilites;
using System.Linq;
using Unity.VisualScripting;

public class GridWindowCreator : EditorWindow
{
    private enum SelectionMode
    {
        None,
        SelectSingle,
        SelectArea,
        Erase,
        EraseArea,
        EraseAll,
        Detect
    }

    private SelectionMode _selectionMode = SelectionMode.None;
    private Tool _previousTool;
    //--- GRID & TILEMAPS

    private Grid _gridScene;
    private Tilemap _outputOverlayTileMap;
    private LogicalGridSO _logicalGrid;

    //--- CURSOR CELL SELECTION ---
    private readonly List<Vector3Int> _selectedCells = new();
    private Vector3Int _startCell;
    private Vector3Int _currentCell;
    private bool _isDragging;

    //--- CELL TYPES SO INSEPCTOR ---
    [SerializeField]
    private CellTypeSO[] _cellTypesArr;

    [SerializeField]
    private CellTypeSO _selectedCellType;
    private SerializedObject _so;
    private SerializedProperty _configsProp;


    //--- UI 
    private const float ButtonHeight = 30f;
    private const float ButtonSpacing = 6f;
    private bool _showFieldsCellTypesSO;
    private bool _showListCellTypesAttached;
    private bool _selectTilesAllow;

    [MenuItem("Xbraxy/Grid/Isometric Cell Preview")]
    public static void Open()
    {
        GetWindow<GridWindowCreator>("Grid Braker");
    }

    private void OnEnable()
    {
        _previousTool = Tools.current;
        if(Tools.current.IsUnityNull())
        {
            Tools.current = Tool.None;
        }

        TryFindGridInScene();
        _selectedCells.Clear();

        _so = new SerializedObject(this);
        _configsProp = _so.FindProperty("_cellTypesArr");

        SceneView.duringSceneGui += OnSceneGUI;
    }

    #region "OnOverride"
    private void OnDisable()
    {
        Tools.current = _previousTool;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnFocus()
    {
        _previousTool = Tools.current;
        Tools.current = Tool.None;
        SceneView.duringSceneGui += OnSceneGUI;

    }
    private void OnGUI()
    {
        if (_so == null)
        {
            _so = new SerializedObject(this);
            _configsProp = _so.FindProperty("_cellTypesArr");
        }

        GUILayout.Space(10);

        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
        titleStyle.fontSize = 18;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("GRID TOOL", titleStyle);

        //--- 1. DISPLAY: ARRAY CELLTYPES SO TO ATTACH ---//
       
        GUILayout.Space(10);
        _so.Update();
        _showFieldsCellTypesSO = MyGroupBoxUI.DrawFoldoutBox("1. Attach CellTypes SO",
            _showFieldsCellTypesSO,
            () =>
            {
                EditorGUILayout.PropertyField(_configsProp, true);
            });
        _so.ApplyModifiedProperties();
        // _cellTypes = (CellTypeScriptableObject)EditorGUILayout.ObjectField("Tile template", _cellTypes, typeof(CellTypeScriptableObject), true);


        //---2 DISPLAY: SELECT CELL TPYES LIST + SELECTED CELL DATA ---//
        _showListCellTypesAttached = MyGroupBoxUI.DrawFoldoutBox("2. Select CellType",
            _showListCellTypesAttached,
            () =>
            {
                if (_cellTypesArr != null)
                {
                    for (int i = 0; i < _cellTypesArr.Length; i++)
                    {
                        var cell = _cellTypesArr[i];
                        if (cell == null) continue;

                        bool isSelected = (cell == _selectedCellType);
                        GUIStyle style = isSelected ? MyStyles.Selected : MyStyles.Normal;
                        GUILayout.Space(isSelected ? 3 : 2);

                        if (GUILayout.Button(cell.name, style))
                        {
                            _selectedCellType = cell;
                        }
                    }


                    //--- 2.1. DISPLAY DATA FROM SO SELECTED ---//
                    if (_selectedCellType != null)
                    {
                        EditorGUILayout.Space(10);
                        GUILayout.Label($"CellType {_selectedCellType.name} data", EditorStyles.boldLabel);

                        using (new EditorGUI.DisabledScope(true)) // Hace todo no editable
                        {
                            EditorGUILayout.ObjectField(
                                "Overlay Tile",
                                _selectedCellType.OverlayTile,
                                typeof(TileBase),
                                false);

                            EditorGUILayout.EnumPopup(
                                "Terrain Type",
                                _selectedCellType.TerrainType);

                            EditorGUILayout.IntField(
                                "Move Cost",
                                _selectedCellType.MoveCost);

                            EditorGUILayout.Toggle(
                                "Walkable",
                                _selectedCellType.Walkable);

                            EditorGUILayout.IntField(
                                "Height",
                                _selectedCellType.Height);
                        }
                    }
                }
            });
        //EditorGUILayout.Space(10);

        //---3 ALLOW SELECT GIRD MAP CELLS
        _selectTilesAllow = MyGroupBoxUI.DrawFoldoutBox("3. Selection enable", _selectTilesAllow, () =>
        {
            float width = EditorGUIUtility.currentViewWidth - 40;
            float buttonWidth2 = (width - ButtonSpacing) / 2f;
            float buttonWidth3 = (width - ButtonSpacing * 2) / 3f;

            // --- SELECT ---
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Single", GUILayout.Width(buttonWidth2), GUILayout.Height(ButtonHeight)))
                _selectionMode = SelectionMode.SelectSingle;

            if (GUILayout.Button("Select Area", GUILayout.Width(buttonWidth2), GUILayout.Height(ButtonHeight)))
                _selectionMode = SelectionMode.SelectArea;
            GUILayout.EndHorizontal();

            GUILayout.Space(8);

            // --- ERASE ---
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Erase Cell", GUILayout.Width(buttonWidth3), GUILayout.Height(ButtonHeight)))
                _selectionMode = SelectionMode.Erase;

            if (GUILayout.Button("Erase Area", GUILayout.Width(buttonWidth3), GUILayout.Height(ButtonHeight)))
                _selectionMode = SelectionMode.EraseArea;

            if (GUILayout.Button("Erase Selection", GUILayout.Width(buttonWidth3), GUILayout.Height(ButtonHeight)))
            {
                _selectionMode = SelectionMode.EraseAll;
                _selectedCells.Clear();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(8);

            // --- DETECT ---
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUI.backgroundColor = _selectionMode == SelectionMode.Detect ? Color.aliceBlue : Color.white;
            if (GUILayout.Button("Detect Tile", GUILayout.Width(buttonWidth2), GUILayout.Height(ButtonHeight)))
                _selectionMode = SelectionMode.Detect;
            GUI.backgroundColor = Color.aliceBlue;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        });


        //--- 4 OUTPUT GO
        GUILayout.Space(10);
        if (GUILayout.Button("Create grid"))
        {
            if (_selectedCells == null || _selectedCells.Count == 0)
            {
                Debug.LogWarning("No cells selected");
                return;
            }

            if (_selectedCellType == null)
            {
                Debug.LogWarning("No CellType selected");
            }

            if (_selectedCellType.OverlayTile == null)
            {
                Debug.LogWarning("The selected CellType has no TileBase attached");
                return;
            }

            FindOverlayOutput();
            AttachOverlayTilesToTilemap();
        }
    }

    #endregion
    private void TryFindGridInScene()
    {
        if (_gridScene == null)
        {
            _gridScene = FindAnyObjectByType<Grid>();
            if (_gridScene != null)
            {
                Debug.Log($"Grid selected: {_gridScene.name}");
            }
        }
    }


    // TileBase tile = _groundTilemap.GetTile(_hoveredCell);

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        Rect fullRect = new Rect(0, 0, sceneView.position.width, sceneView.position.height);
        EditorGUIUtility.AddCursorRect(fullRect, MouseCursor.Arrow);

        if (!TryGetMouseCursorPosition(e.mousePosition, out var mousePos))
        {
            return;
        }
        if (!TryParseWorldPosToCellPos(mousePos, out var cellPos))
        {
            return;
        }

        var cellCenterPos = GetCenterCell(cellPos);

        if(_selectTilesAllow && _selectionMode != SelectionMode.None)
        {
            DrawCellInGrid(cellCenterPos, Color.white);

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                _startCell = cellPos;
                _currentCell = cellPos;
                _isDragging = true;
                ApplySelection(cellPos);
                e.Use();
            }

            if (e.type == EventType.MouseDrag && _isDragging)
            {
                _currentCell = cellPos;
                ApplySelection(cellPos);
                e.Use();
            }

            if (e.type == EventType.MouseUp && e.button == 0)
            {
                _isDragging = false;
                e.Use();
            }

            foreach (var cell in _selectedCells)
            {
                DrawCellInGrid(GetCenterCell(cell), Color.cyan);
            }
        }



        sceneView.Repaint();
    }



    private bool TryGetMouseCursorPosition(in Vector2 mousePosition, out Vector3 worldMousePosition)
    {
        worldMousePosition = new Vector3();
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        Plane plane = new Plane(Vector3.forward, _gridScene.transform.position);
        if (!plane.Raycast(ray, out float distance))
        {
            //   _hasCell = false;
            return false;
        }
        worldMousePosition = ray.GetPoint(distance);
        return true;
    }

    private bool TryParseWorldPosToCellPos(in Vector2 worldPos, out Vector3Int cellPos)
    {
        cellPos = new Vector3Int();
        try
        {
            cellPos = _gridScene.WorldToCell(worldPos);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
        return true;
    }

    private Vector3 GetCenterCell(Vector3Int cellPosition)
    {
        return _gridScene.GetCellCenterWorld(cellPosition);
    }
    private void ApplySelection(Vector3Int cellPos)
    {
        switch (_selectionMode)
        {
            case SelectionMode.SelectSingle:
                SelectSingleCell(cellPos);
                break;

            case SelectionMode.SelectArea:
                SelectAreaCell();
                break;

            case SelectionMode.Erase:
                EraseSingleCell(cellPos);
                break;

            case SelectionMode.EraseArea:
                EraseAreaCell();
                break;
            case SelectionMode.Detect:
                Debug.Log("Detect mode not implemented");
                break;
        }
    }

    private void SelectSingleCell(Vector3Int cellPosition)
    {
        if (!_selectedCells.Contains(cellPosition))
        {
            _selectedCells.Add(cellPosition);
        }
    }

    private void EraseSingleCell(Vector3Int cellPosition)
    {
        _selectedCells.Remove(cellPosition);
    }
    private void SelectAreaCell()
    {
       // _selectedCells.Clear();

        int minX = Mathf.Min(_startCell.x, _currentCell.x);
        int maxX = Mathf.Max(_startCell.x, _currentCell.x);
        int minY = Mathf.Min(_startCell.y, _currentCell.y);
        int maxY = Mathf.Max(_startCell.y, _currentCell.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var cellPosition = new Vector3Int(x, y, _startCell.z);
                if(!_selectedCells.Contains(cellPosition))
                {
                    _selectedCells.Add(cellPosition);
                }
            }
        }
    }

    private void EraseAreaCell()
    {
        int minX = Mathf.Min(_startCell.x, _currentCell.x);
        int maxX = Mathf.Max(_startCell.x, _currentCell.x);
        int minY = Mathf.Min(_startCell.y, _currentCell.y);
        int maxY = Mathf.Max(_startCell.y, _currentCell.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                var cellPosition = new Vector3Int(x, y, _startCell.z);
                if(_selectedCells.Contains(cellPosition))
                    _selectedCells.Remove(cellPosition);
            }
        }
    }

    private void DrawCellInGrid(Vector3 cellCenter, Color cellColor)
    {
        Vector3 size = _gridScene.cellSize;
        float halfX = size.x * 0.5f;
        float halfY = size.y * 0.5f;
        float isoYOffset = size.y * 0.25f;
        Vector3 baseCenter = cellCenter + Vector3.down * isoYOffset;

        Vector3[] diamond =  {
            baseCenter + new Vector3(0, halfY, 0),
            baseCenter + new Vector3(halfX, 0, 0),
            baseCenter + new Vector3(0, -halfY, 0),
            baseCenter + new Vector3(-halfX, 0, 0),
            baseCenter + new Vector3(0,  halfY, 0)
        };

        Handles.color = cellColor;
        Handles.DrawAAPolyLine(2f, diamond);
    }


    private void FindOverlayOutput()
    {
        Tilemap groundTilemap;

        // Buscar todos los Tilemaps hijos de la Grid
        Tilemap[] tilemaps = _gridScene.GetComponentsInChildren<Tilemap>();

        // Intentar encontrar uno llamado "OverlayTilemap"
        foreach (Tilemap tm in tilemaps)
        {
            if (tm.name == "OverlayTilemap")
            {
                _outputOverlayTileMap = tm;
                return;
            }

            if (tm.name == "Ground")
            {
                groundTilemap = tm;
            }
        }


        // Si no existe, lo creamos
        GameObject overlayGO = new GameObject("OverlayTilemap");
        overlayGO.transform.SetParent(_gridScene.transform);

        // Reset local transform
        overlayGO.transform.localPosition = Vector3.zero;
        overlayGO.transform.localRotation = Quaternion.identity;
        overlayGO.transform.localScale = Vector3.one;

        // Añadir componentes necesarios
        _outputOverlayTileMap = overlayGO.AddComponent<Tilemap>();
        _outputOverlayTileMap.tileAnchor = new Vector3(0.5f, 0.5f, 0);
        overlayGO.AddComponent<TilemapRenderer>();

        overlayGO.GetComponent<TilemapRenderer>().sortingLayerName = "EditorUI";
        Undo.RegisterCreatedObjectUndo(overlayGO, "Create Overlay Tilemap");
        Debug.Log("OverlayTilemap creado automáticamente.");
    }

    private void AttachOverlayTilesToTilemap()
    {
        if (_selectedCells == null || _selectedCells.Count == 0)
            return;

        TileBase tile = _selectedCellType.OverlayTile;

        Vector3Int[] positions = _selectedCells.ToArray();
        TileBase[] tiles = new TileBase[positions.Length];

        for (int i = 0; i < tiles.Length; i++)
            tiles[i] = tile;

        _outputOverlayTileMap.SetTiles(positions, tiles);
    }

}
