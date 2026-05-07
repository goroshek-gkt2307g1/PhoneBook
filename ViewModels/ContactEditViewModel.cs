using PhoneBook.Models;
using PhoneBook.Services;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
	public class ContactEditViewModel : ObservableObject, INavigationAware
	{
		private readonly INavigationService _navigation;
		private readonly IDialogService _dialogService;
		private Contact _originalContact = null!;
		private string _editName = string.Empty;
		private string _editPhone = string.Empty;

		public ContactEditViewModel(INavigationService navigation, IDialogService dialogService)
		{
			_navigation = navigation;
			_dialogService = dialogService;

			SaveCommand = new RelayCommand(SaveContact, CanSaveContact);
			CancelCommand = new RelayCommand(CancelEdit);
		}

		public string EditName
		{
			get => _editName;
			set
			{
				_editName = value;
				OnPropertyChanged();
			}
		}

		public string EditPhone
		{
			get => _editPhone;
			set
			{
				_editPhone = value;
				OnPropertyChanged();
			}
		}

		public ICommand SaveCommand { get; }
		public ICommand CancelCommand { get; }

		public void OnNavigatedTo(object? parameter)
		{
			if (parameter is Contact contactToEdit)
			{
				//сохраняем ссылку на оригинальный контакт
				_originalContact = contactToEdit;

				//копируем данные для редактирования
				EditName = _originalContact.Name;
				EditPhone = _originalContact.Phone;
			}
		}

		private void SaveContact()
		{
			if (_originalContact != null)
			{
				//обновляем свойства существующего контакта
				_originalContact.Name = EditName;
				_originalContact.Phone = EditPhone;

				_dialogService.ShowInfo("Контакт успешно обновлен!");
			}

			_navigation.NavigateTo<ContactsListViewModel>();
		}

		private bool CanSaveContact()
		{
			return Contact.IsValid(EditName, EditPhone);
		}

		private void CancelEdit()
		{
			_navigation.NavigateTo<ContactsListViewModel>();
		}
	}
}