using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace FastPaster
{

    /// <summary>
    /// This app places in to the beginig of every added file some text typed in richTextBox.
    /// It was developed for fast adding some important comments to all source files at the same time.
    /// </summary>
    public partial class FastTextPaster : Form
    {


        public FastTextPaster()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adding files to fileList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                if (openFileDialog1.FileNames != null && openFileDialog1.FileNames.Length > 0)
                {
                    foreach (var item in openFileDialog1.FileNames)
                    {
                        lstboxFiles.Items.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Removing the selected file from the fileList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            var removeItem = lstboxFiles.SelectedItem;
            lstboxFiles.Items.Remove(removeItem);
        }

        /// <summary>
        /// Initializes pasting process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            lblState.Text = "Pasting in process...";
            foreach (var file in lstboxFiles.Items)
            {
                string fileContent = null;

                using (FileStream fs = new FileStream((string)file, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs))
                {
                    fileContent = reader.ReadToEnd();                    
                }                

                using (FileStream fs = new FileStream((string)file, FileMode.Open))                
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(richTextForPasting.Text);
                    writer.Write(fileContent);
                }

            }

            lblState.Text = "Text Pasted.";
        }

        /// <summary>
        /// Adding files from all directories to the filelist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            string[] filters = textBox1.Text.Split(',');// Filters for files selection from directories

            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> dirs = new List<string>();
                dirs.Add(folderBrowserDialog1.SelectedPath);
                dirs.AddRange(RecursiveDirectoriesGetter(folderBrowserDialog1.SelectedPath));
                foreach (var directory in dirs)
                {
                    var files = new List<string>();
                    if (filters != null && filters.Length > 0)
                    {
                        foreach (var filter in filters)
                        {
                            files.AddRange(Directory.GetFiles(directory, filter));
                        }
                    }
                    else
                    {
                        files.AddRange(Directory.GetFiles(directory));
                    }
                    foreach (var item in files)
                    {
                        lstboxFiles.Items.Add(item);
                    }
                }
            }
        }



        string[] RecursiveDirectoriesGetter(string dir)
        {
            List<string> dirs = new List<string>();
            dirs.AddRange(Directory.GetDirectories(dir));
            List<string> rslt = new List<string>();
            foreach(var item in dirs)
            {
                var tmp = RecursiveDirectoriesGetter(item);
                if (tmp != null)
                {
                    rslt.AddRange(tmp);
                }
            }
            dirs.AddRange(rslt.ToArray());
            if (dirs != null)
            {
                return dirs.ToArray();
            }
            else
            {
                return null;
            }
        }
       
    }
}
