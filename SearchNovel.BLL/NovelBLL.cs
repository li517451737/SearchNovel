using Search.DAL;
using SearchNovel.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchNovel.BLL
{
    
    public class NovelBLL
    {
        private readonly NovelDAL _dal;

        public NovelBLL()
        {
            _dal = new NovelDAL();
        }


        public int AddNovel(Novel novelInfo)
        {
           return _dal.AddNovel(novelInfo);
        }

        public int AddNovelChapter(NovelChapter chapter)
        {
            return _dal.AddNovelChapter(chapter);
        }
    }
}
