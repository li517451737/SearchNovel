using HtmlAgilityPack;
using SearchNovel.BLL;
using SearchNovel.Model.Models;
using SearchNovel.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SearchNovel.UI
{
    public class FrmCollection : BaseForm
    {
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Label label1;


        private HtmlDocument doc = new HtmlDocument();

        public FrmCollection()
        {
            InitializeComponent();
            this.txtUrl.Text = "http://www.b5200.org/xiaoshuodaquan/";
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtContent);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.txtUrl);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(548, 315);
            this.panel1.TabIndex = 0;
            // 
            // txtContent
            // 
            this.txtContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtContent.Location = new System.Drawing.Point(0, 42);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(548, 273);
            this.txtContent.TabIndex = 3;
            this.txtContent.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(412, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "采集";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(73, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(333, 21);
            this.txtUrl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集地址：";
            // 
            // FrmCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(548, 315);
            this.Controls.Add(this.panel1);
            this.Name = "FrmCollection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "小说采集";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtUrl.Text;
                string html = string.Empty;
                if (!string.IsNullOrEmpty(url))
                {
                    html = HttpHelper.GetHtml(url);
                    if (string.IsNullOrEmpty(html))
                    {
                        txtContent.Text = "采集内容为空";
                        return;
                    }
                    doc.LoadHtml(html);
                    var res = doc.DocumentNode.SelectNodes("//*[@id=\"main\"]//div[@class='novellist']");
                    foreach (var mItem in res)
                    {
                        var novelList = mItem.SelectNodes("ul//li/a");
                        foreach (var item in novelList)
                        {
                            Task.Run(() => AddNovel(item));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtContent.Text = ex.Message;
            }
        }

        private void AddNovel(HtmlNode node)
        {
            Novel novelInfo = new Novel();
            NovelBLL nBill = new NovelBLL();
            string href = node.Attributes["href"].Value.Trim();
            var chapters = HttpHelper.GetNovelHtml(href, ref novelInfo);
            var nId = nBill.AddNovel(novelInfo);
            if (nId > 0)
            {
                Task.Run(()=> {
                    AddNovelChapter(chapters, nId);
                });
            }
        }

        private void AddNovelChapter(HtmlNodeCollection chapters,int nId)
        {
            NovelBLL nBill = new NovelBLL();
            int sort = 0;
            foreach (var chapter in chapters)
            {
                NovelChapter chapterInfo = new NovelChapter();
                chapterInfo.NId = nId;
                chapterInfo.Sort = sort;
                chapterInfo.ChapterName = chapter.InnerText;
                var cHref = chapter.Attributes["href"].Value.ToString();
                var chapterDoc = new HtmlDocument();
                chapterDoc.LoadHtml(HttpHelper.GetHtml(cHref));
                chapterInfo.Content = chapterDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]")?.InnerText;
                nBill.AddNovelChapter(chapterInfo);
                sort++;
                if (this.txtContent.InvokeRequired)
                {
                    Action<string, string> actionDelegate = (x, y) => { this.txtContent.Text = string.Format("小说[{0}]--[{1}]采集成功;\r\n", x, y); };
                    this.txtContent.Invoke(actionDelegate, nId.ToString(), chapterInfo.ChapterName);
                }
                else
                {
                    txtContent.Text += string.Format("小说[{0}]--[{1}]采集成功;\r\n", nId.ToString(), chapterInfo.ChapterName);
                }
            }
        }
    }
}
