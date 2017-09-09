using SearchNovel.Model.Models;
using SearchNovel.Tool.DBHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Search.DAL
{
    public class NovelDAL
    {
        public int AddNovel(Novel novelInfo)
        {
            string cmdText = @"INSERT  dbo.Novels
                                ( Author ,
                                  CoverImg ,
                                  CreationTime ,
                                  Intro ,
                                  LastUpdate ,
                                  NovelState ,
                                  Title ,
                                  SourceUrl
                                )
                        VALUES  ( @Author , -- Author - nvarchar(50)
                                  @CoverImg , -- CoverImg - nvarchar(max)
                                  SYSDATETIME() , -- CreationTime - datetime2
                                  @Intro , -- Intro - nvarchar(max)
                                  @LastUpdate , -- LastUpdate - datetime2
                                  @NovelState , -- NovelState - int
                                  @Title , -- Title - nvarchar(50)
                                  @SourceUrl  -- SourceUrl - nvarchar(max)
                                );
                        SELECT  @@IDENTITY;";
            int num = 0;
            try
            {
                if (!CheckNovelExists(novelInfo.Title, novelInfo.Author))
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                new SqlParameter("Author",novelInfo.Author),
                new SqlParameter("CoverImg",novelInfo.CoverImg),
                new SqlParameter("Intro",novelInfo.Intro),
                new SqlParameter("NovelState",novelInfo.NovelState),
                new SqlParameter("Title",novelInfo.Title),
                new SqlParameter("LastUpdate",novelInfo.LastUpdate),
                new SqlParameter("SourceUrl",novelInfo.SourceUrl),
                };
                    var obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, param);
                    if (obj != null)
                        num = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }

        public int AddNovelChapter(NovelChapter chapter)
        {
            string cmdText = @"INSERT  dbo.NovelChapters
                            ( ChapterName, Content, NId,Sort )
                    VALUES  ( @ChapterName, -- ChapterName - nvarchar(50)
                              @Content, -- Content - nvarchar(max)
                              @NId,  -- NId - int
                              @Sort
                              );";
            int num = 0;
            try
            {
                if (!CheckNovelChapterExists(chapter.NId, chapter.ChapterName))
                {
                    SqlParameter[] param = new SqlParameter[]
                    {
                new SqlParameter("ChapterName",chapter.ChapterName),
                new SqlParameter("NId",chapter.NId),
                new SqlParameter("Content",chapter.Content),
                new SqlParameter("Sort",chapter.Sort)
                };
                    var obj = SqlHelper.ExecuteScalar(CommandType.Text, cmdText, param);
                    if (obj != null)
                        num = Convert.ToInt32(obj);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return num;
        }



        public bool CheckNovelExists(string title, string author)
        {
            string isExistsCmdText = "SELECT COUNT(1) FROM dbo.Novels WHERE Author=@Author AND Title=@Title";

            var obj = SqlHelper.ExecuteScalar(CommandType.Text, isExistsCmdText, new SqlParameter[] {
               new SqlParameter("Title",title),
               new SqlParameter("Author",author),
            });
            return Convert.ToInt32(obj) > 0;
        }


        private bool CheckNovelChapterExists(int nId, string chapterName = "")
        {

            string isExistsCmdText = @"SELECT  COUNT(1)
                                        FROM dbo.NovelChapters
                                        WHERE   NId = @NId
                                                AND ChapterName = @ChapterName; ";

            var obj = SqlHelper.ExecuteScalar(CommandType.Text, isExistsCmdText, new SqlParameter[] {
               new SqlParameter("NId",nId),
               new SqlParameter("ChapterName",chapterName),
            });
            return Convert.ToInt32(obj) > 0;
        }
    }
}
