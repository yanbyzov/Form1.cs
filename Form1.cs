using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace connect_DB
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
        }

         
        //start programm
        private async void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=АЛЕКСАНДР-ПК\SQLEXPRESS;Initial Catalog=Yan;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            button11_Click(null, null);
        }

        //close program
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        //insert product
        private void button1_Click(object sender, EventArgs e)
        {
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;

            textBox1.Text = null;
            textBox2.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;

            textBox1.Visible = true;
            textBox2.Visible = true;
            comboBox1.Visible = true;
            comboBox2.Visible = true;

            button10.Visible = true;
        }

        //save insert data product
        private async void button10_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text)
                && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox1.Text)
                && !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [product] (name, price, color, type)VALUES(@Name, @price, @color, @type)", sqlConnection);

                    SqlDataReader sqlReader1 = null;

                    SqlCommand command1 = new SqlCommand("SELECT [id_color] FROM[color] WHERE [name_color]=@name_color", sqlConnection);

                    command1.Parameters.AddWithValue("name_color", comboBox1.Text);

                    SqlDataReader sqlReader2 = null;

                    SqlCommand command2 = new SqlCommand("SELECT [id_type] FROM[type] WHERE [name_type]=@name_type", sqlConnection);

                    command2.Parameters.AddWithValue("name_type", comboBox2.Text);

                    command.Parameters.AddWithValue("Name", textBox1.Text);
                    command.Parameters.AddWithValue("price", textBox2.Text);
                    try
                    {
                        sqlReader1 = await command1.ExecuteReaderAsync();

                        while (await sqlReader1.ReadAsync())
                        {
                            command.Parameters.AddWithValue("color", sqlReader1["id_color"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sqlReader1 != null)
                            sqlReader1.Close();
                    }

                    try
                    {
                        sqlReader2 = await command2.ExecuteReaderAsync();

                        while (await sqlReader2.ReadAsync())
                        {
                            command.Parameters.AddWithValue("type", sqlReader2["id_type"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sqlReader2 != null)
                            sqlReader2.Close();
                    }

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button11_Click(null, null);
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                textBox1.Visible = false;
                textBox2.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;

                button10.Visible = false;
            }
            else if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrWhiteSpace(textBox1.Text)
                && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrWhiteSpace(textBox2.Text)
                && string.IsNullOrEmpty(comboBox1.Text) && string.IsNullOrWhiteSpace(comboBox1.Text)
                && string.IsNullOrEmpty(comboBox2.Text) && string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                button11_Click(null, null);
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                textBox1.Visible = false;
                textBox2.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;

                button10.Visible = false;
            }
            else
            {
                label8.Visible = true;
                label8.Text = "Все поля должны быть заполнены!";
            }
        }

        //changing data in a product
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    var text = listBox1.GetItemText(listBox1.Items[i]);
                    string[] words = text.Split('\t');
                    var selected = listBox1.GetSelected(i);
                    if (selected)
                    {
                        textBox4.Text = words[0];
                        textBox1.Text = words[1];
                        textBox2.Text = words[2];
                        comboBox1.Text = words[3];
                        comboBox2.Text = words[5];

                        label4.Visible = true;
                        label5.Visible = true;
                        label6.Visible = true;
                        label7.Visible = true;

                        textBox1.Visible = true;
                        textBox2.Visible = true;
                        comboBox1.Visible = true;
                        comboBox2.Visible = true;

                        button12.Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //delete from product
        private async void button3_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [product] WHERE [id] = @id", sqlConnection);
            try
            {
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    var text = listBox1.GetItemText(listBox1.Items[i]);
                    string slovo = text.Split()[0];
                    var selected = listBox1.GetSelected(i);
                    if (selected)
                    {
                        command.Parameters.AddWithValue("id", slovo);
                        break;
                    }
                }
            
            await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button11_Click(null, null);
        }

        //data update
        private async void button11_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            SqlDataReader sqlReader = null;
            SqlDataReader sqlReader1 = null;
            SqlDataReader sqlReader2 = null;

            SqlCommand command = new SqlCommand("SELECT  [product].[id], [product].[name], [product].[price], [color].[name_color], [type].[name_type] FROM[product], [color], [type] WHERE[color].[id_color] = [product].[color] and [product].[type] = [type].[id_type]", sqlConnection);
            SqlCommand command1 = new SqlCommand("SELECT * FROM[color]", sqlConnection);
            SqlCommand command2 = new SqlCommand("SELECT * FROM[type]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["id"]) + "\t" + Convert.ToString(sqlReader["name"]) + "\t" + Convert.ToString(sqlReader["price"]) + "\t" + Convert.ToString(sqlReader["name_color"]) + "\t\t" + Convert.ToString(sqlReader["name_type"]));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }


            try
            {
                sqlReader1 = await command1.ExecuteReaderAsync();

                while (await sqlReader1.ReadAsync())
                {
                    listBox2.Items.Add(Convert.ToString(sqlReader1["id_color"]) + "\t" + Convert.ToString(sqlReader1["name_color"]));
                    comboBox1.Items.Add(Convert.ToString(sqlReader1["name_color"]));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader1 != null)
                    sqlReader1.Close();
            }

            try
            {
                sqlReader2 = await command2.ExecuteReaderAsync();

                while (await sqlReader2.ReadAsync())
                {
                    listBox3.Items.Add(Convert.ToString(sqlReader2["id_type"]) + "\t" + Convert.ToString(sqlReader2["name_type"]));
                    comboBox2.Items.Add(Convert.ToString(sqlReader2["name_type"]));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader2 != null)
                    sqlReader2.Close();
            }
        }

        //save changing data product
        private async void button12_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text)
                && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrWhiteSpace(comboBox1.Text)
                && !string.IsNullOrEmpty(comboBox2.Text) && !string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                try
                {
                    SqlCommand command = new SqlCommand("UPDATE [product] SET [name]=@Name, [price]=@price, [color]=@color, [type]=@type WHERE [id]=@id", sqlConnection);

                    SqlDataReader sqlReader1 = null;

                    SqlCommand command1 = new SqlCommand("SELECT [id_color] FROM[color] WHERE [name_color]=@name_color", sqlConnection);

                    command1.Parameters.AddWithValue("name_color", comboBox1.Text);

                    SqlDataReader sqlReader2 = null;

                    SqlCommand command2 = new SqlCommand("SELECT [id_type] FROM[type] WHERE [name_type]=@name_type", sqlConnection);

                    command2.Parameters.AddWithValue("name_type", comboBox2.Text);

                    command.Parameters.AddWithValue("id", textBox4.Text);
                    command.Parameters.AddWithValue("Name", textBox1.Text);
                    command.Parameters.AddWithValue("price", textBox2.Text);
                    try
                    {
                        sqlReader1 = await command1.ExecuteReaderAsync();

                        while (await sqlReader1.ReadAsync())
                        {
                            command.Parameters.AddWithValue("color", sqlReader1["id_color"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sqlReader1 != null)
                            sqlReader1.Close();
                    }

                    try
                    {
                        sqlReader2 = await command2.ExecuteReaderAsync();

                        while (await sqlReader2.ReadAsync())
                        {
                            command.Parameters.AddWithValue("type", sqlReader2["id_type"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sqlReader2 != null)
                            sqlReader2.Close();
                    }

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button11_Click(null, null);
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                textBox1.Visible = false;
                textBox2.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;

                button12.Visible = false;
            }
            else if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrWhiteSpace(textBox1.Text)
                && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrWhiteSpace(textBox2.Text)
                && string.IsNullOrEmpty(comboBox1.Text) && string.IsNullOrWhiteSpace(comboBox1.Text)
                && string.IsNullOrEmpty(comboBox2.Text) && string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                button11_Click(null, null);
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;

                textBox1.Visible = false;
                textBox2.Visible = false;
                comboBox1.Visible = false;
                comboBox2.Visible = false;

                button12.Visible = false;
            }
            else
            {
                label8.Visible = true;
                label8.Text = "Все поля должны быть заполнены!";
            }
        }

        //insert color
        private void button6_Click(object sender, EventArgs e)
        {
            label13.Visible = true;

            textBox6.Text = null;
            textBox6.Visible = true;

            button14.Visible = true;
        }

        //save insert data color
        private async void button14_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [color] (name_color)VALUES(@Name)", sqlConnection);

                    command.Parameters.AddWithValue("Name", textBox6.Text);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button11_Click(null, null);
                label13.Visible = false;

                textBox6.Visible = false;

                button14.Visible = false;
            }
            else if (string.IsNullOrEmpty(textBox6.Text) && string.IsNullOrWhiteSpace(textBox6.Text))
            {
                button11_Click(null, null);
                label13.Visible = false;

                textBox6.Visible = false;

                button14.Visible = false;
            }
        }

        //delete from color
        private async void button4_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [color] WHERE [id_color] = @id", sqlConnection);
            try
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    var text = listBox2.GetItemText(listBox2.Items[i]);
                    string slovo = text.Split()[0];
                    var selected = listBox2.GetSelected(i);
                    if (selected)
                    {
                        command.Parameters.AddWithValue("id", slovo);
                        break;
                    }
                }

                await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button11_Click(null, null);
        }

        //changing data in a color
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    var text = listBox2.GetItemText(listBox2.Items[i]);
                    string[] words = text.Split('\t');
                    var selected = listBox2.GetSelected(i);
                    if (selected)
                    {
                        textBox3.Text = words[0];
                        textBox6.Text = words[1];

                        label13.Visible = true;

                        textBox6.Visible = true;

                        button13.Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //save changing data color
        private async void button13_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox6.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                try
                {
                    SqlCommand command = new SqlCommand("UPDATE [color] SET [name_color]=@Name WHERE [id_color]=@id", sqlConnection);
                    
                    command.Parameters.AddWithValue("id", textBox3.Text);
                    command.Parameters.AddWithValue("Name", textBox6.Text);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button11_Click(null, null);
                label13.Visible = false;

                textBox6.Visible = false;

                button13.Visible = false;
            }
            else if (string.IsNullOrEmpty(textBox6.Text) && string.IsNullOrWhiteSpace(textBox6.Text))
            {
                button11_Click(null, null);
                label13.Visible = false;

                textBox6.Visible = false;

                button13.Visible = false;
            }
        }

        //insert type
        private void button9_Click(object sender, EventArgs e)
        {
            label11.Visible = true;

            textBox7.Text = null;
            textBox7.Visible = true;

            button16.Visible = true;
        }

        //save insert data type
        private async void button16_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox7.Text))
            {
                try
                {
                    SqlCommand command = new SqlCommand("INSERT INTO [type] (name_type)VALUES(@Name)", sqlConnection);

                    command.Parameters.AddWithValue("Name", textBox7.Text);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button11_Click(null, null);
                label11.Visible = false;

                textBox7.Visible = false;

                button16.Visible = false;
            }
            else if (string.IsNullOrEmpty(textBox7.Text) && string.IsNullOrWhiteSpace(textBox7.Text))
            {
                button11_Click(null, null);
                label11.Visible = false;

                textBox7.Visible = false;

                button16.Visible = false;
            }
        }

        //delete from type
        private async void button7_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM [type] WHERE [id_type] = @id", sqlConnection);
            try
            {
                for (int i = 0; i < listBox3.Items.Count; i++)
                {
                    var text = listBox3.GetItemText(listBox3.Items[i]);
                    string slovo = text.Split()[0];
                    var selected = listBox3.GetSelected(i);
                    if (selected)
                    {
                        command.Parameters.AddWithValue("id", slovo);
                        break;
                    }
                }

                await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button11_Click(null, null);
        }

        //changing data in a type
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listBox3.Items.Count; i++)
                {
                    var text = listBox3.GetItemText(listBox3.Items[i]);
                    string[] words = text.Split('\t');
                    var selected = listBox3.GetSelected(i);
                    if (selected)
                    {
                        textBox5.Text = words[0];
                        textBox7.Text = words[1];

                        label11.Visible = true;

                        textBox7.Visible = true;

                        button15.Visible = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //save changing data type
        private async void button15_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox7.Text))
            {
                try
                {
                    SqlCommand command = new SqlCommand("UPDATE [type] SET [name_type]=@Name WHERE [id_type]=@id", sqlConnection);

                    command.Parameters.AddWithValue("id", textBox5.Text);
                    command.Parameters.AddWithValue("Name", textBox7.Text);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                button11_Click(null, null);
                label11.Visible = false;

                textBox7.Visible = false;

                button15.Visible = false;
            }
            else if (string.IsNullOrEmpty(textBox7.Text) && string.IsNullOrWhiteSpace(textBox7.Text))
            {
                button11_Click(null, null);
                label11.Visible = false;

                textBox7.Visible = false;

                button15.Visible = false;
            }
        }

        private string result = "";

        //заполнение строки result
        private async void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT  [product].[id], [product].[name], [product].[price], [color].[name_color], [type].[name_type] FROM[product], [color], [type] WHERE[color].[id_color] = [product].[color] and [product].[type] = [type].[id_type]", sqlConnection);
            try
            {
                sqlReader = await command.ExecuteReaderAsync();
                result += ("\t\t\t\t" + "ТОВАРЫ" + "\n\n");
                result += ("id" + "\t" + "Название" + "\t" + "Стоимость" + " руб.\t" + "Цвет" + "      \t" + "Тип кузова" + "\n");
                while (await sqlReader.ReadAsync())
                {
                    result += (Convert.ToString(sqlReader["id"]) + "\t" + Convert.ToString(sqlReader["name"]) + "\t\t" + Convert.ToString(sqlReader["price"]) + " руб.\t" + Convert.ToString(sqlReader["name_color"]) + "      \t" + Convert.ToString(sqlReader["name_type"]) + "\n");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }

            SqlDataReader sqlReader1 = null;
            SqlCommand command1 = new SqlCommand("SELECT * FROM[color]", sqlConnection);

            try
            {
                sqlReader1 = await command1.ExecuteReaderAsync();
                result += ("\n\n\t\t\t\t" + "ЦВЕТА" + "\n\n");
                result += ("id" + "\t" + "Название" + "\n");
                while (await sqlReader1.ReadAsync())
                {
                    result += (Convert.ToString(sqlReader1["id_color"]) + "\t" + Convert.ToString(sqlReader1["name_color"]) + "\n");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader1 != null)
                    sqlReader1.Close();
            }

            SqlDataReader sqlReader2 = null;
            SqlCommand command2 = new SqlCommand("SELECT * FROM[type]", sqlConnection);

            try
            {
                sqlReader2 = await command2.ExecuteReaderAsync();
                result += ("\n\n\t\t\t\t" + "ТИПЫ КУЗОВОВ" + "\n\n");
                result += ("id" + "\t" + "Название" + "\n");
                while (await sqlReader2.ReadAsync())
                {
                    result += (Convert.ToString(sqlReader2["id_type"]) + "\t" + Convert.ToString(sqlReader2["name_type"]) + "\n");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader2 != null)
                    sqlReader2.Close();
            }

        // объект для печати
        PrintDocument printDocument = new PrintDocument();

            // обработчик события печати
            printDocument.PrintPage += PrintPageHandler;

            // диалог настройки печати
            PrintDialog printDialog = new PrintDialog();

            // установка объекта печати для его настройки
            printDialog.Document = printDocument;

            // если в диалоге было нажато ОК
            if (printDialog.ShowDialog() == DialogResult.OK)
                printDialog.Document.Print(); // печатаем
            result = "";
        }
        
        // печать строки result
        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            
            e.Graphics.DrawString(result, new Font("Arial", 14), Brushes.Black, 0, 0);
        }

        //info
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Работа выполнена студентом гр.7641 Бызовым Яном", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
