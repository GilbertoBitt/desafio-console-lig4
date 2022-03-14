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
            Console.Write("\n");
            foreach (Move move in _availableMoves)
            {
                Console.Write($"x:{move.X} | y:{move.Y} | weight:{move.Weight} \n");
            }
            Console.Write("\n");
            // NextMove = new Random().Next(1,6);
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
                if (randomValue < 0.0)
                {
                    return available[i];
                }
            }
            return available.FirstOrDefault();
        }

        private int GetAvailableYPosition(int xPos)
        {
            for (int i = 0; i < 5; i++)
            {
                int currentBoardValue = _board[xPos, i];
                if(currentBoardValue == 0)
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
        /// It will be good to add ways to draw instead of just win for ai or player.
        /// 
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        private double CalculateMovesWeight(Move move)
        {
            //add this line to avoid crash when Player 2 (AI) wins and stuck on thinking.
            if(move.Y >= _board.GetLength(1) && _board[move.X, move.Y] != 0) return 1;
            int weight = Constant.DEFAULT_WEIGHT;
            foreach (MoveDirection direction in System.Enum.GetValues(typeof(MoveDirection)))
            {
                switch (direction)
                {
                    case MoveDirection.Horizontal:
                        int comboCounter = 0;
                        int lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 3; i++)
                        {
                            int xPos = move.X + i;
                            if(xPos >= _board.GetLength(0)) break;
                            int currentValue = _board[xPos, move.Y];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 3; i++)
                        {
                            int xPos = move.X - i;
                            if(xPos < 0) break;
                            int currentValue = _board[xPos, move.Y];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        break;
                    case MoveDirection.Vertical:
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 4; i++)
                        {
                            int yPos = move.Y + i;
                            if(yPos >= _board.GetLength(1)) break;
                            int currentValue = _board[move.X, yPos];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 4; i++)
                        {
                            int yPos = move.Y - i;
                            if(yPos < 0) break;
                            int currentValue = _board[move.X, yPos];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        break;
                    case MoveDirection.Diagonal:
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 4; i++)
                        {
                            int yPos = move.Y + i;
                            int xPos = move.X + i;
                            if(xPos >= _board.GetLength(0)) break;
                            if(yPos >= _board.GetLength(1)) break;
                            int currentValue = _board[xPos, yPos];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 4; i++)
                        {
                            int yPos = move.Y - i;
                            int xPos = move.X - i;
                            if(xPos < 0) break;
                            if(yPos < 0) break;
                            int currentValue = _board[xPos, yPos];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 4; i++)
                        {
                            int yPos = move.Y - i;
                            int xPos = move.X + i;
                            if(xPos >= _board.GetLength(0)) break;
                            if(yPos < 0) break;
                            int currentValue = _board[xPos, yPos];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        comboCounter = 0;
                        lastValue = _board[move.X, move.Y];
                        if (lastValue != 0)
                        {
                            throw new Exception("Invalid Move");
                        }
                        for (int i = 1; i < 4; i++)
                        {
                            int yPos = move.Y + i;
                            int xPos = move.X - i;
                            if(xPos < 0) break;
                            if(yPos >= _board.GetLength(1)) break;
                            int currentValue = _board[xPos, yPos];
                            if (currentValue != 1)
                            {
                                break;
                            }
                            if (lastValue == 0 || currentValue == lastValue)
                            {
                                if (currentValue == lastValue)
                                {
                                    comboCounter++;
                                    if (comboCounter >= 2)
                                    {
                                        weight += Constant.DEFAULT_WEIGHT * comboCounter;
                                    }
                                }
                                lastValue = currentValue;
                                weight += Constant.DEFAULT_WEIGHT;
                            } else {
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return weight;
        }
    }
}