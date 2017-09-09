using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchNovel.Model.Models
{
    public class Novel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string CoverImg { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        public string SourceUrl { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string NovelState { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime? LastUpdate { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
