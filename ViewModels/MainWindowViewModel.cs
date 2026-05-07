using PhoneBook.Services;
using PhoneBook.ViewModels;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
	public class MainWindowViewModel
	{
		private readonly INavigationService _navigation;

		public MainWindowViewModel(INavigationService navigation)
		{
			_navigation = navigation;
			ShowContactsCommand = new RelayCommand(() => _navigation.NavigateTo<ContactsListViewModel>());
			ShowAboutCommand = new RelayCommand(() => _navigation.NavigateTo<AboutViewModel>());

			_navigation.NavigateTo<ContactsListViewModel>();
		}

		public INavigationService NavigationService => _navigation;
		public ICommand ShowContactsCommand { get; }
		public ICommand ShowAboutCommand { get; }
	}
}