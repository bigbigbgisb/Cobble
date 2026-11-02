using Cobble.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cobble.Messages
{
    //在新增科目时发送
    public record UpdateDashboardMemoInfoMessage(DashboardMemoInfo DashboardMemoInfo);
}
