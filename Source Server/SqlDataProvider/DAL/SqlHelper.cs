// Decompiled with JetBrains decompiler
// Type: DAL.SqlHelper
// Assembly: SqlDataProvider, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E6C792E1-372D-46D0-B366-36ACC93C90BB
// Assembly location: C:\Users\Pham Van Hungg\Desktop\Decompiler\Road\SqlDataProvider.dll

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace DAL
{
  public sealed class SqlHelper
  {
    private SqlHelper()
    {
    }

    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
      foreach (SqlParameter commandParameter in commandParameters)
      {
        if (commandParameter.Direction == ParameterDirection.InputOutput && commandParameter.Value == null)
          commandParameter.Value = (object) DBNull.Value;
        command.Parameters.Add(commandParameter);
      }
    }

    public static void AssignParameterValues(
      SqlParameter[] commandParameters,
      params object[] parameterValues)
    {
      if (commandParameters == null || parameterValues == null)
        return;
      if (commandParameters.Length != parameterValues.Length)
        throw new ArgumentException("Parameter count does not match Parameter Value count.");
      int index = 0;
      for (int length = commandParameters.Length; index < length; ++index)
      {
        if (parameterValues[index] != null && (commandParameters[index].Direction == ParameterDirection.Input || commandParameters[index].Direction == ParameterDirection.InputOutput))
          commandParameters[index].Value = parameterValues[index];
      }
    }

    public static void AssignParameterValues(
      SqlParameter[] commandParameters,
      Hashtable parameterValues)
    {
      if (commandParameters == null || parameterValues == null)
        return;
      if (commandParameters.Length != parameterValues.Count)
        throw new ArgumentException("Parameter count does not match Parameter Value count.");
      int index = 0;
      for (int length = commandParameters.Length; index < length; ++index)
      {
        if (parameterValues[(object) commandParameters[index].ParameterName] != null && (commandParameters[index].Direction == ParameterDirection.Input || commandParameters[index].Direction == ParameterDirection.InputOutput))
          commandParameters[index].Value = parameterValues[(object) commandParameters[index].ParameterName];
      }
    }

    private static void PrepareCommand(
      SqlCommand command,
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      SqlParameter[] commandParameters)
    {
      if (connection.State != ConnectionState.Open)
        connection.Open();
      command.Connection = connection;
      command.CommandText = commandText;
      if (transaction != null)
        command.Transaction = transaction;
      command.CommandType = commandType;
      if (commandParameters == null)
        return;
      SqlHelper.AttachParameters(command, commandParameters);
    }

    public static int ExecuteNonQuery(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static int ExecuteNonQuery(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, commandParameters);
      }
    }

    public static int ExecuteNonQuery(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQuery(
      string connectionString,
      string spName,
      Hashtable parameterValues)
    {
      if (parameterValues == null || parameterValues.Count <= 0)
        return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQuery(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static int ExecuteNonQuery(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, commandParameters);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      return num;
    }

    public static int ExecuteNonQuery(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static int ExecuteNonQuery(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
      int num = command.ExecuteNonQuery();
      command.Parameters.Clear();
      return num;
    }

    public static int ExecuteNonQuery(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDataset(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static DataSet ExecuteDataset(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlHelper.ExecuteDataset(connection, commandType, commandText, commandParameters);
      }
    }

    public static DataSet ExecuteDataset(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDataset(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDataset(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static DataSet ExecuteDataset(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand sqlCommand = new SqlCommand();
      SqlHelper.PrepareCommand(sqlCommand, connection, (SqlTransaction) null, commandType, commandText, commandParameters);
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
      DataSet dataSet1 = new DataSet();
      DataSet dataSet2 = dataSet1;
      sqlDataAdapter.Fill(dataSet2);
      sqlCommand.Parameters.Clear();
      return dataSet1;
    }

    public static DataSet ExecuteDataset(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static DataSet ExecuteDataset(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteDataset(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static DataSet ExecuteDataset(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand sqlCommand = new SqlCommand();
      SqlHelper.PrepareCommand(sqlCommand, transaction.Connection, transaction, commandType, commandText, commandParameters);
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
      DataSet dataSet1 = new DataSet();
      DataSet dataSet2 = dataSet1;
      sqlDataAdapter.Fill(dataSet2);
      sqlCommand.Parameters.Clear();
      return dataSet1;
    }

    public static DataSet ExecuteDataset(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    private static SqlDataReader ExecuteReader(
      SqlConnection connection,
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      SqlParameter[] commandParameters,
      SqlHelper.SqlConnectionOwnership connectionOwnership)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters);
      SqlDataReader sqlDataReader = connectionOwnership != SqlHelper.SqlConnectionOwnership.External ? command.ExecuteReader(CommandBehavior.CloseConnection) : command.ExecuteReader();
      command.Parameters.Clear();
      return sqlDataReader;
    }

    public static SqlDataReader ExecuteReader(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReader(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlConnection connection = new SqlConnection(connectionString);
      connection.Open();
      try
      {
        return SqlHelper.ExecuteReader(connection, (SqlTransaction) null, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.Internal);
      }
      catch
      {
        connection.Close();
        throw;
      }
    }

    public static SqlDataReader ExecuteReader(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReader(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      return SqlHelper.ExecuteReader(connection, (SqlTransaction) null, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External);
    }

    public static SqlDataReader ExecuteReader(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static SqlDataReader ExecuteReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteReader(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static SqlDataReader ExecuteReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      return SqlHelper.ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External);
    }

    public static SqlDataReader ExecuteReader(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(
      string connectionString,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        return SqlHelper.ExecuteScalar(connection, commandType, commandText, commandParameters);
      }
    }

    public static object ExecuteScalar(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalar(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, commandParameters);
      object obj = command.ExecuteScalar();
      command.Parameters.Clear();
      return obj;
    }

    public static object ExecuteScalar(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static object ExecuteScalar(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteScalar(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static object ExecuteScalar(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
      object obj = command.ExecuteScalar();
      command.Parameters.Clear();
      return obj;
    }

    public static object ExecuteScalar(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static XmlReader ExecuteXmlReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[]) null);
    }

    public static XmlReader ExecuteXmlReader(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, commandParameters);
      XmlReader xmlReader = command.ExecuteXmlReader();
      command.Parameters.Clear();
      return xmlReader;
    }

    public static XmlReader ExecuteXmlReader(
      SqlConnection connection,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static XmlReader ExecuteXmlReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText)
    {
      return SqlHelper.ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[]) null);
    }

    public static XmlReader ExecuteXmlReader(
      SqlTransaction transaction,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters);
      XmlReader xmlReader = command.ExecuteXmlReader();
      command.Parameters.Clear();
      return xmlReader;
    }

    public static XmlReader ExecuteXmlReader(
      SqlTransaction transaction,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues == null || parameterValues.Length == 0)
        return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
      SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
      SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
      return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, spParameterSet);
    }

    public static void BeginExecuteNonQuery(
      SqlConnection connection,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      SqlCommand command = new SqlCommand();
      SqlHelper.PrepareCommand(command, connection, (SqlTransaction) null, commandType, commandText, commandParameters);
      command.BeginExecuteNonQuery();
      command.Parameters.Clear();
    }

    public static void BeginExecuteNonQuery(
      string connectionString,
      CommandType commandType,
      string commandText,
      params SqlParameter[] commandParameters)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        SqlHelper.ExecuteNonQuery(connection, commandType, commandText, commandParameters);
      }
    }

    public static void BeginExecuteNonQuery(
      string connectionString,
      string spName,
      params object[] parameterValues)
    {
      if (parameterValues != null && (uint) parameterValues.Length > 0U)
      {
        SqlParameter[] spParameterSet = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
        SqlHelper.AssignParameterValues(spParameterSet, parameterValues);
        SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, spParameterSet);
      }
      else
        SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
    }

    private enum SqlConnectionOwnership
    {
      Internal,
      External,
    }
  }
}
