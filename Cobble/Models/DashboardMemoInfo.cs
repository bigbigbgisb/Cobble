using System.Collections.ObjectModel;
namespace Cobble.Models
{
    public record DashboardMemoInfo(Dictionary<string, ObservableCollection<MemoInfo>> Memos);
    
}