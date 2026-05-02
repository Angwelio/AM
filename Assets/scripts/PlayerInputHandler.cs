using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Camera cam;
    public GameManager gameManager;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("mouse clicked");
            HandleClick();
        }
    }

    void HandleClick()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            ColumnClick column = hit.collider.GetComponent<ColumnClick>();

            if (column != null)
            {
                Debug.Log("Column clicked: " + column.columnIndex);
                gameManager.PlayerMove(column.columnIndex);
            }
        }
    }
}