using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Slide
    {
        public int Ordem_Apresentacao { get; set; }
        public List<Banner> banners { get; set; }
    }
}
