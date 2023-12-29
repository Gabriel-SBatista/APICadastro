using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICadastro.Models;

public class TipoAcesso
{
    public TipoAcesso()
    {
        Usuarios = new Collection<Usuario>();
    }

    public int TipoAcessoId { get; set; }
    [Required]
    [MaxLength(20)]
    public string Tipos { get; set; }
    public ICollection<Usuario> Usuarios { get; set; }
}
