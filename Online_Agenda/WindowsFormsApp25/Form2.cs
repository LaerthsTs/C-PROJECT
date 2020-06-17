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

namespace WindowsFormsApp25
{
    public partial class Form2 : Form
    {
        
        //Dhmiourgia Database
        static DataSet1 db;
        protected static DataSet1 App
        {
            get
            {
                if (db == null)
                    db = new DataSet1();
                return db;
            }
        }
        public Form2()
        {
            InitializeComponent();
        }

        private void bnew_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = true; //otan patithei to new to panel tha mporei na patithei
                App.AddressBook.AddAddressBookRow(App.AddressBook.NewAddressBookRow()); //dimiourgei mia kainourgia stilei sto database gia thn kainourgia ekxwrish
                addressBookBindingSource.MoveLast(); //h ekxwrhsh paei teleftea
                txtname.Focus(); //H methodos focus epistrefh true mono otan ginete swsto focus entry
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.AddressBook.RejectChanges();
            }
        }

        private void bedit_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            txtname.Focus();
        }

        private void bcancel_Click(object sender, EventArgs e)
        {
            addressBookBindingSource.ResetBindings(false); //epanafora desmevmenwn gia thn enhmerwsh plaisiwn keimenou
            panel1.Enabled = false;
        }

        private void bsave_Click(object sender, EventArgs e)
        {
            try
            {
                addressBookBindingSource.EndEdit(); //h alages dedomenwn antigrafonte sthn sugekrimenh grammh pou exei dimiourgithei sthn vasei amesws meta apo kathe allagh otan patame to save button
                App.AddressBook.AcceptChanges(); //dexete tis alages
                App.AddressBook.WriteXml(string.Format("{0}//data.dat", Application.StartupPath));
                panel1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.AddressBook.RejectChanges();
            }
        }

        private void txtsrc_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are You Sure?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    addressBookBindingSource.RemoveCurrent();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            string fileName = string.Format("{0}//data.dat", Application.StartupPath);
            if (File.Exists(fileName))
                App.AddressBook.ReadXml(fileName);
            addressBookBindingSource.DataSource = App.AddressBook;
            panel1.Enabled = false;
            string s = "dd-MM";
            DataRow[] foundrow;
            foundrow = db.Tables["AddressBook"].Select("DBO Like '%"+DateTime.Today.ToString("dd-MM")+ "%'");
            
            if(foundrow != null)
            {
                
                MessageBox.Show(foundrow.Length.ToString());
            
            }
            else
            {
                MessageBox.Show("Kaneis den exei genethlia");
            }

        }

        private void txtsrc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (!string.IsNullOrEmpty(txtsrc.Text))
                {
                    var query = from o in App.AddressBook
                                where o.Name == txtsrc.Text || o.Surname == txtsrc.Text || o.Phone.Contains(txtsrc.Text)
                                select o;
                    dataGridView1.DataSource = query.ToList();
                }
                else
                    dataGridView1.DataSource = addressBookBindingSource;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

         public class Person
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Phone { get; set; }
            
            public Person(string a,string b,string c)
            {
                Name = a;
                Surname = b;
                Phone = c;
                
            }
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd-MM";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
