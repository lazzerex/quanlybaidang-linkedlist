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
        public string Author { get; set; }
        public Post Next { get; set; }

        public Post(string content, string mediaReference, string author)
        {
            Content = content;
            MediaReference = mediaReference;
            PostTime = DateTime.Now;
            Author = author;
            Next = null;
        }

       
        public Post(string content, string mediaReference, string author, DateTime postTime)
        {
            Content = content;
            MediaReference = mediaReference;
            PostTime = postTime;
            Author = author;
            Next = null;
        }
    }

    public class SocialMediaLinkedList
    {
        public Post Head { get; private set; }

        public void AddPost(string content, string mediaReference, string author)
        {
            Post newPost = new Post(content, mediaReference, author);
            AddPostToList(newPost);
        }

        public void AddPost(string content, string mediaReference, string author, DateTime postTime)
        {
            Post newPost = new Post(content, mediaReference, author, postTime);
            AddPostToList(newPost);
        }

        private void AddPostToList(Post newPost)
        {
            if (Head == null)
            {
                Head = newPost;
                return;
            }

            // Thêm bài đăng mới vào đầu danh sách để hiển thị theo thứ tự mới nhất
            if (newPost.PostTime >= Head.PostTime)
            {
                newPost.Next = Head;
                Head = newPost;
                return;
            }

            // Tìm vị trí phù hợp để chèn bài đăng
            Post current = Head;
            while (current.Next != null && current.Next.PostTime > newPost.PostTime)
            {
                current = current.Next;
            }
            newPost.Next = current.Next;
            current.Next = newPost;
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

        public List<Post> GetUserPosts(string author)
        {
            List<Post> userPosts = new List<Post>();
            Post current = Head;
            while (current != null)
            {
                if (current.Author == author)
                {
                    userPosts.Add(current);
                }
                current = current.Next;
            }
            return userPosts;
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

    public partial class MainForm : Form
    {
        private SocialMediaLinkedList postList;
        private string currentUser = "CurrentUser";
        private bool isProfileView = false;
        private FlowLayoutPanel postsPanel;
        private Random random = new Random();

        private void InitializeComponent()
        {
            this.SuspendLayout();


            //MainForm 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "MainForm";
            this.ResumeLayout(false);
        }
    
            public MainForm()
        {
            InitializeComponent();
            postList = new SocialMediaLinkedList();

            // Thêm các bài đăng mẫu với thời gian ngẫu nhiên
            AddSamplePosts();

            InitializeUI();
        }

        private void AddSamplePosts()
        {
            // Tạo các bài đăng mẫu với thời gian cố định
            postList.AddPost(
                "Hello, world!",
                "https://example.com/hello.jpg",
                "CurrentUser",
                new DateTime(2024, 11, 20, 14, 30, 0) // 20/11/2024 14:30:00
            );

            postList.AddPost(
                "My first video",
                "https://example.com/video.mp4",
                "OtherUser",
                new DateTime(2024, 11, 21, 9, 15, 0) // 21/11/2024 09:15:00
            );

            postList.AddPost(
                "Funny GIF",
                "https://example.com/funny.gif",
                "CurrentUser",
                new DateTime(2024, 11, 21, 16, 45, 0) // 21/11/2024 16:45:00
            );

            postList.AddPost(
                "Beautiful sunset today!",
                "https://example.com/sunset.jpg",
                "OtherUser",
                new DateTime(2024, 11, 22, 10, 20, 0) // 22/11/2024 10:20:00
            );

            postList.AddPost(
                "Just finished my project!",
                null,
                "CurrentUser",
                new DateTime(2024, 11, 22, 11, 30, 0) // 22/11/2024 11:30:00
            );
        }

       /* private DateTime GetRandomTime(int daysBack)
        {
            DateTime now = DateTime.Now;
            int maxMinutesBack = daysBack * 24 * 60;
            int randomMinutes = random.Next(0, maxMinutesBack);
            return now.AddMinutes(-randomMinutes);
        }*/

        private string FormatTimeAgo(DateTime postTime)
        {
            TimeSpan timeDiff = DateTime.Now - postTime;

            if (timeDiff.TotalSeconds < 60)
                return $"{Math.Floor(timeDiff.TotalSeconds)} giây trước";
            if (timeDiff.TotalMinutes < 60)
                return $"{Math.Floor(timeDiff.TotalMinutes)} phút trước";
            if (timeDiff.TotalHours < 24)
                return $"{Math.Floor(timeDiff.TotalHours)} giờ trước";
            if (timeDiff.TotalDays < 7)
                return $"{Math.Floor(timeDiff.TotalDays)} ngày trước";

            return postTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void InitializeUI()
        {
            this.Size = new Size(1000, 700);

            Panel navigationPanel = new Panel
            {
                Size = new Size(960, 40),
                Location = new Point(20, 130)
            };

            Button homeButton = new Button
            {
                Text = "Trang chủ",
                Location = new Point(0, 5),
                Size = new Size(100, 30)
            };

            Button profileButton = new Button
            {
                Text = "Trang cá nhân",
                Location = new Point(110, 5),
                Size = new Size(100, 30)
            };

            navigationPanel.Controls.AddRange(new Control[] { homeButton, profileButton });

            TextBox contentBox = new TextBox
            {
                Multiline = true,
                Size = new Size(400, 100),
                Location = new Point(20, 20)
            };

            Button addMediaBtn = new Button
            {
                Text = "Thêm Media",
                Location = new Point(20, 180)
            };

            Button postBtn = new Button
            {
                Text = "Đăng bài",
                Location = new Point(120, 180)
            };

            postsPanel = new FlowLayoutPanel
            {
                Size = new Size(960, 430),
                Location = new Point(20, 220),
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            homeButton.Click += (s, e) =>
            {
                isProfileView = false;
                RefreshPosts();
            };

            profileButton.Click += (s, e) =>
            {
                isProfileView = true;
                RefreshPosts();
            };

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

            postBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(contentBox.Text))
                {
                    postList.AddPost(contentBox.Text, selectedMediaPath, currentUser);
                    RefreshPosts();
                    contentBox.Clear();
                    selectedMediaPath = "";
                }
            };

            this.Controls.AddRange(new Control[] {
                contentBox,
                navigationPanel,
                addMediaBtn,
                postBtn,
                postsPanel
            });

            RefreshPosts();
        }

        private void RefreshPosts()
        {
            postsPanel.Controls.Clear();
            var posts = isProfileView ?
                postList.GetUserPosts(currentUser) :
                postList.GetAllPosts();

            foreach (var post in posts)
            {
                Panel postPanel = new Panel
                {
                    Size = new Size(920, 350),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(0, 0, 0, 10)
                };

                // Header panel chứa thông tin tác giả và thời gian
                Panel headerPanel = new Panel
                {
                    Size = new Size(900, 30),
                    Location = new Point(10, 10)
                };

                Label authorLabel = new Label
                {
                    Text = post.Author,
                    Font = new Font(this.Font, FontStyle.Bold),
                    Location = new Point(0, 5),
                    AutoSize = true
                };

                Label timeLabel = new Label
                {
                    Text = $"• {FormatTimeAgo(post.PostTime)}",
                    ForeColor = Color.Gray,
                    Location = new Point(authorLabel.Right + 10, 5),
                    AutoSize = true
                };

                // Tooltip cho thời gian chính xác
                ToolTip tooltip = new ToolTip();
                tooltip.SetToolTip(timeLabel, post.PostTime.ToString("dd/MM/yyyy HH:mm:ss"));

                headerPanel.Controls.AddRange(new Control[] { authorLabel, timeLabel });

                Label contentLabel = new Label
                {
                    Text = post.Content,
                    Size = new Size(900, 60),
                    Location = new Point(10, 45)
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
                            Location = new Point(10, 110)
                        };
                        postPanel.Controls.Add(pictureBox);
                    }
                    else if (extension == ".mp4")
                    {
                        Label mediaLabel = new Label
                        {
                            Text = "Selected video: " + Path.GetFileName(post.MediaReference),
                            Location = new Point(10, 110),
                            Size = new Size(300, 20)
                        };
                        postPanel.Controls.Add(mediaLabel);
                    }
                }

                postPanel.Controls.AddRange(new Control[] {
                    headerPanel,
                    contentLabel
                });

                postsPanel.Controls.Add(postPanel);
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
