using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JDBaconWebsite.Controls
{
    public partial class ProgressUpdates: System.Web.UI.UserControl
    {
        List<Contributor> contributorList;
        List<string> updatesList;

        protected void Page_Load(object sender, EventArgs e)
        {
            contributorList = new List<Contributor>();
            updatesList = new List<string>();

            GatherContributors();

        }

        public void GatherContributors()
        {
            //contributorList.Add(new Contributor("Samuel M. Jackson", 10, "Sweet, Thanks!!!"));
            updatesList.Add("11/03/2012<br/>Website is Live!");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            foreach (Contributor c in contributorList)
            {
                ContributorsPlaceholder.Controls.Add(new Literal() { Text = c.GenerateContributorString() });
            }

            ContributorsPlaceholder.Controls.Add(new Literal() { Text = "No Contributors to report yet... We Need You!" });

            foreach (string c in updatesList)
            {
                UpdatesPlaceholder.Controls.Add(new Literal() { Text = c });
            }
        }
        // for now, this will be used to display all the contributors names.
        // If things get crazy, I may have to tie in to the kickstarter or have to train someone to come in and
        // add the new names and amounts to this list.
        public class Contributor
        {
            string name;
            double amount;
            string specialShoutOut;

            public Contributor(string name, double amount, string specialShoutOut)
            {
                this.name = name;
                this.amount = amount;
                this.specialShoutOut = specialShoutOut;
            }

            public string GenerateContributorString()
            {
                return this.name + " Pledged " + this.amount.ToString("C") + (!String.IsNullOrEmpty(this.specialShoutOut) ? "<br/>" + this.specialShoutOut : "");
            }
        }
    }
}