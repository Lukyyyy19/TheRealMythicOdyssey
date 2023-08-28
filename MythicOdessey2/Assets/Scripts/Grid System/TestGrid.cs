using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public CardsTypeSO prefab;
    private GridSystem<GridObject> grid;
    void Start()
    {
        grid = new GridSystem<GridObject>(10,10,10,Vector3.zero,(GridSystem<GridObject> g,int x, int z)=> new GridObject(g,x,z));    
    }

    class GridObject {
        private GridSystem<GridObject> grid;
        private int x;
        private int z;
        private Transform transform;
        //transform getter and setter
        public Transform Transform{
            get => transform;
            set{
                if(transform == value)return;
                transform = value;
                
                // if(grid.onGridValueChanged != null)
                //     grid.onGridValueChanged(x,z);
            }
        }
        public GridObject(GridSystem<GridObject> grid, int x, int z){
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        
        public bool CanBuild(){
            return transform == null;
        }
    }
    private void OnEnable(){
        EventManager.instance.AddAction("OnCardTrigger",(objects => {
            grid.GetXY(Helper.GetMouseWorldPosition(),out int x, out int z);
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
            if(canBuild)
            {
                Transform built;
            //+ new Vector3(_cellSize,0,_cellSize)*.5f es porque no tenemos el anchor point una vez que el objeto lo tenga sacamos esa parte del codigo
                built = Instantiate(prefab.prefab, grid.GetWorldPosition(x,z)+ new Vector3(10,0,10)*.5f, Quaternion.identity);
                foreach (var gridPos in gridPositionList) 
                {
                    grid.GetValue(gridPos.x,gridPos.y).Transform = built;
                }
            }

            else
            {
                Debug.Log("No se puede construir");
            }
        } ));
    }
}
