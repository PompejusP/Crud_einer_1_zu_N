using Web_FIA44_CRUD_einer_1_zu_N.Models;
using Web_FIA44_CRUD_einer_1_zu_N.ViewModels;

namespace Web_FIA44_CRUD_einer_1_zu_N.DAL
{
	public interface IAccessable
	{
		#region Article CRUD
		int InsertArticle(IndexViewModel model);

		Article GetArticleById(int Aid);

		List<Article> GetAllArticles();

		bool UpdateArticle(IndexViewModel model);
		bool DeleteArticle(int Aid);

		List<Article> GetArticlesBySearchIndex(string searchString);

		List<Article> GetArticlesByCategory(int CatId);
		#endregion
		#region Category CRUD
		List<Category> GetAllCategories();
		Category GetCategoryById(int CatId);
		bool DeleteCategory(int CatId);
		bool UpdateCategory(Category category);
		int InsertCategory(Category category);

		#endregion

	}
}
