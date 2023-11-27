using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum ChessType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private ChessType chessType;
    private Ray ray;
    private bool isFirstMove = true;

    private void Start()
    {
        SetDefault();   
    }

    private void OnMouseDown()
    {
        if (IsGrounded())
        {
            Selected(isFirstMove);
        }
    }

    private void SetDefault()
    {
        Vector3Int spawnCellPos = GridCellManager.instance.GetObjCell(this.transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(spawnCellPos);
        this.gameObject.layer = LayerMask.NameToLayer("Wall");
    }

    public void Selected(bool isFirstMove)
    {
        GridCellManager.instance.ClearHighlightedCells();
        Vector3Int currentCellPos = GridCellManager.instance.GetObjCell(this.transform.position);
        Vector3Int[] movePos = GridCellManager.instance.GetPositionsToMove(currentCellPos, chessType, isFirstMove);
        MovementManager.instance.SetMovePos(movePos);
        MovementManager.instance.SetSelectedObj(this.gameObject);
        HightLight(currentCellPos, movePos);
    }

    private void HightLight(Vector3Int chessPos, Vector3Int[] movePos)
    {
        GridCellManager.instance.HighlightCells(chessPos);

        foreach (Vector3Int pos in movePos)
        {
            if (GridCellManager.instance.IsPlaceableArea(pos))
            {
                GridCellManager.instance.HighlightCells(pos);
            }
        }
    }

    public bool IsFirstMove()
    {
        return isFirstMove;
    }

    public void DoneFirstMove()
    {
        isFirstMove = false;
    }

    public bool IsGrounded()
    {
        Vector2 direction = Vector2.down;
        ray = new Ray(this.transform.position, direction);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, .6f, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * .6f);
    }
}
