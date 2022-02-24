using System.Threading.Tasks;

namespace ConsoleLig4.Core.Interfaces
{
    public interface IAIService
    {
        int NextMove { get; }

        Task IsProcessing();
        void SetBoard(int[,] board);
        void SetPlayerMove(int position);
    }
}
