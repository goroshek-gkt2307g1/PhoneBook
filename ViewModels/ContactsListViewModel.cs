using PhoneBook.Entities;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
	//связывает view и model
	public class ContactsListViewModel : ObservableObject
	{
		//коллекция контактов
		public ObservableCollection<Models.Contact> Contacts { get; set; }
		private string _name = string.Empty; //приватное поле для временного вводимого имени
		private string _phone = string.Empty; //приватное поле для временного вводимого телефона
		private Models.Contact? _selectedContact; //приватное поле для хранения выбранного в датагрид контакта
		private readonly IDialogService _dialogService;
		private readonly INavigationService _navigation;
		private readonly PhoneBookDbVlasova2307g1Context _context;

		//свойство для привязки к текстбокс имени
		public string Name
		{
			get => _name;
			set => Set(ref _name, value);
		}

		//свойство для привязки к текстбокс номера
		public string Phone
		{
			get => _phone;
			set => Set(ref _phone, value);
		}

		//свойство для привязки выбранного объекта в датагрид
		public Models.Contact? SelectedContact
		{
			get => _selectedContact;
			set => Set(ref _selectedContact, value);
		}

		//команды
		public ICommand AddCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand EditCommand { get; }

		//конструктор инициализирует коллекцию и команды
		// Constructor Injection: DI-контейнер автоматически
		// передаёт реализацию IDialogService
		public ContactsListViewModel(IDialogService dialogService, INavigationService navigation, PhoneBookDbVlasova2307g1Context context)
		{
			_context = context;
			var contactsFromDb = _context.Contacts
				.Select(c => new Models.Contact(c.Name, c.Phone)) //используем существующий конструктор
				.ToList();
			Contacts = new ObservableCollection<Models.Contact>(contactsFromDb);

			AddCommand = new RelayCommand(AddContact, () => CanAddContact());
			DeleteCommand = new RelayCommand(DeleteContact, () => CanDeleteContact());
			EditCommand = new RelayCommand(EditContact, () => SelectedContact != null);

			_navigation = navigation;
			_dialogService = dialogService;
		}

		//метод выполнения команды добавления контакта
		private void AddContact()
		{
			if (Contacts.Any(c => c.Phone == _phone))
			{
				_dialogService.ShowWarning("Контакт с таким номером уже существует!");
				return;
			}

			//создаём Model (с валидацией)
			Models.Contact contact = new Models.Contact(Name, Phone);

			//маппим в Entity и сохраняем в БД
			var entityContact = new Entities.Contact
			{
				Name = contact.Name,
				Phone = contact.Phone
			};
			_context.Contacts.Add(entityContact);
			_context.SaveChanges();

			//добавляем Model в коллекцию
			Contacts.Add(contact);

			_dialogService.ShowInfo("Номер телефона успешно добавлен!");
			Name = string.Empty;
			Phone = string.Empty;
		}

		//метод проверки возможности добавления контакта
		private bool CanAddContact()
		{
			return Models.Contact.IsValid(Name, Phone);
		}

		//метод выполнения команды удаления контакта
		private void DeleteContact()
		{
			if (SelectedContact != null)
			{
				bool result = _dialogService.ShowYesOrNo("Вы уверены, что хотите удалить этот контакт?");
				if (result)
				{
					//находим Entity
					var entityToDelete = _context.Contacts
						.FirstOrDefault(c => c.Phone == SelectedContact.Phone);

					if (entityToDelete != null)
					{
						_context.Contacts.Remove(entityToDelete);
						_context.SaveChanges();
					}

					Contacts.Remove(SelectedContact);
				}
			}
		}

		//метод проверки возможности удаления контакта
		private bool CanDeleteContact()
		{
			return SelectedContact != null;
		}

		//метод редактирования контакта
		private void EditContact()
		{
			if (SelectedContact != null)
			{
				_navigation.NavigateTo<ContactEditViewModel>(SelectedContact);
			}
		}
	}
}