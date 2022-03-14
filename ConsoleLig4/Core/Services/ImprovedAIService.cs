using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleLig4.Core.Enum;
using ConsoleLig4.Core.Interfaces;
using ConsoleLig4.Core.Constants;

namespace ConsoleLig4.Core.Services
{
    public class ImprovedAiService : IAIService
    {
        private struct Move
        {
            public readonly int X;
            public readonly int Y;
            public double Weight;

            public Move(int x, int y)
            {
                this.Weight = Constant.DEFAULT_WEIGHT;
                this.X = x;
                this.Y = y;
            }
        }
        public int NextMove { get; private set; }

        private TaskCompletionSource AiProcessing { get; set; }

        private int[,] _board;
        private Move[] _availableMoves = new Move[5];

        public async Task IsProcessing()
        {
            if (AiProcessing == null)
            {
                await Task.CompletedTask;
            }

            if (AiProcessing != null) await AiProcessing.Task;
        }

        public void SetBoard(int[,] board)
        {
            AiProcessing = new TaskCompletionSource();
            // SET BOARD
            _board = board;
            AiProcessing.SetResult();
        }

        public void SetPlayerMove(int position)
        {
            AiProcessing = new TaskCompletionSource();
            _availableMoves = new Move[5];
            for (int i = 0; i < 5; i++)
            {
                _availableMoves[i] = new Move(i, GetAvailableYPosition(i));
                _availableMoves[i].Weight = CalculateMovesWeight(_availableMoves[i]);
            }
            // SET PLAYER MOVE
            NextMove = GetRandomWeightedMove().X+1;
            AiProcessing.SetResult();
        }

        private Move GetRandomWeightedMove()
        {
            double weightSum = _availableMoves.Sum(move => move.Weight);
            double randomValue = weightSum * new Random().NextDouble();
            Move[] available = _availableMoves.OrderBy(move => move.Weight).ToArray();
            for (int i = 0; i < available.Length; i++)
            {
                randomValue -= available[i].Weight;
                if (!(randomValue < 0.0)) continue;
                return available[i];
            }
            return available.FirstOrDefault();
        }

        private int GetAvailableYPosition(int xPos)
        {
            for (int i = 0; i < 5; i++)
            {
                if(_board[xPos, i] == 0)
                {
                    return i;
                }
            }
            return 0;
        }

        
        /// <summary>
        /// This can easily be improve with a ai framework so we can use tree analysis and learn with players..
        /// also because we use dynamic board we can easily change the value to win to a constant so the system can
        /// change and improve based on it.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private double CalculateMovesWeight(Move move)
        {
            double weight = Constant.DEFAULT_WEIGHT;
            if (move.X == 0) return weight;
            foreach (MoveDirection direction in System.Enum.GetValues(typeof(MoveDirection)))
            {
                int posX;
                int posY;
                int playerInRow;
                int enemyInRow;
                switch (direction)
                {
                    case MoveDirection.DiagonalRight:
                        ResetInitialValues(move, out posX, out posY, out playerInRow, out enemyInRow);
                        for (int i = 1; i < 3; i++)
                        {
                            posX += i;
                            if(posX <= 0 || posX >= _board.GetLength(0)) continue;
                            for (int j = 1; j < 3; j++)
                            {
                                posY -= j;
                                if(posY <= 0 || posY >= _board.GetLength(1)) continue;
                                int currentBoard = _board[posX, posY];
                                PlayerInRow(currentBoard, ref playerInRow, ref weight, ref enemyInRow);
                            }
                        }
                        break;
                    case MoveDirection.DiagonalLeft:
                        ResetInitialValues(move, out posX, out posY, out playerInRow, out enemyInRow);
                        for (int i = 1; i < 3; i++)
                        {
                            posX -= i;
                            if(posX <= 0 || posX >= _board.GetLength(0)) continue;
                            for (int j = 1; j < 3; j++)
                            {
                                posY -= j;
                                if(posY <= 0 || posY >= _board.GetLength(1)) continue;
                                int currentBoard = _board[posX, posY];
                                PlayerInRow(currentBoard, ref playerInRow, ref weight, ref enemyInRow);
                            }
                        }
                        break;
                    case MoveDirection.HorizontalRight:
                        ResetInitialValues(move, out posX, out posY, out playerInRow, out enemyInRow);
                        for (int i = 1; i < 3; i++)
                        {
                            posX += i;
                            if(posX <= 0 || posX >= _board.GetLength(0)) continue;
                            if(posY <= 0 || posY >= _board.GetLength(1)) continue;
                            int currentBoard = _board[posX, posY];
                            PlayerInRow(currentBoard, ref playerInRow, ref weight, ref enemyInRow);
                        }
                        break;
                    case MoveDirection.HorizontalLeft:
                        ResetInitialValues(move, out posX, out posY, out playerInRow, out enemyInRow);
                        for (int i = 1; i < 3; i++)
                        {
                            posX -= i;
                            if(posX <= 0 || posX >= _board.GetLength(0)) continue;
                            if(posY <= 0 || posY >= _board.GetLength(1)) continue;
                            int currentBoard = _board[posX, posY];
                            PlayerInRow(currentBoard, ref playerInRow, ref weight, ref enemyInRow);
                        }
                        break;
                    case MoveDirection.VerticalRight:
                        ResetInitialValues(move, out posX, out posY, out playerInRow, out enemyInRow);
                        for (int i = 1; i < 3; i++)
                        {
                            posY += i;
                            if(posX <= 0 || posX >= _board.GetLength(0)) continue;
                            if(posY <= 0 || posY >= _board.GetLength(1)) continue;
                            int currentBoard = _board[posX, posY];
                            PlayerInRow(currentBoard, ref playerInRow, ref weight, ref enemyInRow);
                        }
                        break;
                    case MoveDirection.VerticalLeft:
                        ResetInitialValues(move, out posX, out posY, out playerInRow, out enemyInRow);
                        for (int i = 1; i < 3; i++)
                        {
                            posY -= i;
                            if(posX <= 0 || posX >= _board.GetLength(0)) continue;
                            if(posY <= 0 || posY >= _board.GetLength(1)) continue;
                            int currentBoard = _board[posX, posY];
                            PlayerInRow(currentBoard, ref playerInRow, ref weight, ref enemyInRow);
                        }
                        break;
                }
            }
            return weight;
        }

        private static void ResetInitialValues(Move move, out int posX, out int posY, out int playerInRow, out int enemyInRow)
        {
            posX = move.X;
            posY = move.Y;
            playerInRow = 0;
            enemyInRow = 0;
        }

        private static void PlayerInRow(int currentBoard, ref int playerInRow, ref double weight, ref int enemyInRow)
        {
            switch (currentBoard)
            {
                case 1:
                    playerInRow++;
                    weight += Constant.DEFAULT_WEIGHT * playerInRow;
                    if (playerInRow >= 3)
                    {
                        weight = Constant.DEFAULT_WEIGHT * 3;
                    }
                    enemyInRow = 0;
                    break;
                case 2:
                    enemyInRow++;
                    weight = Constant.DEFAULT_WEIGHT;
                    if (enemyInRow >= 3)
                    {
                        weight = Constant.DEFAULT_WEIGHT * 3;
                    }
                    playerInRow = 0;
                    break;
            }
        }
    }
}