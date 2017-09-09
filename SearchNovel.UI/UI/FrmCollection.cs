using HtmlAgilityPack;
using SearchNovel.UI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.RichTextBox();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集地址：";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(73, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(333, 21);
            this.txtUrl.TabIndex = 1;
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
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtContent.Location = new System.Drawing.Point(0, 42);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(548, 273);
            this.txtContent.TabIndex = 3;
            this.txtContent.Text = "";
            // 
            // FrmCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(548, 315);
            this.Controls.Add(this.panel1);
            this.Name = "FrmCollection";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;
            string html = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                html = HttpHelper.GetHtml(url);
                doc.LoadHtml(html);
                var res = doc.DocumentNode.SelectNodes("//*[@id=\"main\"]//div[@class='novellist']");
                foreach (var mItem in res)
                {
                    var novelList = mItem.SelectNodes("ul//li/a");
                    foreach (var item in novelList)
                    {
                        string href = item.Attributes["href"].Value.Trim();
                        string name = item.InnerText;
                        var desDoc = new HtmlDocument();
                        desDoc.LoadHtml(HttpHelper.GetHtml(href));
                        var mainInfo = desDoc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[5]");
                        string novelName = mainInfo.SelectSingleNode("//*[@id=\"info\"]/h1").InnerText;
                        string author = mainInfo.SelectSingleNode("//*[@id=\"info\"]/p[1]").InnerText.Replace("作&nbsp;&nbsp;&nbsp;&nbsp;者：", "");
                        string lastUpdate = mainInfo.SelectSingleNode("//*[@id=\"info\"]/p[3]").InnerText.Replace("最后更新：", "");
                        string intro = mainInfo.SelectSingleNode("//*[@id=\"intro\"]/p").InnerText;
                        string ConverImg = mainInfo.SelectSingleNode("//*[@id=\"fmimg\"]/img")?.Attributes["src"].Value.ToString();

                        var chapters = desDoc.DocumentNode.SelectNodes("//*[@id=\"list\"]/dl/dd/a");
                        foreach (var chapter in chapters)
                        {
                            var cHref = chapter.Attributes["href"].Value.ToString();
                            var chapterDoc = new HtmlDocument();
                            chapterDoc.LoadHtml(HttpHelper.GetHtml(cHref));
                            var content = chapterDoc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]").InnerText;
                        }

                    }
                }
              
            }
        }
    }
}
