using Microsoft.AspNetCore.Mvc.Rendering;
using Web_FIA44_CRUD_einer_1_zu_N.Models;

namespace Web_FIA44_CRUD_einer_1_zu_N.ViewModels
{
	public class IndexViewModel
	{
		//Versorgt eine DropdownList
		public SelectList DropDownList { get; set; }
        //Speichert den Wert der DropdownList
        public int DropDownValue { get; set; }
		
        //Versorgt die Suchfunktion
        public List<Article> Articles { get; set; }
        
        public Article Article { get; set; }

		public Category Category { get; set; }
    }
}
