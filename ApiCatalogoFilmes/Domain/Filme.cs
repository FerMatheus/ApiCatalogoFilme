namespace ApiCatalogoFilmes;
public class Filme
{
    public int FilmeId { get; set; }
    public string? Nome { get; set; }
    public string? Sinopse { get; set; }
    public Genero? Genero { get; set; }
    public int GeneroId { get; set; }
}
