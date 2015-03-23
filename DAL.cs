using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;


namespace DuryTools.Data
{
	public class DAL
	{
		private string connectionString;
		public string ConnectionString
		{
			get
			{
				return this.connectionString;
			}
			set
			{
				this.connectionString = value;
			}
		}


		public DAL()
		{
			getConnectionString();
		}

		private void getConnectionString()
		{
			this.connectionString = System.Configuration.ConfigurationSettings.AppSettings["connectionString"];
			if (this.connectionString == null)
			{
				throw new ErrorHandler("Connection String retrieval from web.config problem. Need 'connectionString' key entry",new System.Exception());                
			}
		}

		#region MS Sql Helper Wrapper 
		/// <summary>
		/// Execute SQL Stored Procedure
		/// </summary>
		/// <param name="spName">Stored Procedure Name</param>
		/// <param name="p">parameters</param>
		/// <returns>DataRow</returns>
		public DataRow execDataRow(string spName,params object[] p)
		{
			SqlConnection con = new SqlConnection(this.connectionString);
			DataSet ds = new DataSet();
			try
			{
				ds = SqlHelper.ExecuteDataset(con,spName,p);
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: spName='" + spName + "'",e);
			}
			finally
			{
				con.Close();
			}

			DataRow dr = ds.Tables[0].Rows[0];
			return dr;
		}
		/// <summary>
		/// Execute SQL Stored Procedure
		/// </summary>
		/// <param name="spName">Stored Procedure Name</param>
		/// <param name="p">parameters</param>
		/// <returns>DataRow</returns>
		public DataRow execDataRow(string sql)
		{
			SqlConnection con = new SqlConnection(this.connectionString);
			DataSet ds = new DataSet();
			try
			{
				ds = SqlHelper.ExecuteDataset(con,System.Data.CommandType.Text,sql);
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: sql='" + sql + "'",e);
			}
			finally
			{
				con.Close();
			}
			if (ds.Tables[0].Rows.Count > 0)
				return ds.Tables[0].Rows[0];
			else
				return null;			
		}
 
		/// <summary>
		/// Executes Stored Procedure with supplied parameters
		/// </summary>
		/// <param name="spName">Stored Procedure to execute</param>
		/// <param name="p">parameters</param>
		/// <returns>DataSet</returns>
		public DataSet execDataSet(string spName,params object[] p)
		{
			SqlConnection con = new SqlConnection(this.connectionString);
			DataSet ds = new DataSet();
			try
			{
				ds = SqlHelper.ExecuteDataset(con,spName,p);
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: spName='" + spName + "'",e);
			}
			finally
			{
				con.Close();
			}
			return ds;
		}

		/// <summary>
		/// Executes Stored Procedure with supplied parameters
		/// </summary>
		/// <param name="spName">Stored Procedure to execute</param>
		/// <param name="p">parameters</param>
		/// <returns>DataSet</returns>
		public DataSet execDataSet(string sqlText)
		{
			SqlConnection con = new SqlConnection(this.connectionString);
			DataSet ds = new DataSet();
			try
			{
				ds = SqlHelper.ExecuteDataset(this.connectionString,System.Data.CommandType.Text,sqlText);
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: sqlText='" + sqlText + "'",e);
			}
			finally
			{
				con.Close();
			}
			return ds;
		}


		public bool execNonQuery(string spName, params object[] p)
		{
			SqlConnection con = new SqlConnection(this.connectionString);
			try
			{
				SqlHelper.ExecuteNonQuery(con,spName,p);
			}
			catch (Exception e)
			{
				throw new ErrorHandler("Error: spName='" + spName + "'",e);
			}
			finally
			{
				con.Close();
			}
			return true; // for now.. return false when db fails
		}

		public bool execNonQuery(string sql)
		{
			SqlConnection con = new SqlConnection(this.connectionString);
			bool t = false;
			try
			{
				SqlHelper.ExecuteNonQuery(con,System.Data.CommandType.Text,sql);
				t = true;
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: " + sql,e);
			}
			finally
			{
				con.Close();
			}
			return t;
		}


		public object execScalar(string spName, params object[] p)
		{
			object i = 0;
			SqlConnection con = new SqlConnection(this.connectionString);
			try
			{
				i = SqlHelper.ExecuteScalar(con,spName,p);
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: spName='" + spName + "'",e);
			}
			finally
			{
				con.Close();
			}
			return i;
		}

		public object execScalar(string selectText)
		{
			object i = 0;
			SqlConnection con = new SqlConnection(this.connectionString);
			try
			{
				i = SqlHelper.ExecuteScalar(con,System.Data.CommandType.Text,selectText);
			}
			catch(Exception e)
			{
				throw new ErrorHandler("Error: selectText='" + selectText + "'",e);
			}
			finally
			{
				con.Close();
			}
			return i;
		}

		#endregion
	}
}
