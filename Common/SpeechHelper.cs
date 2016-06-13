using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Recognition;

namespace TopFashion
{
    /// <summary>
    /// 语音识别辅助类
    /// </summary>
    public class SpeechHelper
    {
        //private SpeechRecognizer spRecognizer;  //语音识别

        /// <summary>
        /// 读文本函数
        /// </summary>
        /// <param name="content">文本内容</param>
        public void Read(string content)
        {
            SpeechSynthesizer speak = new SpeechSynthesizer();
            speak.SpeakAsync(content);
        }

        //判断词库  
        public static Choices ChoiceLibrary()
        {
            return new Choices("任务", "我的任务", "生产规划", "物料管理", "财务会计", "销售分销", "企业情报", "仓库管理", "行政办公", "客户管理", "供应管理", "人力资源", "工作日志", "舆情监控", "系统管理", "回收站");
        }
    }
}
