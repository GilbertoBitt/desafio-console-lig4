using System;
using System.Threading.Tasks;
using ConsoleLig4.Core.Interfaces;

namespace ConsoleLig4.Core.Services
{
    public class InputService : IInputService
    {
        public event Action EscKeyPressed;
        public event Action LeftKeyPressed;
        public event Action RightKeyPressed;
        public event Action SpaceKeyPressed;

        public InputService()
        {
            _ = Task.Run(InputLoop);
        }

        private void InputLoop()
        {
            while (true)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        EscKeyPressed?.Invoke();
                        break;
                    case ConsoleKey.LeftArrow:
                        LeftKeyPressed?.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        RightKeyPressed?.Invoke();
                        break;
                    case ConsoleKey.Spacebar:
                        SpaceKeyPressed?.Invoke();
                        break;
                }
            }
        }
    }
}
