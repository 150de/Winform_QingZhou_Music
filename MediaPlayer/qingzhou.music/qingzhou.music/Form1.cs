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

namespace qingzhou.music
{
    public partial class Form1 : Form
    {
        //声明存储路径与歌单变量
        public static List<string> list = new List<string>();
        public static string path = Application.StartupPath + "/Resources/";

        //加载本地资源到listBox控件中的方法
        public void loadLocalResources()
        {
            //每次加载窗体前都清空之前listBox中的内容
            list = new List<string>();
            listBox1.Items.Clear();

            //判断路径是否存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //获取到所有的资源的文件名
            foreach (var temp in Directory.GetFiles(path))
            {
                var name = temp.Split('/');
                list.Add(name[name.Length-1]);
            }
            this.listBox1.Items.AddRange(list.ToArray());
        }


        public Form1()
        {
            InitializeComponent();
            //将MediaPlayer的工具栏显示设置为不显示，在属性中调整一样可以
            axWindowsMediaPlayer1.uiMode = "none";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadLocalResources();
        }

        //播放点击事件
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("未选中歌曲！");
            }
            else
            {
                axWindowsMediaPlayer1.URL = path + list[this.listBox1.SelectedIndex];
            }
        }
        //暂停点击事件
        private void button2_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }
        //继续点击事件
        private void button3_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        //上一曲点击事件
        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("未选中歌曲！");
            }
            else
            {
                try
                {
                    this.listBox1.SelectedIndex--;
                    axWindowsMediaPlayer1.URL = path + list[this.listBox1.SelectedIndex];
                }
                catch
                {
                    MessageBox.Show("已经是第一首歌了！");
                    listBox1.SelectedIndex = 0;
                }
            }
        }
        //下一曲点击事件
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("未选中歌曲！");
            }
            else
            {
                try
                {
                    this.listBox1.SelectedIndex++;                    
                }
                catch
                {
                    listBox1.SelectedIndex = 0;
                }
                axWindowsMediaPlayer1.URL = path + list[this.listBox1.SelectedIndex];
            }
        }
        //进度条功能的实现
        private void timer1_Tick(object sender, EventArgs e)
        {
            //只有是播放状态时才进行进度条的更新
            if ((int)axWindowsMediaPlayer1.playState == 3)
            {
                this.panel2.BackColor = Color.LightGreen;
                double current = axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                double All = axWindowsMediaPlayer1.currentMedia.duration;
                //播放的百分比
                double flag = current / All;
                //为应对计时器不是第一时间启动，我们的panel1的大小其实是393,14
                //但是，在这里我乘以一个略大于393的400，能让我们的进度条跑满，情况不定，灵活调整
                this.panel2.Size = new Size((int)(flag*400),14);
            }
        }
        //添加歌曲按钮功能实现
        private void button6_Click(object sender, EventArgs e)
        {
            //OpenFileDialog可以打开文件资源管理器，先实例化一个
            OpenFileDialog ofd = new OpenFileDialog();
            //属性InitialDirectory表示首先打开的位置
            ofd.InitialDirectory = Application.StartupPath;
            //属性Title显示在左上角的打开文件资源管理器的标题
            ofd.Title = "[请选择要打开的文件]";
            //属性Multiselect表示是否允许多选
            ofd.Multiselect = false;
            //属性Filter表示能够在右下角选择哪些类型的文件进行显示
            ofd.Filter = "图片文件|*.jpg|所有文件|*.*";
            //属性FilterIndex表示我打开文件资源管理器时优先显示的是哪种文件
            //比如在此处如果设为1，那么久优先在打开的文件资源管理器中显示.jpg格式的图片文件
            ofd.FilterIndex = 2;
            //属性RestoreDirectory表示需不需要记住上次选中时的目录
            ofd.RestoreDirectory = true;
            //ofd.ShowDialog() == DialogResult.OK判断我们是否成功的选中了文件
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //获取到打开的文件，存储到path下，允许覆盖同名文件
                File.Copy(ofd.FileName,path+ofd.SafeFileName,true);
                //重新加载列表
                loadLocalResources();
            }
        }
        //删除歌单中的歌曲
        private void button7_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("未选中需要删除的歌曲");
            }
            else
            {
                //获取到选中要删除歌曲的路径
                string currentPath = path + list[listBox1.SelectedIndex];
                File.Delete(currentPath);
                //删除后需要重新加载列表
                loadLocalResources();
            }
        }

        //换肤
        private void button8_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Cyan;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.SkyBlue;
        }
    }
}
