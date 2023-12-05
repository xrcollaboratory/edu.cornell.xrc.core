using System;

namespace XRC.Core
{
    public interface IBeginEndToggle
    {
        public void Begin();
        public void End();
        
        public void Toggle();

        // public bool isRunning;

    }
}
