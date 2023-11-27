using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    [SerializeField]
    private GameObject selectedObj;

    [SerializeField]
    private Vector3Int[] movePos;

    private void Update()
    {
        if(selectedObj != null)
        {
            if (Input.GetMouseButtonDown(0) && selectedObj.GetComponent<Player>().IsGrounded() && EventSystem.current.currentSelectedGameObject == null)
            {
                Vector3Int mouseDownCell = GridCellManager.instance.GetObjCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                MoveCurrentChess(mouseDownCell, selectedObj.GetComponent<Player>().IsFirstMove());
            }
            if (selectedObj.GetComponent<Player>().IsGrounded())
            {
                selectedObj.GetComponent<Player>().Selected(selectedObj.GetComponent<Player>().IsFirstMove());
            }
        }
    }

    private void MoveCurrentChess(Vector3Int mouseDownCell, bool isFirstMove)
    {
        if (movePos != null)
        {
            for (int i = 0; i < movePos.Length; i++)
            {
                if (GridCellManager.instance.IsPlaceableArea(mouseDownCell)
                    && mouseDownCell == movePos[i])
                {
                    selectedObj.GetComponent<Collider2D>().enabled = false;
                    selectedObj.transform.DOMove(GridCellManager.instance.PositonToMove(mouseDownCell), .3f).OnComplete(() 
                        => selectedObj.GetComponent<Collider2D>().enabled = true );
                    if(isFirstMove)
                    {
                        selectedObj.GetComponent<Player>().DoneFirstMove();
                    }
                    break;
                }
            }
        }
    }

    public void SetMovePos(Vector3Int[] movePos)
    {
        this.movePos = movePos;
    }

    public void SetSelectedObj(GameObject selectedObj)
    {
        if(this.selectedObj != null)
        {
            this.selectedObj.layer = LayerMask.NameToLayer("Wall");
        }
        this.selectedObj = selectedObj;
        this.selectedObj.layer = LayerMask.NameToLayer("Selected");
    }
}
