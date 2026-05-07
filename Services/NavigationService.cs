using Microsoft.Extensions.DependencyInjection;
using PhoneBook.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
	public class NavigationService : ObservableObject, INavigationService
	{
		private readonly IServiceProvider _serviceProvider;
		private object? _currentViewModel;

		public NavigationService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public object? CurrentViewModel
		{
			get => _currentViewModel;
			private set
			{
				_currentViewModel = value;
				OnPropertyChanged();
			}
		}

		public void NavigateTo<TVireModel>(object? parameter = null) where TVireModel : class
		{
			//получаем vm из DI
			var vm = _serviceProvider.GetRequiredService<TVireModel>();

			//если vm поддерживает прием параметров
			if (vm is INavigationAware navigationAware)
			{
				navigationAware.OnNavigatedTo(parameter);
			}

			//обнровление currentVM
			//ContentControl подхватит изменения
			CurrentViewModel = vm;
		}
	}
}
