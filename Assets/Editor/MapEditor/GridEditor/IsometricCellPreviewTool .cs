using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using System.Collections.Generic;

public class IsometricCellPreviewTool : EditorWindow
{
    private Tool _previousTool;

    private Grid _gridScene;
    private Vector3 _cursorPositionFirstClick;
    private readonly List<Vector3Int> _selectedCells = new();
    private Vector3Int _startCell;
    private Vector3Int _currentCell;
    private bool _isDragging;


    [MenuItem("Xbraxy/Grid/Isometric Cell Preview")]
    public static void Open()
    {
        GetWindow<IsometricCellPreviewTool>("Iso Cell Preview");
    }

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
    private void OnEnable()
    {
        _previousTool = Tools.current;
        Tools.current = Tool.None;
        TryFindGridInScene();    
        _selectedCells.Clear();
        SceneView.duringSceneGui += OnSceneGUI;

    }
    private void OnDisable()
    {
        Tools.current = _previousTool;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {

        GUILayout.Label("Isometric Cell Preview", EditorStyles.boldLabel);

        //_overlayTiles = (TileBase)EditorGUILayout.ObjectField("Tile template", _overlayTiles, typeof(TileBase), true);

        if (GUILayout.Button("Detect Tilemap"))
        {
            //Tools.current = Tool.None;
         
        }
        //if (_overlayTilemap == null)
        //    EditorGUILayout.HelpBox("Select a Tilemap and click Detect", MessageType.Info);
        //else
        //    EditorGUILayout.LabelField("Tilemap:", _overlayTilemap.name);
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
        if(!TryParseWorldPosToCellPos(mousePos, out var cellPos))
        {
            return;
        }

        var cellCenterPos = GetCenterCell(cellPos);
        DrawCellInGrid(cellCenterPos, Color.white);

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            _startCell = cellPos;
            _currentCell = cellPos;
            _isDragging = true;
            UpdateSelection();
            e.Use();
        }

        if (e.type == EventType.MouseDrag && _isDragging)
        {
            _currentCell = cellPos;
            UpdateSelection();
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
            cellPos  = _gridScene.WorldToCell(worldPos);
        }
        catch(Exception ex)
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
    private void UpdateSelection()
    {
        _selectedCells.Clear();

        int minX = Mathf.Min(_startCell.x, _currentCell.x);
        int maxX = Mathf.Max(_startCell.x, _currentCell.x);
        int minY = Mathf.Min(_startCell.y, _currentCell.y);
        int maxY = Mathf.Max(_startCell.y, _currentCell.y);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                _selectedCells.Add(new Vector3Int(x, y, _startCell.z));
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
}
