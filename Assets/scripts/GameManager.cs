using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public BoardManager board;
    public SimpleAI ai;
    public BoardView view;

    int currentPlayer = 1; // 1 = player, 2 = AI
    bool isGameOver = false;

    public void PlayerMove(int col)
    {
        if (isGameOver || currentPlayer != 1)
            return;

        if (board.IsColumnFull(col))
            return;

        int row = board.DropPiece(col, 1);
        view.SpawnDisc(row, col, 1);
        //board.DropPiece(col, 1);
        board.PrintBoard();

        StartCoroutine(NextTurn());
    }

    IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(0.2f);

        if (board.CheckWin(1))
        {
            Debug.Log("Player wins");
            isGameOver = true;
            yield break;
        }

        currentPlayer = 2;

        yield return new WaitForSeconds(0.3f);

        int aiMove = ai.GetMove(board.board);
        int row = board.DropPiece(aiMove, 2);
        view.SpawnDisc(row, aiMove, 2);
        board.PrintBoard();

        //int aiMove = ai.GetMove(board.board);
        //board.DropPiece(aiMove, 2);

        if (board.CheckWin(2))
        {
            Debug.Log("AI wins");
            isGameOver = true;
            yield break;
        }

        currentPlayer = 1;
    }
}