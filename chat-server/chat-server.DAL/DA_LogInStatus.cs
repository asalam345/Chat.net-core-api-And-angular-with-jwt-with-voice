﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_server.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using chat_server.Entity;
using chat_server.Entity.interfaces;

namespace chat_server.DAL
{
	public class DA_LogInStatus : SqlServerCommands, IGenericService<tblLogedinStatus>
    {
        public DA_LogInStatus() 
        {
        }
        public Task<Result> Delete(long id)
		{
			throw new NotImplementedException();
		}

		public async Task<Result> Entry(tblLogedinStatus model)
		{
            Result result = DefaultResult("Successfully!");
            try
            {
                if (model.IsLoged)
                {
                    //long loginStatusId = await getMaxRow("LoginStatusId", "LogInStatus") + 1;
                    DateTime dt = DateTime.Now;
                    string query = @"INSERT INTO tblLogedinStatus(LoginStatusId,IpAddress,UserId,IsLoged,Date,Time)
output INSERTED.LoginStatusId VALUES ((SELECT 1 + coalesce(max(LoginStatusId), 0) FROM tblLogedinStatus),'"
+ model.IpAddress + "','" + model.UserId + "', 1,GetDate(),'" + dt.ToShortTimeString() + "')";
                    result = await InsertOrUpdateOrDelete(query);
                }
                else
                {
                    await Update(model);
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }

            return result;
        }

		public Task<Result> Get(tblLogedinStatus model)
		{
			throw new NotImplementedException();
		}

		public async Task<Result> Update(tblLogedinStatus model)
		{
            Result result = DefaultResult("Successfully!");
            try
            {
                DateTime dt = DateTime.Now;
                string query = "UPDATE tblLogedinStatus SET IsLoged = 0,LogOutDate=GetDate(),LogOutTime = '"
                    + dt.ToShortTimeString() + "' WHERE IpAddress='" + model.IpAddress 
                    + "' AND UserId = '" + model.UserId + "'";
                result = await InsertOrUpdateOrDelete(query);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error:" + ex.InnerException;
            }

            return result;
        }

		
    }
}
