using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MovieApp.v2.Web.Models;

public class Movie
{
    public int Id { get; set; }

    [DisplayName("Başlık")]
    [Required(ErrorMessage = "Film başlığı zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Film başlığı 3-100 karakter arasında olmalıdır.")]
    public string Title { get; set; } = string.Empty;

    [DisplayName("Yönetmen")]
    [Required(ErrorMessage = "Yönetmen adı zorunludur.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Yönetmen adı 3-100 karakter arasında olmalıdır.")]
    public string Director { get; set; } = string.Empty;

    [DisplayName("Açıklama")]
    [Required(ErrorMessage = "Film açıklaması zorunludur.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Film açıklaması 10-1000 karakter arasında olmalıdır.")]
    public string Description { get; set; } = string.Empty;

    [DisplayName("Puan")]
    [Required(ErrorMessage = "Film puanı zorunludur.")]
    [Range(0, 10, ErrorMessage = "Film puanı 0-10 arasında olmalıdır.")]
    public double Rating { get; set; }

    [DisplayName("Yayın Tarihi")]
    [Required(ErrorMessage = "Yayın tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [DisplayName("Poster URL")]
    [Url(ErrorMessage = "Geçerli bir URL giriniz.")]
    public string? PosterUrl { get; set; }

    [DisplayName("Film Türü")]
    [Required(ErrorMessage = "Film türü zorunludur.")]
    public int GenreId { get; set; }

    public Genre? Genre { get; set; }

    [DisplayName("Oluşturulma Tarihi")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Son Güncelleme")]
    public DateTime? UpdatedAt { get; set; }
} 