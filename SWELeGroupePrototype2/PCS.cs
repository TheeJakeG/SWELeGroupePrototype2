using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using Microsoft.Win32.SafeHandles;


public class PCS
{
    public static PCS Singleton;
    /// <summary>
    /// holds information of active employees
    /// </summary>
    public class EmployeeData 
	{
		public ulong EmployeeID;
		public string Title, Address, FirstName, LastName, PhoneNumber, Email, Username, Password;
		public EmployeeData(ulong EmployeeID) 
		{
			this.EmployeeID = EmployeeID;
			//get data from PCS
		}
		public EmployeeData() { }
	}
    /// <summary>
    /// holds information of active customer
    /// </summary>
    public class CustomerData 
	{
        /// <summary>
        /// holds info for customer payment cards
        /// </summary>
        public class CardInfo 
		{
			public string CardHolderName, ExpirationDate, CardNum, CSV, BillingAddress;
		}
		public ulong CustomerID;
		public string FirstName, LastName, Address, PhoneNumber, Email, Username, Password;
		public CardInfo CreditCard;
		public CustomerData(ulong CustomerID)
		{
			this.CustomerID = CustomerID;
		}
		public CustomerData() { }
	}
    /// <summary>
    /// represents an order
    /// </summary>
    public class OrderData 
	{
        public enum OrderStatus
        {

        }
        public ulong ID;
        public DateTime Date; //Time placed
		public ulong CustomerID;
        public List<Product> Items;
        public decimal OrderTotal;
        public OrderStatus Status;
	}
    /// <summary>
    /// represents an order for delivery
    /// </summary>
    public class DeliveryOrder 
	{
		public enum DeliveryStatus
		{

		}
		public ulong OrderID, CustomerID, DeliveryID, EmployeeID;
		public string DeliveryAddress;
		public DateTime ExpectedArrival;
		public string Notes;
		public DeliveryStatus Status;
	}
	/// <summary>
	/// represents a product that can be ordered - stored with format: id, name, description, category, price
	/// </summary>
	public class Product 
	{
		public enum ProductCategory
		{

		}
        public ulong ProductID;
        public decimal UnitPrice;
        public ProductCategory Category;
        public string ProductName, ProductDescription;
        public int Count;

		public Product() { }
		public Product(ulong ProductID) 
		{
			this.ProductID = ProductID;
		}
	}

	//Main PCS
	enum Account
	{
		none,
		customer,
		employee,
		admin
	}
	Account AccountType;
	ulong AccountID;
	EmployeeData[] Employees;
	OrderData[] E_CurrentOrders;
	DeliveryOrder[] E_ReadyToDeliver;
	DeliveryOrder[] E_DeliveriesInProgress;
	OrderData C_OrderInProgress;
	DeliveryOrder C_ActiveDelivery;


	//Database standin variables 
	string fp_Customers = "./DataFiles/Customers.txt", fp_Employees, fp_Products, fp_Orders, fp_Deliveries;

    /// <summary>
    /// compares log in information with existing accounts
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public bool LogIn(string username, string password)
	{
        try
		{
            List<string> customers = new List<string>();
            StreamReader sr = new StreamReader(fp_Customers);
            while (!sr.EndOfStream)
            {
                customers.Add(sr.ReadLine());
            }
            sr.Close();

			for (int i = 0; i < customers.Count; i++)
			{
				string[] c = customers[i].Split(',');
				if (c[1] == username && c[2] == password)
				{
					AccountType = Account.customer;
					AccountID = ulong.Parse(c[0]);
					return true;
				}
			}
        }
		catch (Exception e) { }
		return false;
	}

    /// <summary>
    /// stores account info for a customer using the following csv format: id, username, password, first name, last name, email, phone number, address, CardHolderName|ExpirationDate|CardNum|CSV|BillingAddress
    /// </summary>
    /// <param name="customerID"></param>
    /// <param name="newData"></param>
    /// <returns></returns>
    public bool UpdateAccountInfo(ulong customerID, CustomerData newData) 
	{
		try
		{
			string toAdd = "" + newData.CustomerID + "," + newData.Username + "," + newData.Password + "," + newData.FirstName + "," + newData.LastName + "," + newData.Email + "," + newData.PhoneNumber + "," + newData.Address + ","+ (newData.CreditCard == null ? "none" : "" + newData.CreditCard.CardHolderName + "|" + newData.CreditCard.ExpirationDate + "|" + newData.CreditCard.CardNum + "|" + newData.CreditCard.CSV + "|" + newData.CreditCard.BillingAddress);

			List<string> customers = new List<string>();
			StreamReader sr = new StreamReader(fp_Customers);
			while (!sr.EndOfStream)
			{
				customers.Add(sr.ReadLine());
			}
			sr.Close();

			if (customers.Count == 0)
			{
				customers.Add(toAdd);
			}
			else
			{
				for (int i = 0; i < customers.Count; i++)
				{
					if (customers[i].Split(',')[0] == customerID.ToString())
					{
						customers[i] = toAdd;
					}
					else if (i == customers.Count - 1)
					{
						customers.Add(toAdd);
					}
				}
			}

			StreamWriter sw = new StreamWriter(fp_Customers);
			for (int i = 0; i < customers.Count; i++)
			{
				sw.WriteLine(customers[i]);
			}
			sw.Close();

            return true;
		}
		catch (Exception e) { Console.WriteLine(e.Message); return false; }
    }

    public OrderData RetrieveOrder(ulong orderID)
	{
		try
		{
			List<string> orders = new List<string>();
			StreamReader sr = new StreamReader(fp_Orders);
			while (!sr.EndOfStream) { orders.Add(sr.ReadLine()); }
			sr.Close();

			for (int i = 0; i < orders.Count; i++)
			{
				if(orderID.ToString() == orders[i].Split(',')[0])
				{
					string[] data = orders[i].Split(',');
					OrderData newData = new OrderData();

					newData.ID = ulong.Parse(data[0]);
					newData.Date = DateTime.Parse(data[1]);
					newData.CustomerID = ulong.Parse(data[2]);
					newData.OrderTotal = decimal.Parse(data[3]);
					newData.Status = (OrderData.OrderStatus)int.Parse(data[4]);

					for(int j = 5; j < data.Length; j++)
					{
						Product p = new Product();
						string[] pData = data[j].Split('|');
						p.ProductID = ulong.Parse(pData[0]);
						p.Count = int.Parse(pData[1]);

                        List<string> products = new List<string>();
                        sr = new StreamReader(fp_Products);
                        while (!sr.EndOfStream) { products.Add(sr.ReadLine()); }
                        sr.Close();

						for(int k  = 0; k < products.Count; k++)
						{
							string[] split = products[k].Split(',');
							if (p.ProductID.ToString() == split[0])
							{
                                p.ProductName = split[1];
								p.ProductDescription = split[2];
								p.Category = (Product.ProductCategory)int.Parse(split[3]);
								p.UnitPrice = decimal.Parse(split[4]);
								break;
                            }
						}
                    }

					return newData;
				}
			}
		}
		catch (Exception e) { }
        return null;
    }

	public DeliveryOrder RetrieveDelivery(ulong deliveryID)
	{
        try
        {
            List<string> orders = new List<string>();
            StreamReader sr = new StreamReader(fp_Deliveries);
            while (!sr.EndOfStream) { orders.Add(sr.ReadLine()); }
            sr.Close();

            for (int i = 0; i < orders.Count; i++)
            {
                if (deliveryID.ToString() == orders[i].Split(',')[0])
                {
                    string[] data = orders[i].Split(',');
                    DeliveryOrder newOrder = new DeliveryOrder();

					newOrder.DeliveryID = ulong.Parse(data[0]);
					newOrder.OrderID = ulong.Parse(data[1]);
					newOrder.CustomerID = ulong.Parse(data[2]);
					newOrder.EmployeeID = ulong.Parse(data[3]);
					newOrder.DeliveryAddress = data[4];
					newOrder.ExpectedArrival = DateTime.Parse(data[5]);
					newOrder.Notes = data[6];
					newOrder.Status = (DeliveryOrder.DeliveryStatus)int.Parse(data[7]);

                    return newOrder;
                }
            }
        }
        catch (Exception e) { }
        return null;
    }
    

    public void PrintReceipt(ulong orderID)
	{

	}

    /// <summary>
    ///  stores order data with format: order id, date placed, customer id, order total, order status, for each item: product id|count, 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool StoreOrderData(OrderData data)
	{
		try
		{
			string file = File.ReadAllText(fp_Orders);

			string toAdd = "" + data.ID + "," + data.Date.ToString() + "," + data.CustomerID + "," + data.OrderTotal + "," + (int)data.Status;
			foreach(Product p in data.Items)
			{
				toAdd += "," + p.ProductID + "|" + p.Count;
			}

			file += Environment.NewLine + toAdd;

			File.WriteAllText(fp_Orders, file);

            return true;
		}
		catch (Exception e) { return false; }
	}

	/// <summary>
	/// stores delivery data with the format: Delivery ID, Order ID, Customer ID, EmployeeID, DeliveryAddress, Expected Arrival, Notes, Status
	/// </summary>
	/// <param name="delOrder"></param>
	/// <returns></returns>
    public bool StoreDeliveryOrder(DeliveryOrder delOrder)
	{
		try
		{
			string file = File.ReadAllText(fp_Deliveries);

			string toAdd = "" + delOrder.DeliveryID + "," + delOrder.OrderID + "," + delOrder.CustomerID + "," + delOrder.EmployeeID + "," + delOrder.DeliveryAddress + "," + delOrder.ExpectedArrival + "," + delOrder.Notes + "," + delOrder.Status;

			file += Environment.NewLine + toAdd;

			File.WriteAllText(fp_Deliveries, file);

            return true;
		}
		catch (Exception e) { return false; }
	}

    /// <summary>
    ///  updates or stores order data with format: order id, date placed, customer id, order total, order status, for each item: product id|count, 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool UpdateOrderData(ulong orderID, OrderData newData)
    {
        try
        {
            string toAdd = "" + newData.ID + "," + newData.Date.ToString() + "," + newData.CustomerID + "," + newData.OrderTotal + "," + (int)newData.Status;
            foreach (Product p in newData.Items)
            {
                toAdd += "," + p.ProductID + "|" + p.Count;
            }

			List<string> orders = new List<string>();
			StreamReader sr = new StreamReader(fp_Orders);
			while(!sr.EndOfStream) { orders.Add(sr.ReadLine()); }
            sr.Close();

            for (int i = 0; i < orders.Count; i++)
			{
				if (orders[i].Split(',')[0] == orderID.ToString())
				{
					orders[i] = toAdd;
				}
				else if(i == orders.Count - 1)
				{
					orders.Add(toAdd);
				}
			}

			StreamWriter sw = new StreamWriter(fp_Orders);
			for(int i = 0; i < orders.Count; i++) { sw.WriteLine(orders[i]); }
			sw.Close();

			return true;
        }
        catch { return false; }
    }
    /// <summary>
    /// updates or stores delivery data with the format: Delivery ID, Order ID, Customer ID, EmployeeID, DeliveryAddress, Expected Arrival, Notes, Status
    /// </summary>
    /// <param name="delOrder"></param>
    /// <returns></returns>
    public bool UpdateDeliveryOrder(ulong delOrderID, DeliveryOrder newData)
    {
        try
        {
            string toAdd = "" + newData.DeliveryID + "," + newData.OrderID + "," + newData.CustomerID + "," + newData.EmployeeID + "," + newData.DeliveryAddress + "," + newData.ExpectedArrival + "," + newData.Notes + "," + newData.Status;
            List<string> orders = new List<string>();
            StreamReader sr = new StreamReader(fp_Deliveries);
            while (!sr.EndOfStream) { orders.Add(sr.ReadLine()); }
			sr.Close();

            for (int i = 0; i < orders.Count; i++)
            {
				if (orders[i].Split(',')[0] == delOrderID.ToString())
                {
                    orders[i] = toAdd;
                }
                else if (i == orders.Count - 1)
                {
                    orders.Add(toAdd);
                }
            }

            StreamWriter sw = new StreamWriter(fp_Deliveries);
            for (int i = 0; i < orders.Count; i++) { sw.WriteLine(orders[i]); }
			sw.Close();

            return true;
        }
        catch { return false; }
    }
    public bool AddToOrder(Product newProduct)
    {
		C_OrderInProgress.Items.Add(newProduct);
		return true;
    }
    public bool RemoveFromOrder(Product product)
    {
		if (C_OrderInProgress.Items.Remove(product)) return true;
		else return false;
    }
    public OrderData FinalizeOrder(OrderData order)
    {
		foreach(Product p in order.Items)
		{
			order.OrderTotal += p.UnitPrice * p.Count;
		}
		//store order?
		return order;
    }
    public void ShowOrders()
    {

    }
    public void DisplayOrder(ulong orderID)
    {

    }
    public PCS()
    {
        CustomerData sample = new CustomerData();
        sample.FirstName = "John";
        sample.LastName = "Doe";
        sample.Username = "username";
        sample.Password = "password";
        sample.Email = "email@some.thing";
        sample.Address = "123 Someplace cool City zip";
        sample.CustomerID = ulong.MinValue;
        sample.PhoneNumber = "098-765-4321";
        sample.CreditCard = new CustomerData.CardInfo();
        sample.CreditCard.CardNum = "1234-5678-9110-1984";
        sample.CreditCard.CSV = "451";
        sample.CreditCard.CardHolderName = "John Doe";
        sample.CreditCard.ExpirationDate = "12/24";
        sample.CreditCard.BillingAddress = "123 Someplace cool City zip";
        UpdateAccountInfo(sample.CustomerID, sample);
    }
}
