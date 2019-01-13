using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersUiWf
{
    public class CallBack
    {
        public CallBack()
        {
        }

        /// <summary>
        /// Base callback Methods
        /// </summary>

        public virtual void Initialize()
        {
            Trace("Initialize");
        }

        public virtual void Trace(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }

        public virtual void Panic(string text)
        {
            Trace("Panic: " + text);
            throw new Exception(text);
        }

    }
}
