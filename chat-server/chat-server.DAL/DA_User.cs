﻿using chat_server.Entity;
using chat_server.Entity.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using chat_server.Service;
using System.Data;
using chat_server.Entity.interfaces;

namespace chat_server.DAL
{
	public class DA_User : SqlServerCommands, IGenericService<UserVM>, IAuth<UserVM>
	{
		public async Task<Result> Login(UserVM model)
		{
			model.ForLogin = true;
			return await Get(model);
		}
		public async Task<Result> Register(UserVM model)
		{
			return await Entry(model);
		}
		public async Task<Result> Delete(long id)
		{
			Result result = DefaultResult("Success");
			try
			{
				string query = "Delete FROM tblUser WHERE UserId = '" + id + "'";
				result.Message = (await InsertOrUpdateOrDelete(query)).IsSuccess ? "Delete Successfullly" : "User Not Found!";
				result.Data = null;
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result); 
		}

		public async Task<Result> Entry(UserVM _model)
		{
			Result result = DefaultResult("Registered Successfully!");
			
			try
			{
					UserVM ui = await isExists(_model.Email);
					if (ui != null)
					{
						result.Message = "Email Alreay Exists!";
						result.IsSuccess = false;
						return await Task.FromResult<Result>(result);
					}
					ui = await isExists(_model.FirstName, _model.LastName);
					if (ui != null)
					{
						result.Message = "Name Alreay Exists!";
						result.IsSuccess = false;
						return await Task.FromResult<Result>(result);
					}

					string query = @"INSERT INTO tblUser(UserId, email,firstname,lastName) VALUES
								(NEWID(),'"
					+ _model.Email + "','" + _model.FirstName + "', '" + _model.LastName + "')";
					result.Data = _model;
					await InsertOrUpdateOrDelete(query);
			}
			catch (Exception ex)
			{
				result.Message = "Error:" + ex.InnerException;
				result.IsSuccess = false;
				result.Data = ex.Message;
			}
			return await Task.FromResult<Result>(result);
		}
		async Task<UserVM> isExists(string email)
		{
			try
			{
				UserVM user = new UserVM();
				string query = "SELECT * FROM tblUser WITH(NOLOCK) WHERE Email = '" + email + "'";
				DataTable dataTable = await GetData(query);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					DataRow dr = dataTable.Rows[0];
					user = Converter.GetItem<UserVM>(dr);
				}
				return user;
			}
			catch(Exception ex)
			{
				return null;
			}
		}
		async Task<UserVM> isExists(string fristName, string lastName)
		{
			try
			{
				string query = "SELECT * FROM tblUser WITH(NOLOCK) WHERE FirstName = '" + fristName + "' AND LastName = '" + fristName + "'";
				DataTable dataTable = await GetData(query);
				DataRow dr = dataTable.Rows[0];
				UserVM user = Converter.GetItem<UserVM>(dr);
				return user;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		
		public async Task<Result> Get(UserVM model)
		{
			Result result = DefaultResult("Success");

			try
			{
				IEnumerable<UserVM> users;

				if (model != null)
				{
					if (model.ForLogin)
					{
						UserVM user = await isExists(model.Email);
						result.Data = user;
					}
					else
					{
						string query = "SELECT * FROM tblUser WITH(NOLOCK) WHERE USERID != '" + model.UserId + "'";
						DataTable dataTable = await GetData(query);
						users = Converter.ConvertDataTable<UserVM>(dataTable);
						result.Data = users;
					}
				}
				else
				{
					string query = "SELECT * FROM tblUser WITH(NOLOCK)";
					DataTable dataTable = await GetData(query);
					users = Converter.ConvertDataTable<UserVM>(dataTable);
					result.Data = users;
				}
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result);
		}

		public async Task<Result> Update(UserVM model)
		{
			Result result = DefaultResult("Success");
			try
			{
					
			}
			catch (Exception ex)
			{
				result.Message = "UnSuccess";
				result.IsSuccess = false;
				result.Data = ex.Message;
			}

			return await Task.FromResult<Result>(result);
		}
	}
}
