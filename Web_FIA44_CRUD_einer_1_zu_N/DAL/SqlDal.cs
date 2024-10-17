using Microsoft.Data.SqlClient;
using Web_FIA44_CRUD_einer_1_zu_N.Models;
using Web_FIA44_CRUD_einer_1_zu_N.ViewModels;
namespace Web_FIA44_CRUD_einer_1_zu_N.DAL
{
	public class SqlDal : IAccessable
	{
        #region Sql Connection
        // Der Konstruktor erstellt eine neue SqlConnection mit dem angegebenen ConnectionString.
        private readonly SqlConnection conn;
        // Der Konstruktor wird in der HomeController-Klasse aufgerufen.
        public SqlDal(string connString)
		{
            // Erstellt eine neue SqlConnection mit dem angegebenen ConnectionString.
            conn = new SqlConnection(connString);
		}
		#endregion


		#region CRUD-Methoden für Artikel
		#region Delete Article
		
		public bool DeleteArticle(int Aid)
		{
            // SQL-Befehl zum Löschen eines Artikel mit der angegebenen Aid.
            string Deletestring = "DELETE FROM Article WHERE Aid = @Aid";
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand deleteCmd = new SqlCommand(Deletestring, conn);
            // Fügt den Wert der angegebenen Aid als Parameter zum SqlCommand-Objekt hinzu.
            deleteCmd.Parameters.AddWithValue("@Aid", Aid);
            // Öffnet die Verbindung zur Datenbank.
            conn.Open();
            // Führt den SQL-Befehl aus und speichert die Anzahl der betroffenen Zeilen in affectedRows.
            int affectedRows = deleteCmd.ExecuteNonQuery();
            // Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt zurück, ob eine Zeile betroffen war.
            return affectedRows == 1;
		}
		#endregion
		#region Get All Articles
		public List<Article> GetAllArticles()
		{
            // SQL-Befehl zum Abrufen aller Artikel mit der zugehörigen Kategorie.
            string selectAll = "select Article.*, Category.CatName from Article INNER JOIN Category ON Article.CatId = Category.Cid";
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand selectAllCmd = new SqlCommand(selectAll, conn);
            // Öffnet die Verbindung zur Datenbank.
            conn.Open();
            // Führt den SQL-Befehl aus und speichert das Ergebnis in einem SqlDataReader-Objekt.
            SqlDataReader reader = selectAllCmd.ExecuteReader();
            // Erstellt eine neue Liste von Artikel-Objekten.
            List<Article> articles = new List<Article>();
            // Liest die Ergebnisse des SQL-Befehls aus und fügt sie der Liste hinzu.
            while (reader.Read())
            {
                // Erstellt ein neues Artikel-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                Article article = new Article
				{
                    // Fügt die Werte der Spalten des SQL-Befehls den Eigenschaften des Artikel-Objekts hinzu.
                    Aid = (int)reader["Aid"],
					ArticleName = (string)reader["ArticleName"],
					Price = (decimal)reader["Price"],
					Description = (string)reader["Description"],
                    // Erstellt ein neues Kategorie-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                    Category = new Category
					{

						CatName = (string)reader["CatName"]
					}
				};
                // Fügt das Artikel-Objekt der Liste hinzu.
                articles.Add(article);
			}
            // Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt die Liste der Artikel zurück.
            return articles;
		}
		#endregion
		#region Get Article By Id
		public Article GetArticleById(int Aid)
		{
            // SQL-Befehl zum Abrufen eines Artikel mit der angegebenen Aid und der zugehörigen Kategorie.
            string selectArticleById = "select Article.*, Category.CatName from Article INNER JOIN Category ON Article.CatId = Category.Cid WHERE Aid = @Aid";
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand selectCmd = new SqlCommand(selectArticleById, conn);
            // Fügt den Wert der angegebenen Aid als Parameter zum SqlCommand-Objekt hinzu.
            selectCmd.Parameters.AddWithValue("@Aid", Aid);
            // Öffnet die Verbindung zur Datenbank.
            conn.Open();
            // Führt den SQL-Befehl aus und speichert das Ergebnis in einem SqlDataReader-Objekt.
            SqlDataReader reader = selectCmd.ExecuteReader();
            // Erstellt ein neues Artikel-Objekt.
            Article article = new Article();
            // Liest die Ergebnisse des SQL-Befehls aus und füllt das Artikel-Objekt mit den Daten.
            if (reader.Read())
			{
                // Fügt die Werte der Spalten des SQL-Befehls den Eigenschaften des Artikel-Objekts hinzu.
                article.Aid = (int)reader["Aid"];
				article.ArticleName = (string)reader["ArticleName"];
				article.Price = (decimal)reader["Price"];
				article.Description = (string)reader["Description"];
				article.CatId = (int)reader["CatId"];
                // Erstellt ein neues Kategorie-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                article.Category = new Category
				{

					CatName = (string)reader["CatName"]
				};
			}
            // Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt das Artikel-Objekt zurück.
            return article;
		}
		#endregion
		#region Get Articles By Search Index
		public List<Article> GetArticlesBySearchIndex(string searchString)
		{
            // SQL-Befehl zum Abrufen aller Artikel, die den angegebenen Suchbegriff enthalten.
            string searchQuery = $"SELECT Article.*, Category.CatName FROM Article INNER JOIN Category ON Article.CatId = Category.Cid WHERE ArticleName LIKE '%{searchString.ToLower()}%' OR Description LIKE '%{searchString.ToLower()}%'";
            // Erstellt eine neue Liste von Artikel-Objekten.
            List<Article> articles = new List<Article>();
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand searchCmd = new SqlCommand(searchQuery, conn);
            //Fügt den Wert der angegebenen searchString als Parameter zum SqlCommand-Objekt hinzu.
            searchCmd.Parameters.AddWithValue("@searchString", searchString);
            //Öffnet die Verbindung zur Datenbank.
            conn.Open();
            //Führt den SQL-Befehl aus und speichert das Ergebnis in einem SqlDataReader-Objekt.
            SqlDataReader reader = searchCmd.ExecuteReader();
            //Liest die Ergebnisse des SQL-Befehls aus und fügt sie der Liste hinzu.
            while (reader.Read())
			{
                //Erstellt ein neues Artikel-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                Article article = new Article();
				article.Aid = (int)reader["Aid"];
				article.ArticleName = (string)reader["ArticleName"];
				article.Price = (decimal)reader["Price"];
				article.Description = (string)reader["Description"];
                //Erstellt ein neues Kategorie-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                article.Category = new Category
				{

					CatName = (string)reader["CatName"]
				};
                //Fügt das Artikel-Objekt der Liste hinzu.
                articles.Add(article);
			}
            //Schließt die Verbindung zur Datenbank.
            conn.Close();
            //Gibt die Liste der Artikel zurück.
            return articles;
		}
        #endregion
        #region Insert Article
        public int InsertArticle(IndexViewModel model)
        {
            // Überprüfen, ob das Model null ist
            if (model == null)
            {
                //wenn ja, wird eine ArgumentNullException ausgelöst
                throw new ArgumentNullException(nameof(model));
            }
            // SQL-Befehl zum Einfügen eines neuen Artikels in die Datenbank.
            string insertString = @"
        INSERT INTO Article (ArticleName, Price, Description, CatId) 
        OUTPUT inserted.Aid 
        VALUES (@ArticleName, @Price, @Description, @CatId)";
			// Versuche...
            try
            {
                // Öffne die Verbindung zur Datenbank
                conn.Open();
                // Erstelle ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
                using (SqlCommand insertCmd = new SqlCommand(insertString, conn))
                    {
                    // Füge die Werte der Eigenschaften des Artikel-Objekts als Parameter zum SqlCommand-Objekt hinzu.
                    insertCmd.Parameters.AddWithValue("@ArticleName", model.Article.ArticleName );
                        insertCmd.Parameters.AddWithValue("@Price", model.Article.Price);
                        insertCmd.Parameters.AddWithValue("@Description", model.Article.Description );
                        insertCmd.Parameters.AddWithValue("@CatId", model.Article.CatId);
                    // Führe den SQL-Befehl aus und speichere die ID des eingefügten Artikels.
                    int insertedId = (int)insertCmd.ExecuteScalar();
                    // Schließe die Verbindung zur Datenbank.
                    conn.Close();
                    // Gibt die eingefügte ID zurück.
                    return insertedId;
                    }
                
            }
            // Wenn ein Fehler auftritt...
            catch (Exception ex)
            {
                // Logge den Fehler und zeige eine Nachricht an
                Console.WriteLine($"Fehler beim Einfügen des Artikels: {ex.Message}");
                throw;
            }
        }
        #endregion
        #region Update Article
        public bool UpdateArticle(IndexViewModel model)
		{
            // SQL-Befehl zum Aktualisieren eines Artikels in der Datenbank.
            string UpdateString = "UPDATE Article SET ArticleName = @ArticleName, Price = @Price, Description = @Description, CatId = @CatId WHERE Aid = @Aid";
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand updateCmd = new SqlCommand(UpdateString, conn);
            // Fügt die Werte der Eigenschaften des Artikel-Objekts als Parameter zum SqlCommand-Objekt hinzu.
            updateCmd.Parameters.AddWithValue("@Aid", model.Article.Aid);
			updateCmd.Parameters.AddWithValue("@ArticleName", model.Article.ArticleName);
			updateCmd.Parameters.AddWithValue("@Price", model.Article.Price);
			updateCmd.Parameters.AddWithValue("@Description", model.Article.Description);
			updateCmd.Parameters.AddWithValue("@CatId", model.Article.CatId);
            // Öffnet die Verbindung zur Datenbank.
            conn.Open();
            // Führt den SQL-Befehl aus und speichert die Anzahl der betroffenen Zeilen in affectedRows.
            int affectedRows = updateCmd.ExecuteNonQuery();
            // Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt zurück, ob eine Zeile betroffen war.
            return affectedRows == 1;
		}
		#endregion
		#region Get Articles By Category
		public List<Article> GetArticlesByCategory(int CatId)
		{
            // SQL-Befehl zum Abrufen aller Artikel mit der angegebenen Kategorie.
            string selectByCategory = "SELECT Article.*, Category.CatName FROM Article INNER JOIN Category ON Article.CatId = Category.Cid WHERE Cid = @Cid";
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand selectCmd = new SqlCommand(selectByCategory, conn);
            // Fügt den Wert der angegebenen CatId als Parameter zum SqlCommand-Objekt hinzu.
            selectCmd.Parameters.AddWithValue("@Cid", CatId);
            // Öffnet die Verbindung zur Datenbank.
            conn.Open();
            // Führt den SQL-Befehl aus und speichert das Ergebnis in einem SqlDataReader-Objekt.
            SqlDataReader reader = selectCmd.ExecuteReader();
            // Erstellt eine neue Liste von Artikel-Objekten.
            List<Article> articles = new List<Article>();
            // Liest die Ergebnisse des SQL-Befehls aus und fügt sie der Liste hinzu.
            while (reader.Read())
			{
                // Erstellt ein neues Artikel-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                Article article = new Article
                {
                    // Fügt die Werte der Spalten des SQL-Befehls den Eigenschaften des Artikel-Objekts hinzu.
                    Aid = (int)reader["Aid"],
					ArticleName = (string)reader["ArticleName"],
					Price = (decimal)reader["Price"],
					Description = (string)reader["Description"],
					CatId = (int)reader["CatId"],
                    // Erstellt ein neues Kategorie-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                    Category = new Category
					{
						CatName = (string)reader["CatName"]
					}
				};
                // Fügt das Artikel-Objekt der Liste hinzu.
                articles.Add(article);
			}
            // Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt die Liste der Artikel zurück.
            return articles;
		}
		#endregion
		#endregion


		#region CRUD-Methoden für Kategorien
		#region Get All Categories
		public List<Category> GetAllCategories()
		{
            // SQL-Befehl zum Abrufen aller Kategorien.
            string selectAll = "SELECT * FROM Category";
            // Erstellt eine neue Liste von Kategorie-Objekten.
            List<Category> categories = new List<Category>();
            // Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand selectAllCmd = new SqlCommand(selectAll, conn);
            // Öffnet die Verbindung zur Datenbank.
            conn.Open();
            // Führt den SQL-Befehl aus und speichert das Ergebnis in einem SqlDataReader-Objekt.
            SqlDataReader reader = selectAllCmd.ExecuteReader();
            // Liest die Ergebnisse des SQL-Befehls aus und fügt sie der Liste hinzu.
            while (reader.Read())
			{
                // Erstellt ein neues Kategorie-Objekt und füllt es mit den Daten aus dem SqlDataReader-Objekt.
                Category category = new Category
                {
                    // Fügt die Werte der Spalten des SQL-Befehls den Eigenschaften des Kategorie-Objekts hinzu.
                    Cid = (int)reader["Cid"],
					CatName = (string)reader["CatName"]
				};
                // Fügt das Kategorie-Objekt der Liste hinzu.
                categories.Add(category);
            }
            // Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt die Liste der Kategorien zurück.
            return categories;
		}
		#endregion
		#region Insert Category
		public int InsertCategory(Category category)
		{
            //SQL-Befehl zum Einfügen einer neuen Kategorie in die Datenbank.
            string InsertString = "INSERT INTO Category (CatName) OUTPUT inserted.cid VALUES (@CatName);";
            //Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand insertCmd = new SqlCommand(InsertString, conn);
            //Fügt den Wert der Eigenschaften des Kategorie-Objekts als Parameter zum SqlCommand-Objekt hinzu.
            insertCmd.Parameters.AddWithValue("@CatName", category.CatName);
            //Öffnet die Verbindung zur Datenbank.
            conn.Open();
            //Führt den SQL-Befehl aus und speichert die ID der eingefügten Kategorie.
            int insertedId = (int)insertCmd.ExecuteScalar();
            //Schließt die Verbindung zur Datenbank.
            conn.Close();
            // Gibt die eingefügte ID zurück.
            return insertedId;
		}
		#endregion
		#region Update Category
		public bool UpdateCategory(Category category)
		{
            //SQL-Befehl zum Aktualisieren einer Kategorie in der Datenbank.
            string UpdateString = "UPDATE Category SET CatName = @CatName WHERE Cid = @Cid";
            //Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand updateCmd = new SqlCommand(UpdateString, conn);
            //Fügt den Wert der Eigenschaften des Kategorie-Objekts als Parameter zum SqlCommand-Objekt hinzu.
            updateCmd.Parameters.AddWithValue("@Cid", category.Cid);
            //Fügt den Wert der Eigenschaften des Kategorie-Objekts als Parameter zum SqlCommand-Objekt hinzu.
            updateCmd.Parameters.AddWithValue("@CatName", category.CatName);
            //Öffnet die Verbindung zur Datenbank.
            conn.Open();
            //Führt den SQL-Befehl aus und speichert die Anzahl der betroffenen Zeilen in affectedRows.
            int affectedRows = updateCmd.ExecuteNonQuery();
            //Schließt die Verbindung zur Datenbank.
            conn.Close();
            //Gibt zurück, ob eine Zeile betroffen war.
            return affectedRows == 1;

		}
		#endregion
		#region Delete Category
		public bool DeleteCategory(int Cid)
		{
            //SQL-Befehl zum Löschen einer Kategorie mit der angegebenen Cid.
            string DeleteString = "DELETE FROM Category WHERE Cid = @Cid";
            //Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand deleteCmd = new SqlCommand(DeleteString, conn);
            //Fügt den Wert der angegebenen Cid als Parameter zum SqlCommand-Objekt hinzu.
            deleteCmd.Parameters.AddWithValue("@Cid", Cid);
            //Öffnet die Verbindung zur Datenbank.
            conn.Open();
            //Führt den SQL-Befehl aus und speichert die Anzahl der betroffenen Zeilen in affectedRows.
            int affectedRows = deleteCmd.ExecuteNonQuery();
            //Schließt die Verbindung zur Datenbank.
            conn.Close();
            //Gibt zurück, ob eine Zeile betroffen war.
            return affectedRows == 1;
		}
		#endregion
		#region Get Category By Id
		public Category GetCategoryById(int Cid)
        {
            //SQL-Befehl zum Abrufen einer Kategorie mit der angegebenen Cid.
            string selectCategoryById = "SELECT * FROM Category WHERE Cid = @Cid";
            //Erstellt ein neues SqlCommand-Objekt mit dem angegebenen SQL-Befehl und der SqlConnection.
            SqlCommand selectCmd = new SqlCommand(selectCategoryById, conn);
            //Fügt den Wert der angegebenen Cid als Parameter zum SqlCommand-Objekt hinzu.
            selectCmd.Parameters.AddWithValue("@Cid", Cid);
            //Öffnet die Verbindung zur Datenbank.
            conn.Open();
            //Führt den SQL-Befehl aus und speichert das Ergebnis in einem SqlDataReader-Objekt.
            SqlDataReader reader = selectCmd.ExecuteReader();
            //Erstellt ein neues Kategorie-Objekt.
            Category category = new Category();
            //Liest die Ergebnisse des SQL-Befehls aus und füllt das Kategorie-Objekt mit den Daten.
            if (reader.Read())
			{
                //Fügt die Werte der Spalten des SQL-Befehls den Eigenschaften des Kategorie-Objekts hinzu.
                category.Cid = (int)reader["Cid"];
				category.CatName = (string)reader["CatName"];
			}
            //Schließt die Verbindung zur Datenbank.
            conn.Close();
            //Gibt das Kategorie-Objekt zurück.
            return category;

		}
		#endregion
		#endregion

	}
}