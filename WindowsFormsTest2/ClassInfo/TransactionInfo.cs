using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace WindowsFormsTest2.ClassInfo
{
    class TransactionInfo
    {
        public TransactionInfo()
        {
        }

        public TransactionInfo(string id, string productName, float newData, float transactionsMoney, float lastIncome, float price)
        {
            this.ID = id;
            this.ProductName = productName;
            this.NewData = newData;
            this.TransactionsMoney = transactionsMoney;
            this.LastIncome = lastIncome;
            this.Price = price;
        }

        //商品代码
        private string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        //商品名
        private string _productName;
        public string ProductName
        {
            get
            {
                return _productName;
            }
            set
            {
                _productName = value;
            }
        }

        //最新数据
        private float _newData;
        public float NewData
        {
            get
            {
                return _newData;
            }
            set
            {
                _newData = value;
            }
        }

        //成交金额
        private float _transactionsMoney;
        public float TransactionsMoney
        {
            get
            {
                return _transactionsMoney;
            }
            set
            {
                _transactionsMoney = value;
                //RaisePropertyChanged("TransactionsMoneyStr");
            }
        }

        //成交金额格式化显示
        public string TransactionsMoneyStr
        {
            get
            {
                return TransactionsMoney > 10000 ? string.Format("{0:0.00万}", TransactionsMoney / 10000) : string.Format("{0:0.00}", TransactionsMoney);
            }
        }


        //昨收
        private float _lastIncome;
        public float LastIncome
        {
            get
            {
                return _lastIncome;
            }
            set
            {
                _lastIncome = value;
            }
        }

        //委卖价
        private float _price;
        public float Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void RaisePropertyChanged(string name)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(name));
        //    }
        //}

    }
}