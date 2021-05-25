using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicBusinessLogic.ViewModels;
using KursWorkVetClinicDatabaseImplements.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KursWorkVetClinicDatabaseImplements.Implements
{
    public class UserStorage : IUserStorage
    {
        public UserViewModel GetElement(UserBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new VetClinicDataBase())
            {
                var user = context.Users.FirstOrDefault(rec => rec.Login == model.Login || rec.Id == model.Id);
                return user != null ?
                new UserViewModel
                {
                    UserId = user.Id,
                    Login = user.Login,
                    Fullname = user.Fullname,
                    Password = user.Password
                } :
               null;
            }
        }
        public List<UserViewModel> GetFullList()
        {
            using (var context = new VetClinicDataBase())
            {
                return context.Users
                .Select(rec => new UserViewModel
                {
                    UserId = rec.Id,
                    Login = rec.Login,
                    Fullname = rec.Fullname,
                    Password = rec.Password
                })
                .ToList();
            }
        }
        public void Insert(UserBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Users.Add(CreateModel(model, new User()));
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Update(UserBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element == null)
                        {
                            throw new Exception("Пользователь не найден");
                        }
                        CreateModel(model, element);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(UserBindingModel model)
        {
            using (var context = new VetClinicDataBase())
            {
                User element = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
                if (element != null)
                {
                    context.Users.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Пользователь не найден");
                }
            }
        }
        private User CreateModel(UserBindingModel model, User user)
        {
            user.Login = model.Login;
            user.Fullname = model.Fullname;
            user.Password = model.Password;
            return user;
        }
    }
}