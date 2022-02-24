using System;
using ConsoleLig4.Core.Interfaces;
using ConsoleLig4.Core.Models;

namespace ConsoleLig4.Core.Services
{
    public class PrintService : IPrintService
    {
        private Configuration Configuration { get; }

        public PrintService(Configuration configuration)
        {
            Configuration = configuration;
            Console.Clear();
            Console.CursorVisible = false;
        }

        public void PrintBoardPieces(int[,] pieces)
        {
            for (int column = 0; column < pieces.GetLength(0); column++)
            {
                for (int row = 0; row < pieces.GetLength(1); row++)
                {
                    switch (pieces[column, row])
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.SetCursorPosition(column * 8 + 2, (Configuration.BoardSize - 1 - row) * 4 + 6);
                            Console.Write("X   X");
                            Console.SetCursorPosition(column * 8 + 4, (Configuration.BoardSize - 1 - row) * 4 + 7);
                            Console.Write("X");
                            Console.SetCursorPosition(column * 8 + 2, (Configuration.BoardSize - 1 - row) * 4 + 8);
                            Console.Write("X   X");
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.SetCursorPosition(column * 8 + 3, (Configuration.BoardSize - 1 - row) * 4 + 6);
                            Console.Write("OOO");
                            Console.SetCursorPosition(column * 8 + 2, (Configuration.BoardSize - 1 - row) * 4 + 7);
                            Console.Write("OO OO");
                            Console.SetCursorPosition(column * 8 + 3, (Configuration.BoardSize - 1 - row) * 4 + 8);
                            Console.Write("OOO");
                            break;
                        case 0:
                        default:
                            break;
                    }
                }
            }
            Console.ResetColor();
        }

        public void PrintCursorPosition(int position)
        {
            Console.SetCursorPosition(0, 4);
            Console.Write(new string(' ', (Configuration.BoardSize * 8) + 1));
            Console.SetCursorPosition((position * 8) - 4, 4);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("V");
            Console.ResetColor();
        }

        public void PrintGameBoard()
        {
            Console.SetCursorPosition(0, 5);
            Console.Write("┌");
            for (int column = 0; column < Configuration.BoardSize - 1; column++)
            {
                Console.Write("───────┬");
            }
            Console.WriteLine("───────┐");
            for (int row = 0; row < Configuration.BoardSize - 1; row++)
            {
                for (int it = 0; it < 3; it++)
                {
                    for (int column = 0; column < Configuration.BoardSize; column++)
                    {
                        Console.Write("│       ");
                    }
                    Console.WriteLine("│");
                }
                Console.Write("├");
                for (int column = 0; column < Configuration.BoardSize - 1; column++)
                {
                    Console.Write("───────┼");
                }
                Console.WriteLine("───────┤");
            }
            for (int it = 0; it < 3; it++)
            {
                for (int column = 0; column < Configuration.BoardSize; column++)
                {
                    Console.Write("│       ");
                }
                Console.WriteLine("│");
            }
            Console.Write("└");
            for (int column = 0; column < Configuration.BoardSize - 1; column++)
            {
                Console.Write("───────┴");
            }
            Console.WriteLine("───────┘");
        }

        public void PrintTitle()
        {
            Console.SetCursorPosition(((Configuration.BoardSize * 8) - 18) / 2, 0);
            Console.WriteLine("== Console Lig 4 ==");
            Console.SetCursorPosition(((Configuration.BoardSize * 8) - 23) / 2, 1);
            Console.WriteLine("Move with <- and -> keys");
            Console.SetCursorPosition(((Configuration.BoardSize * 8) - 36) / 2, 2);
            Console.WriteLine("Drop piece with SPACEBAR. ESC to quit");
        }

        public void PrintWinner(int winner)
        {
            Console.SetCursorPosition(0, 4 * Configuration.BoardSize + 6);
            Console.Write($"Player {winner} won!");
        }

        public void SetAIPlaying(bool isPlaying)
        {
            Console.SetCursorPosition(((Configuration.BoardSize * 8) - 14) / 2, 3);
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (isPlaying)
            {
                Console.Write("AI IS THINKING!");
            }
            else
            {
                Console.Write("               ");
            }
        }
    }
}
