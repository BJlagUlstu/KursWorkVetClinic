using KursWorkVetClinicBusinessLogic.BindingModels;
using KursWorkVetClinicBusinessLogic.Interfaces;
using KursWorkVetClinicBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class UserLogic
    {
        private readonly IUserStorage _userStorage;
        private readonly int _passwordMaxLength = 50;
        private readonly int _passwordMinLength = 10;
        public UserLogic(IUserStorage userStorage)
        {
            _userStorage = userStorage;
        }
        public List<UserViewModel> Read(UserBindingModel model)
        {
            if (model == null)
            {
                return _userStorage.GetFullList();
            }
            return new List<UserViewModel> { _userStorage.GetElement(model) };
        }
        public void Login(UserBindingModel model)
        {
            var element = _userStorage.GetElement(new UserBindingModel
            {
                Login = model.Login
            });
            if (element == null)
            {
                throw new Exception("Пользователь не найден");
            }
            if (element.Password != model.Password)
            {
                throw new Exception("Неверный пароль");
            }
        }
        public void CreateOrUpdate(UserBindingModel model)
        {
            var element = _userStorage.GetElement(new UserBindingModel
            {
                Login = model.Login
            });
            if (element != null && element.UserId != model.Id)
            {
                throw new Exception("Уже есть пользователь с таким логином");
            }
            CheckData(model);
            if (model.Id.HasValue)
            {
                _userStorage.Update(model);
            }
            else
            {
                _userStorage.Insert(model);
            }
        }
        private void CheckData(UserBindingModel model)
        {
            if (!Regex.IsMatch(model.Login, @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"))
            {
                throw new Exception("В качестве логина должна быть указана почта");
            }
            if (model.Password.Length > _passwordMaxLength || model.Password.Length < _passwordMinLength || !Regex.IsMatch(model.Password,
            @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль длиной от {_passwordMinLength} до {_passwordMaxLength }" +
                    $" должен состоять и из цифр, букв и небуквенных символов");
            }
        }
    }
}