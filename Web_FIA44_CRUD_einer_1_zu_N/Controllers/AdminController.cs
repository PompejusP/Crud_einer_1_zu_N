using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Web_FIA44_CRUD_einer_1_zu_N.DAL;
using Web_FIA44_CRUD_einer_1_zu_N.Models;
using Web_FIA44_CRUD_einer_1_zu_N.ViewModels;

namespace Web_FIA44_CRUD_einer_1_zu_N.Controllers
{
	public class AdminController : Controller
	{
        #region Dal-Connection
        //Die DAL wird instanziiert
        private readonly IAccessable dal;
        //Der ConnectionString wird aus dem appsettings.json geholt
        public AdminController(IConfiguration conf)
		{
            //Der ConnectionString wird aus dem appsettings.json geholt
            string connString = conf.GetConnectionString("SqlServer");
            //Die DAL wird instanziiert
            dal = new SqlDal(connString);
		}
		#endregion
		#region Alle Artikel anzeigen inkl. Suchfunktion und Kategoriefilter
		[HttpGet]
        //Die Index-Seite wird aufgerufen
        public IActionResult AIndex(string searchString)
		{
            //Wenn die Suchzeile leer ist, werden alle Artikel angezeigt
            if (string.IsNullOrEmpty(searchString))
			{
                //neue Instanz der IndexViewModel wird erstellt
                IndexViewModel model = new IndexViewModel();
                //alle Kategorien werden aus der Datenbank geholt
                List<Category> CatList = dal.GetAllCategories();
                //die Kategorien werden in ein DropdownListe-Objekt umgewandelt
                model.DropDownList = new SelectList(CatList, "Cid", "CatName");
                //alle Artikel werden aus der Datenbank geholt
                model.Articles = dal.GetAllArticles();
                //die View Index wird aufgerufen
                return View(model);
            }
            //Wenn die Suchzeile nicht leer ist, werden die Artikel nach dem Suchbegriff gefiltert
            else
            {
                //neue Instanz der IndexViewModel wird erstellt
                IndexViewModel model = new IndexViewModel();
                //alle Kategorien werden aus der Datenbank geholt
                List<Category> CatList = dal.GetAllCategories();
                //die Kategorien werden in ein DropdownListe-Objekt umgewandelt
                model.DropDownList = new SelectList(CatList, "Cid", "CatName");
                //die Artikel werden nach dem Suchbegriff gefiltert
                model.Articles = dal.GetArticlesBySearchIndex(searchString);
                //die View Index wird aufgerufen
                return View(model);
			}
		}
		[HttpPost]
		public IActionResult AIndex(IndexViewModel model)
		{
			if (model.DropDownValue == 0)
			{
				return RedirectToAction("AIndex");
			}
			else
			{
				List<Category> CatList = dal.GetAllCategories();
				model.DropDownList = new SelectList(CatList, "Cid", "CatName");
				model.Articles = dal.GetArticlesByCategory(model.DropDownValue);
				return View(model);
			}
		}
        #endregion
        #region Artikel löschen
        //Die Delete-Funktion wird aufgerufen
        public IActionResult Delete(int Aid)
		{
			//die Person wird anhand der ID aus der Datenbank gelöscht
			dal.DeleteArticle(Aid);
            //die Admin-Index-Seite wird aufgerufen
            return RedirectToAction("AIndex");
		}
		#endregion
		#region Artikel bearbeiten
		[HttpGet]
        //Die Update-Funktion wird aufgerufen
        public IActionResult Update(int Aid)
		{
            //es wird eine leere Instanz der IndexViewModel erstellt
            IndexViewModel model = new IndexViewModel();
            //alle Kategorien werden aus der Datenbank geholt
            List<Category> CatList = dal.GetAllCategories();
            //die Kategorien werden in ein DropdownListe-Objekt umgewandelt
            model.DropDownList = new SelectList(CatList, "Cid", "CatName");
            //der Artikel wird anhand der ID aus der Datenbank geholt
            model.Article = dal.GetArticleById(Aid);
            //die View Update wird aufgerufen
            return View(model);

		}
		[HttpPost]
		public IActionResult Update(IndexViewModel model)
		{
            //alle Kategorien werden aus der Datenbank geholt
            model.DropDownList = new SelectList(dal.GetAllCategories(), "Cid", "CatName");
            //die Kategorie wird aus der DropdownListe geholt
            //sollte keine Kategorie ausgewählt sein, wird eine Fehlermeldung ausgegeben
            if (model.DropDownValue == 0)
			{
                //Die Fehlermeldung wird ausgegeben
                ModelState.AddModelError("DropDownValue", "Bitte wählen Sie eine Kategorie aus.");
            }
            //sollte eine Kategorie ausgewählt sein, wird die Kategorie in den Artikel geschrieben
            else
            {
                //Die Kategorie wird in den Artikel geschrieben
                model.Article.CatId = model.DropDownValue;
			}
            //die nicht benötigten Model-States werden entfernt
            ModelState.Remove("Category");
			ModelState.Remove("Articles");
			ModelState.Remove("DropDownList");
			ModelState.Remove("Article.Category");

            //sollte das Model-State gültig sein, wird der Artikel in die Datenbank geschrieben
            if (ModelState.IsValid)
            {
                //der Artikel wird in die Datenbank geschrieben
                dal.UpdateArticle(model);
                //die Admin-Index-Seite wird aufgerufen
                return RedirectToAction("AIndex");

			}
            //sollte das Model-State nicht gültig sein, wird die View Create aufgerufen
            return View(model);
		}
		#endregion
		#region Artikel detailiert anzeigen
		[HttpGet]
		public IActionResult Details(int Aid)
		{
            //der Artikel wird anhand der ID aus der Datenbank geholt
            Article article = dal.GetArticleById(Aid);
            //sollte der Artikel nicht existieren, wird die Admin-Index-Seite aufgerufen
            if (article == null)
			{
				return RedirectToAction("AIndex");

			}
            //ansonsten wird die View Details aufgerufen
            return View(article);
		}
		#endregion
		#region Artikel hinzufügen
		[HttpGet]
		public IActionResult Create()
		{
			//eine leere Artikel-Instanz wird erstellt und zur View Create aufgerufen
			IndexViewModel model = new IndexViewModel();
            //alle Kategorien werden aus der Datenbank geholt
            List<Category> CatList = dal.GetAllCategories();
            //die Kategorien werden in ein DropdownListe-Objekt umgewandelt
            model.DropDownList = new SelectList(CatList, "Cid", "CatName");
            //es wird eine leere Artikel-Instanz erstellt
            model.Article = new Article();
            //die View Create wird aufgerufen
            return View(model);


		}
		[HttpPost]
		public IActionResult Create(IndexViewModel model)
		{
            //alle Kategorien werden aus der Datenbank geholt
            model.DropDownList = new SelectList(dal.GetAllCategories(), "Cid", "CatName");
            //die Kategorie wird aus der DropdownListe geholt
            //sollte keine Kategorie ausgewählt sein, wird eine Fehlermeldung ausgegeben
            if (model.DropDownValue == 0)
			{
                //Die Fehlermeldung wird ausgegeben
                ModelState.AddModelError("DropDownValue", "Bitte wählen Sie eine Kategorie aus.");
			}
            //sollte eine Kategorie ausgewählt sein, wird die Kategorie in den Artikel geschrieben
            else
            {
                //Die Kategorie wird in den Artikel geschrieben
                model.Article.CatId = model.DropDownValue;
			}
            //die nicht benötigten Model-States werden entfernt
            ModelState.Remove("Category");
			ModelState.Remove("Articles");
			ModelState.Remove("DropDownList");
			ModelState.Remove("Article.Category");

            //sollte das Model-State gültig sein, wird der Artikel in die Datenbank geschrieben
            if (ModelState.IsValid)
			{
                //der Artikel wird in die Datenbank geschrieben
                dal.InsertArticle(model);
                //die Admin-Index-Seite wird aufgerufen
                return RedirectToAction("AIndex");

			}
            //sollte das Model-State nicht gültig sein, wird die View Create aufgerufen
            return View(model);



		}
		#endregion

		#region Kategorie verwaltung
		[HttpGet]
        //Die Kategorie-Index-Seite wird aufgerufen
        public IActionResult CIndex()
		{
            //alle Kategorien werden aus der Datenbank geholt
            List<Category> categories = dal.GetAllCategories();
            //die View CIndex wird aufgerufen
            return View(categories);
		}
        #endregion
        #region Kategorie löschen
        //Die Delete-Funktion wird aufgerufen
        public IActionResult DeleteCategory(int Cid)
		{
			//die Kategorie wird anhand der ID aus der Datenbank gelöscht
			dal.DeleteCategory(Cid);
            //die Admin-Kategorie-Index-Seite wird aufgerufen
            return RedirectToAction("CIndex");
		}
		#endregion
		#region Kategorie bearbeiten
		[HttpGet]
        //Die Update-Funktion wird aufgerufen
        public IActionResult UpdateCategory(int Cid)
		{
			//eine leere Kategorie-Instanz wird erstellt 
			Category cat = dal.GetCategoryById(Cid);
            //die View Update aufgerufen
            return View(cat);
        }
		[HttpPost]
        //Die Update-Funktion wird aufgerufen
        public IActionResult UpdateCategory(Category category)
        {
            //die nicht benötigten Model-States werden entfernt
            ModelState.Remove("Articles");
            //sollte das Model-State gültig sein, wird die Kategorie in die Datenbank geschrieben
            if (ModelState.IsValid)
            {
                //die Kategorie wird in die Datenbank geschrieben
                dal.UpdateCategory(category);
                //die Admin-Kategorie-Index-Seite wird aufgerufen
                return RedirectToAction("CIndex");
            }
            //sollte das Model-State nicht gültig sein, wird die View Update aufgerufen
            return View(category);
        }
        #endregion
        #region Kategorie hinzufügen
        [HttpGet]
        //Die Create-Funktion wird aufgerufen
        public IActionResult CreateCategory()
        {
            //eine leere Kategorie-Instanz wird erstellt und zur View Create aufgerufen
            Category cat = new Category();
            return View(cat);
        }
        [HttpPost]
        //Die Create-Funktion wird aufgerufen
        public IActionResult CreateCategory(Category category)
        {
            //die nicht benötigten Model-States werden entfernt
            ModelState.Remove("Articles");
            //sollte das Model-State gültig sein, wird die Kategorie in die Datenbank geschrieben
            if (ModelState.IsValid)
            {
                //die Kategorie wird in die Datenbank geschrieben
                dal.InsertCategory(category);
                //die Admin-Kategorie-Index-Seite wird aufgerufen
                return RedirectToAction("CIndex");
            }
            //sollte das Model-State nicht gültig sein, wird die View Create aufgerufen
            return View(category);
        }
        #endregion
        #region Kategorie detailiert anzeigen
        [HttpGet]
        //Die Details-Funktion wird aufgerufen
        public IActionResult DetailsCategory(int Cid)
        {
			//die Kategorie wird anhand der ID aus der Datenbank geholt
			Category category = dal.GetCategoryById(Cid);
            //sollte die Kategorie nicht existieren, wird die Admin-Kategorie-Index-Seite aufgerufen
            if (category == null)
            {
                return RedirectToAction("CIndex");

            }
            //ansonsten wird die View Details aufgerufen
            return View(category);
		}
        #endregion
	}


    }


