using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Pages
{
    public interface IPage
    {
        /// <summary>
        /// Called when the page are needing for the updating
        /// </summary>
        void Update() { }
    }
}
