using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TopFashion
{
    public enum SystemType
    {
        /// <summary>
        /// 字符
        /// </summary>
        [Description("字符")]
        字符,
        /// <summary>
        /// 数字
        /// </summary>
        [Description("数字")]
        数字,
        /// <summary>
        /// 时间
        /// </summary>
        [Description("时间")]
        时间,
        /// <summary>
        /// 附件(如：各类文档、图片、声音、视频等)
        /// </summary>
        [Description("附件")]
        附件
    }
}
