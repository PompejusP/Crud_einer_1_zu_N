using System.ComponentModel.DataAnnotations;

namespace Web_FIA44_CRUD_einer_1_zu_N.Models
{
	public class Category
	{
		[Display(Name = "KategorieId")]
		public int Cid { get; set; }
		[Display(Name = "Kategorie")]
		public string CatName { get; set; }

		public List<Article> Articles { get; set; }
	}
}
