using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class TestGrid : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject planePrefab;
    private GridSystem<GridObject> grid;
    private Dictionary<Vector2Int, GameObject> _ghostPlanes = new Dictionary<Vector2Int, GameObject>();
    public List<Transform> startTiles = new List<Transform>();
    public static TestGrid instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        grid = new GridSystem<GridObject>(10, 10, 10, Vector3.zero, (GridSystem<GridObject> g, int x, int z) => new GridObject(g, x, z));
        var planesParent = new GameObject("Planes");
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                var plane = Instantiate(planePrefab, grid.GetWorldPosition(x, y) + new Vector3(10, 0, 10) * .5f, Quaternion.identity, planesParent.transform);
                plane.SetActive(false);
                _ghostPlanes.Add(new Vector2Int(x, y), plane);

            }
        }
        planesParent.transform.position += Vector3.up * .1f;
        foreach (var startTile in startTiles)
        {
            grid.GetXY(startTile.position, out int x, out int y);
            grid.GetValue(x, y).Transform = startTile;
            _ghostPlanes[new Vector2Int(x, y)].GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 43);
        }
    }

    class GridObject
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
    }

    private void BuildObject(CardsTypeSO prefab)
    {
        grid.GetXY(Helper.GetMouseWorldPosition(), out int x, out int z);
        var gridPositionList = prefab.GetGridPositionList(new Vector2Int(x, z));
        var gridObject = grid.GetValue(x, z);

        bool canBuild = true;
        foreach (var gridPos in gridPositionList)
        {
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
            built = Instantiate(prefab.prefab, grid.GetWorldPosition(x, z) + new Vector3(10, 0, 10) * .5f,
                Quaternion.identity);
            foreach (var gridPos in gridPositionList)
            {
                grid.GetValue(gridPos.x, gridPos.y).Transform = built;
                _ghostPlanes[gridPos].GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 43);
            }
            var builtNavMeshSurface = built.GetComponent<NavMeshSurface>();
            EventManager.instance.TriggerEvent("OnUpdateNavMesh", builtNavMeshSurface);
        }

        else
        {
            Debug.Log("No se puede construir");
        }
    }
}
