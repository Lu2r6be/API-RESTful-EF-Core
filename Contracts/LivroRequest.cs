namespace BibliotecaApi.Contracts;

public record LivroRequest(string Titulo, string Autor, int AnoPublicacao, bool Disponivel)
{
    public Dictionary<string, string[]> Validar()
    {
        var erros = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(Titulo) || Titulo.Trim().Length > 200)
            erros["titulo"] = ["Informe um título com até 200 caracteres."];

        if (string.IsNullOrWhiteSpace(Autor) || Autor.Trim().Length > 150)
            erros["autor"] = ["Informe um autor com até 150 caracteres."];

        if (AnoPublicacao < 1 || AnoPublicacao > DateTime.UtcNow.Year)
            erros["anoPublicacao"] = ["Informe um ano entre 1 e o ano atual."];

        return erros;
    }
}
