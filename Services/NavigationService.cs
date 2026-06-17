using Microsoft.Extensions.DependencyInjection;
using PhoneBook.ViewModels;
using System;

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

		public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
		{
			// Получаем ViewModel из DI контейнера
			var vm = _serviceProvider.GetRequiredService<TViewModel>();

			// Если ViewModel поддерживает INavigationAware
			if (vm is INavigationAware navigationAware)
			{
				navigationAware.OnNavigatedTo(parameter);
			}

			// Обновляем CurrentViewModel
			CurrentViewModel = vm;
		}
	}
}