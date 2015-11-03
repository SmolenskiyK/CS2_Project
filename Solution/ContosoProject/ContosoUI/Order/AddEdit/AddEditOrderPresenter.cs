﻿using Data.EFData;
using Domain.DAO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUI.Order.AddEdit
{
    public class AddEditOrderPresenter : INotifyPropertyChanged
    {
        readonly AddEditOrderView view;
        readonly AddOrderProxy modelProxy = new AddOrderProxy();
        public List<AddEditViewModel> vm { get; set; }

        private int orderId;
        Domain.Entities.Order order;

        public List<Customer> Customers
        {
            get { return modelProxy.CustomerModel.GetAll().ToList(); }
        }

        public List<OrderStatus> Statuses
        {
            get { return modelProxy.OrderStatusModel.GetAll().Distinct().ToList(); }
        }

        public List<Goods> GoodsList
        {
            get { return modelProxy.GoodsModel.GetAll().ToList(); }
        }

        public List<Comment> Comments
        {
            get { return order.Comments; }
            set
            {
                if (Comments != value)
                {
                    Comments = value;
                    NotifyPropertyChanged("Comments");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddEditOrderPresenter(AddEditOrderView view, int orderId)
        {
            this.view = view;
            this.orderId = orderId;
            vm = new List<AddEditViewModel>();

            if (orderId >= 0)
            {
                order = modelProxy.OrderModel.GetById(orderId);
                foreach (GoodsRow row in order.GoodsList)
                {
                    vm.Add(new AddEditViewModel() { Id = row.Id, Good = row.Goods, Count = row.Count, TotalCost = row.TotalPrice });
                }
            }
            else
            {
                order = new Domain.Entities.Order();
            }
        }

        public Customer Customer
        {
            get { return order.Customer; }
            set
            {
                if (order.Customer != value)
                {
                    order.Customer = value;
                    NotifyPropertyChanged("Customer");
                }
            }    
        }

        public OrderStatus Status
        {
            get { return order.Status; }
            set
            {
                if (order.Status != value)
                {
                    order.Status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private Goods selectedGood { get; set; }
        public Goods SelectedGood
        {
            get { return selectedGood; }
            set
            {
                if (selectedGood != value)
                {
                    selectedGood = value;
                    NotifyPropertyChanged("SelectedGood");
                }
            }
        }

        private int countOfGood { get; set; }
        public int CountOfGood
        {
            get { return countOfGood; }
            set
            {
                if (countOfGood != value)
                {
                    countOfGood = value;
                    NotifyPropertyChanged("CountOfGood");
                }
            }
        }

        public double TotalCost
        {
            get { return CalculateOrderCost(); }
        }

        private string message { get; set; }
        public string Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        public Comment Comment
        {
            get { return new Comment() { Message = message, Type = CommentType.Order }; }
        }

        public void AddNewGoodRow()
        {
            GoodsRow item = new GoodsRow() {Goods = SelectedGood, Count = CountOfGood };
            vm.Add(new AddEditViewModel() { Id = item.Id, Good = item.Goods, Count = item.Count, TotalCost = item.TotalPrice });
            order.GoodsList.Add(item);
            modelProxy.GoodsRowModel.Add(item);
        }

        public void DeleteGoodRow(int id)
        {
            vm.Remove(vm.Find(x => x.Id == id));
            order.GoodsList.Remove(order.GoodsList.Find(x => x.Id == id));
            modelProxy.GoodsRowModel.Delete(order.GoodsList.Find(x => x.Id == id));
        }

        public void AddNewComment(Comment value)
        {
            order.Comments.Add(value);
            modelProxy.CommentModel.Add(value);
        }

        private double CalculateOrderCost()
        {
            double result = 0;
            foreach (GoodsRow row in order.GoodsList)
            {
                result += row.TotalPrice;
            }
            return result;
        }

        public void Save()
        {
            if (order.Id >= 0)
            {
                modelProxy.OrderModel.Update(order);
            }
            else
            {
                modelProxy.OrderModel.Add(order);
            }
        }

        public void SaveAndNew()
        {
            Save();
            order = new Domain.Entities.Order();
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }

    public class AddEditViewModel
    {
        public int Id { get; set; }
        public Goods Good{ get; set; }
        public int Count { get; set; }
        public double TotalCost { get; set; }
    }
}
