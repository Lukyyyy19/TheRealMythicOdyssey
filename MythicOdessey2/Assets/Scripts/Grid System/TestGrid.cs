using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class TestGrid : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject planePrefab;
    public GridSystem<GridObject> grid;
    private Dictionary<Vector2Int, GameObject> _ghostPlanes = new Dictionary<Vector2Int, GameObject>();
    public List<Transform> startTiles = new List<Transform>();
    public static TestGrid instance;
    [SerializeField,Range(0,15)]int _cellSize = 4;
    [SerializeField,Range(0,15)]int _width = 7;
    [SerializeField,Range(0,15)]int _height = 7;
    [SerializeField] Vector3 _originPosition;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        grid = new GridSystem<GridObject>(_width, _height, _cellSize, _originPosition, (GridSystem<GridObject> g, int x, int z) => new GridObject(g, x, z), true);
        var planesParent = new GameObject("Planes");
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if ((x == 0 && y == 0)) continue;
                else if(x==_width-1 && y == 0) continue;
                else if(x==0 && y == _height-1) continue;
                else if(x==_width-1 && y == _height-1) continue;
                var plane = Instantiate(planePrefab, grid.GetWorldPosition(x, y) + new Vector3(_cellSize, 0, _cellSize) * .5f, Quaternion.identity, planesParent.transform);
                plane.transform.localScale = Vector3.one*(_cellSize/10f);
                plane.SetActive(false);
                _ghostPlanes.Add(new Vector2Int(x, y), plane);

            }
        }
        planesParent.transform.position += Vector3.up * .1f;
        foreach (var startTile in startTiles)
        {
            Debug.Log(startTile.name);
            grid.GetXY(startTile.position, out int x, out int y);
            grid.GetValue(x, y).Transform = startTile;
            _ghostPlanes[new Vector2Int(x, y)].GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 43);
        }
    }

   public class GridObject
    {
        private GridSystem<GridObject> grid;
        private int x;
        private int z;
        private Transform transform;
        //transform getter and setter
        public Transform Transform
        {
            get => transform;
            set
            {
                if (transform == value) return;
                transform = value;

                // if(grid.onGridValueChanged != null)
                //     grid.onGridValueChanged(x,z);
            }
        }
        public GridObject(GridSystem<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public bool CanBuild()
        {
            return transform == null;
        }

        public void ResetValue(){
            transform = null;
        }
    }
    private void OnEnable()
    {
        EventManager.instance.AddAction("OnOpenMenu", (objects =>
        {
            bool open = (bool)objects[0];
            if (open)
            {
                foreach (var plane in _ghostPlanes)
                {
                    plane.Value.SetActive(true);
                }
            }
            else
            {
                foreach (var plane in _ghostPlanes)
                {
                    plane.Value.SetActive(false);
                }
            }
        }));
        EventManager.instance.AddAction("OnCardTrigger", (objects => { BuildObject((CardsTypeSO)objects[0]); }));
        EventManager.instance.AddAction("OnTrapDestroyed", (objects =>
        {
            Vector2Int pos = (Vector2Int)objects[0];
            //grid.GetXY((Vector3)objects[0], out int x, out int z);
            grid.GetValue(pos.x, pos.y).ResetValue();
            UpdateGhostPlaneColors(pos);
        }));
    }

    private void BuildObject(CardsTypeSO prefab)
    {
        grid.GetXY(Helper.GetMouseWorldPosition(), out int x, out int z);
        var gridPositionList = prefab.GetGridPositionList(new Vector2Int(x, z));
        var gridObject = grid.GetValue(x, z);

        bool canBuild = true;
        foreach (var gridPos in gridPositionList)
        {
            Debug.Log(grid.GetValue(gridPos.x, gridPos.y).Transform);
            if (!grid.GetValue(gridPos.x, gridPos.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            Transform built;
            //+ new Vector3(_cellSize,0,_cellSize)*.5f es porque no tenemos el anchor point una vez que el objeto lo tenga sacamos esa parte del codigo
            built = Instantiate(prefab.prefab, grid.GetWorldPosition(x, z) + new Vector3(_cellSize, 0.01f, _cellSize) * .5f,
                Quaternion.identity);
            built.transform.localScale = Vector3.one * (_cellSize / 10f);
            built.TryGetComponent(out BombTrap bombTrap);
            bombTrap._gridPosition = gridPositionList[0];
            foreach (var gridPos in gridPositionList)
            {
                grid.GetValue(gridPos.x, gridPos.y).Transform = built;
                Debug.Log(grid.GetValue(gridPos.x, gridPos.y).Transform.name);
                _ghostPlanes[gridPos].GetComponent<GhostPlane>().SetColor(true);
            }
            // var builtNavMeshSurface = built.GetComponent<NavMeshSurface>();
            // EventManager.instance.TriggerEvent("OnUpdateNavMesh", builtNavMeshSurface);
        }

        else
        {
            Debug.Log("No se puede construir");
            EventManager.instance.TriggerEvent("OnCantBuild");
        }
    }

   public void UpdateGhostPlaneColors(Vector2Int pos){
        _ghostPlanes[pos].GetComponent<GhostPlane>().SetColor(!grid.GetValue(pos.x,pos.y).CanBuild());
    }
}
