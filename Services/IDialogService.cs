using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
	public interface IDialogService
	{
		void ShowInfo(string message, string title = "Информация");
		void ShowWarning(string message, string title = "Предупреждение");
		void ShowError(string message, string title = "Ошибка");
		bool ShowYesOrNo(string message, string title = "Подтверждение");
	}
}
