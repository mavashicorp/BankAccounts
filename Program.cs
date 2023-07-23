//Создать консольное приложение на C#, которое позволит пользователям управлять своими банковскими счетами.

//Описание:

//Команда должна состоять из 2-3 человек.
//Создайте публичный репозиторий на GitHub для вашего проекта.
//Разработайте класс BankAccount, который будет представлять банковский счет с полями: номер счета, баланс и владелец счета.
//Создайте класс Bank, который будет отвечать за управление банковскими счетами: открытие новых счетов, пополнение счетов, списание средств, переводы между счетами и вывод информации о счетах.
//Реализуйте консольный интерфейс для взаимодействия с пользователями. Приложение должно предоставлять следующие команды:
//"Создать счет" - создание нового банковского счета с указанием номера счета и имени владельца.
//"Пополнить счет" - пополнение баланса счета по его номеру.
//"Снять со счета" - списание средств со счета по его номеру.
//"Перевести средства" - перевод денег с одного счета на другой с указанием номеров счетов.
//"Показать информацию о счете" - вывод информации о банковском счете по его номеру.
//"Выход" - завершение работы приложения.
//Каждое изменение в состоянии банковских счетов должно быть сохранено в файле (например, в формате JSON) с использованием библиотеки для работы с файлами, такой как System.IO.
//При запуске приложения, оно должно загружать ранее сохраненное состояние счетов из файла (если такой файл есть) и предоставлять возможность продолжить работу с ними.
//Бонусные задания:

//Реализуйте возможность удаления счетов.
//Добавьте проверку на недостаточность средств при списании.
//Обеспечьте валидацию вводимых данных пользователя и информативные сообщения об ошибках.


using System.Security.Principal;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

Bank bank = new();
//Bank bank = new Bank();

bool run = true;
while (run)
{
    Console.Clear();
    Console.WriteLine("\t------------Банк меню------------");
    Console.WriteLine("\t1.Создать счет                   ");
    Console.WriteLine("\t2.Пополнить счет                 ");
    Console.WriteLine("\t3.Перевести средства             ");
    Console.WriteLine("\t4.Показать информацию о счете    ");
    Console.WriteLine("\t5.Выход                          ");


    int answer;//проверка что бы ввели число и его ввод
    if (!int.TryParse(Console.ReadLine(), out answer))
    {
        Console.WriteLine("Ошибка: введите число.");
        continue;
    }

    switch (answer)
    {
        case 1:
            Console.WriteLine("Введите номер счета: ");
            int accountnumber = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите имя владельца счета: ");
            string accountholder = Console.ReadLine();
            bank.createAnAccount(accountnumber, accountholder);
            break;

        case 2:
            Console.WriteLine("Введите номер счета: ");
            accountnumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите сумму для пополнения: ");


            if (!double.TryParse(Console.ReadLine(), out double deposit))
            {
                Console.WriteLine("Ошибка: введите число.");
                continue;
            }
            bank.topUpYourAccount(accountnumber,deposit);
            Console.WriteLine("Счет успешно пополнен");
            break;
        case 3:
            bank.transferFunds();
            break;
        case 4:
            Console.Write("Введите номер счета: ");
            accountnumber = Convert.ToInt32(Console.ReadLine());
            bank.showAccountInformation(accountnumber);
            break;
        case 5:
            Environment.Exit(0); //команда закрытия приложения 
            break;
    
    }


}







class BankAccount //банковский счет
{
    public int AccountNumber { get;  set; }//номер счета
    public double Balance { get; set; }//баланс
    public string AccountHolder { get; set; }//владелец счета
};

class Bank //управляет банковскими счетами
{
    private List<BankAccount> accounts;
    private const string dataFilePath = "bank_accounts.json";


    public Bank()//конструктор по умолчанию
    {
        accounts = new List<BankAccount>();
        LoadDataFromFile();
    }

    private void LoadDataFromFile()//загрузка с файла
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            accounts = JsonConvert.DeserializeObject<List<BankAccount>>(jsonData);
        }
    }

    private void SaveDataToFile()//сохранение в файл
    {
        string jsonData = JsonConvert.SerializeObject(accounts);
        File.WriteAllText(dataFilePath, jsonData);
    }


    //создать счет
    public void createAnAccount(int accountnumber, string accountholder)
    {
        BankAccount Account = new BankAccount() {AccountNumber = accountnumber, AccountHolder = accountholder, Balance = 0  };
        accounts.Add(Account);
        SaveDataToFile();
    }

    //пополнить счет
    public void topUpYourAccount(int accountnumber, double deposit) 
    {
        BankAccount account = GetAccount(accountnumber);
        if (account != null)
        {
            account.Balance += deposit;
            SaveDataToFile();
        }
    }

    //перевести средства
    public void transferFunds() { }


    //показать информацию о счете
    public void showAccountInformation(int accountnumber) 
    {
        BankAccount account = GetAccount(accountnumber);
        if (account != null)
        {
            Console.WriteLine($"Номер счета: {account.AccountNumber}");
            Console.WriteLine($"Баланс: {account.Balance}");
            Console.WriteLine($"Владелец счета: {account.AccountHolder}");
        }
        else
        {
            Console.WriteLine("Счет не найден...");
        }
    }

    public BankAccount GetAccount(int accountnumber)
    { 
        return accounts.Find(a=> a.AccountNumber == accountnumber);
    }

};

