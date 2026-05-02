using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int rows = 6;
    public int cols = 7;

    public int[,] board;

    void Awake()
    {
        board = new int[rows, cols];
    }

    public bool IsColumnFull(int col)
    {
        return board[0, col] != 0;
    }

    public int DropPiece(int col, int player)
    {
        for (int row = rows - 1; row >= 0; row--)
        {
            if (board[row, col] == 0)
            {
                board[row, col] = player;
                return row; // important for visuals later
            }
        }
        return -1;
    }

    public bool IsBoardFull()
    {
        for (int col = 0; col < cols; col++)
        {
            if (!IsColumnFull(col))
                return false;
        }
        return true;
    }
    public bool CheckWin(int player)
{
    // Horizontal
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols - 3; col++)
        {
            if (board[row, col] == player &&
                board[row, col + 1] == player &&
                board[row, col + 2] == player &&
                board[row, col + 3] == player)
                return true;
        }
    }

    // Vertical
    for (int col = 0; col < cols; col++)
    {
        for (int row = 0; row < rows - 3; row++)
        {
            if (board[row, col] == player &&
                board[row + 1, col] == player &&
                board[row + 2, col] == player &&
                board[row + 3, col] == player)
                return true;
        }
    }

    // Diagonal (bottom-left → top-right)
    for (int row = 3; row < rows; row++)
    {
        for (int col = 0; col < cols - 3; col++)
        {
            if (board[row, col] == player &&
                board[row - 1, col + 1] == player &&
                board[row - 2, col + 2] == player &&
                board[row - 3, col + 3] == player)
                return true;
        }
    }

    // Diagonal (top-left → bottom-right)
    for (int row = 0; row < rows - 3; row++)
    {
        for (int col = 0; col < cols - 3; col++)
        {
            if (board[row, col] == player &&
                board[row + 1, col + 1] == player &&
                board[row + 2, col + 2] == player &&
                board[row + 3, col + 3] == player)
                return true;
        }
    }

    return false;
}
public void PrintBoard()
{
    for (int row = 0; row < rows; row++)
    {
        string line = "";
        for (int col = 0; col < cols; col++)
        {
            line += board[row, col] + " ";
        }
        Debug.Log(line);
    }
    Debug.Log("------");
}
}