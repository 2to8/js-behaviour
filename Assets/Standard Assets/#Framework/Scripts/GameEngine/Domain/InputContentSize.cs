using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameEngine.Domain {

public class InputContentSize : MonoBehaviour {

    //文本区
    public Text text;

    //输入框
    public InputField input;

    //内容区
    public RectTransform contents;

    void FixedUpdate()
    {
        //得到内容的大小
        var size = contents.sizeDelta;

        //获取行数，不要使用Text组件来分割，不知道什么原因无法完整分割。
        var texts = input.text.Split('\n');

        //下面是设置宽度的，因为每段文字都有自己的长度，这里我们取最长的文字长度
        //先将长度保存到数组中
        var ints = new List<int>();

        foreach (var v in texts) {
            ints.Add(v.Length);
        }

        //GetMax是取得最大的数，本函数来自于网络
        var maxw = GetMax(ints.ToArray());

        //设置宽度：文字长度乘以文字大小，保留一些空白内容所以要在乘以2，也可以根据自己的要求加上一写像素
        size.x = maxw * text.fontSize * 2;

        //设置高度：文字行数乘以文字大小，同样保留空白区
        size.y = texts.Length * text.fontSize * 2;

        //以下是防止内容变小
        //判断当前宽度是否小于原宽度，如果小于的话则不设置
        if (size.x < 1545.2f) {
            size.x = 1545.2f;
        }

        //判断当前高度是否小于原高度，如果小于的话则不设置
        if (size.y < 714.42f) {
            size.y = 714.42f;
        }

        //赋值
        contents.sizeDelta = new Vector2(size.x, size.y);
    }

    /// <summary>
    /// 数组中最大的值
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    static int GetMax(int[] array)
    {
        var max = 0;

        for (var i = 0; i < array.Length; i++) {
            max = max > array[i] ? max : array[i];
        }

        return max;
    }

}

}