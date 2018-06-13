using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WindowsFormsApp1
{
    public partial class Renamer : Form
    {

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
            string renameText = tbxFileName.Text;
            for(int x = 0; x < files.Count(); x++)
            {
                String fileName = files[x].Substring(0, files[x].LastIndexOf("\\"));
                System.IO.File.Move(files[x], fileName + "\\" + renameText + " " + x);
                prgFiles.Value = (x+1)/files.Count() * 100;
                tbxProgress.Text = "changing name of " + files[x].Substring(fileName.LastIndexOf("\\") + 1);
            }
            tbxProgress.Text = "Completed";
            lbxFiles.Items.Clear();
            files.Clear();
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
    }
}
