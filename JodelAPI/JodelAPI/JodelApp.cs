using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JodelAPI
{
    /// <summary>
    /// Simulates real use of the JodelApp on Android
    /// </summary>
    public class JodelApp
    {
        public int Karma { get; private set; }

        public JodelApp()
        {
            Karma = 0;
        }

        public bool Start()
        {
            return true;
        }


    }
}
