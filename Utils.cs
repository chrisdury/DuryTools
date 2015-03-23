using System;
using System.Collections;
using System.Collections.Specialized;

namespace DuryTools.Utils
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public sealed class QueryStringUtils
	{
		private QueryStringUtils() {}
		/// <summary>
		/// parses string delimited by "&{itemName}={itemValue}" into a CustomQueryString
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static CustomQueryString QueryStringBuilder(string input)
		{
			CustomQueryString qs = new CustomQueryString();
			if (input.IndexOf("?") > -1)
			{
				input = input.Substring(input.LastIndexOf("?")+1);
				string[] items = input.Split('&');
				if (items.Length > 0)
				{
					for (int i=0; i<items.Length; i++)
					{
						string[] r = items[i].Split('=');
						qs.Add(r[0],r[1]);
					}
				}
			}
			return qs;
		}

	}


	/// <summary>
	/// A Custom System.Collections.Specialized.NameValueCollection with a handy ToString() that outputs
	/// a proper URL encoded string
	/// </summary>
	public class CustomQueryString : NameValueCollection
	{
		public CustomQueryString() {}

		public CustomQueryString(string input)
		{
			if (input.IndexOf("?") > -1)
			{
				input = input.Substring(input.LastIndexOf("?")+1);
				string[] items = input.Split('&');
				if (items.Length > 0)
				{
					for (int i=0; i<items.Length; i++)
					{
						string[] r = items[i].Split('=');
						this.Add(r[0],r[1]);
					}
				}
			}
		}

		public override string ToString()
		{
			string s = String.Empty;
			for(int i=0; i<this.Keys.Count; i++)
			{
				s += this.Keys[i] + "=" + this[i];
				s += "&";
			}
			s = s.Substring(0,s.Length-1);
			return s;
		}

	}



	/// <summary>
	/// Sorts objects in array.
	/// </summary>
	public class GenericSort : IComparer
	{
		String sortMethodName;
		bool isAscending;
		/// <summary>
		/// initializes the sorting
		/// </summary>
		/// <param name="sortBy">thing to sort by</param>
		/// <param name="isAscending">set to true to sort ASC and false to sort DESC</param>
		public GenericSort(String sortBy, bool isAscending) 
		{
			this.sortMethodName = sortBy;
			this.isAscending = isAscending;
		}
		public int Compare(object x, object y)
		{
			IComparable ic1 = (IComparable)x.GetType().GetProperty(sortMethodName).GetValue(x,null);
			IComparable ic2 = (IComparable)y.GetType().GetProperty(sortMethodName).GetValue(y,null);//.Invoke(y,null);
			if(isAscending)
				return ic1.CompareTo(ic2);
			else
				return ic2.CompareTo(ic1);
		}

	}


	/*
		public class cleanString
		{
			private cleanString() {}
			public static cleanString(string input)
			{




			}

		}
	*/

	public class MenuItem : System.Collections.CollectionBase
	{
		public MenuItem()
		{
		}

		public void Add(string url, string title, string imageName)
		{
			List.Add(new string[] { url, title, imageName});
		}

		public string[] this[int index]
		{
			get
			{
				return (string[])List[index];
			}
			set
			{
				List[index] = value;
			}
		}
	}
}
