using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveService
{
    class Face
    {
        public int id { get; set; }
        public string nome{ get; set; }
        public List<string> imagens { get; set; }

        public Face(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
        }
    }
}
