using UnityEditor;
using UnityEngine.Tilemaps;
using UnityEngine;

public class IsometricCellPreviewTool : EditorWindow
{

    [SerializeField]
    private Tilemap _groundTilemap;

    [SerializeField]
    private UnityEngine.Tilemaps.Tile _overlayTiles;
    private Tilemap _overlayTilemap;

    private Vector3Int _hoveredCell;
    private bool _hasCell;

    [MenuItem("Xbraxy/Grid/Isometric Cell Preview")]
    public static void Open()
    {
        GetWindow<IsometricCellPreviewTool>("Iso Cell Preview");
    }

    private void OnGUI()
    {
        GUILayout.Label("Isometric Cell Preview", EditorStyles.boldLabel);

        _groundTilemap = (Tilemap)EditorGUILayout.ObjectField("TilemapGround", _groundTilemap, typeof(Tilemap), true);

        _overlayTilemap = (Tilemap)EditorGUILayout.ObjectField("Tilemap", _overlayTilemap, typeof(Tilemap), true);

        _overlayTiles = (Tile)EditorGUILayout.ObjectField("Tile template", _overlayTiles, typeof(TileBase), true);

        if (GUILayout.Button("Detect Tilemap"))
            DetectTilemap();

        if (_overlayTilemap == null)
            EditorGUILayout.HelpBox("Select a Tilemap and click Detect", MessageType.Info);
        else
            EditorGUILayout.LabelField("Tilemap:", _overlayTilemap.name);
    }

    private void DetectTilemap()
    {
        if (Selection.activeGameObject == null)
            return;

        _groundTilemap = Selection.activeGameObject.GetComponent<Tilemap>();
        _overlayTilemap = Selection.activeGameObject.GetComponent<Tilemap>();
        _overlayTiles = Selection.activeGameObject.GetComponent<Tile>();


        if (_overlayTilemap == null)
        {
            Debug.Log("[DetectTilemap] NOT Tilemap detected");
            return;
        }

        Debug.Log("[DetectTilemap] Tilemap detected");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    // CLAVE para isométrico + Z as Y

    private void DrawCellPreview()
    {

        if (!_hasCell || _overlayTilemap == null)
            return;

        Vector3 center = _overlayTilemap.GetCellCenterWorld(_hoveredCell);
        // Debug.Log($"Center hovered cell {center}");
        Vector3 anchorOffset = _overlayTilemap.tileAnchor;
        // Debug.Log($"AnchorOffset {anchorOffset}");
        Vector3 size = _overlayTilemap.cellSize;
        // Debug.Log($"GridCellSize {size}");

        float halfX = size.x * 0.5f;
        float halfY = size.y * 0.5f;
        float isoYOffset = size.y * 0.5f;
        Vector3 baseCenter = center + Vector3.up * isoYOffset;

        Vector3[] diamond =  {
            baseCenter + new Vector3(0,  halfY, 0),
            baseCenter + new Vector3( halfX, 0, 0),
            baseCenter + new Vector3(0, -halfY, 0),
            baseCenter + new Vector3(-halfX, 0, 0),
            baseCenter + new Vector3(0,  halfY, 0)
        };
        Handles.color = Color.cyan;
        Handles.DrawAAPolyLine(2f, diamond);
    }

    private bool DetectGroundTilemap(Event e)
    {
        if (e.type != EventType.MouseDown || e.button != 0)
            return false;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition + Vector2.down * 0.5f);

        Plane plane = new Plane(Vector3.forward, _groundTilemap.transform.position);

        if (!plane.Raycast(ray, out float distance))
        {
            _hasCell = false;
            return false;
        }

        Vector3 worldPos = ray.GetPoint(distance);
        Vector3Int cell = _groundTilemap.WorldToCell(worldPos);

        _hoveredCell = cell;
        _hasCell = true;

        TileBase tile = _groundTilemap.GetTile(cell);

        if (tile == null)
        {
            Debug.LogWarning($"Cell {cell} → HAS NO TILE"); ;
            return false;
            //#if UNITY_EDITOR
            //            bool continueAction = EditorUtility.DisplayDialog(
            //                "No Tile in Ground Tilemap Detected",
            //                "No tile was detected in the selected cell.\n\nAre you sure you want to continue?",
            //                "Continue",
            //                "Cancel"
            //            );

            //            return continueAction;
            //#else
            //        return false;
            //#endif
        }
        Debug.Log($"Cell {cell} → HAS TILE"); ;
        e.Use();
        return true;
    }
    private void HandleClick(Event e)
    {

        if (DetectGroundTilemap(e))
        {
            //Debug.Log("OK CLICK");
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (_groundTilemap == null || _overlayTilemap == null)
            return;

        HandleUtility.AddDefaultControl(
            GUIUtility.GetControlID(FocusType.Passive)
        );

        Event e = Event.current;
        DrawCellPreview();
        HandleClick(e);

        sceneView.Repaint();
    }

}
