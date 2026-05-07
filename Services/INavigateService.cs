using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
	public interface INavigationService
	{
		object? CurrentViewModel { get; }
		void NavigateTo<TVireModel>(object? parameter = null)
			where TVireModel : class;
	}
}
}
