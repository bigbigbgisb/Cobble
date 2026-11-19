using Cobble.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Cobble.ViewModels
{
    public partial class LuckyDrawPageViewModel(ThemeViewModel themeViewModel, IWindowService windowService) : PageViewModelBase(themeViewModel, windowService)
    {
        [ObservableProperty]
        private string _currentItem = "还没抽奖噢";
        //硬编码是为了垄断一下修改权 不用管
        private Dictionary<string, int> _prizePercentages = new()
        {
            ["免作业券 (保质期3天)"] = 2,
            ["电影鉴赏券"] = 2,
            ["午自习放歌券 (1首)"] = 4,
            ["免值日券 (1次)"] = 3,
            ["任意座位体验券 (1天)"] = 3,
            ["免除一次点名提问"] = 4,
            ["去食堂第一个"] = 8,
            ["一颗橡皮(唯一花钱的奖品)"] = 3,
            ["祝你考试考的好吧😉"] = 3,
            ["指定同学唱首歌(执行对象加一分)"] = 7,
            ["指定同学深情并茂慷慨激昂朗读一段课文(执行对象加一分)"] = 6,
            ["指定同学大声喊一句你的专属口号(执行对象加一分)"] = 6,
            ["小组的课堂表现表升级成皇室风格"] = 10,
            ["指定一名同学说出你的10个优点"] = 10,
            ["指定一名同学做5个深蹲"] = 6,
            ["与同桌互换座位体验卡 (1天)"] = 4,
            ["再来一次 (无限套娃)"] = 19
        };
        [RelayCommand]
        private async Task Draw()
        {
            //随机跳动
            int time = 0;
            while (time < 3000)
            {
                int index = Random.Shared.Next(_prizePercentages.Count);
                CurrentItem = _prizePercentages.ElementAt(index).Key;
                await Task.Delay(120);
                time += 120;
            }
            //正式抽取
            int randomNumber = Random.Shared.Next(100);
            //耍起 (如果概率和不为100%，返回此对象默认值)
            KeyValuePair<string, int> pairResult = new("软件内嵌概率哈希表存储的值键对中值总和不为 0x64 %，请联系软件开发者解决", 0);
            //缩小区间落点
            foreach (var pair in _prizePercentages)
            {
                if (randomNumber < pair.Value)
                {
                    pairResult = pair;
                    break;
                }
                else randomNumber -= pair.Value;
            }
            CurrentItem = pairResult.Key;
        }
    }
}
