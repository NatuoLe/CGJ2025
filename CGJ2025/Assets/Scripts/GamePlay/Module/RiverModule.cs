using System;
using System.Collections.Generic;

public class RiverModule
{
    public List<char> UpStr = new List<char>();
    public List<char> DownStr = new List<char>();

    // 检查并执行合并
    public void CheckAndMerge(List<char> list, int index)
    {
        // 确保索引有效
        if (index < 0 || index >= list.Count - 1) return;

        char current = list[index];
        char next = list[index + 1];

        // 尝试合并当前元素和下一个元素
        char? merged = TryMerge(current, next);

        if (merged != null)
        {
            // 移除原来的两个元素
            list.RemoveAt(index);
            list.RemoveAt(index);

            // 插入合并后的元素
            list.Insert(index, merged.Value);

            // 递归检查新合并的元素是否还能继续合并
            if (index > 0) CheckAndMerge(list, index - 1);
            if (index < list.Count - 1) CheckAndMerge(list, index);
        }
    }

    // 尝试合并两个字符
    private char? TryMerge(char a, char b)
    {
        // 数字合并逻辑
        if (char.IsDigit(a) && char.IsDigit(b))
        {
            int numA = a - '0';
            int numB = b - '0';

            // 升序合并 (7和8合并为9)
            if (numA + 1 == numB)
            {
                int merged = numA + 2;
                return merged <= 9 ? (char) ('0' + merged) : (char?) null;
            }
            // 降序合并 (9和8合并为7)
            else if (numA - 1 == numB)
            {
                int merged = numA - 2;
                return merged >= 0 ? (char) ('0' + merged) : (char?) null;
            }
        }
        // 字母合并逻辑 (只处理小写字母)
        else if (char.IsLetter(a) && char.IsLetter(b))
        {
            char lowerA = char.ToLower(a);
            char lowerB = char.ToLower(b);

            // 升序合并 (a和b合并为c)
            if (lowerA + 1 == lowerB)
            {
                char merged = (char) (lowerA + 2);
                // 不允许合并到超过'z'
                return merged <= 'z' ? merged : (char?) null;
            }
            // 降序合并 (z和y合并为x)
            else if (lowerA - 1 == lowerB)
            {
                char merged = (char) (lowerA - 2);
                // 不允许合并到低于'a'
                return merged >= 'a' ? merged : (char?) null;
            }
        }

        return null;
    }

    // 移动元素时的处理
    public void MoveItem(int fromList, int fromIndex, int toList, int toIndex)
    {
        List<char> sourceList = fromList == 0 ? UpStr : DownStr;
        List<char> targetList = toList == 0 ? UpStr : DownStr;

        if (fromIndex < 0 || fromIndex >= sourceList.Count) return;

        char item = sourceList[fromIndex];
        sourceList.RemoveAt(fromIndex);

        // 确保目标索引有效
        toIndex = Math.Clamp(toIndex, 0, targetList.Count);
        targetList.Insert(toIndex, item);

        // 检查合并
        if (toIndex > 0) CheckAndMerge(targetList, toIndex - 1);
        if (toIndex < targetList.Count - 1) CheckAndMerge(targetList, toIndex);
    }
}