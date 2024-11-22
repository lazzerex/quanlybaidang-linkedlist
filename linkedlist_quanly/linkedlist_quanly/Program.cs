using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace linkedlist_quanly
{
    public class Post
    {
        public string Content { get; set; }
        public string MediaReference { get; set; }
        public DateTime PostTime { get; set; }
        public Post Next { get; set; }

        public Post(string content, string mediaReference)
        {
            Content = content;
            MediaReference = mediaReference;
            PostTime = DateTime.Now;
            Next = null;
        }
    }

    // Custom LinkedList class để quản lý các bài đăng
    public class SocialMediaLinkedList
    {
        public Post Head { get; private set; }

        public void AddPost(string content, string mediaReference)
        {
            Post newPost = new Post(content, mediaReference);
            if (Head == null)
            {
                Head = newPost;
            }
            else
            {
                Post current = Head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newPost;
            }
        }

        public List<Post> GetAllPosts()
        {
            List<Post> posts = new List<Post>();
            Post current = Head;
            while (current != null)
            {
                posts.Add(current);
                current = current.Next;
            }
            return posts;
        }

        public void DeletePost(DateTime postTime)
        {
            Post current = Head;
            Post previous = null;

            while (current != null && current.PostTime != postTime)
            {
                previous = current;
                current = current.Next;
            }

            if (current != null)
            {
                if (previous == null)
                {
                    Head = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }
            }
        }
    }


    // Form chính của ứng dụng
    public partial class MainForm : Form
    {
        private SocialMediaLinkedList postList;
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Text = "Social Media Post Manager";
        }
        public MainForm()
        {
            InitializeComponent();
            postList = new SocialMediaLinkedList();


            // Thêm sẵn một vài bài đăng vào danh sách
            postList.AddPost("Hello, world!", "https://example.com/hello.jpg");
            postList.AddPost("My first video", "https://example.com/video.mp4");
            postList.AddPost("Funny GIF", "https://example.com/funny.gif");

            // Khởi tạo các control cơ bản
            this.Size = new Size(1000, 700);

            TextBox contentBox = new TextBox
            {
                Multiline = true,
                Size = new Size(400, 100),
                Location = new Point(20, 20)
            };

            Button addMediaBtn = new Button
            {
                Text = "Thêm Media",
                Location = new Point(20, 130)
            };

            Button postBtn = new Button
            {
                Text = "Đăng bài",
                Location = new Point(120, 130)
            };

            FlowLayoutPanel postsPanel = new FlowLayoutPanel
            {
                Size = new Size(960, 480),
                Location = new Point(20, 170),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Xử lý sự kiện thêm media
            string selectedMediaPath = "";
            addMediaBtn.Click += (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Media files (*.jpg, *.gif, *.mp4)|*.jpg;*.gif;*.mp4";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        selectedMediaPath = ofd.FileName;
                    }
                }
            };

            // Xử lý sự kiện đăng bài
            postBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(contentBox.Text))
                {
                    postList.AddPost(contentBox.Text, selectedMediaPath);
                    RefreshPosts(postsPanel);
                    contentBox.Clear();
                    selectedMediaPath = "";
                }
            };

            // Thêm controls vào form
            this.Controls.AddRange(new Control[] {
            contentBox,
            addMediaBtn,
            postBtn,
            postsPanel
        });

            // Hiển thị các bài đăng sẵn có
            RefreshPosts(postsPanel);
        }

        // Cập nhật hiển thị các bài đăng
        private void RefreshPosts(FlowLayoutPanel panel)
        {
            panel.Controls.Clear();
            var posts = postList.GetAllPosts();

            foreach (var post in posts)
            {
                Panel postPanel = new Panel
                {
                    Size = new Size(920, 350),
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label contentLabel = new Label
                {
                    Text = post.Content,
                    Size = new Size(900, 60),
                    Location = new Point(10, 10)
                };

                Label timeLabel = new Label
                {
                    Text = post.PostTime.ToString(),
                    Location = new Point(10, 80)
                };

                if (!string.IsNullOrEmpty(post.MediaReference))
                {
                    string extension = Path.GetExtension(post.MediaReference).ToLower();
                    if (extension == ".jpg" || extension == ".gif")
                    {
                        PictureBox pictureBox = new PictureBox
                        {
                            ImageLocation = post.MediaReference,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(500, 300),
                            Location = new Point(10, 100)
                        };
                        postPanel.Controls.Add(pictureBox);
                    }
                    else if (extension == ".mp4")
                    {
                        Label mediaLabel = new Label
                        {
                            Text = "Selected video: " + Path.GetFileName(post.MediaReference),
                            Location = new Point(10, 100),
                            Size = new Size(300, 20)
                        };
                        postPanel.Controls.Add(mediaLabel);
                    }
                }
                            
                postPanel.Controls.AddRange(new Control[] {
                contentLabel,
                timeLabel,
                
                });

                panel.Controls.Add(postPanel);
            }
        }
    }

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
