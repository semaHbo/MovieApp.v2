using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MovieApp.v2.Web.Models;

public class Genre
{
    public int Id { get; set; }

    [DisplayName("Tür Adı")]
    [Required(ErrorMessage = "Tür adı zorunludur.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Tür adı 2-50 karakter arasında olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Açıklama")]
    [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
    public string? Description { get; set; }

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
} 