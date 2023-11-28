using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private Tilemap wallMap;
    [SerializeField]
    private Tilemap hightlightedMap;
    [SerializeField]
    private TileBase mark;

    [SerializeField]
    private List<Vector3Int> locations = new List<Vector3Int>();
    [SerializeField]
    private List<Vector3Int> wallLocations = new List<Vector3Int>();
    [SerializeField]
    private List<Vector3Int> hightlightedLocations = new List<Vector3Int>();

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GetMapLocation();
        GetWallsLocation();
    }

    public void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log(GetObjCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        //}
    }

    #region GetMapLocation
    private void GetMapLocation()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);

                if (tileMap.HasTile(localLocation))
                {
                    locations.Add(localLocation);
                }
            }
        }
    }

    private void GetWallsLocation()
    {

        for (int x = wallMap.cellBounds.xMin; x < wallMap.cellBounds.xMax; x++)
        {
            for (int y = wallMap.cellBounds.yMin; y < wallMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);

                Vector3 location = wallMap.GetCellCenterWorld(localLocation);
                if (wallMap.HasTile(localLocation))
                {
                    wallLocations.Add(localLocation);
                }
            }
        }

    }

    #endregion

    #region Movement

    public Vector3Int[] GetPositionsToMove(Vector3Int currentCellPos, ChessType chessType, bool isFirstMove)
    {
        switch (chessType)
        {
            case ChessType.Pawn:
                return GetPawnMovePos(currentCellPos, isFirstMove);
            case ChessType.Rook:
                return GetRookMovePos(currentCellPos);
            case ChessType.Knight:
                return GetKnightMovePos(currentCellPos);
            case ChessType.Bishop:
                return GetBishopMovePos(currentCellPos);
            case ChessType.Queen:
                return GetQueenMovePos(currentCellPos);
            case ChessType.King:
                return GetNear(currentCellPos);
        }

        return null;
    }

    private Vector3Int[] GetPawnMovePos(Vector3Int currentCellPos, bool isFirstMove)
    {
        List<Vector3Int> movePos = new List<Vector3Int>();
        Vector3Int nextTop = currentCellPos + new Vector3Int(0, 1, 0);
        movePos.Add(nextTop);   
        if (isFirstMove)
        {
            Vector3Int next2 = currentCellPos + new Vector3Int(0, 2, 0);
            movePos.Add(next2);
        }
        return movePos.ToArray();
    }

    private Vector3Int[] GetQueenMovePos(Vector3Int currentCellPos)
    {
        return GetBishopMovePos(currentCellPos).Concat(GetRookMovePos(currentCellPos)).ToArray();
    }

    private Vector3Int[] GetBishopMovePos(Vector3Int currentCellPos)
    {
        Vector3Int nextRightUp = currentCellPos + new Vector3Int(1, 1, 0);
        Vector3Int nextLeftUp = currentCellPos + new Vector3Int(-1, 1, 0);
        Vector3Int nextRightDown = currentCellPos + new Vector3Int(1, -1, 0);
        Vector3Int nextLeftDown = currentCellPos + new Vector3Int(-1, -1, 0);

        List<Vector3Int> movePos = new List<Vector3Int>();

        while (IsPlaceableArea(nextRightUp))
        {
            movePos.Add(nextRightUp);
            nextRightUp += new Vector3Int(1, 1, 0);
        }

        while (IsPlaceableArea(nextLeftUp))
        {
            movePos.Add(nextLeftUp);
            nextLeftUp += new Vector3Int(-1, 1, 0);
        }

        while (IsPlaceableArea(nextRightDown))
        {
            movePos.Add(nextRightDown);
            nextRightDown += new Vector3Int(1, -1, 0);
        }

        while (IsPlaceableArea(nextLeftDown))
        {
            movePos.Add(nextLeftDown);
            nextLeftDown += new Vector3Int(-1, -1, 0);
        }

        return movePos.ToArray();
    }

    private Vector3Int[] GetRookMovePos(Vector3Int currentCellPos)
    {
        Vector3Int nextRight = currentCellPos + new Vector3Int(1, 0, 0);
        Vector3Int nextLeft = currentCellPos + new Vector3Int(-1, 0, 0);
        Vector3Int nextUp = currentCellPos + new Vector3Int(0, 1, 0);
        Vector3Int nextDown = currentCellPos + new Vector3Int(0, -1, 0);

        List<Vector3Int> movePos = new List<Vector3Int>();

        while (IsPlaceableArea(nextRight))
        {
            movePos.Add(nextRight);
            nextRight += new Vector3Int(1, 0, 0);
        }
        while (IsPlaceableArea(nextLeft))
        {
            movePos.Add(nextLeft);
            nextLeft += new Vector3Int(-1, 0, 0);
        }
        while (IsPlaceableArea(nextUp))
        {
            movePos.Add(nextUp);
            nextUp += new Vector3Int(0, 1, 0);
        }
        while (IsPlaceableArea(nextDown))
        {
            movePos.Add(nextDown);
            nextDown += new Vector3Int(0, -1, 0);
        }

        return movePos.ToArray();
    }

    private Vector3Int[] GetKnightMovePos(Vector3Int currentCellPos)
    {
        Vector3Int rightPos = currentCellPos + new Vector3Int(2, 0, 0);
        Vector3Int leftPos = currentCellPos + new Vector3Int(-2, 0, 0);
        Vector3Int upPos = currentCellPos + new Vector3Int(0, 2, 0);
        Vector3Int downPos = currentCellPos + new Vector3Int(0, -2, 0);

        Vector3Int leftDown = leftPos + new Vector3Int(0, -1, 0);
        Vector3Int leftUp = leftPos + new Vector3Int(0, 1, 0);
        Vector3Int rightDown = rightPos + new Vector3Int(0, -1, 0);
        Vector3Int rightUp = rightPos + new Vector3Int(0, 1, 0);
        Vector3Int upLeft = upPos + new Vector3Int(-1, 0, 0);
        Vector3Int upRight = upPos + new Vector3Int(1, 0, 0);
        Vector3Int downLeft = downPos + new Vector3Int(-1, 0, 0);
        Vector3Int downRight = downPos + new Vector3Int(1, 0, 0);

        return new Vector3Int[] { leftDown, leftUp, rightDown, rightUp, upLeft, upRight, downLeft, downRight };
    }

    private Vector3Int[] GetNear(Vector3Int currentCellPos)
    {
        Vector3Int rightPos = currentCellPos + new Vector3Int(0, 1, 0);
        Vector3Int leftPos = currentCellPos + new Vector3Int(0, -1, 0);
        Vector3Int upPos = currentCellPos + new Vector3Int(1, 0, 0);
        Vector3Int downPos = currentCellPos + new Vector3Int(-1, 0, 0);
        Vector3Int upRightPos = currentCellPos + new Vector3Int(1, 1, 0);
        Vector3Int upLeftPos = currentCellPos + new Vector3Int(1, -1, 0);
        Vector3Int downRightPos = currentCellPos + new Vector3Int(-1, 1, 0);
        Vector3Int downLeftPos = currentCellPos + new Vector3Int(-1, -1, 0);

        return new Vector3Int[] { rightPos, leftPos, upPos, downPos, upRightPos, upLeftPos, downRightPos, downLeftPos };
    }


    #endregion

    #region Highlighted

    public void HighlightCells(Vector3Int cellPosition)
    {
        hightlightedMap.SetTile(cellPosition, mark);
        hightlightedLocations.Add(cellPosition);
    }

    public void ClearHighlightedCells()
    {
        foreach (Vector3Int location in hightlightedLocations)
        {
            hightlightedMap.SetTile(location, null);
        }
        hightlightedLocations.Clear();
    }

    #endregion

    #region Getters

    public bool IsPlaceableArea(Vector3Int cellPos)
    {
        if (tileMap.GetTile(cellPos) == null || wallMap.GetTile(cellPos) != null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3Int> GetCellsPosition()
    {
        return locations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

    #endregion

    #region Setters
    public void SetMap(Grid map)
    {
        this.tileMap = map.transform.GetChild(0).GetComponent<Tilemap>();
        this.wallMap = map.transform.GetChild(1).GetComponent<Tilemap>();
        this.hightlightedMap = map.transform.GetChild(2).GetComponent<Tilemap>();
    }

    #endregion
}
