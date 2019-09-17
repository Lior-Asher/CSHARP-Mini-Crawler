using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mini_Crawler
{
    public partial class CrawlerForm : Form
    {
        public CrawlerForm()
        {
            InitializeComponent();
        }

        private Thread _t;

        private void btnStart_Click(object sender, EventArgs e)
        {
            _t = new Thread(StartCrawling);
            _t.IsBackground = true; // Run in background
            _t.Start();
        }

        // https://www.youtube.com/watch?v=9AIApJmbulY
        // Update a UI element from another thread by raising an event.
        private void StartCrawling()
        {
            Crawler crawler = new Crawler();

            // Subscribe 'Crawler_LinkFound' method to LinkFoundEventArgs' EventHandler. 
            crawler.LinkFound += new EventHandler<LinkFoundEventArgs>(Crawler_LinkFound);
            crawler.Crawl(textBoxStartURL.Text);
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netframework-4.8
        // https://www.tutorialsteacher.com/csharp/csharp-action-delegate
        private void Crawler_LinkFound(object sender, LinkFoundEventArgs e)
        {
            // Calling parameterized delegate
            Action<string> foundLink = new Action<string>(AddURLToListBox);
            Invoke(foundLink, e.Link);
        }

        private void AddURLToListBox(string url)
        {
            listBoxURLs.Items.Add(url);
            // SelectedIndex is the next url to crawl
            listBoxURLs.SelectedIndex = listBoxURLs.Items.Count - 1; // Select last url
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_t.IsAlive)
                _t.Abort();
        }

        // Enable the start button click only if a url was provided.
        private void textBoxStartURL_TextChanged(object sender, EventArgs e)
        {
            btnStart.Enabled = textBoxStartURL.Text != "";
        }
    }
}
