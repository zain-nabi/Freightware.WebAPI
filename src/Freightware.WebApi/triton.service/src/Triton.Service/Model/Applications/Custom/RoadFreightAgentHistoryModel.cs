using System.Collections.Generic;
using Triton.Model.TritonSecurity.Tables;
using Triton.Service.Model.Applications.Tables;

namespace Triton.Service.Model.Applications.Custom
{
    public class RoadFreightAgentHistoryModel
    {
        public List<RoadFreightAgentHistory> RoadFreightAgentHistory { get; set; }
        public Users Users { get; set; }
        public int RoadFreightAgentID { get; set; }
    }
}
