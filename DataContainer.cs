using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace DuryTools.Data
{
	/// <summary>
	/// DataContainer is a helper class for datasets and datarows.  Common functionality is provided through several methods.
	/// </summary>
	public class DataContainer
	{
		private string tableName;
		private SqlConnection sqlConn;
		private string connectionString;
		public DataSet ds;

		/// <summary>
		/// Create a new DataContainer which provides easy to use data access methods that relate to a single table or view
		/// </summary>
		/// <param name="tableName">Name of Database table to be operated on</param>
		public DataContainer(string TableName)
		{
			tableName = TableName;
			getConnectionString();
		}

		private void getConnectionString()
		{
			this.connectionString = System.Configuration.ConfigurationSettings.AppSettings["connectionString"];
			if (this.connectionString == null || connectionString.Length < 1)
			{
				throw new ErrorHandler("Connection String retrieval from web.config problem. Need 'connectionString' key entry");                
			}
		}


		private void openConn() 
		{
			if (sqlConn == null) sqlConn = new SqlConnection(connectionString);
			if (sqlConn.State != ConnectionState.Open)
				sqlConn.Open();
			else
				throw new ErrorHandler("can't open sql connection.  connectionstring=" + connectionString);
		}
		
		private void closeConn()
		{
			if (sqlConn != null)
				sqlConn.Close();
			else
				throw new ErrorHandler("Can't close sql connection");
		}



		private DataSet getDataSet(string strSql) 
		{
			if (sqlConn == null) openConn();

			SqlCommand sqlComm = new SqlCommand(strSql, sqlConn);
			SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
			sqlAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
			try 
			{
				ds = new DataSet();
				sqlAdapter.Fill(ds, tableName);
				DataColumn[] myKey = new DataColumn[1];
				myKey[0] = ds.Tables[0].Columns[0];
				ds.Tables[0].PrimaryKey = myKey;
			}
			catch (Exception e) 
			{ 
				throw new ErrorHandler("getDataSet()... sql="+strSql ,e); 
			}
			finally 
			{
				closeConn();
			}
			return ds;
		
		}

		private void putDataSet()
		{
			if (sqlConn == null) openConn();			
			SqlCommand sqlComm = new SqlCommand("select * from " + tableName, sqlConn);
			SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
			SqlCommandBuilder sqlCommBuilder = new SqlCommandBuilder(sqlAdapter);

			try 
			{
				sqlAdapter.Update(ds, tableName);
			}
			catch (Exception e)
			{
				throw new ErrorHandler("datasql.putDataSet()...conn="+sqlConn.State,e);
			}
			
			finally 
			{
				closeConn();
			}
			
		}


		/// <summary>
		/// Fills objects own DataSet with every row from the database table
		/// </summary>
		public void GetAll()
		{
			getDataSet("select * from " + tableName);
		}

		/// <summary>
		/// Gets the Top 1 row from the database.  Used mostly to fill DataSet schema
		/// </summary>
		public void GetTop1()
		{
			getDataSet("select top 1 * from " + tableName);
		}

		/// <summary>
		/// Fills objects own DataSet with rows returned from custom SQL query
		/// </summary>
		/// <param name="sql">Custom query to the database</param>
		public void GetByCustomSql(string sql)
		{
			getDataSet(sql);
		}

		/// <summary>
		/// Retrieves a single row from the database with a custom query.
		/// </summary>
		/// <param name="strSql">custom query</param>
		/// <returns>a datarow</returns>
		public DataRow GetRowByCustomSql(string strSql)
		{
			this.getDataSet(strSql);
			if (ds.Tables[0].Rows.Count > 0)
				return ds.Tables[0].Rows[0];
			else
				return null;
		}

		/// <summary>
		/// Retrieves a single row from the database based on a key
		/// </summary>
		/// <param name="keyColumnName">Name of the column to look at</param>
		/// <param name="key">value of key</param>
		/// <returns>datarow</returns>
		public DataRow GetRowByKey(string keyColumnName,object key)
		{
			return GetRowByCustomSql("SELECT * FROM " + this.tableName + " WHERE " + keyColumnName + " = " + key);
		}

		/// <summary>
		/// Adds a new row to the objects datarow
		/// </summary>
		/// <param name="dr"></param>
		public void AddNewRow(DataRow dr)
		{
			checkDataSet();
			//ds.Tables[0].ImportRow(dr);
			ds.Tables[0].Rows.Add(dr.ItemArray);
		}

		/// <summary>
		/// Creates a new row according to the objects dataset schema
		/// </summary>
		/// <returns></returns>
		public DataRow GetNewRow()
		{
			checkDataSet();
			DataRow mydr = ds.Tables[0].NewRow();
			return mydr;
		}

		/// <summary>
		/// Updates the supplied row against the objects dataset.  If no matching row is found, then the row is added.  Also updates database.
		/// </summary>
		/// <param name="dr">a datarow to update</param>
		public void UpdateRow(DataRow dr)
		{
			//checkDataSet();
			ds.Tables[0].BeginLoadData();
			ds.Tables[0].LoadDataRow(dr.ItemArray,false);
			ds.Tables[0].EndLoadData();
			Update();
		}
		
		/// <summary>
		/// Removes a datarow from the dataset where the supplied key matches the rows primary key
		/// </summary>
		/// <param name="myid">primary key value</param>
		public void DeleteRow(object key)
		{
			checkDataSet();
			DataRow dr = ds.Tables[0].Rows.Find(key);
			dr.Delete();
		}

		private void checkDataSet()
		{
			if (ds == null || ds.Tables[0].Rows.Count <= 1)	{ GetTop1(); }
		}

		/// <summary>
		/// Updates dataset back to database
		/// </summary>
		public void Update() 
		{	
			putDataSet();
			//GetAll();
		}

	}
}
