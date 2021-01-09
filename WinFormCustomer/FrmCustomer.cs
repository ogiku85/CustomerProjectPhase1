using FactoryCustomer;
using FactoryDal;
using InterfaceCustomer;
using InterfaceDal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormCustomer
{
    public partial class FrmCustomer : Form
    {
        private CustomerBase customer = null;
        private IRepository<CustomerBase> Idal = null;
        public FrmCustomer()
        {
            InitializeComponent();
            Idal = FactoryDal<IRepository<CustomerBase>>
                   .Create("ADODal");
        }
        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            DalLayer.Items.Add("ADODal");
            DalLayer.Items.Add("EFDal");
            DalLayer.SelectedIndex = 0;
            Idal = FactoryDal<IRepository<CustomerBase>>
                               .Create(DalLayer.Text);
            LoadGrid();
        }
        private void LoadGrid()
        {
           // IRepository<CustomerBase> Idal = FactoryDal<IRepository<CustomerBase>>.Create(DalLayer.Text);
            List<CustomerBase> custs = Idal.Get();
            dtgGridCustomer.DataSource = custs;

        }

        private void LoadGridInMemory()
        {
            
            List<CustomerBase> custs = Idal.GetInMemoryData();
            dtgGridCustomer.DataSource = custs;

        }
        private void ddlCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            customer = Factory<CustomerBase>.Create(ddlCustomerType.Text);
        }
        private void SetCustomer()
        {
            customer.Address = txtAddress.Text;
            customer.BillAmount = !string.IsNullOrWhiteSpace(txtBillAmount.Text) ? Convert.ToDecimal(txtBillAmount.Text) : 0;
            customer.BillDate = !string.IsNullOrWhiteSpace(txtBillDate.Text) ? Convert.ToDateTime(txtBillDate.Text) : DateTime.Now;
            customer.CustomerName = txtCustomerName.Text;
            customer.PhoneNumber = txtPhoneNumber.Text;
            customer.CustomerType = ddlCustomerType.Text;
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                SetCustomer();
                customer.Validate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetCustomer();
          
            Idal.Add(customer); // In memory
           // Idal.Save(); // Physical committ
            LoadGridInMemory();
            ClearCustomer();
        }
        private void ClearCustomer()
        {
            txtCustomerName.Text = "";
            txtPhoneNumber.Text = "";
            txtBillDate.Text = DateTime.Now.Date.ToString();
            txtBillAmount.Text = "";
            txtAddress.Text = "";
        }
        private void DalLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Idal = FactoryDal<IRepository<CustomerBase>>
            //       .Create(DalLayer.Text);
            LoadGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Idal.Save();
            ClearCustomer();
            LoadGrid();
        }

        private void dtgGridCustomer_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            customer = Idal.GetInMemoryData(e.RowIndex);
            LoadCustomerOnUI();
        }

        private void LoadCustomerOnUI()
        {
            txtAddress.Text = customer.Address;
            txtBillAmount.Text = customer.BillAmount.ToString();
            txtBillDate.Text = customer.BillDate.ToString("d/MM/yyyy");
            txtPhoneNumber.Text = customer.PhoneNumber;
            txtCustomerName.Text = customer.CustomerName;
            ddlCustomerType.SelectedItem = customer.CustomerType;
        }
    }
}
