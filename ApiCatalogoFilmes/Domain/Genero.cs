using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoFilmes
{
    public class Genero
    {
        public int GeneroId { get; set; }
        public string? Nome { get; set; }
        public ICollection<Filme>? Filmes { get; set; }
    }
}