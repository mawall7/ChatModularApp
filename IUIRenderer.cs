using SimpleUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModularApp
{
    public interface IUIRenderer
    {
        public void Render(IInputProcessingResult processingResult);
    }
}
