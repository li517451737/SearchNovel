using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchNovel.Model.Models
{
    public class NovelChapter
    {
        /// <summary>
        /// 章节编码
        /// </summary>
        public int NId { get; set; }
        /// <summary>
        /// 章节名称
        /// </summary>
        public string ChapterName { get; set; }

        /// <summary>
        /// 章节内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
