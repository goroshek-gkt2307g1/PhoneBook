МИНИСТЕРСТВО ОБРАЗОВАНИЯ И НАУКИ РОССИЙСКОЙ ФЕДЕРАЦИИ

Федеральное государственное автономное образовательное учреждение

высшего образования

«Новосибирский национальный исследовательский государственный
университет»

(Новосибирский государственный университет, НГУ)

Структурное подразделение Новосибирского государственного университета
-- Высший колледж информатики Университета (ВКИ НГУ)

КАФЕДРА ИНФОРМАТИКИ

**Лабораторно-практическая работа №11**

**Выполнила**: Власова А.А

**Группа**: 2307г1

**Проверил**: Макаров М. С.

Новосибирск

2026

# Оглавление

Формулировка задания

Теоретическое обоснование

Описание выполненных действий

Результат выполненной работы 

Исходный код модуля

# Формулировка задания

**Тема**: Навигация в MVVM-приложениях. Подход ViewModel-First

**Цель работы:** изучить механизмы навигации между экранами в MVVM-приложении без использования стандартных средств WPF (Frame/Page). В ходе работы будет реализован паттерн ViewModel-First навигации с использованием ContentControl и DataTemplate. Будет создан сервис INavigationService для управления переключением экранов, а существующая логика «Телефонной книги» будет энкапсулирована в отдельный модуль (View/ViewModel), встраиваемый в главное окно приложения.

**Задание**: выполнить рефакторинг приложения «Телефонная книга» из Лабораторной работы No10 в архитектуру Shell (Оболочка).

Требования к доработке:

- Создать структуру папок: Разделить Views и ViewModels по папкам (Views и ViewModels).
- Рефакторинг: переименовать ViewModel в ContactsListViewModel, а MainWindow.xaml — в ContactsListView (UserControl).
- Создать Shell (Главное окно) с меню и областью контента (ContentControl) и MainWindowViewModel.
- Разработать интерфейс INavigationService с методом NavigateTo\<TViewModel\>() и реализовать сервис навигации.
- Настроить DI: зарегистрировать ViewModels, определить DataTemplate в App.xaml.
- Создать дополнительный экран «О программе» (AboutViewModel / AboutView).

# Теоретическое обоснование

**2.1. Подходы к навигации в WPF**
В WPF существует несколько способов организации навигации, каждый из которых имеет свои плюсы и минусы в контексте MVVM. В данной работе используется подход ContentControl + DataTemplate как наиболее «чистый» с точки зрения архитектуры WPF и MVVM. Этот подход обеспечивает полную поддержку DI-контейнеров и не требует подключения сторонних библиотек.

**2.2. ViewModel-First навигация и ContentControl**
В основе подхода ViewModel-First лежит утверждение: «ViewModel — это приложение». Мы не создаём View явно; мы создаём ViewModel (с помощью DI-контейнера), а WPF самостоятельно подбирает подходящее визуальное представление, используя DataTemplate. Главное окно (Shell) содержит ContentControl, привязанный к свойству CurrentViewModel. При изменении этого свойства ContentControl автоматически перерисовывает содержимое, заменяя один UserControl другим.

**2.3. Навигация vs Диалоговые окна**
Навигация по приложению используется для смены основного контента в окне и не блокирует главное окно, реализуется через INavigationService. Диалоговые окна, напротив, используются для временного прерывания потока работы, блокируют родительское окно и реализуются через IDialogService. В этой работе мы реализуем именно навигацию внутри главного окна, превращая текущее главное окно в «контейнер» (Shell) для различных экранов.

# Описание выполненных действий

**4.1. Подготовка и рефакторинг проекта**
Открыто решение с проектом «Телефонная книга» из лабораторной работы №10. В проекте созданы папки Views и ViewModels для структурирования классов. Существующий файл MainWindow.xaml преобразован в UserControl с именем ContactsListView и перемещён в папку Views. Класс MainViewModel переименован в ContactsListViewModel и перенесён в папку ViewModels. Из App.xaml удалён атрибут StartupUri, так как старый MainWindow теперь является UserControl'ом, а не окном.

**4.2. Реализация интерфейсов INavigationService и INavigationAware**
В папке Services созданы интерфейсы INavigationService и INavigationAware. INavigationService объявляет метод NavigateTo\<TViewModel\>() для переключения экранов и свойство CurrentViewModel для отслеживания текущего состояния. INavigationAware содержит метод OnNavigatedTo для приёма параметров при навигации. Создан класс NavigationService, реализующий оба интерфейса. Сервис получает ViewModel из DI-контейнера, опционально передаёт параметры через OnNavigatedTo и обновляет CurrentViewModel. NavigationService наследуется от ObservableObject для уведомления об изменениях через INotifyPropertyChanged.

**4.3. Создание ViewModel и View для списка контактов**
ContactsListViewModel (бывший MainViewModel) доработан: в конструктор добавлен параметр INavigationService, создана команда EditCommand, которая вызывает NavigateTo\<ContactEditViewModel\>, передавая выбранный контакт. ContactsListView.xaml содержит разметку с полями ввода имени и телефона, кнопками «Добавить», «Удалить», «Редактировать» и DataGrid для отображения списка контактов.

**4.4. Экран редактирования контакта**
Создан новый экран ContactEditViewModel, реализующий интерфейс INavigationAware. При навигации через OnNavigatedTo принимает объект Contact, копирует его данные в свойства EditName и EditPhone. Содержит команды SaveCommand и CancelCommand, при выполнении которых происходит возврат к списку контактов через NavigationService. Файл ContactEditView.xaml представляет собой UserControl с полями редактирования и кнопками «Сохранить» и «Отмена».

**4.5. Создание дополнительного экрана «О программе»**
Для демонстрации переключения экранов созданы AboutViewModel с строковыми свойствами AppName и Version, и AboutView (UserControl), отображающий эти свойства через привязку данных.

**4.6. Создание оболочки приложения (Shell)**
Создан новый файл MainWindow.xaml, который является «настоящим» главным окном приложения. Он содержит DockPanel с меню навигации (кнопки «Контакты» и «О программе») и ContentControl, Content которого привязан к свойству NavigationService.CurrentViewModel. Создана MainWindowViewModel, которая через команды ShowContactsCommand и ShowAboutCommand управляет навигацией, а при создании автоматически открывает экран контактов.

**4.7. Настройка DataTemplate и DI**
В App.xaml в ресурсах приложения добавлены DataTemplate, связывающие типы ViewModel с соответствующими View (ContactsListViewModel → ContactsListView, AboutViewModel → AboutView, ContactEditViewModel → ContactEditView). В App.xaml.cs в методе OnStartup настроен DI-контейнер: NavigationService и MainWindowViewModel зарегистрированы как Singleton, экранные ViewModels — как Transient для получения новых экземпляров при каждой навигации. Главное окно создаётся с явной установкой DataContext через лямбда-выражение.

# Результат выполненной работы
<img width="901" height="390" alt="image" src="https://github.com/user-attachments/assets/02d66cac-a874-4081-86d8-ce7c0b96dad8" />


using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.ViewModels;
namespace TestProject1;

[TestClass]

public class UnitTest1
{

	//Mock-реализация IDialogService для тестирования
	internal class MockDialogService(bool isConfirmed) : IDialogService
	{
  
		private bool _isConfirmed = isConfirmed;
    
		public bool ShowYesOrNo(string message, string title = "Подтверждение")
		{
			return _isConfirmed;
		}

		public void ShowError(string message, string title = "Ошибка")
		{
			throw new NotImplementedException();
		}

		public void ShowInfo(string message, string title = "Информация")
		{
		}

		public void ShowWarning(string message, string title = "Предупреждение")
		{
		}
	}

	//Mock-реализация INavigationService для тестирования навигации
	internal class MockNavigationService : INavigationService
	{
		public object? CurrentViewModel { get; private set; }
		public object? LastNavigatedParameter { get; private set; }
		public int NavigateCallCount { get; private set; }

		public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
		{
			LastNavigatedParameter = parameter;
			NavigateCallCount++;
		}
	}

	//Удаление контакта с подтверждением
	[TestMethod]
	public void BaseMethod()
	{
		var dialogService = new MockDialogService(true);
		var navigationService = new MockNavigationService();
		var vm = new ContactsListViewModel(dialogService, navigationService);
		var contact = new Contact("Vasily", "+78005553535");
		vm.Contacts.Add(contact);
		vm.SelectedContact = contact;
		vm.DeleteCommand?.Execute(null);

		Assert.AreEqual(0, vm.Contacts.Count);
	}

	//Валидация имени (пустое имя вызывает исключение)
	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void ValidateName()
	{
		var dialogService = new MockDialogService(true);
		var navigationService = new MockNavigationService();
		var vm = new ContactsListViewModel(dialogService, navigationService);
		var contact = new Contact("", "+78005553535");
		vm.Contacts.Add(contact);
	}

	//Валидация номера (пустой номер вызывает исключение)
	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void ValidateEmptyPhone()
	{
		var dialogService = new MockDialogService(true);
		var navigationService = new MockNavigationService();
		var vm = new ContactsListViewModel(dialogService, navigationService);
		var contact = new Contact("Vasily", "");
		vm.Contacts.Add(contact);
	}

	//Валидация номера (номер не начинается с +7 вызывает исключение)
	[TestMethod]
	[ExpectedException(typeof(ArgumentException))]
	public void ValidateStartsWithPhone()
	{
		var dialogService = new MockDialogService(true);
		var navigationService = new MockNavigationService();
		var vm = new ContactsListViewModel(dialogService, navigationService);
		var contact = new Contact("Vasily", "88005553535");
		vm.Contacts.Add(contact);
	}

	//Проверка навигации к экрану редактирования
	[TestMethod]
	public void EditContact_NavigatesToEditView()
	{
		var dialogService = new MockDialogService(true);
		var navigationService = new MockNavigationService();
		var vm = new ContactsListViewModel(dialogService, navigationService);
		var contact = new Contact("Vasily", "+78005553535");
		vm.Contacts.Add(contact);
		vm.SelectedContact = contact;

		vm.EditCommand?.Execute(null);

		Assert.AreEqual(1, navigationService.NavigateCallCount);
		Assert.AreSame(contact, navigationService.LastNavigatedParameter);
	}

	//Отмена удаления контакта (пользователь отказался)
	[TestMethod]
	public void DeleteContact_UserDeclines_ContactRemains()
	{
		var dialogService = new MockDialogService(false);
		var navigationService = new MockNavigationService();
		var vm = new ContactsListViewModel(dialogService, navigationService);
		var contact = new Contact("Vasily", "+78005553535");
		vm.Contacts.Add(contact);
		vm.SelectedContact = contact;

		vm.DeleteCommand?.Execute(null);

		Assert.AreEqual(1, vm.Contacts.Count);
	}
}
