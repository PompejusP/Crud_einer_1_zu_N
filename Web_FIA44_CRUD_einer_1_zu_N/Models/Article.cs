using System.ComponentModel.DataAnnotations;

namespace Web_FIA44_CRUD_einer_1_zu_N.Models
{
	public class Article
	{
        // Properties
        // Primary Key

        [Display(Name = "Artikelnummer")]
		public int Aid { get; set; }
        [Display(Name = "KategorieId")]
        public int CatId { get; set; }
        [Display(Name = "Artikelname")]
        public string ArticleName { get; set; }
        [Display(Name = "Preis")]
        public decimal Price { get; set; }
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
        [Display(Name = "Kategorie")]
		public Category Category { get; set; }

	}

}
