using System.Collections.ObjectModel;
using static Cobble.ViewModels.MemoPageViewModel;

namespace Cobble.Models
{
    public record DashboardMemoInfo(Dictionary<string, ObservableCollection<MemoInfo>> Memos);
    
}