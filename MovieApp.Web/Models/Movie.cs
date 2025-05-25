using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Web.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "Film başlığı eklemelisiniz.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Film başlığı 5-100 karakter aralığında olmalıdır.")]
        public string Title { get; set; }

        [DisplayName("Yönetmen")]
        [Required(ErrorMessage = "Film yönetmeni eklemelisiniz.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Yönetmen adı 5-100 karakter aralığında olmalıdır.")]
        public string Director { get; set; }

        [DisplayName("Açıklama")]
        [Required(ErrorMessage = "Film açıklaması eklemelisiniz.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Film açıklaması 5-500 karakter aralığında olmalıdır.")]
        public string Description { get; set; }

        [DisplayName("Puan")]
        [Range(0, 10, ErrorMessage = "Film puanı 0-10 arasında olmalıdır.")]
        public double Point { get; set; }

        [DisplayName("Yayınlanma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
    }
}


