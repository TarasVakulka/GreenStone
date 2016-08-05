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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GreenStone.DataContext;
using GreenStone.Models;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GreenStone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        List<DpService> TempDpService;
        private readonly ManagerContext _context = new ManagerContext();
        int profitsum;
        int idemp;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TempDpService = new List<DpService>();
            profitsum = 0;
            int soldsum = 0;
            soldsum = DeaprtmentProfitability();
            foreach(Department value in _context.Departments.ToList())
            {
                profitsum += value.Profitability;
            }
            LabelProfit.Content = profitsum+soldsum;
        }

        
        private IEnumerable<Department> _departmentses;
        public IEnumerable<Department> Departmentses 
        {
            get { return new ObservableCollection<Department>(_context.Departments); }
            set 
            {
                _departmentses = value;
                OnPropertyChanged("Departmentses");
            }
        }

        private IEnumerable<Employee> _employees;
        public IEnumerable<Employee> Employees 
        {
            get { return new ObservableCollection<Employee>(_context.Employees.Include(e=>e.Department)); }
            set 
            { 
                _employees = value;
                OnPropertyChanged("Employees");

            }
        }
        private IEnumerable<DpService> _dpservices;
        public IEnumerable<DpService> DpServices
        {
            get { return new ObservableCollection<DpService>(_context.DpServices.Where(dp=>dp.DepartmentId!=null)); }
            set
            {
                _dpservices = value;
                OnPropertyChanged("DpServices");

            }
        }
        private IEnumerable<Product> _products;
        public IEnumerable<Product> Products
        {
            get { return new ObservableCollection<Product>(_context.Products.Include("DpServices").ToList()); }
            set
            {
                _products = value;
                OnPropertyChanged("Products");

            }
        }
        private IEnumerable<Stone> _stones;
        public IEnumerable<Stone> Stones
        {
            get { return new ObservableCollection<Stone>(_context.Stones); }
            set
            {
                _stones = value;
                OnPropertyChanged("Stones");

            }
        }
        private IEnumerable<Product> _productsinclient;
        public IEnumerable<Product> ProductsInClient
        {
            get { return new ObservableCollection<Product>(_context.Products.Where(pr=>pr.IsMade==true).ToList()); }
            set
            {
                _productsinclient = value;
                OnPropertyChanged("ProductsInClient");

            }
        }
        private IEnumerable<Client> _clients;
        public IEnumerable<Client> Clients
        {
            get { return new ObservableCollection<Client>(_context.Clients.Include("Products")); }
            set
            {
                _clients = value;
                OnPropertyChanged("Clients");

            }
        }

        public Employee _currentSelectedEmployee { get; set; }
        public Employee CurrentSelectedEmployee 
        {
            get { return _currentSelectedEmployee; }
            set 
            {   
                _currentSelectedEmployee = value;
                OnPropertyChanged("CurrentSelectedEmployee");
            }
        }

        public Department _currentSelectedDepartment { get; set; }
        public Department CurrentSelectedDepartment
        {
            get { return _currentSelectedDepartment; }
            set
            {
                _currentSelectedDepartment = value;
                OnPropertyChanged("CurrentSelectedDepartment");
            }
        }
        public DpService _currentSelectedDpService { get; set; }
        public DpService CurrentSelectedDpService
        {
            get { return _currentSelectedDpService; }
            set
            {
                _currentSelectedDpService = value;
                OnPropertyChanged("CurrentSelectedDpService");
            }
        }
        public Product _currentSelectedProduct { get; set; }
        public Product CurrentSelectedProduct
        {
            get { return _currentSelectedProduct; }
            set
            {
                _currentSelectedProduct = value;
                OnPropertyChanged("CurrentSelectedProduct");
            }
        }
        public Stone _currentSelectedStone { get; set; }
        public Stone CurrentSelectedStone
        {
            get { return _currentSelectedStone; }
            set
            {
                _currentSelectedStone = value;
                OnPropertyChanged("CurrentSelectedStone");
            }
        }
        public Client _currentSelectedClient { get; set; }
        public Client CurrentSelectedClient 
        {
            get { return _currentSelectedClient; }
            set
            {
                _currentSelectedClient = value;
                OnPropertyChanged("CurrentSelectedClient");
            }
        }
        public Department _currentSelectedDepartmentofProduct { get; set; }
        public Department CurrentSelectedDepartmentofProduct
        {
            get { return _currentSelectedDepartmentofProduct; }
            set
            {
                _currentSelectedDepartmentofProduct = value;
                OnPropertyChanged("CurrentSelectedDepartmentofProduct");
            }
        }
        public Employee _currentSelectedEmployeeofRadioButton { get; set; }
        public Employee CurrentSelectedEmployeeofRadioButton
        {
            get { return _currentSelectedEmployeeofRadioButton; }
            set
            {
                _currentSelectedEmployeeofRadioButton = value;
                OnPropertyChanged("CurrentSelectedEmployeeofRadioButton");
            }
        }

        public DpService CurrentSelectedDpServiceonPrListBoxoinDataGrid
        {
            get;
            set;

        }



        
        #region Employee_CRUD_Method
        private void AddNewEmployee(Employee employee)
        {
            //foreach (Product p in Products)
            //{
            //    MessageBoxResult res = MessageBox.Show(Convert.ToString(p.DpServices.Count()), "Coursework", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            //    if (res == MessageBoxResult.Cancel)
            //        break;
            //}

            _context.Employees.Add(employee);
            _context.SaveChanges();
            DataGrid1.ItemsSource = Employees;
        }
        private void UpdateCurrentEmployee(Employee employee)
        {
            var employeeToUpdate = _context.Employees.SingleOrDefault(e => e.Id == employee.Id);
            if (employeeToUpdate == null) return;
            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.BirthDay = employee.BirthDay;
            employeeToUpdate.DepartmentId = employee.DepartmentId;
            employeeToUpdate.Salary = EmployeeSalary(employeeToUpdate.ServicesForSalary); 
            _context.SaveChanges();
            DataGrid1.ItemsSource = Employees;

        }
        private void DeleteCurrentEmployee(Employee employee)
        {
            var EmployeeToUpdate = _context.Employees.SingleOrDefault(e => e.Id == employee.Id);
            List<DpService> DpServicewithSalaryDelete = _context.DpServices.Where(dp => dp.EmployeeForSalaryId == CurrentSelectedEmployee.Id).ToList();
            if (DpServicewithSalaryDelete != null)
            {
                foreach (DpService dp in DpServicewithSalaryDelete)
                {
                    _context.DpServices.Remove(dp);
                }
                _context.SaveChanges();
            }
            _context.Employees.Remove(EmployeeToUpdate);
            _context.SaveChanges();

            DataGrid1.ItemsSource = Employees;
        }

        private int EmployeeSalary(List<DpService> dpservice)
        {
            int result; int tempsalary = 0;
            if (dpservice.Count != 0)
            {
                foreach (DpService dp in dpservice)
                {
                    tempsalary += dp.ServicePrice;
                }

                result = Convert.ToInt32(tempsalary / 1.5);

                return result;
            }
            else return 0;
        }
                                
        #endregion

        #region INotifyPropertyChanged Implementing
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Buttons_Employee
        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (DtpkBirthDay.SelectedDate == null || CbbDepartment.SelectedValue == null) return;
            AddNewEmployee(new Employee
            {
                FirstName = TxtFirstName.Text,
                LastName = TxtLastName.Text,
                BirthDay = (DateTime)DtpkBirthDay.SelectedDate,
                DepartmentId = (int)CbbDepartment.SelectedValue
               

            });

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            
            UpdateCurrentEmployee(CurrentSelectedEmployee);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити?", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentEmployee(CurrentSelectedEmployee);

        }
        private void DataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid1.UnselectAll();
        }
        private void BtnNullSalary_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectedEmployee != null)
            {
                CurrentSelectedEmployee.Salary = 0;
                List<DpService> DpServicewithSalaryDelete = _context.DpServices.Where(dp => dp.EmployeeForSalaryId == CurrentSelectedEmployee.Id).ToList();
                if (DpServicewithSalaryDelete != null)
                {
                    foreach (DpService dp in DpServicewithSalaryDelete)
                    {
                        _context.DpServices.Remove(dp);                        
                    }
                    _context.SaveChanges();
                }
                
                CurrentSelectedEmployee.ServicesForSalary.Clear();
                _context.SaveChanges();
                DataGrid1.ItemsSource = Employees;
            }
            else return;
        }


        #endregion

        #region Department_CRUD_MEthod
        private void AddNewDepartment(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
            DataGridDDepartmentses.ItemsSource = Departmentses;

        }
        private void UpdateCurrentDepartment(Department department)
        {
            var departmentToUpdate = _context.Departments.SingleOrDefault(d => d.Id == department.Id);
            if (departmentToUpdate == null) return;
            departmentToUpdate.Name = department.Name;
            _context.SaveChanges();
            DataGridDDepartmentses.ItemsSource = Departmentses;
        }
        private void DeleteCurrentDepartment(Department department)
        {
            var DepartmentToUpdate = _context.Departments.SingleOrDefault(d => d.Id == department.Id);
            var EmployeeOfDepartment = _context.Employees.Where(e => e.DepartmentId == department.Id);
            var DpServiceOfDepartment = _context.DpServices.Where(dp => dp.DepartmentId == department.Id);
            _context.Employees.RemoveRange(EmployeeOfDepartment);
            _context.DpServices.RemoveRange(DpServiceOfDepartment);
            _context.Departments.Remove(DepartmentToUpdate);
            _context.SaveChanges();

            DataGridDDepartmentses.ItemsSource = Departmentses;
            DataGrid1.ItemsSource = Employees;
            DpDataGridServices.ItemsSource = DpServices;

        }

        #endregion

        #region Buttons_Department
        private void DDepartmentBtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (TxtDDepartmnetName.Text == null) return;
            AddNewDepartment(new Department
            {
                Name = TxtDDepartmnetName.Text

            });
        }

        private void DDepartmentBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentDepartment(CurrentSelectedDepartment);

        }

        private void DDepartmentBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити? Будуть видалені всі робітники відділу!", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentDepartment(CurrentSelectedDepartment);
        }
        private void DataGridDDepartmentses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridDDepartmentses.UnselectAll();
        }
        #endregion
      
        #region Department_DpService_CRUD_Method
        private void AddNewDpService(DpService dpservice)
        {
            _context.DpServices.Add(dpservice);
            _context.SaveChanges();
            DpDataGridServices.ItemsSource = DpServices;

        }
        private void UpdateCurrentDpService(DpService dpservice)
        {
            var dpserviceToUpdate = _context.DpServices.SingleOrDefault(dp => dp.Id == dpservice.Id);
            if (dpserviceToUpdate == null) return;
            dpserviceToUpdate.Name = dpservice.Name;
            dpserviceToUpdate.DepartmentId = dpservice.DepartmentId;
            dpserviceToUpdate.ServicePrice = dpservice.ServicePrice;
            _context.SaveChanges();
            DpDataGridServices.ItemsSource = DpServices;
        }
        private void DeleteCurrentDpService(DpService dpservice)
        {
            var DpServiceToUpdate = _context.DpServices.SingleOrDefault(dp => dp.Id == dpservice.Id);
            _context.DpServices.Remove(DpServiceToUpdate);
            _context.SaveChanges();

            DpDataGridServices.ItemsSource = DpServices;
            
        }

        #endregion
       
        #region Buttons_Department_DpService
        private void DpBtnServiceInsert_Click(object sender, RoutedEventArgs e)
        {
            if (DpCbbServiceDepartment.SelectedValue == null || DpTxtServiceName.Text==null || DpTxtServicePrice==null) return;
            AddNewDpService(new DpService
            {
                Name = DpTxtServiceName.Text,
                ServicePrice = Convert.ToInt32(DpTxtServicePrice.Text),
                DepartmentId = (int)DpCbbServiceDepartment.SelectedValue

            });
        }

        private void DpBtnServiceEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentDpService(CurrentSelectedDpService);
        }

        private void DpBtnServiceDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити?", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentDpService(CurrentSelectedDpService);
        }
        private void DpDataGridServices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DpDataGridServices.UnselectAll();
        }
        #endregion

        #region Main_Stone_Crud_Operation
        private void AddNewStone(Stone stone)
        {
            _context.Stones.Add(stone);
            _context.SaveChanges();
            MnDataGridStones.ItemsSource = Stones;

        }
        private void UpdateCurrentStone(Stone stone)
        {
            var stoneToUpdate = _context.Stones.SingleOrDefault(st => st.Id == stone.Id);
            if (stoneToUpdate == null) return;
            stoneToUpdate.Name = stone.Name;
            stoneToUpdate.StonePrice = stone.StonePrice;
            
            _context.SaveChanges();
            MnDataGridStones.ItemsSource = Stones;
        }
        private void DeleteCurrentStone(Stone stone)
        {
            var StoneToUpdate = _context.Stones.SingleOrDefault(st => st.Id == stone.Id);
            _context.Stones.Remove(StoneToUpdate);
            _context.SaveChanges();

            MnDataGridStones.ItemsSource = Stones;

        }

        #endregion

        #region Button_Main_Stone

        private void MnStoneBtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (MnTxtStoneName.Text == null || MnTxtStonePrice.Text == null) return;
            AddNewStone(new Stone
            {
                Name = MnTxtStoneName.Text,
                StonePrice = Convert.ToInt32(MnTxtStonePrice.Text),

            });
        }

        private void MnStoneBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentStone(CurrentSelectedStone);
        }

        private void MnStoneBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити?", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentStone(CurrentSelectedStone);
        }
        private void MnDataGridStones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MnDataGridStones.UnselectAll();
        }

        #endregion

        #region Product_CRUD_Method
        private void AddNewProduct(Product product)
        {
            
            _context.Products.Add(product);
            _context.SaveChanges();
            PrDataGridProducts.ItemsSource = Products;
            //PrDataGridProducts.Items.Refresh();
        }
        private void UpdateCurrentProduct(Product product)
        {
            var productToUpdate = _context.Products.SingleOrDefault(pr => pr.Id == product.Id);
            if (productToUpdate == null) return;
            productToUpdate.Name = product.Name;
            productToUpdate.Size = product.Size;
            productToUpdate.StoneId = product.StoneId;
            productToUpdate.DpServices = product.DpServices;
            productToUpdate.Price = PriceofProduct(product.Size, product.DpServices, (int)product.StoneId).ToString();
            productToUpdate.IsMade = product.IsMade;
            _context.SaveChanges();
            PrDataGridProducts.ItemsSource = Products;
        }
        private void DeleteCurrentProduct(Product product)
        {
            var ProductToUpdate = _context.Products.SingleOrDefault(pr => pr.Id == product.Id);
            _context.Products.Remove(ProductToUpdate);

            List<DpService> DpServiceWithOneProduct = _context.DpServices.Where(dp=>dp.Products.FirstOrDefault().Id==ProductToUpdate.Id).ToList();
            foreach(DpService dp in DpServiceWithOneProduct)
            {
                _context.DpServices.Remove(dp);
                _context.SaveChanges();
            }
                     
            _context.SaveChanges();

            PrDataGridProducts.ItemsSource = Products;
        }

        public bool MethodSplit(string size)
        {
            string[] separators = { "*" };
            string[] sizeresult = size.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            bool flag = true;

            if (sizeresult.Count() > 3 || sizeresult.Count() < 3)
            {
                MessageBox.Show("Введіть правильно розмір");
                return false;
            }
            foreach ( string value in sizeresult)
            {
                int iValue;
                flag = int.TryParse(value, out iValue);
            }

            if (flag == false)
            {
                MessageBox.Show("Введіть правильно розмір");
                return false;
            }

            return true;
        }
        public int PriceofProduct(string size, ICollection<DpService> services, int idstone)
        {
            string[] separators = {"*"};
            string[] sizeresult = size.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            double h = Convert.ToDouble(sizeresult[0]);
            double a = Convert.ToDouble(sizeresult[1]);
            double b = Convert.ToDouble(sizeresult[2]);
            double V = (a*b*h)/1000000;
            double S = (2*(a*b + a*h + h*b))/10000;
           
            int servPrice = 0;
            foreach (DpService serv in services)
            {
                servPrice+=Convert.ToInt32(serv.ServicePrice);
            }
            int price = Convert.ToInt32((servPrice*S) + (_context.Stones.Find(idstone).StonePrice*V));

            return price;
        }

        #endregion

        #region Buttons_Product

      
        private void PrProductBtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (PrTxtProductName.Text == null || PrTxtProductSize.Text == null || PrCbbStones.SelectedValue == null) { MessageBox.Show("Введены не всі данні!"); return; }
            if (MethodSplit(PrTxtProductSize.Text) == false) return;
            if (TempDpService.Count == 0) 
            { 
                MessageBox.Show("Виберіть послуги!"); 
                return; 
            }
            List<DpService> ss = new List<DpService>();
            
            foreach(DpService value in TempDpService)
            {
                ss.Add(new DpService
                {   
                    
                    Name = value.Name,
                    ServicePrice=value.ServicePrice,
                    EmployeeId = value.EmployeeId
                });
                
            }

            AddNewProduct(new Product
            {
                Name = PrTxtProductName.Text,
                Size = PrTxtProductSize.Text,
                StoneId = (int)PrCbbStones.SelectedValue,
                DpServices = ss,
                Price = PriceofProduct(PrTxtProductSize.Text, TempDpService, (int)PrCbbStones.SelectedValue).ToString()
            });

            TempDpService.Clear();
          
        }

        private void PrProductBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            List<DpService> nss = new List<DpService>();

            foreach (DpService value in TempDpService)
            {
                nss.Add(new DpService
                {

                    Name = value.Name,
                    ServicePrice = value.ServicePrice,
                    EmployeeId = value.EmployeeId,
                    

                });

            }
            foreach (DpService dp in nss)
            {
                CurrentSelectedProduct.DpServices.Add(dp);
            }

            UpdateCurrentProduct(CurrentSelectedProduct);
            TempDpService.Clear();
        }
        
        private void PrProductBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити?", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentProduct(CurrentSelectedProduct);
            
        }
        private void CheckBoxDpService_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chBoxDpService = (CheckBox)sender;
            int idserv = Convert.ToInt32(chBoxDpService.Tag);
            TempDpService.Add(_context.DpServices.Find(idserv));
        }
        private void PrRadioButtonEmployee_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            idemp = Convert.ToInt32(pressed.Tag);
            Employee emp = _context.Employees.Find(idemp);
            if (TempDpService.Count == 0)
            {
                MessageBox.Show("Виберіть послуги!");
                return;
            }
            else
            {
                
                foreach (DpService serv in TempDpService)
                {
                    if (emp.DepartmentId == serv.DepartmentId)
                    {
                        emp.ServicesForSalary.Add(new DpService
                        {
                            Name = serv.Name,
                            ServicePrice = serv.ServicePrice,
                            EmployeeForSalaryId = emp.Id

                        });
                    }
                   

                }

                _context.SaveChanges();

                foreach (DpService value in TempDpService)
                {
                    if (value.DepartmentId == emp.DepartmentId)
                    {
                        value.EmployeeId = idemp;

                    }
                }
                

                _context.SaveChanges();
            }
           
        }
        private void PrDataGridProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PrDataGridProducts.UnselectAll();
        }
        private void CheckBoxIsmade_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxIsmade = (CheckBox)sender;
            if (CurrentSelectedProduct != null)
            {
                CurrentSelectedProduct.IsMade = true;
            }

            CheckBoxIsmade.IsEnabled = false;
            _context.SaveChanges();
            ClDataGridProducts.ItemsSource = Products;
            CurrentSelectedProduct = null;

        }

        private void CheckBoxDpServiceonGrid_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxDpServiceonGrid = (CheckBox)sender;
            if (CurrentSelectedDpServiceonPrListBoxoinDataGrid != null)
            {
                foreach (DpService dps in CurrentSelectedProduct.DpServices)
                {
                    if (dps.Name == CurrentSelectedDpServiceonPrListBoxoinDataGrid.Name)
                    {
                        dps.IsDone = true;
                    }
                }
            }

            CheckBoxDpServiceonGrid.IsEnabled = false;
            _context.SaveChanges();


        }
        #endregion

        #region Client_CRUD_Method
        private void AddNewClient(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
            ClDataGridClients.ItemsSource = Clients;
        }
        private void UpdateCurrentClient(Client client)
        {
            var clientToUpdate = _context.Clients.SingleOrDefault(e => e.Id == client.Id);
            if (clientToUpdate == null) return;
            clientToUpdate.FirstName = client.FirstName;
            clientToUpdate.LastName = client.LastName;
            clientToUpdate.BirthDay = client.BirthDay;
            
            _context.SaveChanges();
            ClDataGridClients.ItemsSource = Clients;

        }
        private void DeleteCurrentClient(Client client)
        {
            var ClientToUpdate = _context.Clients.SingleOrDefault(e => e.Id == client.Id);
            _context.Clients.Remove(ClientToUpdate);
            _context.SaveChanges();

            ClDataGridClients.ItemsSource = Clients;
        }
        #endregion

        #region Buttons_Client
        private void ClClientBtnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (ClTxtClientFirstName.Text == null || ClTxtClientLastName.Text == null || ClientDtpkBirthDay.SelectedDate == null ) { MessageBox.Show("Введіть данні!"); return; }
            AddNewClient(new Client
            {
                FirstName = ClTxtClientFirstName.Text,
                LastName = ClTxtClientLastName.Text,
                BirthDay = (DateTime)ClientDtpkBirthDay.SelectedDate,
            });
        }

        private void ClClientBtnEdit_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentClient(CurrentSelectedClient);
        }

        private void ClClientBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити?", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentClient(CurrentSelectedClient);
        }
        private void CheckBoxIsSold_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBoxIsSold = (CheckBox)sender;
            if (CurrentSelectedProduct != null)
            {
                CurrentSelectedProduct.IsSold = true;
            }

            CheckBoxIsSold.IsEnabled = false;
            _context.SaveChanges();
        }
        private void ClProductBtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            ClDataGridProducts.ItemsSource = ProductsInClient;
            ClDataGridClients.ItemsSource = Clients;
        }

        private void ClProductBtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var status = MessageBox.Show("Ви дійсно хочете видалити?", "Обережно!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (status == MessageBoxResult.No) return;
            DeleteCurrentProduct(CurrentSelectedProduct);
            ClDataGridProducts.ItemsSource = ProductsInClient;
        }


        private void ClcbClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox ComboBoxClient = (ComboBox)sender;
            if (CurrentSelectedProduct != null && ComboBoxClient.SelectedValue != null)
            {
                CurrentSelectedProduct.ClientId = (int)ComboBoxClient.SelectedValue;
                _context.SaveChanges();
                ClDataGridClients.ItemsSource = Clients;
            }
            else return;
        }

        private void ClDataGridProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentSelectedProduct = null;
        }

        private void ClDataGridClients_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentSelectedClient = null;
        }
        #endregion


        #region Main_Diagram
        ObservableCollection<Point> points = new ObservableCollection<Point>();
        
        
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

            int soldprodsum = 0;
            soldprodsum = DeaprtmentProfitability();
            DiagramEmployee.ItemsSource = Employees;
            profitsum = 0;
            foreach (Department value in _context.Departments.ToList())
            {
                profitsum += value.Profitability;
            }
            LabelProfit.Content = profitsum+soldprodsum;
          
           
        }

        public int DeaprtmentProfitability()
        {
            int SoldProductSum = 0; 
            List<Product> SoldProd = new List<Product>();
            SoldProd = _context.Products.Where(pr => pr.IsSold == true).ToList();
            foreach(Product prod in SoldProd)
            {
                SoldProductSum += Convert.ToInt32(prod.Price);
            }
            SoldProd.Clear();

            List<int> ProfitList = new List<int>();
            int sum = 0;

            foreach (Department dp in _context.Departments.ToList())
            {
                foreach (Employee emp in dp.Employees)
                {
                    ProfitList.Add(emp.Salary);
                }
                foreach (int element in ProfitList)
                {
                    sum += element;
                }
                _context.Departments.Find(dp.Id).Profitability = Convert.ToInt32(sum*(1.5));
                
                ProfitList.Clear();
                sum = 0;
            }
            DiagramDepartment.ItemsSource = Departmentses;
            _context.SaveChanges();
            
            return SoldProductSum;
        }
        #endregion






    }

}
