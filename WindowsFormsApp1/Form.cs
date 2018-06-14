using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    public partial class Renamer : Form
    {

            TestObject test = new TestObject();

        List<string> files = new List<string>();

        public Renamer()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                insertionOperation(openFileDialog.FileNames);
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lbxFiles.SelectedIndex >= 0)
            {
                files.RemoveAt(lbxFiles.SelectedIndex);
                lbxFiles.Items.RemoveAt(lbxFiles.SelectedIndex);
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            lbxFiles.Items.Clear();
            files.Clear();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnAddFile.Enabled = false;
            btnClearList.Enabled = false;
            btnRemoveFile.Enabled = false;
            btnStart.Enabled = false;
            cbxImageResolution.Enabled = false;
            tbxFileName.Enabled = false;
            tbxProgress.Text = "Starting Operation...";
            backgroundWorker.RunWorkerAsync(2000);
        }

        private void Renamer_DragDrop(object sender, DragEventArgs e)
        {
            //Store Dropped items in array
            string[] droppedFiles = (string[]) e.Data.GetData(DataFormats.FileDrop);
            insertionOperation(droppedFiles);
        }

        private void Renamer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }



        private void insertionOperation(string[] input)
        {
            foreach (string file in input)
            {
                bool flagExist = false;
                foreach (string item in files)
                {
                    if (item == file)
                    {
                        flagExist = true;
                    }
                }
                if (!flagExist)
                {
                    lbxFiles.Items.Add(file.Substring(file.LastIndexOf("\\") + 1));
                    files.Add(file);
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
                int progress = 0;
                int x = 0;
                string renameText = tbxFileName.Text;
                int width = 0;
                int height = 0;
                List<int> widths = new List<int>();
                List<int> heights = new List<int>();
                BackgroundWorker worker = (BackgroundWorker)sender;

            foreach (string file in files)
                {
                    using (Stream stream = File.OpenRead(file))
                    {
                        try
                        {
                            using (Image sourceImage = Image.FromStream(stream, false, false))
                            {
                                width = sourceImage.Width;
                                height = sourceImage.Height;
                                widths.Add(width);
                                heights.Add(height);
                            }
                        }
                        catch
                        {

                        }
                    }

                    string fileName = files[x].Substring(0, files[x].LastIndexOf("\\"));
                    string extension = files[x].Substring(files[x].LastIndexOf("."));


                    if (cbxImageResolution.Checked)
                    {
                        int counter = 0;
                        int loopcount = 0;
                        foreach (int wid in widths)
                        {
                            if (heights[loopcount] == height && wid == width)
                            {
                                counter++;
                            }
                            loopcount++;
                        }
                        try
                        {
                            System.IO.File.Move(files[x], fileName + "\\" + renameText + " " + width + "x" + height + " " + counter + extension);
                        }
                        catch
                        {
                            System.IO.File.Move(files[x], fileName + "\\" + renameText + " " + x + extension);
                        }
                    }
                    else
                    {
                        System.IO.File.Move(files[x], fileName + "\\" + renameText + " " + x + extension);
                    }
                progress = ((x + 1) / files.Count()*100);
                worker.ReportProgress(progress);
                x++;

                }
                files.Clear();

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tbxProgress.Text = "Completed";
            btnAddFile.Enabled = true;
            btnClearList.Enabled = true;
            btnRemoveFile.Enabled = true;
            tbxFileName.Enabled = true;
            cbxImageResolution.Enabled = true;
            btnStart.Enabled = true;
            lbxFiles.Items.Clear();
            tbxFileName.Text = "";
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prgFiles.Value = e.ProgressPercentage;
            tbxProgress.Text = e.ProgressPercentage + " Complete";
        }
    }
    class TestObject { }
}
