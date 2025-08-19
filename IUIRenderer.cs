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
        public static StringBuilder WriteBuffer { get; set; }
        public void Render(IInputProcessingResult processingResult);
    }
}
