using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public int GetMove(int[,] board)
    {
        List<int> validMoves = GetValidMoves(board);
        Dictionary<int, int> scores = new Dictionary<int, int>();

        foreach (int col in validMoves)
        {
            int[,] sim = SimulateMove(board, col, 2);
            int score = EvaluateBoard(sim, 2);

            scores[col] = score;
        }

        return ChooseMove(scores);
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
}