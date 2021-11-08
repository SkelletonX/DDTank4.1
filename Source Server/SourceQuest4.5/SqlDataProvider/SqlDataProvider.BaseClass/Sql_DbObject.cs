using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SqlDataProvider.BaseClass
{
	public sealed class Sql_DbObject : IDisposable
	{
		private SqlConnection _SqlConnection;

		private SqlCommand _SqlCommand;

		private SqlDataAdapter _SqlDataAdapter;

		public Sql_DbObject()
		{
			_SqlConnection = new SqlConnection();
		}

		public Sql_DbObject(string Path_Source, string Conn_DB)
		{
			switch (Path_Source)
			{
			case "WebConfig":
				_SqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[Conn_DB].ConnectionString);
				break;
			case "File":
				_SqlConnection = new SqlConnection(Conn_DB);
				break;
			case "AppConfig":
			{
				string str = ConfigurationSettings.AppSettings[Conn_DB];
				_SqlConnection = new SqlConnection(str);
				break;
			}
			default:
				_SqlConnection = new SqlConnection(Conn_DB);
				break;
			}
		}

		private static bool OpenConnection(SqlConnection _SqlConnection)
		{
			bool result = false;
			try
			{
				if (_SqlConnection.State != ConnectionState.Open)
				{
					_SqlConnection.Open();
					return true;
				}
				return true;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("\u00b4ò¿ªÊý¾Ý¿âÁ¬½Ó\u00b4íÎó:" + ex.Message.Trim());
				return false;
			}
		}

		public bool Exesqlcomm(string Sqlcomm)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.CommandType = CommandType.Text;
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandText = Sqlcomm;
				_SqlCommand.ExecuteNonQuery();
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐsqlÓï¾ä: " + Sqlcomm + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
			return true;
		}

		public int GetRecordCount(string Sqlcomm)
		{
			int retval = 0;
			if (!OpenConnection(_SqlConnection))
			{
				return 0;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.Text;
				_SqlCommand.CommandText = Sqlcomm;
				if (_SqlCommand.ExecuteScalar() != null)
				{
					retval = (int)_SqlCommand.ExecuteScalar();
					return retval;
				}
				retval = 0;
				return retval;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐsqlÓï¾ä: " + Sqlcomm + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return retval;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public DataTable GetDataTableBySqlcomm(string TableName, string Sqlcomm)
		{
			DataTable ResultTable = new DataTable(TableName);
			if (!OpenConnection(_SqlConnection))
			{
				return ResultTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.Text;
				_SqlCommand.CommandText = Sqlcomm;
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				_SqlDataAdapter.Fill(ResultTable);
				return ResultTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐsqlÓï¾ä: " + Sqlcomm + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return ResultTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public DataSet GetDataSetBySqlcomm(string TableName, string Sqlcomm)
		{
			DataSet ResultTable = new DataSet();
			if (!OpenConnection(_SqlConnection))
			{
				return ResultTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.Text;
				_SqlCommand.CommandText = Sqlcomm;
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				_SqlDataAdapter.Fill(ResultTable);
				return ResultTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐSqlÓï¾ä£º" + Sqlcomm + "\u00b4íÎóÐÅÏ¢Îª£º" + ex.Message.Trim());
				return ResultTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public bool FillSqlDataReader(ref SqlDataReader Sdr, string SqlComm)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.Text;
				_SqlCommand.CommandText = SqlComm;
				Sdr = _SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
				return true;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐSqlÓï¾ä£º" + SqlComm + "\u00b4íÎóÐÅÏ¢Îª£º" + ex.Message.Trim());
			}
			finally
			{
				Dispose(disposing: true);
			}
			return false;
		}

		public DataTable GetDataTableBySqlcomm(string TableName, string Sqlcomm, int StartRecordNo, int PageSize)
		{
			DataTable RetTable = new DataTable(TableName);
			if (!OpenConnection(_SqlConnection))
			{
				RetTable.Dispose();
				Dispose(disposing: true);
				return RetTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.Text;
				_SqlCommand.CommandText = Sqlcomm;
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				DataSet ds = new DataSet();
				ds.Tables.Add(RetTable);
				_SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
				return RetTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐsqlÓï¾ä: " + Sqlcomm + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return RetTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public bool RunProcedure(string ProcedureName, SqlParameter[] SqlParameters)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlCommand.ExecuteNonQuery();
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
			return true;
		}

		public bool RunProcedure(string ProcedureName)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				_SqlCommand.ExecuteNonQuery();
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
			return true;
		}

		public bool GetReader(ref SqlDataReader ResultDataReader, string ProcedureName)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				ResultDataReader = _SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			return true;
		}

		public bool GetReader(ref SqlDataReader ResultDataReader, string ProcedureName, SqlParameter[] SqlParameters)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				ResultDataReader = _SqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			return true;
		}

		public DataSet GetDataSet(string ProcedureName, SqlParameter[] SqlParameters)
		{
			DataSet FullDataSet = new DataSet();
			if (!OpenConnection(_SqlConnection))
			{
				FullDataSet.Dispose();
				return FullDataSet;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				_SqlDataAdapter.Fill(FullDataSet);
				return FullDataSet;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì£º" + ProcedureName + "\u00b4íÐÅÐÅÏ¢Îª£º" + ex.Message.Trim());
				return FullDataSet;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public bool GetDataSet(ref DataSet ResultDataSet, ref int row_total, string TableName, string ProcedureName, int StartRecordNo, int PageSize, SqlParameter[] SqlParameters)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				row_total = 0;
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				DataSet ds = new DataSet();
				row_total = _SqlDataAdapter.Fill(ds);
				_SqlDataAdapter.Fill(ResultDataSet, StartRecordNo, PageSize, TableName);
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì£º" + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª£º" + ex.Message.Trim());
				return false;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
			return true;
		}

		public DataSet GetDateSet(string DatesetName, string ProcedureName, SqlParameter[] SqlParameters)
		{
			DataSet FullDataSet = new DataSet(DatesetName);
			if (!OpenConnection(_SqlConnection))
			{
				FullDataSet.Dispose();
				return FullDataSet;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				_SqlDataAdapter.Fill(FullDataSet);
				return FullDataSet;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì£º" + ProcedureName + "\u00b4íÐÅÐÅÏ¢Îª£º" + ex.Message.Trim());
				return FullDataSet;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public DataTable GetDataTable(string TableName, string ProcedureName, SqlParameter[] SqlParameters)
		{
			return GetDataTable(TableName, ProcedureName, SqlParameters, -1);
		}

		public DataTable GetDataTable(string TableName, string ProcedureName, SqlParameter[] SqlParameters, int commandTimeout)
		{
			DataTable FullTable = new DataTable(TableName);
			if (!OpenConnection(_SqlConnection))
			{
				FullTable.Dispose();
				Dispose(disposing: true);
				return FullTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				if (commandTimeout >= 0)
				{
					_SqlCommand.CommandTimeout = commandTimeout;
				}
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				_SqlDataAdapter.Fill(FullTable);
				return FullTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return FullTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public DataTable GetDataTable(string TableName, string ProcedureName)
		{
			DataTable FullTable = new DataTable(TableName);
			if (!OpenConnection(_SqlConnection))
			{
				FullTable.Dispose();
				Dispose(disposing: true);
				return FullTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				_SqlDataAdapter.Fill(FullTable);
				return FullTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return FullTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public DataTable GetDataTable(string TableName, string ProcedureName, int StartRecordNo, int PageSize)
		{
			DataTable RetTable = new DataTable(TableName);
			if (!OpenConnection(_SqlConnection))
			{
				RetTable.Dispose();
				Dispose(disposing: true);
				return RetTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				DataSet ds = new DataSet();
				ds.Tables.Add(RetTable);
				_SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
				return RetTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return RetTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public DataTable GetDataTable(string TableName, string ProcedureName, SqlParameter[] SqlParameters, int StartRecordNo, int PageSize)
		{
			DataTable RetTable = new DataTable(TableName);
			if (!OpenConnection(_SqlConnection))
			{
				RetTable.Dispose();
				Dispose(disposing: true);
				return RetTable;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				DataSet ds = new DataSet();
				ds.Tables.Add(RetTable);
				_SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
				return RetTable;
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return RetTable;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
		}

		public bool GetDataTable(ref DataTable ResultTable, string TableName, string ProcedureName, int StartRecordNo, int PageSize)
		{
			ResultTable = null;
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				DataSet ds = new DataSet();
				ds.Tables.Add(ResultTable);
				_SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
				ResultTable = ds.Tables[TableName];
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
			return true;
		}

		public bool GetDataTable(ref DataTable ResultTable, string TableName, string ProcedureName, int StartRecordNo, int PageSize, SqlParameter[] SqlParameters)
		{
			if (!OpenConnection(_SqlConnection))
			{
				return false;
			}
			try
			{
				_SqlCommand = new SqlCommand();
				_SqlCommand.Connection = _SqlConnection;
				_SqlCommand.CommandType = CommandType.StoredProcedure;
				_SqlCommand.CommandText = ProcedureName;
				foreach (SqlParameter parameter in SqlParameters)
				{
					_SqlCommand.Parameters.Add(parameter);
				}
				_SqlDataAdapter = new SqlDataAdapter();
				_SqlDataAdapter.SelectCommand = _SqlCommand;
				DataSet ds = new DataSet();
				ds.Tables.Add(ResultTable);
				_SqlDataAdapter.Fill(ds, StartRecordNo, PageSize, TableName);
				ResultTable = ds.Tables[TableName];
			}
			catch (SqlException ex)
			{
				ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				return false;
			}
			finally
			{
				_SqlConnection.Close();
				Dispose(disposing: true);
			}
			return true;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(true);
		}

		private void Dispose(bool disposing)
		{
			if (!disposing || _SqlDataAdapter == null)
			{
				return;
			}
			if (_SqlDataAdapter.SelectCommand != null)
			{
				if (_SqlCommand.Connection != null)
				{
					_SqlDataAdapter.SelectCommand.Connection.Dispose();
				}
				_SqlDataAdapter.SelectCommand.Dispose();
			}
			_SqlDataAdapter.Dispose();
			_SqlDataAdapter = null;
		}

		public void BeginRunProcedure(string ProcedureName, SqlParameter[] SqlParameters)
		{
			if (OpenConnection(_SqlConnection))
			{
				try
				{
					_SqlCommand = new SqlCommand();
					_SqlCommand.Connection = _SqlConnection;
					_SqlCommand.CommandType = CommandType.StoredProcedure;
					_SqlCommand.CommandText = ProcedureName;
					foreach (SqlParameter parameter in SqlParameters)
					{
						_SqlCommand.Parameters.Add(parameter);
					}
					_SqlCommand.BeginExecuteNonQuery();
				}
				catch (SqlException ex)
				{
					ApplicationLog.WriteError("Ö\u00b4ÐÐ\u00b4æ\u00b4¢¹ý³Ì: " + ProcedureName + "\u00b4íÎóÐÅÏ¢Îª: " + ex.Message.Trim());
				}
				finally
				{
					_SqlConnection.Close();
					Dispose(disposing: true);
				}
			}
		}
	}
}
