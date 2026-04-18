using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Linq;
using PhoneBook.VIewModels;

namespace PhoneBook.Models
{
	//модель контакта - отвечает за данные и бизнес-логику валидации
    public class Contact : ObservableObject
	{
		private string _name = string.Empty; //приватное имя
		private string _phone = string.Empty; //приватный телефон

		//новых контакт с валидацией входных данных
		public Contact(string name, string phone)
		{
			//присваиваем свойства 
			Name = name;
			Phone = phone;

			//валидность после установки
			if (Validate() == false)
				throw new ArgumentException("Неверные данные");
		}

		//публичное свойство для доступа к имени с уведом об изменениях
		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		//публичное свойство для доступа к номеру с уведом об изменениях
		public string Phone
		{
			get => _phone;
			set
			{
				_phone = value;
				OnPropertyChanged(nameof(Phone));
			}
		}

		//статический метод валидации, исп. до создания объекта
		public static bool IsValid(string name, string phone)
		{
			return !string.IsNullOrEmpty(name) && //имя не пустое
				!string.IsNullOrEmpty(phone) && //номер не пустой
				phone.StartsWith("+7") && //начинается на +7
				phone.Length >=12 && phone.Length <13; //12 цифр с кодом
		}

		//исп. после создания объекта
		public bool Validate() => IsValid(_name, _phone);

	}
}
