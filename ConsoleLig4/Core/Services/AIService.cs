using System;
using System.Threading.Tasks;
using ConsoleLig4.Core.Interfaces;

namespace ConsoleLig4.Core.Services
{
    public class AIService : IAIService
    {
        public int NextMove { get; private set; }

        private TaskCompletionSource AIProcessing { get; set; }

        public async Task IsProcessing()
        {
            if (AIProcessing == null)
            {
                await Task.CompletedTask;
            }
            await AIProcessing.Task;
        }

        public void SetBoard(int[,] board)
        {
            AIProcessing = new TaskCompletionSource();
            // SET BOARD
            AIProcessing.SetResult();
        }

        public void SetPlayerMove(int position)
        {
            AIProcessing = new TaskCompletionSource();
            // SET PLAYER MOVE
            NextMove = new Random().Next(1, 6);
            AIProcessing.SetResult();
        }
    }
}
