using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Services;
using PhoneBook.VIewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PhoneBook
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			//создание коллекции сервисов
			var services = new ServiceCollection();

			//регистрация сервисов
			//DialogService - singleton, не хранит состояние пользователя
			services.AddSingleton<IDialogService, DialogService>();

			//vm - transient (при навигации нужны будут новые экземпляры)
			services.AddTransient<ContactsListViewModel>();

			//главное окно - синглтон с явной передачей датаконтекст через лямбда-выражение
			services.AddSingleton<MainWindow>(sp =>
			{
				var window = new MainWindow();
				window.DataContext = sp.GetRequiredService<ContactsListViewModel>();
				return window;
			});

			//создаем контейнер (ServiceProvider)
			var serviceProvider =
			services.BuildServiceProvider();
			//получаем главное окно и запускаем его
			var mainWindow =
			serviceProvider.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}
	}

}
