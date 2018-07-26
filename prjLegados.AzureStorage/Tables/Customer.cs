using Microsoft.Azure.CosmosDB.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.AzureServices.Tables
{
    class Customer:TableEntity
    {
        private int customerID;
        private string customerName;
        private string customerDetails;
        private string customerType;
        public void AssignRowKey()
        {
            this.RowKey = customerID.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = customerType;
        }
        public int CustomerID
        {
            get
            {
                return customerID;
            }

            set
            {
                customerID = value;
            }
        }
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                customerName = value;
            }
        }
        public string CustomerDetails
        {
            get
            {
                return customerDetails;
            }

            set
            {
                customerDetails = value;
            }
        }

        public string CustomerType
        {
            get
            {
                return customerType;
            }

            set
            {
                customerType = value;
            }
        }
    }
}
