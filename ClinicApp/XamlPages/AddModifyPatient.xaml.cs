using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ClinicApp.EntityModels;
using System.Configuration;
using System.Data.SqlClient;

namespace ClinicApp.XamlPages
{
    /// <summary>
    /// Interaction logic for AddModifyPatient.xaml
    /// </summary>
    public partial class AddModifyPatient : Page
    {
        //this constructor launches only on adding mode
        public AddModifyPatient()
        {
            InitializeComponent();
            isModifyingMode = false;
        }
        //this constructor launches only on modifying mode
        public AddModifyPatient(PatientCard cardToModify, Frame main)
        {
            InitializeComponent();
            m_cardToModify = cardToModify;
            InitializeFields(cardToModify);
            isModifyingMode = true;
            mainFrame = main;
        }
        //local ref to the modifying patient card
        private PatientCard m_cardToModify;
        //if the page has been launched in order to modify a card, isModifyingMode is true
        private Boolean isModifyingMode;
        //the main frame which represents all the forms in the app
        private Frame mainFrame;


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerBirth.DisplayDateStart = DateTime.Now.AddYears(-100);
            datePickerBirth.DisplayDateEnd = DateTime.Now;
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                btn.IsEnabled = false;
                txtNotify.Content = "";
                //the message will notify the user if the operation has been done successfully
                StringBuilder messageBuilder = new StringBuilder();
                Boolean status = true;
                //get all the entered values from the UI
                var name = txtBoxName.Text;
                var phone = mskTxtPhone.Value;
                var dateOfBirth = datePickerBirth.SelectedDate;
                var address = txtBoxAddress.Text;
                var gender = (Gender)Enum.Parse(typeof(Gender),
                    ((ComboBoxItem)comboBlockGender.SelectedValue).Tag.ToString());
                #region Checking entered name
                if (!name.IsNameValid())
                {
                    status = false;
                    messageBuilder.Append("Введите ФИО корректно, \nнапример: Иванов Иван Иванович.\n");
                }
                #endregion
                #region Checking entered date of birth
                if (dateOfBirth == null)
                {
                    status = false;
                    messageBuilder.Append("Дата рождения - обязательное поле для ввода.\n");
                }
                #endregion
                #region Checking entered date of birth
                if (dateOfBirth == null || dateOfBirth.Value.Year < 1900)
                {
                    txtNotify.Foreground = Brushes.Red;
                    txtNotify.BorderBrush = Brushes.Red;
                    status = false;
                    messageBuilder.Append("Введите корректную дату рождения.\n");
                }
                #endregion
                #region Checking entered address
                if (String.IsNullOrEmpty(address))
                {
                    status = false;
                    messageBuilder.Append("Адрес - обязательное поле для ввода.\n");

                }
                #endregion
                #region Checking entered phone number
                if (phone == null)
                {
                    status = false;
                    messageBuilder.Append("Номер телефона - обязателен для ввода.");
                }
                #endregion
                #region Submitting the patient
                if (status)
                {
                    ClinicDataRepository repository = new ClinicDataRepository();
                    #region adding mode
                    if (!isModifyingMode)
                    {
                        var dateOfBirthValue = dateOfBirth.GetValueOrDefault();

                        // Проверка на допустимость даты рождения
                        if (dateOfBirthValue > DateTime.Now)
                        {
                            // Дата рождения находится в будущем, обработайте эту ошибку
                            txtNotify.Foreground = Brushes.Red;
                            txtNotify.BorderBrush = Brushes.Red;
                            txtNotify.Content = "Дата рождения не может быть в будущем!";
                            return; // Выход из метода, не выполняя добавление
                        }

                        var patientCard = new PatientCard()
                        {
                            Name = name,
                            PhoneNumber = phone.ToString(),
                            DateOfBirth = dateOfBirthValue,
                            Address = address,
                            Gender = gender
                        };
                        var success = repository.AddPatientCard(patientCard);
                        if (await success)
                        {
                            txtNotify.Foreground = Brushes.Black;
                            txtNotify.BorderBrush = Brushes.LimeGreen;
                            txtNotify.Content = "Новая карта успешно добавлена";
                            ClearTheUI();
                        }
                    }
                    #endregion
                    #region modifying mode
                    else
                    {
                        m_cardToModify.Name = name;
                        m_cardToModify.PhoneNumber = phone.ToString();
                        m_cardToModify.DateOfBirth = dateOfBirth.GetValueOrDefault();
                        m_cardToModify.Address = address;
                        m_cardToModify.Gender = gender;
                        repository.ModifyPatientCard(m_cardToModify);
                        mainFrame.GoBack();
                    }
                }
                #endregion
            }
            #region catch, finaly
            catch (SqlException)
            {
                var err = ConfigurationManager.AppSettings["dbError"].ToString();
                MessageBox.Show(err, "Ошибка");
                throw;
            }
            catch
            {
                MessageBox.Show("Что то пошло не так, приложение будет закрыто", "Ошибка");
                throw;
            }
            finally
            {
                //activate the submit button
                btnSubmit.IsEnabled = true;
            }
            #endregion
        }

        private void ClearTheUI()
        {
            txtBoxName.Text = "";
            mskTxtPhone.Value = null;
            datePickerBirth.SelectedDate = null;
            txtBoxAddress.Text = "";
        }
        private void InitializeFields(PatientCard cardToModify)
        {
            txtBoxName.Text = cardToModify.Name;
            mskTxtPhone.Value = cardToModify.PhoneNumber;
            datePickerBirth.SelectedDate = cardToModify.DateOfBirth;
            txtBoxAddress.Text = cardToModify.Address;
            comboBlockGender.SelectedIndex = (Int32)cardToModify.Gender;
        }
    }
}
#endregion