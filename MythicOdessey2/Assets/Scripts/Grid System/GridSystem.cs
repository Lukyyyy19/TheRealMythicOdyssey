using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem <T>{
    //variables
    private int _width;
    private int _height;
    private T[,] _gridArray;
    private float _cellSize;
    private Vector3 _originPosition;
    public delegate void OnGridValueChangedDelegate(int x, int y);
    public event OnGridValueChangedDelegate onGridValueChanged;
    public GridSystem(int width, int height, float cellSize, Vector3 originPosition,Func<GridSystem<T>,int , int, T> createGridObject){
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;
        
        _gridArray = new T[width, height];
        
        // for loops
        for (int x = 0; x < _gridArray.GetLength(0); x++){
            for (int y = 0; y < _gridArray.GetLength(1); y++){
                Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x,y+1),Color.white,100f);
                Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y),Color.white,100f);
                _gridArray[x, y] = createGridObject(this,x,y);
            }
        }
        Debug.DrawLine(GetWorldPosition(0,_height),GetWorldPosition(_width,_height),Color.white,100f);
        Debug.DrawLine(GetWorldPosition(_width,0),GetWorldPosition(_width,_height),Color.white,100f);
    }

    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x,0,y) * _cellSize + _originPosition;
    }
    //GetXY
    public void GetXY(Vector3 worldPosition, out int x, out int y){
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).z / _cellSize);
    }

    public void SetValue(int x, int y, T value){
        //ignore out of range
        if (x < 0 || y < 0 || x >= _width || y >= _height) return;
        _gridArray[x, y] = value;
        if(onGridValueChanged != null) onGridValueChanged(x,y);
    }
    //set value world position
    public void SetValue(Vector3 worldPosition, T value){
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
    
    //GetValue
    public T GetValue(int x, int y){
        //ignore out of range
        if (x < 0 || y < 0 || x >= _width || y >= _height) return default(T);
        return _gridArray[x, y];
    }
    //get value world position
    public T GetValue(Vector3 worldPosition){
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}