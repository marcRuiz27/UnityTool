using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Engine.Grid.EditorData
{
    public class TilemapCellEditorData : MonoBehaviour
    {
        //[SerializeField]
        //private Tilemap _tilemap;

        //[SerializeField]
        //private List<Vector3Int> _listPositions = new();
        //[SerializeField]
        //private List<CellEditorData> _listCells = new();

        //private Dictionary<Vector3Int, CellEditorData> _cache;

        //public Tilemap Tilemap => _tilemap;

        //private void OnEnable()
        //{
        //    if (_tilemap == null)
        //        _tilemap = GetComponent<Tilemap>();

        //    BuildCache();
        //}

        //private void BuildCache()
        //{
        //    _cache = new Dictionary<Vector3Int, CellEditorData>();
        //    for (int i = 0; i < _listPositions.Count; i++)
        //        _cache[_listPositions[i]] = _listCells[i];
        //}

        //public void SetCell(Vector3Int pos, CellEditorData data)
        //{
        //    if (_cache.ContainsKey(pos))
        //    {
        //        _cache[pos] = data;
        //        int idx = _listPositions.IndexOf(pos);
        //        _listCells[idx] = data;
        //    }
        //    else
        //    {
        //        _listPositions.Add(pos);
        //        _listCells.Add(data);
        //        _cache[pos] = data;
        //    }
        //}

        //public bool TryGetCell(Vector3Int pos, out CellEditorData data)
        //{
        //    return _cache.TryGetValue(pos, out data);
        //}
    }
}
