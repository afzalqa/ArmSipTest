using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Text; // ����� using ��������� ����������� System.Text, ������� �������� ����� StringBuilder. ���� ����� ����� ��� ��������� ������

namespace TestProject1
{
    class LoginGenerator // ���������� ����� LoginGenerator
    {
        private static readonly Random random = new Random(); // ������� ����������� ���� random, ������� ������������ ��� ��������� ��������� ��������
        // pravte ��������, ��� ���� �� ����� ���� ������������ �� ��������� ������ LoginGenerator
        // readonly - ��������, ��� ��� �������� ����� ���� ������������ ���� ��� � �� ����� ���� �������� ����� �����

        public static string GenerateLogin(int length) // ����� GenerateLogin ��� ��������� ������, ��������� �������� length, ������� ���������� ����� ������
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";// ������� ��������� chars, ������� �������� ��� ��������� �����, ���������� ��� ������ ��� ���������
            var builder = new StringBuilder(length); // ������� ��������� ������ StringBuilder, ������� ���������� ��� ���������� ������ ������
            // �������� �������� length � �����������, ������� ��������� ��������� ������� StringBuilder. ��� ��������� ������� ��������������� ����������� ���-�� ������ ��� ���������� ������
            for (int i = 0; i < length; i++) // ������� ���� for, ������� ����� ����������� length ���
            {
                builder.Append(chars[random.Next(chars.Length)]); // ���������� ���������� ����� ������. random.Next(chars.Length) ���������� ��������� ����� �� 0 �� ����� ������ chars
                // ��� ����� ����� ������������ ��� ��������� ����� �� ������ chars
            }
            return builder.ToString(); // ����� ����, ��� ���� ��������, ���������� ������ ������, ������� �� ��������� � ������� StringBuilder
        }
    }
        
    class Program
    {
        public static string GenerateRandomEmail()
        {
            var random = new Random();

            // ���������� ��������� ��� �� ������� "user_{��������� �����}@mail.ru"
            string username = "user_" + random.Next(1000, 9999);
            string domain = "mail.ru";

            // �������� email �� ����� ������������ � ������
            return username + "@" + domain;
        }
    }

    public class SnilsGenerator // ����������� ������ SnilsGenerator, ������� ����� ������������ �����
    {
        private Random random; //��������� ��������� ���������� "random" ���� Random, ������� ����� �������������� ��� ��������� ��������� �����

        public SnilsGenerator() // ����������� ������ SnilsGenerator, ������� �������������� ���������� "random" ��� �������� ���������� ������
        {
            random = new Random();
        }

        public string GenerateSnils()
        {
            int[] snilsArray = new int[9]; // ��������� ������ snilsArray �� 9 ����� �����, ������� ����� �������������� ��� �������� ������ 9 ���� �����
            for (int i = 0; i < 9; i++) // ���� for, ������� �������� �� ������� �������� ������� snilsArray
            {
                snilsArray[i] = random.Next(0, 9); // ��������� ���������� ����� �� 0 �� 9 � ��� ���������� �������� i ������� snilsArray
            }

            int sum = snilsArray[8] * 1 +  // ����������� ����������� ����� �����. �������� ������ ����� � ������� snilsArray �� �������������� ���
                      snilsArray[7] * 2 +  // (1,2,3 � �.�.) ����� ��������� ��� ��� ������������
                      snilsArray[6] * 3 +
                      snilsArray[5] * 4 +
                      snilsArray[4] * 5 +
                      snilsArray[3] * 6 +
                      snilsArray[2] * 7 +
                      snilsArray[1] * 8 +
                      snilsArray[0] * 9;
            int checkDigit = sum % 101; // ��������� ������� �� ������� ����� �� 101. ������� ����� ����������� ������ �����
            if (checkDigit == 100) // ���� ����������� ����� ����� 100, �� ����������� ����� ����� ����� 0
            {
                checkDigit = 0;
            }
            // ��������������� �������� ����� � ������� XXX-XXX-XXX YY. ����������� ����� ������������ ���������� 0, ���� ��� ������ 10
            return $"{snilsArray[0]}{snilsArray[1]}{snilsArray[2]}-{snilsArray[3]}{snilsArray[4]}{snilsArray[5]}-{snilsArray[6]}{snilsArray[7]}{snilsArray[8]} {checkDigit:D2}";
        }
    }

    public class Tests
    {
        private IWebDriver driver; // ��������� ���������� driver

        [SetUp] // ��, ��� ���������� �� ������
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://eskso2.sapfir.corp:17155/login");
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);
        }

        [Test]
        //[Order(1)]
        public void authValidData()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/administration", driver.Url);

        }

        [Test]
        //[Order(2)]
        public void authInvalidPassword()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("qwerty");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("password" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }

        [Test]
        //[Order(3)]
        public void authBlockedUser()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("blockadmin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }

        [Test]
        //[Order(4)]
        public void authClearLogin()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("login");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//span[@class=\"dx-icon dx-icon-clear\"]")).Click();
        }

        [Test]
        //[Order(5)]
        public void authClearPassword()
        {
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("password");
            Thread.Sleep(1000);

            IList<IWebElement> elements = driver.FindElements(By.CssSelector("span.dx-icon.dx-icon-clear")); // ������� �������� CssSelector
            Assert.That(elements.Count >= 2, "Expected at least 2 elements with class 'dx-icon dx-icon-clear'"); // ��������������, ��� �� ������ ��� ����� 2

            IWebElement secondElement = elements[1]; // ����������� ������ ������� ���������� secondElement
            secondElement.Click();
        }

        [Test]
        //[Order(6)]
        public void authUppercasedata()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("ADMIN");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("ASDF1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }

        [Test]
        //[Order(7)]
        public void authEmptyFields()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("��������� ��� ����"));
        }

        [Test]
        //[Order(8)]
        public void authReverseData()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("asdf1234");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("admin" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }

        [Test]
        //[Order(9)]
        public void authLoginWithSpace()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin ");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement errorMessage = driver.FindElement(By.ClassName("dx-toast-message"));
            Assert.IsTrue(errorMessage.Text.Contains("������������ ����� ��� ������"));
        }

        [Test]
        public void checkAdminPartition()
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);

            // ���� ��� �������� �� �������� � ���������� XPath
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath("//a[@test-id=\"menuitem-requests-portal\"] | //a[@test-id=\"menuitem-requests-cabinet\"] " +
                "| //a[@test-id=\"menuitem-requests-callCenter\"] | //a[@test-id=\"menuitem-requests-socialNet\"] | //a[@test-id=\"menuitem-report\"] | " +
                "//a[@test-id=\"menuitem-administration\"] | //a[@test-id=\"menuitem-administration\"] | //a[@test-id=\"menuitem-profile\"] | //a[@test-id=\"menuitem-logout\"]"));

            // ���������, ��� ���������� ��������� ��������� ������������� ����������
            Assert.AreEqual(8, elements.Count);

            // ���������� �� ��������� ��������� � ���������, ��� ������ �� ��� ������������ �� ��������
            foreach (IWebElement element in elements)
            {
                Assert.IsTrue(element.Displayed);
            }

        }

        [Test]
        public void checkCabinetPartition()
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("operator_lk");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);

            // ���� ��� �������� �� �������� � ���������� XPath
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath("//a[@test-id=\"menuitem-requests-cabinet\"] | //a[@test-id=\"menuitem-profile\"] | //a[@test-id=\"menuitem-logout\"]"));

            // ���������, ��� ���������� ��������� ��������� ������������� ����������
            Assert.AreEqual(3, elements.Count);

            // ���������� �� ��������� ��������� � ���������, ��� ������ �� ��� ������������ �� ��������
            foreach (IWebElement element in elements)
            {
                Assert.IsTrue(element.Displayed);
            }

        }

        [Test]
        public void inaccessibleSectionsForPortal()
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("operator_p");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);

            // ���� ��� �������� �� �������� � ���������� XPath
            ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath("//a[@test-id=\"menuitem-requests-cabinet\"] | " +
                "//a[@test-id=\"menuitem-requests-callCenter\"] | //a[@test-id=\"menuitem-requests-socialNet\"]"));

            // ���������, ��� ���������� ��������� ��������� ������������� ����������
            Assert.AreEqual(0, elements.Count);

            // ���������� �� ��������� ��������� � ���������, ��� ������ �� ��� �� ������������ �� ��������
            foreach (IWebElement element in elements)
            {
                Assert.IsFalse(element.Displayed);
            }

        }


        [Test]
        public void createPortalUser() 
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement createUser = driver.FindElement(By.XPath("//div[@test-id=\"add-user-button\"]"));
                createUser.Click();

            IWebElement inputLogin = driver.FindElement(By.XPath("//input[@test-id=\"login\"]"));
            string generateLogin = LoginGenerator.GenerateLogin(8); // ���������� ����� ������ 8 ��������
            inputLogin.SendKeys(generateLogin);// �������� ����� SendKeys � �������� ��� ��������������� �����, ��������� ����� GenerateLogin ������ LoginGenerator
            Thread.Sleep(1000);
            Program program = new Program();
            string email = Program.GenerateRandomEmail();
            IWebElement inputEmail = driver.FindElement(By.XPath("//input[@test-id=\"e-mail\"]"));
            inputEmail.SendKeys(email);

            IWebElement inputSurname = driver.FindElement(By.XPath("//input[@test-id=\"surname\"]"));
            inputSurname.SendKeys("������");
            IWebElement inputFirstName = driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]"));
            inputFirstName.SendKeys("�������");
            IWebElement inputMiddleName = driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]"));
            inputMiddleName.SendKeys("����");
            IWebElement inputPassword = driver.FindElement(By.XPath("//input[@test-id=\"password\"]"));
            inputPassword.SendKeys("asdf1234");
            IWebElement confirmPassword = driver.FindElement(By.XPath("//input[@test-id=\"confirmPassword\"]"));
            confirmPassword.SendKeys("asdf1234");
            driver.FindElement(By.XPath("//div[@test-id=\"operatorPortal\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//div[@test-id=\"confirm-button\"]")).Click();
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/administration", driver.Url);
        }

        [Test]
        public void createPortalAdminUser()
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement createUser = driver.FindElement(By.XPath("//div[@test-id=\"add-user-button\"]"));
            createUser.Click();

            IWebElement inputLogin = driver.FindElement(By.XPath("//input[@test-id=\"login\"]"));
            string generateLogin = LoginGenerator.GenerateLogin(4);
            inputLogin.SendKeys(generateLogin);

            Program program = new Program();
            string email = Program.GenerateRandomEmail();
            IWebElement inputEmail = driver.FindElement(By.XPath("//input[@test-id=\"e-mail\"]"));
            inputEmail.SendKeys(email);

            IWebElement inputSurname = driver.FindElement(By.XPath("//input[@test-id=\"surname\"]"));
            inputSurname.SendKeys("�����");
            IWebElement inputName = driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]"));
            inputName.SendKeys("�������");
            IWebElement inputMiddleName = driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]"));
            inputMiddleName.SendKeys("�����������");
            IWebElement inputPassword = driver.FindElement(By.XPath("//input[@test-id=\"password\"]"));
            inputPassword.SendKeys("asdf1234");
            IWebElement confirmPassword = driver.FindElement(By.XPath("//input[@test-id=\"confirmPassword\"]"));
            confirmPassword.SendKeys("asdf1234");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@test-id=\"operatorPortal\"]")).Click();
            driver.FindElement(By.XPath("//div[@test-id=\"isAdmin\"]")).Click();
            driver.FindElement(By.XPath("//div[@test-id=\"confirm-button\"]")).Click();
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/administration", driver.Url);

        }

        [Test]
        public void cancelCreateUser()
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement createUser = driver.FindElement(By.XPath("//div[@test-id=\"add-user-button\"]"));
            createUser.Click();

            IWebElement inputLogin = driver.FindElement(By.XPath("//input[@test-id=\"login\"]"));
            inputLogin.SendKeys("qwerty");
            IWebElement inputEmail = driver.FindElement(By.XPath("//input[@test-id=\"e-mail\"]"));
            inputEmail.SendKeys("qwerty@mail.ru");
            IWebElement inputSurname = driver.FindElement(By.XPath("//input[@test-id=\"surname\"]"));
            inputSurname.SendKeys("��������");
            IWebElement inputName = driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]"));
            inputName.SendKeys("������");
            IWebElement inputMiddleName = driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]"));
            inputMiddleName.SendKeys("����������");
            IWebElement inputPassword = driver.FindElement(By.XPath("//input[@test-id=\"password\"]"));
            inputPassword.SendKeys("asdf1234");
            IWebElement confirmPassword = driver.FindElement(By.XPath("//input[@test-id=\"confirmPassword\"]"));
            confirmPassword.SendKeys("asdf1234");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@test-id=\"operatorPortal\"]")).Click();
            driver.FindElement(By.XPath("//div[@test-id=\"cancel-button\"]")).Click();
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/administration", driver.Url);

        }

        [Test]
        public void createUserWithCyrillicLogin() // �������� ����, �� ������� ������������� ���������
        {
            IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            Thread.Sleep(1000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement createUser = driver.FindElement(By.XPath("//div[@test-id=\"add-user-button\"]"));
            createUser.Click();

            IWebElement inputLogin = driver.FindElement(By.XPath("//input[@test-id=\"login\"]"));
            inputLogin.SendKeys("��������_�����" + Keys.Enter);
            Thread.Sleep(1000);

            IWebElement errorMessageElement = driver.FindElement(By.XPath("//div[@class=\"dx-overlay-content dx-invalid-message-content dx-resizable\"]"));
            string errorMessageText = errorMessageElement.Text;
            Assert.AreEqual("��������� ������ ��������� ����� � �����", errorMessageText);
        }

        [Test]
        public void createUserWithLatinName()
        {
              IWebElement Authlogin = driver.FindElement(By.Name("login"));
            Authlogin.SendKeys("admin");
            Thread.Sleep(1000);
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement createUser = driver.FindElement(By.XPath("//div[@test-id=\"add-user-button\"]"));
            createUser.Click();

            IWebElement inputSurname = driver.FindElement(By.XPath("//input[@test-id=\"surname\"]"));
            inputSurname.SendKeys("Warner");
            IWebElement inputName = driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]"));
            inputName.SendKeys("James");
            IWebElement inputMiddleName = driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]"));
            inputMiddleName.SendKeys("Bob" + Keys.Enter);
            Thread.Sleep(1000);

            ReadOnlyCollection<IWebElement> errorMessageElements = driver.FindElements(By.XPath("//div[contains(text(), '��������� ������ ���������, ����� � ������')]"));
            Assert.AreEqual(3, errorMessageElements.Count);
        }

        [Test] 
        public void createCallCentrAppeal()
        {
            SnilsGenerator snilsGenerator = new SnilsGenerator(); // ������� ����� ��������� ������ 'SnilsGenerator' � ���������� ����� �����
            string snils = snilsGenerator.GenerateSnils(); // ��������� ��������������� ����� � ���������� snils

            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement callCenter = driver.FindElement(By.PartialLinkText("����-�����"));
                callCenter.Click();
            IWebElement callClick = driver.FindElement(By.ClassName("dx-button-text"));
                callClick.Click();
            IWebElement inputCardNum = driver.FindElement(By.ClassName("dx-texteditor-input"));
                inputCardNum.SendKeys(snils + Keys.Enter); // ���������� snils � �������� ��������� ������ SendKeys � �������� ����� GenerateSnils
            Thread.Sleep(1000);
            IWebElement continueButton = driver.FindElement(By.XPath("//div[@test-id=\"select-button\"]"));
                continueButton.Click();
            driver.FindElement(By.XPath("//input[@test-id=\"surname\"]")).SendKeys("������");
            driver.FindElement(By.XPath("//input[@test-id=\"firstName\"]")).SendKeys("����");
            driver.FindElement(By.XPath("//input[@test-id=\"middleName\"]")).SendKeys("��������");
            /*WebElement cardNum = (WebElement)driver.FindElement(By.XPath("//input[@test-id=\"card-number\"]"));
            cardNum.Clear();
            var myString = "9620812001307270";
            for (int i = 0; i < myString.Length; i++)
            {
                cardNum.SendKeys(myString[i].ToString());
            };
            cardNum.SendKeys("9620812001307270");*/
            driver.FindElement(By.XPath("//div[@class=\"dx-dropdowneditor-icon\"]")).Click();
            driver.FindElement(By.XPath("//div[@class=\"dx-item-content dx-list-item-content\"]")).Click();
            IWebElement characterDropDownList = driver.FindElement(By.XPath("//div[@test-id='tone-select']"));
            characterDropDownList.Click();
            IWebElement characterClick = driver.FindElement(By.XPath("//div[@class='dx-item-content dx-list-item-content' and text()='����������']"));
            characterClick.Click();
            IWebElement inputQuery = driver.FindElement(By.XPath("//textarea[@test-id='request-query']"));
            inputQuery.Click();
            inputQuery.SendKeys("��� � ��� ��� ����������?!");

            IWebElement sendButton = driver.FindElement(By.XPath("//div[@test-id='send-button']"));
            sendButton.Click(); 
        }

        [Test]
        public void cabinet()
        {
            IWebElement login = driver.FindElement(By.Name("login"));
            login.SendKeys("admin");
            IWebElement password = driver.FindElement(By.Name("password"));
            password.SendKeys("asdf1234" + Keys.Enter);
            Thread.Sleep(1000);
            IWebElement cabinetClick = driver.FindElement(By.XPath("//a[@test-id='menuitem-requests-cabinet']"));
            cabinetClick.Click();
            Assert.AreEqual("http://eskso2.sapfir.corp:17155/requests/cabinet", driver.Url);
        }

        [TearDown] // ���������� ����� ������ 
        public void TearDown()
        {
            Thread.Sleep(3000);

            driver.Quit();
        }

    }
}