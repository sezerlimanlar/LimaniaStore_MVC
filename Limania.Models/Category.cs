using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Limania.Models;

public class Category
{
	[Key]
	public int Id { get; set; }
	[Required(ErrorMessage = "Kategori ismi boş geçilemez")]
	[DisplayName("Kategori İsmi")]
	[MaxLength(30, ErrorMessage = "Maks limit 30 karakter.")]
	public string Name { get; set; }
	[DisplayName("Kategori Sıralaması")]
	[Range(1, 100, ErrorMessage = "Girilen değer aralığı 1-100 arası olmalı")]
	public int DisplayOrder { get; set; }
}
