using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Services;
using PhoneBook.ViewModels;
using System.Windows;

namespace PhoneBook
{
    public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var services = new ServiceCollection();

			//регистрация сервисов (Singleton - один экземпляр на всё приложение)
			services.AddSingleton<INavigationService, NavigationService>();
			services.AddSingleton<IDialogService, DialogService>();
			services.AddSingleton<ContactsListViewModel>();

			//регистрация ViewModels (Transient - новый экземпляр при каждом запросе)
			services.AddTransient<AboutViewModel>();
			services.AddTransient<ContactEditViewModel>();

			//регистрация MainWindowViewModel как Singleton
			services.AddSingleton<MainWindowViewModel>();

			//регистрация MainWindow
			services.AddSingleton<MainWindow>(sp =>
			{
				var window = new MainWindow();
				window.DataContext = sp.GetRequiredService<MainWindowViewModel>();
				return window;
			});

			var sp = services.BuildServiceProvider();

			//показываем главное окно
			var mainWindow = sp.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}
	}
}