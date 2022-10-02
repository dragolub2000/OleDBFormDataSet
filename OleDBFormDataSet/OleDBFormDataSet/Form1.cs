using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OleDBFormDataSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void ShowForm()
        {
            try
            {
                OleDbConnection conn = new OleDbConnection();
                conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\testdatabase.xls;Extended Properties=\"Excel 8.0\"";
                String strSQL = "Select * from Radnik2";
                // Kreiranje  OleDbDataAdapter objekta  
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = new OleDbCommand(strSQL, conn);
                // Kreiranje Data Set objekta!
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Radnici");
                DataTable table = ds.Tables[0];
                for (int i = 0; i < table.Rows.Count;i++)
                {
                    this.lvRadnici.Items.Add(table.Rows[i][0].ToString());
                    this.lvRadnici.Items[i].SubItems.Add(table.Rows[i][1].ToString());
                    this.lvRadnici.Items[i].SubItems.Add(table.Rows[i][2].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Postoji izuzetak" + ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowForm();
        }

        private void btnPrikazi_Click(object sender, EventArgs e)
        {
            this.ShowForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection();
                String selectQuery = "Select * from Radnik2";
                string insertQuery = "INSERT INTO Radnik2 (id,Ime,Prezime)  VALUES (@id,@Ime,@Prezime)";
                conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\testdatabase.xls;Extended Properties=\"Excel 8.0\"";
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectQuery, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "Radnici");

                int id = Int32.Parse(txtID.Text);
                string Ime = txtIme.Text;
                string Prezime = txtPrezime.Text;
                // dodavanje nove vrste u tabelu
                DataTable table = ds.Tables[0];
                DataRow newRow = ds.Tables[0].NewRow();
                newRow[0] = id;
                newRow[1] = Ime;
                newRow[2] = Prezime;
                table.Rows.Add(newRow);

                adapter.InsertCommand = new OleDbCommand(insertQuery,conn);
                adapter.InsertCommand.Parameters.AddWithValue("@id", id);
                adapter.InsertCommand.Parameters.AddWithValue("@Ime", Ime);
                adapter.InsertCommand.Parameters.AddWithValue("@Prezime", Prezime);

                // osvezavanje/update  adaptera sa novim DataSource-om ds
                adapter.Update(ds, "Radnici");
                // brisanje svih stavki iz this.lvRadnici.Items
                // a zatim novo prikazivanje listview-a lvRadnici sa novim podacima
                this.lvRadnici.Items.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.lvRadnici.Items.Add(table.Rows[i][0].ToString());
                    this.lvRadnici.Items[i].SubItems.Add(table.Rows[i][1].ToString());
                    this.lvRadnici.Items[i].SubItems.Add(table.Rows[i][2].ToString());
                }
                MessageBox.Show("Podatak dodat!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Postoji izuzetak" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection conn = new OleDbConnection();
                String selectQuery = "Select * from Radnik2";
                string updateQuery = "UPDATE Radnik2 SET ID=@id,Ime=@Ime,Prezime=@Prezime WHERE ID=@id";
                conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\testdatabase.xls;Extended Properties=\"Excel 8.0\"";
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectQuery, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "Radnici");

                int id = Int32.Parse(txtID.Text);
                string Ime = txtIme.Text;
                string Prezime = txtPrezime.Text;
                // update vrste u tabeli
                DataTable table = ds.Tables[0];
                DataRow updatedRow = ds.Tables[0].Rows[this.lvRadnici.SelectedIndices[0]];
                updatedRow[0] = id;
                updatedRow[1] = Ime;
                updatedRow[2] = Prezime;

                adapter.UpdateCommand = new OleDbCommand(updateQuery, conn);
                adapter.UpdateCommand.Parameters.AddWithValue("@id", id);
                adapter.UpdateCommand.Parameters.AddWithValue("@Ime", Ime);
                adapter.UpdateCommand.Parameters.AddWithValue("@Prezime", Prezime);

                // osvezavanje/update  adaptera sa novim DataSource-om ds
                adapter.Update(ds, "Radnici");
                // brisanje svih stavki iz this.lvRadnici.Items
                // a zatim novo prikazivanje listview-a lvRadnici sa novim podacima
                this.lvRadnici.Items.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    this.lvRadnici.Items.Add(table.Rows[i][0].ToString());
                    this.lvRadnici.Items[i].SubItems.Add(table.Rows[i][1].ToString());
                    this.lvRadnici.Items[i].SubItems.Add(table.Rows[i][2].ToString());
                }
                MessageBox.Show("Podatak izmenjen!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Postoji izuzetak" + ex.Message);
            }
        }

        private void lvRadnici_Click(object sender, EventArgs e)
        {
            var item = this.lvRadnici.SelectedItems[0];
            string Ime = item.SubItems[1].Text;
            string Prezime = item.SubItems[2].Text;

            this.txtID.Text = item.SubItems[0].Text;
            this.txtIme.Text = Ime;
            this.txtPrezime.Text = Prezime;
        }
    }
}
