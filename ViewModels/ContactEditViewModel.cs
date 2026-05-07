using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.VIewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
	public class ContactEditViewModel : ObservableObject, INavigationAware
	{
		private readonly INavigationService _navigation;
		private Contact _contact = null!;

		public ContactEditViewModel(INavigationService navigation)
		{
			_navigation = navigation;
			SaveCommand = new RelayCommand(
				() => _navigation.NavigateTo<ContactsListViewModel>());
		}

		public string EditName
		{
			get => _contact.Name;
			set { _contact.Name = value; OnPropertyChanged(); }
		}
		public string EditPhone
		{
			get => _contact.Phone;
			set { _contact.Phone = value; OnPropertyChanged(); }
		}

		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }




		public void OnNavigatedTo(object? parameter)
		{
			throw new NotImplementedException();
		}
	}
}
