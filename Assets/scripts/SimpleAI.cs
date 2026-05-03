using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public int simulationsPerMove = 50;

public int GetMove(int[,] board)
{
    List<int> validMoves = GetValidMoves(board);

    float bestScore = float.MinValue;
    int bestMove = validMoves[0];

    foreach (int move in validMoves)
    {
        float score = RunSimulations(board, move);

        if (score > bestScore)
        {
            bestScore = score;
            bestMove = move;
        }
    }

    return bestMove;
}
        List<int> GetValidMoves(int[,] board)
    {
        List<int> moves = new List<int>();

        for (int col = 0; col < 7; col++)
        {
            if (board[0, col] == 0)
                moves.Add(col);
        }

        return moves;
    }
        int[,] SimulateMove(int[,] board, int col, int player)
    {
        int[,] copy = (int[,])board.Clone();

        for (int row = 5; row >= 0; row--)
        {
            if (copy[row, col] == 0)
            {
                copy[row, col] = player;
                break;
            }
        }

        return copy;
    }
        int EvaluateBoard(int[,] board, int player)
    {
        int score = 0;

        // center preference
        for (int row = 0; row < 6; row++)
        {
            if (board[row, 3] == player)
                score += 3;
        }

        return score;
    }
        int ChooseMove(Dictionary<int, int> scores)
    {
        int total = 0;

        foreach (var s in scores.Values)
            total += Mathf.Max(1, s);

        int rand = Random.Range(0, total);

        int sum = 0;

        foreach (var kvp in scores)
        {
            sum += Mathf.Max(1, kvp.Value);

            if (rand < sum)
                return kvp.Key;
        }

        return 0;
    }
    float RunSimulations(int[,] board, int move)
{
    float totalScore = 0;

    for (int i = 0; i < simulationsPerMove; i++)
    {
        int[,] simBoard = SimulateMove(board, move, 2); // AI plays first

        int result = PlayRandomGame(simBoard, 1); // player turn next

        if (result == 2) totalScore += 1f;     // AI win
        else if (result == 0) totalScore += 0.5f; // draw
        // loss = 0
    }

    return totalScore / simulationsPerMove;
}
int PlayRandomGame(int[,] board, int currentPlayer)
{
    while (true)
    {
        List<int> moves = GetValidMoves(board);

        if (moves.Count == 0)
            return 0; // draw

        int move = moves[Random.Range(0, moves.Count)];

        board = SimulateMove(board, move, currentPlayer);

        if (CheckWinSim(board, currentPlayer))
            return currentPlayer;

        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }
}
bool CheckWinSim(int[,] board, int player)
{
    int rows = 6;
    int cols = 7;

    // Horizontal
    for (int r = 0; r < rows; r++)
        for (int c = 0; c < cols - 3; c++)
            if (board[r,c] == player &&
                board[r,c+1] == player &&
                board[r,c+2] == player &&
                board[r,c+3] == player)
                return true;

    // Vertical
    for (int c = 0; c < cols; c++)
        for (int r = 0; r < rows - 3; r++)
            if (board[r,c] == player &&
                board[r+1,c] == player &&
                board[r+2,c] == player &&
                board[r+3,c] == player)
                return true;

    // Diagonals...
    for (int r = 3; r < rows; r++)
        for (int c = 0; c < cols - 3; c++)
            if (board[r,c] == player &&
                board[r-1,c+1] == player &&
                board[r-2,c+2] == player &&
                board[r-3,c+3] == player)
                return true;

    for (int r = 0; r < rows - 3; r++)
        for (int c = 0; c < cols - 3; c++)
            if (board[r,c] == player &&
                board[r+1,c+1] == player &&
                board[r+2,c+2] == player &&
                board[r+3,c+3] == player)
                return true;

    return false;
}
}