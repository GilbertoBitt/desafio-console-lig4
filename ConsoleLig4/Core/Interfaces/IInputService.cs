using System;

namespace ConsoleLig4.Core.Interfaces
{
    public interface IInputService
    {
        event Action EscKeyPressed;
        event Action LeftKeyPressed;
        event Action RightKeyPressed;
        event Action SpaceKeyPressed;
    }
}
