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
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private int UpdatingInstrumentId;
        private Tool_RecordEntities db;
        private static List<Instrument> instrumentList = new List<Instrument>();
        private static List<Instrument> filteredList = new List<Instrument>();
        public MainMenu()
        {
            db = new Tool_RecordEntities();
            InitializeComponent();
            FillDataList();

            TB_Head.Text = $"Перечень инструментов по адресу: {AuthWindow.addressname}"; 
        }
        private void FillDataList()
        {
            //dataGridWorkouts.ItemsSource = null;
            try
            {
                instrumentList = db.Instruments.Where(s => s.FirmId == GD.CompID && s.AddressId == GD.AddrID).ToList();
                DG_Instruments.ItemsSource = instrumentList;
                CB_Category.ItemsSource = instrumentList.Select(i => i.Category).Distinct();
                CB_Type.ItemsSource = instrumentList.Select(i => i.Type).Distinct();
                CB_Index.ItemsSource = instrumentList.Select(i => i.Index).Distinct();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                db = new Tool_RecordEntities();
            }
        }
        private void ClearTextBox()
        {
            try
            {
                TB_Name.Clear();
                TB_Type.Clear();
                TB_Category.Clear();
                TB_Index.Clear();
                FillDataList();
                TB_Name.Text = "Название";
                TB_Category.Text = "Категория";
                TB_Type.Text = "Тип инструмента";
                TB_Index.Text = "Инвентарный номер";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                db = new Tool_RecordEntities();
            }
        }
        private void DG_SelectedInstrument(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                Instrument selectedInstrument = (Instrument)DG_Instruments.SelectedItem;

                if (selectedInstrument != null)
                {
                    UpdatingInstrumentId = selectedInstrument.Id;

                    TB_Name.Text = selectedInstrument.Name; //(int)selectedInstrument.Times;
                    TB_Category.Text = selectedInstrument.Category.ToString();
                    TB_Type.Text = selectedInstrument.Type.ToString();
                    TB_Index.Text = selectedInstrument.Index.ToString();
                }
                FillDataList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                db = new Tool_RecordEntities();
            }
            //как конвертировать переменную из типа  int? в int

        }
        private void ButtonDeleteInstrument_Click(object sender, RoutedEventArgs e)
        {
            Instrument selectedInstrument = (Instrument)DG_Instruments.SelectedItem;
            try
            {
                db.Instruments.Remove(selectedInstrument);
                ToolLogger.OnToolDeleted(selectedInstrument);
                db.SaveChanges();
                MessageBox.Show("Вы удалили инструмент");
                FillDataList();
                ClearTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                FillDataList();
                db = new Tool_RecordEntities();
            }
        }


        private void dataGridInstruments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //не используется
            try
            {
                FillDataList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                db = new Tool_RecordEntities();
            }
        }

        private void AddInstrument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TB_Name.Text.Length < 1)
                {
                    MessageBox.Show("Неверное название инструмента.");
                }
                else if (TB_Category.Text.Length < 1)
                {
                    MessageBox.Show("Неверная категория инструмента");
                }
                else if (TB_Type.Text.Length < 1)
                {
                    MessageBox.Show("Неверный тип инструмента");
                }
                else if (TB_Index.Text.Length == 0 || instrumentList.Count(s => s.Index == TB_Index.Text) > 0)
                {
                    MessageBox.Show("Неверный инвентарный номер");
                }
                else
                {
                    Instrument newInstrument = new Instrument()
                    {
                        FirmId = GD.CompID,
                        AddressId = GD.AddrID,
                        Name = TB_Name.Text,
                        Category = TB_Category.Text,
                        Type = TB_Type.Text,
                        Index = TB_Index.Text
                    };

                    db.Instruments.Add(newInstrument);
                    ToolLogger.OnToolAdded(newInstrument);
                    FillDataList();
                    db.SaveChanges();
                    MessageBox.Show("Вы добавили инструмент");
                    ClearTextBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                db = new Tool_RecordEntities();
            }

        }
        private void EditInstrument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TB_Name.Text.Length < 2)
                {
                    MessageBox.Show("Неверное название инструмента");
                }
                if (TB_Category.Text.Length < 1)
                {
                    MessageBox.Show("Неверная категория инструмента");
                }
                if (TB_Type.Text.Length < 1 )
                {
                    MessageBox.Show("Неверный тип инструмента");
                }
                if (TB_Index.Text.Length <1)
                {
                    MessageBox.Show("Неверный индекс инструмента");
                }
                else
                {

                    Instrument updatingInstrument = db.Instruments.FirstOrDefault(s => s.Id == UpdatingInstrumentId);
                    Instrument tool1 = updatingInstrument;
                    updatingInstrument.Name = TB_Name.Text;
                    updatingInstrument.Category = TB_Category.Text;
                    updatingInstrument.Type = TB_Type.Text;
                    updatingInstrument.Index = TB_Index.Text;
                    Instrument tool2 = updatingInstrument;
                    db.SaveChanges();
                    ToolLogger.OnToolUpdated(tool1, tool2); 
                    FillDataList();
                    ClearTextBox();

                    UpdatingInstrumentId = 0;

                    MessageBox.Show("Упражнение изменено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Исключение типа: {ex.Message}");
                db = new Tool_RecordEntities();
            }
        }
        private void BT_Filter_Click(object sender, RoutedEventArgs e)
        {
            filteredList = instrumentList.ToList();
            if (CB_Category.Text.Length > 0)
            {
                filteredList = filteredList.Where(f => f.Category == CB_Category.Text).ToList();
            }
            if (CB_Type.Text.Length > 0)
            {
                filteredList = filteredList.Where(f => f.Category == CB_Type.Text).ToList();
            }
            if (CB_Index.Text.Length > 0)
            {
                filteredList = filteredList.Where(f => f.Category == CB_Index.Text).ToList();
            }

            DG_Instruments.ItemsSource = filteredList;
        }

        private void BT_ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            CB_Index.Text = "";
            CB_Category.Text = "";
            TB_Type.Text = "";

            DG_Instruments.ItemsSource = instrumentList;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TB_Name_GotFocus(object sender, RoutedEventArgs e)
        {
            if(TB_Name.Text == "Название") { TB_Name.Clear(); }
        }

        private void TB_Category_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Category.Text == "Категория") { TB_Category.Clear(); }
        }

        private void TB_Type_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Type.Text == "Тип инструмента") { TB_Type.Clear(); }
        }

        private void TB_Index_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Index.Text == "Инвентарный номер") { TB_Index.Clear(); }
        }

        private void TB_Index_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Index.Text.Length < 1) { TB_Index.Text = "Инвентарный номер"; }
        }

        private void TB_Type_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Type.Text.Length < 1) { TB_Type.Text = "Тип инструмента"; }
        }

        private void TB_Category_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Category.Text.Length < 1) { TB_Category.Text = "Категория"; }
        }

        private void TB_Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TB_Name.Text.Length < 1) { TB_Name.Text = "Название"; }
        }

        private void BT_Auth_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AuthWindow window = new AuthWindow();
            window.TB_Name.Text = GD.CompName;
            window.TB_INN.Text = GD.CompINN;
            window.ShowDialog();
            
        }

        
    }
}
