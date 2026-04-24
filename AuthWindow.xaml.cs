using APP_WPF_InstrumentControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace APP_WPF_InstrumentControl
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private Tool_RecordEntities db;
        public static string addressname;
        public AuthWindow()
        {
            InitializeComponent();
            db = new Tool_RecordEntities();

        }
        private void AddingNewAddress(int compid, string adres)
        {
            try
            {
                Address newaddress = new Address()
                {
                    FirmId = compid,
                    Address1 = adres
                };
                db.Addresses.Add(newaddress);
                db.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось добавить адрес.");
                db = new Tool_RecordEntities();
            }
        }
        private bool CheckTextBoxes()
        {
            if(TB_INN.Text.Length < 1 || int.TryParse(TB_INN.Text, out _) == false)
            {
                MessageBox.Show("Некорректный ИНН");
                return false;
            }
            else if(TB_Name.Text.Length < 1)
            {
                MessageBox.Show("Некорректное название организации.");
                return false;
            }
            else if(CB_address.Text.Length < 1)
            {
                MessageBox.Show("Некорректный адрес.");
                return false;
            }
            else
            {
                return true;
            }

        }
        private void BT_Auth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckTextBoxes())
                {
                    Company compexis = db.Companies.FirstOrDefault(c => c.Name == TB_Name.Text && c.INN == TB_INN.Text);
                    Address addrexis = db.Addresses.AsNoTracking().FirstOrDefault(s => s.FirmId == compexis.Id && s.Address1 == CB_address.Text);
                    if (compexis == null)
                    {
                        MessageBox.Show("Такой учётной записи не существует.\nПроверьте корректность внесённых данных.");
                        return;
                    }
                    if (addrexis == null)
                    {
                        AddingNewAddress(compexis.Id, CB_address.Text);
                        addrexis = db.Addresses.First(s => s.FirmId == compexis.Id && s.Address1 == CB_address.Text);
                    }
                    
                    GD.CompID = compexis.Id;
                    GD.AddrID = addrexis.Id;

                    addressname = CB_address.Text;

                    MessageBox.Show("Вы успешно авторизованы!");
                    GD.CompName = compexis.Name;
                    GD.CompINN = compexis.INN;
                    this.Close();
                    MainMenu auth = new MainMenu();
                    auth.ShowDialog();
                }

            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Такой учётной записи не существует.\nПроверьте корректность внесённых данных.");
                db = new Tool_RecordEntities();
            }
        }

        private void BT_Reg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckTextBoxes())
                {
                    Company compexis = db.Companies.FirstOrDefault(c => c.Name == TB_Name.Text && c.INN == TB_INN.Text);
                    if (compexis == null)
                    {
                        Company newcomp = new Company()
                        {
                            Name = TB_Name.Text,
                            INN = TB_INN.Text
                        };
                        db.Companies.Add(newcomp);
                        db.SaveChanges();
                        compexis = db.Companies.First(c => c.Name == TB_Name.Text && c.INN == TB_INN.Text);
                        AddingNewAddress(compexis.Id, CB_address.Text);
                        db.SaveChanges();
                        addressname = CB_address.Text;

                        MessageBox.Show("Вы успешно зарегистрированы");
                        this.Close();
                        MainMenu auth = new MainMenu();
                        auth.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Данная учётная запись уже существует.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось зарегистрироваться.");
                db = new Tool_RecordEntities();
            }
        }
        private void CB_C(object sender, EventArgs ee)
        {
            try
            {
                Company comp = db.Companies.FirstOrDefault(a => a.Name == TB_Name.Text && a.INN == TB_INN.Text);
                CB_address.ItemsSource = db.Addresses.Where(x => x.FirmId == comp.Id).ToList();
                CB_address.DisplayMemberPath = "Address1";
            }
            catch 
            {
                MessageBox.Show("");
                db = new Tool_RecordEntities();
            }
        }


        

        
    }
}
