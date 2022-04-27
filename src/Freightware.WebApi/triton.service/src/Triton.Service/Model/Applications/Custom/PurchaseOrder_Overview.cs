using System.Collections.Generic;
using Triton.Service.Model.Applications.StoredProcs;

namespace Triton.Service.Model.Applications.Custom
{
    public class PurchaseOrder_Overview
    {
        public List<proc_AgentIssues_Tab> AgentIssueTabs { get; set; }
        public List<RoadFreightAgentModel> RoadFreightAgent { get; set; }
        public string SelectedDate { get; set; }
        public int ActiveTab { get; set; }
    }
}
