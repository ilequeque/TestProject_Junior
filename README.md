# TestProject_Junior
1. найти и исправить следующие баги:
- в карточке пациента можно добавить дату рождения в будущем
```
  Проблема с добавлением даты рождения в будущем может быть решена путем добавления проверки на допустимость даты рождения перед добавлением или модификацией карточки пациента. 
Добавление проверки "if (dateOfBirthValue > DateTime.Now)" перед созданием или модификацией карточки пациента, чтобы убедиться, что дата рождения не находится в будущем. 
Если дата рождения в будущем, он отображает ошибку и не выполняет действие.
```

- при вводе даты рождения 00.00.0001 приложение падает
```
Для исправления этой ошибки вам нужно добавить проверку на допустимость даты рождения перед ее использованием.
if (dateOfBirth == null || dateOfBirth.Value.Year < 1900)
{
    txtNotify.Foreground = Brushes.Red;
    txtNotify.BorderBrush = Brushes.Red;
    txtNotify.Content = "Введите корректную дату рождения!";
    return; // Выход из метода, не выполняя добавление
}
Этот код проверяет, что дата рождения не равна null и что год даты рождения больше или равен 1900. 
Если дата рождения не соответствует этим условиям, он отображает ошибку и предотвращает попытку использования недопустимой даты.
```

- при редактировании существующего обращения пациента, приложение падает
```
Для решения проблемы с падением приложения при редактировании существующего обращения пациента, следует убедиться, что перед редактированием запроса у вас есть существующая запись в базе данных для этого запроса. 
Также, нужно обработать исключение, если запись не существует.
```

2. на форму со списком пациентов добавить столбец «Возраст», в котором соответственно необходимо вывести полный возраст пациента на текущей день
добавил код для добавления поля "Возраст" в классе PatientCard.cs:
```
public int Age
        {
            get
            {
                if (DateOfBirth != null)
                {
                    DateTime currentDate = DateTime.Now;
                    int age = currentDate.Year - DateOfBirth.Year;
                    if (currentDate < DateOfBirth.AddYears(age))
                    {
                        age--;
                    }

                    return age;
                }
                return 0; // В случае отсутствия даты рождения
            }
        }
```
а для показа возраста пациента в приложении использовал в ListOfPatientsPage.xaml:
```
<DataGridTextColumn x:Name="ageColumn" Binding="{Binding Age}" Header="Возраст" Width="80"/>
```
