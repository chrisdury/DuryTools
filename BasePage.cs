using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;


namespace DuryTools.UI
{
	public class BasePage : System.Web.UI.Page
	{
		private string mainTitle;
		public string MainTitle 
		{
			get { return mainTitle; }
			set { mainTitle = value; }
		}


		private string pageTitle;
		public string PageTitle 
		{
			get { return pageTitle; }
			set { pageTitle = value; }
		}


		public string MetaKeywords = String.Empty;
		public string MetaDescription = String.Empty;

		public DateTime pt;
		public bool showMenu = true;
		private string _headerFile = "modules/header.ascx";
		private string _footerFile = "modules/footer.ascx";

		public string HeaderFile
		{
			get
			{
				return _headerFile;
			}
			set
			{
				_headerFile = value;
			}
		}
		public string FooterFile
		{
			get
			{
				return _footerFile;
			}
			set
			{
				_footerFile = value;
			}
		}

    
		protected override void OnInit(System.EventArgs e) 
		{
			this.mainTitle = System.Configuration.ConfigurationSettings.AppSettings["mainTitle"];
			this.Controls.AddAt(0, LoadControl(_headerFile));
			base.OnInit(e);
			this.Controls.Add(LoadControl(_footerFile));

		}

	}

	public class BaseControl : System.Web.UI.UserControl
	{
		public new BasePage Page 
		{
			get { return (BasePage)base.Page; }
		}
	}



}
