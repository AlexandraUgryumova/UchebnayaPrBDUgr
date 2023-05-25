using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WorkDB_Ugryumova
{
    public partial class Form1 :Form
    {
        DateTime dt = new DateTime( );

        SqlDataAdapter adptr;
        DataTable table;
        SqlConnection connect = new SqlConnection(@"Data Source=PC325L12\SQLEXPRESS;Initial Catalog=SecurityDB_Ugryumova;Integrated Security=True");
        public Form1 ()
        {
            InitializeComponent( );
            TableGridView( );
            pictureBox1.Image = Image.FromFile("mouse.png");
        }

        private void Form1_Load (object sender, EventArgs e)
        {

        }

        private void button1_Click (object sender, EventArgs e)
        {
            Application.Exit( );
        }

        private void dataGridView1_CellContentClick (object sender, DataGridViewCellEventArgs e)
        {
        }
        private void TableGridView ()
        {
            connect.Open( );
            adptr = new SqlDataAdapter("select * from User_tbl", connect);
            table = new DataTable( );
            adptr.Fill(table);
            dataGridView1.DataSource = table;
            connect.Close( );
        }

        private void button2_Click (object sender, EventArgs e)
        {
            dt = DateTime.Today;
            string log = Logintxt.Text;
            string pass = textBox2.Text;
            if ( log == "" || pass == "" )
            {
                MessageBox.Show("заполнены не все поля", "сообщение");
            }
            else
            {
                connect.Open( );
                string prov = $"SELECT * FROM dbo.User_tbl WHERE Login = '{log}'";
                SqlDataAdapter adptr1 = new SqlDataAdapter(prov, connect);
                DataTable table = new DataTable( );
                adptr1.Fill(table);

                if ( table.Rows.Count == 1 ) 
                {
                    MessageBox.Show("данный логин уже зарезервирован другим пользователем", "сообщение"); 
                    connect.Close( );
                }
                else
                {
                    if ( pass == textBox4.Text )
                    {
                        string query = $"INSERT INTO dbo.User_tbl (login,  password,  Count,  date,  active,   role) VALUES('{log}','{pass}',0,'{dt.ToString("yyyy-MM-dd")}','True','user')";
                        SqlDataAdapter adptr2 = new SqlDataAdapter(query, connect);
                        DataTable table2 = new DataTable( );
                        adptr2.Fill(table2);
                    }
                    else
                    {
                        MessageBox.Show("пароли не одинаковы", "сообщение");
                    }
                        connect.Close( );
                        TableGridView( );
                    
                }
            }
        }

        private void button3_Click (object sender, EventArgs e)
        {
            string log = textBox1.Text;
            string pass = textBox3.Text;
            if ( log == "" || pass == "" )
            {
                MessageBox.Show("заполнены не все поля", "сообщение");
            }
            else
            {
                connect.Open( );
                string prov = $"SELECT Role, Active,Login FROM dbo.User_tbl WHERE Login = '{log}' and Password = '{pass}'";
                string s = $"UPDATE dbo.User_tbl set Count+=1 where Login = '{log}'";
                string select = $"SELECT Login, Active From dbo.User_tbl WHERE Login = '{log}' and Active ='False'";

                SqlDataAdapter adptr1 = new SqlDataAdapter(select, connect);
                DataTable table = new DataTable( );
                adptr1.Fill(table);
                if ( table.Rows.Count == 1 )
                {
                    MessageBox.Show("этот пользователь заблокирован", "сообщение");
                    connect.Close( );
                }
                else
                {

                    SqlDataAdapter adptr2 = new SqlDataAdapter(prov, connect);

                    adptr2.Fill(table);

                    if ( table.Rows.Count == 0 )
                    {
                        MessageBox.Show("вы ввели неверный пароль или логин, попробуйте ещё раз", "сообщение");
                        SqlDataAdapter adptr3 = new SqlDataAdapter(s, connect);
                        adptr3.Fill(table);
                        string sq = $"UPDATE dbo.User_tbl set Active = 'False' where Count >=3";
                        SqlDataAdapter adptr4 = new SqlDataAdapter(sq, connect);
                        adptr4.Fill(table);
                        connect.Close( );
                    }
                    else
                    {
                        if ( table.Rows [ 0 ] [ 0 ].ToString( ) == "user" )
                        {
                            User fr2 = new User( );
                            this.Hide( );
                            fr2.Show( );
                        }
                        if ( table.Rows [ 0 ] [ 0 ].ToString( ) == "Admin" )
                        {
                            Admin fr1 = new Admin( );
                            this.Hide( );
                            fr1.Show( );
                        }
                        connect.Close( );
                    }
                    TableGridView( );
                }
            }
        }

        private void label4_Click (object sender, EventArgs e)
        {
            MessageBox.Show("Шумкова какашка");
        }
    }
}
