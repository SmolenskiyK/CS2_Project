﻿using Data.DumbData;
using Domain.DAO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUI.GoodsAll.AddGoods
{
    public class AddGoodsPresenter
    {
        AddGoods view = new AddGoods();
        IGoodsRepository model = new GoodsDao();
        Goods thisGoods;
       // public List<Domain.Entities.Goods> Goods { get { return thisGoods.Name; } set { } }
        public List<Comment> Comments { get { return thisGoods.Coments; } set { } }

        public AddGoodsPresenter(AddGoods view, int id)
        {
            this.view = view;
            if (id < 1)
            {
                thisGoods = new Goods();
            }
            else
            {
                thisGoods = model.GetById(id);
            }
        }

        public string Name
        {
            get { return thisGoods.Name; }
            set { thisGoods.Name = value; }
        }

        public string SKU
        {
            get { return thisGoods.SKU; }
            set { thisGoods.SKU = value; }
        }

        public double Price
        {
            get { return thisGoods.Price; }
            set { thisGoods.Price = value; }
        }

        public int Count
        {
            get { return thisGoods.Count; }
            set { thisGoods.Count = value; }
        }

        public string Category
        {
            get { return thisGoods.Category.CategoryName; }
            set { thisGoods.Category.CategoryName = value; }
        }

        public string Coments
        {
            get { return thisGoods.Coments.ToString(); }
            //set { thisGoods.Coments = value; }
        }

        public void Save(Goods goods)
        {
            model.Add(goods);
        }
    }
}