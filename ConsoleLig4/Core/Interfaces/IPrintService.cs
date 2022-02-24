namespace ConsoleLig4.Core.Interfaces
{
    public interface IPrintService
    {
        void PrintBoardPieces(int[,] pieces);
        void PrintCursorPosition(int position);
        void PrintGameBoard();
        void PrintTitle();
        void PrintWinner(int winner);
        void SetAIPlaying(bool isPlaying);
    }
}
