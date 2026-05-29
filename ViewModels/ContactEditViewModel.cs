using PhoneBook.Entities;
using PhoneBook.Models;
using PhoneBook.Services;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
	public class ContactEditViewModel : ObservableObject, INavigationAware
	{
		private readonly INavigationService _navigation;
		private readonly IDialogService _dialogService;
		private Models.Contact _originalContact = null!;
		private string _editName = string.Empty;
		private string _editPhone = string.Empty;
		private readonly PhoneBookDbVlasova2307g1Context _context;


		public ContactEditViewModel(INavigationService navigation, IDialogService dialogService, PhoneBookDbVlasova2307g1Context context)
		{
			_context = context;
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
			if (parameter is Models.Contact contactToEdit)
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
				var entityToUpdate = _context.Contacts
					.FirstOrDefault(c => c.Phone == _originalContact.Phone);

				if (entityToUpdate != null)
				{
					try
					{
						//обновляем свойства существующего контакта
						entityToUpdate.Name = EditName;
						entityToUpdate.Phone = EditPhone;
						_context.SaveChanges();
						_dialogService.ShowInfo("Контакт успешно обновлен!");
						_navigation.NavigateTo<ContactsListViewModel>();	
					}
					catch(Exception ex)
					{
						_dialogService.ShowError(ex.Message)
	;				}
				}
			}

		}

		private bool CanSaveContact()
		{
			return Models.Contact.IsValid(EditName, EditPhone);
		}

		private void CancelEdit()
		{
			_navigation.NavigateTo<ContactsListViewModel>();
		}
	}
}