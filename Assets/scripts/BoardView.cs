using UnityEngine;
using System.Collections;

public class BoardView : MonoBehaviour
{
    public BoardManager board;

    public GameObject discPrefab;

    public float cellSize = 1f;
    public Vector2 origin = Vector2.zero;

void Start()
{
    float screenHeight = Camera.main.orthographicSize * 2f;
    float screenWidth = screenHeight * Screen.width / Screen.height;

    cellSize = Mathf.Min(screenWidth / 7f, screenHeight / 6f);

    origin = new Vector2(
        -cellSize * 3.5f + cellSize / 2f,
        cellSize * 2.5f + cellSize / 2f
    );
}

    public void SpawnDisc(int row, int col, int player)
{
    Vector3 targetPos = new Vector3(
        origin.x + col * cellSize,
        origin.y - row * cellSize,
        0
    );

    Vector3 spawnPos = targetPos + Vector3.up * 5f;

    //GameObject disc = Instantiate(discPrefab, spawnPos, Quaternion.identity);
    GameObject disc = Instantiate(discPrefab, spawnPos, Quaternion.Euler(90f, 0f, 0f));

    /*SpriteRenderer sr = disc.GetComponent<SpriteRenderer>();
    if (sr != null)
        sr.color = (player == 1) ? Color.red : Color.yellow;*/
    Renderer rend = disc.GetComponent<Renderer>();

if (rend != null)
{
    if (player == 1)
        rend.material.color = Color.red;
    else
        rend.material.color = Color.yellow;
}

    StartCoroutine(Fall(disc.transform, targetPos));
}
IEnumerator Fall(Transform obj, Vector3 target)
{
    float speed = 5f;

    while (Vector3.Distance(obj.position, target) > 0.01f)
    {
        obj.position = Vector3.MoveTowards(obj.position, target, speed * Time.deltaTime);
        yield return null;
    }

    obj.position = target;
}
}