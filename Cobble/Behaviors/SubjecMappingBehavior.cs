using MaterialDesignThemes.Wpf.Converters;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Cobble.Behaviors
{
    public class SubjectMappingBehavior : Behavior<TextBox>
    {
        private bool _isUpdating = false;
        private static readonly Dictionary<int, string> _subjectMapping = new()
        {
            { 1, "语文" },
            { 2, "数学" },
            { 3, "英语" },
            { 4, "物理" },
            { 5, "化学" },
            { 6, "生物" },
            { 7, "道法" },
            { 8, "历史" },
            { 9, "地理" },
            { 10, "体育" },
            { 11, "音乐" },
            { 12, "美术" },
            { 13, "信息技术" },
            { 14, "人工智能基础" },
            { 15, "班会" },
            { 16, "自习" },
            { 17, "课后服务" },
            { 18, "生态ODC课程" },
            { 19, "校本" }
        };
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += OnTextChanged;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= OnTextChanged;
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
            string text = AssociatedObject.Text;
            if (!text.Contains(';') && !text.Contains('；')) return;
            bool isChanged = false;
            string newText = Regex.Replace(text, @"(\d+)[;；]",match => 
            {
                string numString = match.Groups[1].Value;
                if (int.TryParse(numString,out var key) && _subjectMapping.TryGetValue(key,out string? lessonName))
                {
                    isChanged = true;
                    return lessonName;
                }
                return match.Value;
            });
            
            AssociatedObject.Text = newText;
            if (isChanged) AssociatedObject.CaretIndex = AssociatedObject.Text.Length;
            
        }
    }
}
