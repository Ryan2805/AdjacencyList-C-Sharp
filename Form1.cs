using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace WindowsFormTreeView
{
    public partial class Form1 : Form
    {
        public class Author
        {
            public int id { get; set; }
            public string title { get; set; }
            public int? parent_id { get; set; }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS01;Database=master;Trusted_Connection=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connection successful!");

                    string query = "SELECT id, title, parent_id FROM dbo.authors";
                    List<Author> authors = new List<Author>();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                authors.Add(new Author
                                {
                                    id = reader.GetInt32(0),
                                    title = reader.GetString(1),
                                    parent_id = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2)
                                });
                            }
                        }
                    }

                    treeView1.Nodes.Clear();
                    Dictionary<int, TreeNode> nodes = new Dictionary<int, TreeNode>();

                    foreach (var author in authors)
                    {
                        TreeNode newNode = new TreeNode(author.title) { Tag = author.id };
                        nodes[author.id] = newNode;

                        if (author.parent_id == null)
                        {
                            treeView1.Nodes.Add(newNode);
                        }
                        else
                        {
                            nodes[(int)author.parent_id].Nodes.Add(newNode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
    }
}
