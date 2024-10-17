namespace Web_FIA44_CRUD_einer_1_zu_N
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllersWithViews();
			var app = builder.Build();

			app.MapControllerRoute(name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.UseStaticFiles();

			app.UseRouting();
			app.Run();
		}
	}
}
