using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMusicSearch.MusicCommon;
using CMusicSearch.MusicRunner;

namespace CMusicSearch.MusicWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            using (MSLRCRunner Finder = new MSLRCRunner())
            {
                Finder.Initialize();
                SearchMusicInfo info = new SearchMusicInfo() { MusicName = EncodeConverter.UrlEncode(txtMusicName.Text.Trim()) };
                var list = Finder.SearchM(info);
                grdMain.DataSource = list;
                grdMain.DataBind();
            }
        }
    }
}
