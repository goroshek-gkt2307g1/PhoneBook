using PhoneBook.Models;
using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneBook.VIewModels
{
    //связывает view и model
    public class MainViewModel : ObservableObject
    {
        //коллекция контактов
        public ObservableCollection<Contact> Contacts { get; }
        private string _name = string.Empty; //приватное поле для временного вводимого имени
        private string _phone = string.Empty; //приватное поле для временного вводимого телефона
		private Contact? _selectedContact; //приватное поле для хранения выбранного в датагрид контакта
        private readonly IDialogService _dialogService;

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
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        //команды
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

		//конструктор инициализирует коллекцию и команды
		// Constructor Injection: DI-контейнер автоматически
		// передаёт реализацию IDialogService
		public MainViewModel(IDialogService dialogService)
        {
            Contacts = new ObservableCollection<Contact>();
            AddCommand = new RelayCommand(AddContact, () => CanAddContact()); //создание команды добавления с методом выполнения и проверкой возможности
            DeleteCommand = new RelayCommand(DeleteContact, () => CanDeleteContact());
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
            Contact contact = new Contact(Name, Phone);
            Contacts.Add(contact);
            _dialogService.ShowInfo("Номер телефона успешно добавлен!");
            Name = string.Empty;
            Phone = string.Empty;
        }

		//метод проверки возможности добавления контакта
		private bool CanAddContact()
        {
            return Contact.IsValid(Name, Phone);
        }

		//метод выполнения команды удаления контакта
		private void DeleteContact()
        {
            if (SelectedContact != null)
            {
                bool result = _dialogService.ShowYesOrNo("Вы уверены, что хотите удалить этот контакт?");
                if (result) 
                {
					Contacts.Remove(SelectedContact);
				}
                else
                {
                    return;
                }
				
            }
        }

		//метод проверки возможности удаления контакта
		private bool CanDeleteContact()
        {
            if (SelectedContact != null)
                return true;
            return false;
        }

    }
}
