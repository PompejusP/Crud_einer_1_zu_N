using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_FIA44_CRUD_einer_1_zu_N.DAL;
using Web_FIA44_CRUD_einer_1_zu_N.Models;
using Web_FIA44_CRUD_einer_1_zu_N.ViewModels;

namespace Web_FIA44_CRUD_einer_1_zu_N.Controllers
{
	public class HomeController : Controller
	{

        #region Dal-Connection
        // DAL-Connection erstellen
        // IAccessable-Objekt erstellen
        private readonly IAccessable dal;
        // Konstruktor erstellen
        public HomeController(IConfiguration conf)
		{
            // Connection-String aus appsettings.json holen
            string connString = conf.GetConnectionString("SqlServer");
            // DAL-Objekt erstellen
            dal = new SqlDal(connString);
		}
		#endregion
		#region Index get nach allen Artikeln suchen oder nach einem bestimmten
		[HttpGet]
		public IActionResult Index(string searchString)
		{
            // Wenn die Suchzeile leer ist, dann alle Artikel anzeigen
            if (string.IsNullOrEmpty(searchString))
			{
                // IndexViewModel erstellen
                IndexViewModel model = new IndexViewModel();
                // Alle Kategorien holen
                List<Category> CatList = dal.GetAllCategories();
                // DropdownList erstellen mit allen Kategorien
                model.DropDownList = new SelectList(CatList, "Cid", "CatName");
                // Alle Artikel holen
                model.Articles = dal.GetAllArticles();
                // View anzeigen
                return View(model);
			}
            // Wenn die Suchzeile nicht leer ist, dann nach dem Suchbegriff suchen
            else
            {
                // IndexViewModel erstellen
                IndexViewModel model = new IndexViewModel();
                // Alle Kategorien holen
                List<Category> CatList = dal.GetAllCategories();
                // DropdownList erstellen mit allen Kategorien
                model.DropDownList = new SelectList(CatList, "Cid", "CatName");
                // Artikel nach Suchbegriff suchen
                model.Articles = dal.GetArticlesBySearchIndex(searchString);
                // View anzeigen
                return View(model);
			}
		}
		[HttpPost]
		public IActionResult Index(IndexViewModel model)
		{
            // Wenn kein Wert in der DropdownList ausgewählt wurde, dann alle Artikel anzeigen
            if (model.DropDownValue == 0)
			{
				return RedirectToAction("Index");
			}
            // Wenn ein Wert in der DropdownList ausgewählt wurde, dann Artikel nach Kategorie suchen
            else
            {
                // IndexViewModel erstellen
                List<Category> CatList = dal.GetAllCategories();
                // DropdownList erstellen mit allen Kategorien
                model.DropDownList = new SelectList(CatList, "Cid", "CatName");
                // Artikel nach Kategorie suchen
                model.Articles = dal.GetArticlesByCategory(model.DropDownValue);
                // View anzeigen
                return View(model);
			}
		}
		#endregion
		#region Artikel Details anzeigen
		[HttpGet]
		public IActionResult Details(int Aid)
		{
            // Artikel nach Artikelnummer suchen
            Article article = dal.GetArticleById(Aid);
            // View anzeigen
            return View(article);
		}
		#endregion
	}
}
