//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training
{
    class Face
    {
        public int id { get; set; }
        public string nome { get; set; }
        public List<string> imagens { get; set; }

        public Face(int id, string nome)
        {
            this.id = id;
            this.nome = nome;
            //File.WriteAllText(Form1.FacesPath + @"\" + nome + ".txt", JsonConvert.SerializeObject(this));
        }
    }
}
