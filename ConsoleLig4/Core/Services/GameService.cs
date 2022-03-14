using System;
using System.Threading.Tasks;
using ConsoleLig4.Core.Interfaces;
using ConsoleLig4.Core.Models;

namespace ConsoleLig4.Core.Services
{
    public class GameService : IGameService
    {
        private IAIService AIService { get; }
        private IPrintService PrintService { get; }

        private int[,] Board { get; }
        private Configuration Configuration { get; }
        private int CursorPosition { get; set; }
        private TaskCompletionSource GameRunningTask { get; }
        private bool IsPlayerTurn { get; set; }

        public GameService(IAIService aiService,
                           IInputService inputService,
                           IPrintService printService,
                           Configuration configuration)
        {
            AIService = aiService;
            PrintService = printService;
            Configuration = configuration;

            Board = new int[Configuration.BoardSize, Configuration.BoardSize];
            AIService.SetBoard(Board);
            CursorPosition = 1;
            GameRunningTask = new TaskCompletionSource();

            inputService.EscKeyPressed += InputService_EscKeyPressed;
            inputService.LeftKeyPressed += InputService_LeftKeyPressed;
            inputService.RightKeyPressed += InputService_RightKeyPressed;
            inputService.SpaceKeyPressed += InputService_SpaceKeyPressed;
        }

        public async Task PlayAsync()
        {
            IsPlayerTurn = true;
            PrintService.PrintTitle();
            PrintService.PrintCursorPosition(CursorPosition);
            PrintService.PrintGameBoard();
            await GameRunningTask.Task;
        }

        private async Task AIPlayAsync()
        {
            PrintService.SetAIPlaying(true);
            await AIService.IsProcessing();
            int aiPosition = AIService.NextMove;
            DropPiece(aiPosition, 2);
            int winner = TestForVictory();
            if (winner > 0)
            {
                PrintService.PrintWinner(winner);
                GameRunningTask.SetResult();
            }
            else
            {
                PrintService.SetAIPlaying(false);
                IsPlayerTurn = true;
            }
        }

        private bool CanDropPiece(int position)
        {
            return Board[position - 1, Configuration.BoardSize - 1] == 0;
        }

        private void DropPiece(int position, int piece)
        {
            for (int row = 0; row < Configuration.BoardSize; row++)
            {
                if (Board[position - 1, row] == 0)
                {
                    Board[position - 1, row] = piece;
                    break;
                }
            }
            PrintService.PrintBoardPieces(Board);
        }

        private void InputService_EscKeyPressed()
        {
            GameRunningTask.SetResult();
        }

        private void InputService_LeftKeyPressed()
        {
            CursorPosition = Math.Max(--CursorPosition, 1);
            PrintService.PrintCursorPosition(CursorPosition);
        }

        private void InputService_RightKeyPressed()
        {
            CursorPosition = Math.Min(++CursorPosition, Configuration.BoardSize);
            PrintService.PrintCursorPosition(CursorPosition);
        }

        private void InputService_SpaceKeyPressed()
        {
            if (!IsPlayerTurn)
            {
                return;
            }
            if (CanDropPiece(CursorPosition))
            {
                IsPlayerTurn = false;
                DropPiece(CursorPosition, 1);
                int winner = TestForVictory();
                if (winner > 0)
                {
                    PrintService.PrintWinner(winner);
                    GameRunningTask.SetResult();
                }
                else
                {
                    AIService.SetPlayerMove(CursorPosition);
                    _ = AIPlayAsync();
                }
            }
        }

        
        /// <summary>
        /// Can't we use a dynamic system to detect if and who win and also we need to allow draw games.
        /// </summary>
        /// <returns></returns>
        private int TestForVictory()
        {
            for (int column = 0; column < Configuration.BoardSize - 3; column++)
            {
                for (int row = 0; row < Configuration.BoardSize - 3; row++)
                {
                    // HORIZONTAL
                    if (Board[column, row] != 0 &&
                        Board[column, row] == Board[column + 1, row] &&
                        Board[column + 1, row] == Board[column + 2, row] &&
                        Board[column + 2, row] == Board[column + 3, row])
                    {
                        return Board[column, row];
                    }

                    // VERTICAL
                    if (Board[column, row] != 0 &&
                        Board[column, row] == Board[column, row + 1] &&
                        Board[column, row + 1] == Board[column, row + 2] &&
                        Board[column, row + 2] == Board[column, row + 3])
                    {
                        return Board[column, row];
                    }

                    // DIAGONAL 1
                    if (Board[column, row] != 0 &&
                        Board[column, row] == Board[column + 1, row + 1] &&
                        Board[column + 1, row + 1] == Board[column + 2, row + 2] &&
                        Board[column + 2, row + 2] == Board[column + 3, row + 3])
                    {
                        return Board[column, row];
                    }

                    // DIAGONAL 2
                    if (Board[column + 3, row] != 0 &&
                        Board[column + 3, row] == Board[column + 2, row + 1] &&
                        Board[column + 2, row + 1] == Board[column + 1, row + 2] &&
                        Board[column + 1, row + 2] == Board[column, row + 3])
                    {
                        return Board[column + 3, row];
                    }
                }
            }
            for (int column = 0; column < Configuration.BoardSize - 3; column++)
            {
                for (int row = Configuration.BoardSize - 3; row < Configuration.BoardSize; row++)
                {
                    // HORIZONTAL
                    if (Board[column, row] != 0 &&
                        Board[column, row] == Board[column + 1, row] &&
                        Board[column + 1, row] == Board[column + 2, row] &&
                        Board[column + 2, row] == Board[column + 3, row])
                    {
                        return Board[column, row];
                    }
                }
            }
            for (int column = Configuration.BoardSize - 3; column < Configuration.BoardSize; column++)
            {
                for (int row = 0; row < Configuration.BoardSize - 3; row++)
                {
                    // VERTICAL
                    if (Board[column, row] != 0 &&
                    Board[column, row] == Board[column, row + 1] &&
                    Board[column, row + 1] == Board[column, row + 2] &&
                    Board[column, row + 2] == Board[column, row + 3])
                    {
                        return Board[column, row];
                    }
                }
            }
            return 0;
        }
    }
}
