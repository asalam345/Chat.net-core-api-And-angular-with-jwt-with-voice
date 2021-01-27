using chat_server.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chat_server.Manager.IManagers
{
	public interface IManagerUser
	{
		Task<Result> loginDetails(UserVM model, string tokenValue);

	}
}
