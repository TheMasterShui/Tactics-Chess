using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] tileGraphics;

    public float hoverAmount;

    public LayerMask obstacleLayer;

    public Color highlightedColor;
    public bool isWalkable;
    GameManager gm;

    public Color creatableColor;
    public bool isCreatable;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int randomTile = Random.Range(0, tileGraphics.Length);
        rend.sprite = tileGraphics[randomTile];

        gm = FindObjectOfType<GameManager>();
    }

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
    }

    public bool isClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.1f, obstacleLayer);
        if (obstacle != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Highlight()
    {
        rend.color = highlightedColor;
        isWalkable = true;      
    }

    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;
        isCreatable = false;
    }

    public void SetCreatable()
    {
        rend.color = creatableColor;
        isCreatable = true;
    }

    private void OnMouseDown()
    {
        if (isWalkable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(this.transform.position);
        }
        else if (isCreatable == true)
        {
            BarrackItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if (unit != null)
            {
                unit.hasMoved = true;
                unit.hasAttacked = true;
            }
        }
    }
}
