using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web
{
    public static class TempDataExtension
    {
        /// <summary>
        /// Метод добавляет сообщение для пользователя
        /// </summary>
        /// <param name="tempData">Параметр определяет тип, с которым оперирует extension метод</param>
        /// <param name="message">Сообщение для пользователя</param>
        public static void AddMessage(this ITempDataDictionary tempData, Message message)
        {
            List<MessageData> messageList = tempData.GetMessages();

            var messageData = new MessageData(message);

            tempData.GetMessages();
            messageList.Add(new MessageData(message));
            tempData["Messages"] = Newtonsoft.Json.JsonConvert.SerializeObject(messageList);
        }

        /// <summary>
        /// Метод добавляет несколько сообщений для пользователя
        /// </summary>
        /// <param name="tempData">Параметр определяет тип, с которым оперирует extension метод</param>
        /// <param name="messageList">List сообщений для пользователя</param>
        public static void AddMessageRange(this ITempDataDictionary tempData, List<Message> messageList)
        {
            foreach (var message in messageList)
            {
                tempData.AddMessage(message);
            }
        }

        /// <summary>
        /// Метод выполняет поиск имеющихся сообщений для пользователя
        /// </summary>
        /// <param name="tempData">Параметр определяет тип, с которым оперирует extension метод</param>
        /// <returns>List сообщений для пользователя</returns>
        public static List<MessageData> GetMessages(this ITempDataDictionary tempData)
        {
            object o;
            tempData.TryGetValue("Messages", out o);
            return o == null ? new List<MessageData>() : Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageData>>((string)o);

            //var messages = (List<Message>)tempData["Messages"];
            //return messages ?? new List<Message>();
        }
    }

    /// <summary>
    /// Тип, используемый в extension методах для ITempDataDictionary
    /// </summary>
    public abstract class Message
    {
        public abstract string Text { get; protected private set; }
        public abstract MessageType Type { get; protected private set; }
    }

    public class MessageData : Message
    {
        public MessageData(Message message)
        {
            if (message != null)
            {
                this.Text = message.Text;
                this.Type = message.Type;
            }
        }

        [Newtonsoft.Json.JsonProperty("Text")]
        public override string Text { get; private protected set; }
        [Newtonsoft.Json.JsonProperty("Type")]
        public override MessageType Type { get; private protected set; }
    }

    /// <summary>
    /// Тип сообщения для пользователя
    /// </summary>
    public enum MessageType
    {
        Success,
        Info,
        Warning,
        Danger
    }

    public static class SystemMessages
    {
        public sealed class LoginMessage : Message
        {
            public LoginMessage(Services.AuthServices.Models.LoginStatus loginStatus, string linkResendConfirmRegistrationEmail)
            {
                switch (loginStatus.Status)
                {
                    case Services.AuthServices.Models.LoginStatus.UserLoginStatus.DataIsNotValid:
                        {
                            Text = "Были указаны некорректные данные.";
                            Type = MessageType.Danger;
                            break;
                        }
                    case Services.AuthServices.Models.LoginStatus.UserLoginStatus.NoUser:
                        {
                            Text = "Вы ввели неправильную почту/логин или пароль. Попробуйте снова.";
                            Type = MessageType.Warning;
                            break;
                        }
                    case Services.AuthServices.Models.LoginStatus.UserLoginStatus.EmailNotConfirmed:
                        {
                            Text = $@"Ваша учётная запись ещё не активирована. 
                                      Для её активации, пожалуйста, перейдите по ссылке, которая указана в письме,
                                      отправленном на Вашу почту ({loginStatus.User.Email}). 
                                      <a href=""{linkResendConfirmRegistrationEmail}?email={loginStatus.User.Email}"" class=""alert-link""> Отправить повторно</a>";
                            Type = MessageType.Warning;
                            break;
                        }
                    case Services.AuthServices.Models.LoginStatus.UserLoginStatus.UserIsDeactivated:
                        {
                            Text = @"Вы ввели неправильную почту/логин или пароль. Попробуйте снова.";
                            Type = MessageType.Warning;
                            break;
                        }
                }
            }

            public override string Text { get; protected private set; }
            public override MessageType Type { get; protected private set; }
        }

        public sealed class SignUpMessage : Message
        {
            public SignUpMessage(Services.UserServices.Models.SignUpStatus signUpStatus)
            {
                switch (signUpStatus.Status)
                {
                    case Services.UserServices.Models.SignUpStatus.UserSignUpStatus.DataIsNotValid:
                        {
                            Text = "При регистрации были указаны некорректные данные.";
                            Type = MessageType.Danger;
                            break;
                        }
                    case Services.UserServices.Models.SignUpStatus.UserSignUpStatus.EmailAlreadyExist:
                        {
                            Text = "Регистрация с таким Email невозможна. Пользователь с таким Email уже зарегистрирован с системе.";
                            Type = MessageType.Warning;
                            break;
                        }
                    case Services.UserServices.Models.SignUpStatus.UserSignUpStatus.Success:
                        {
                            Text = "Для завершения регистрации, пожалуйста, следуйте инструкциям, отправленным на Вашу почту.";
                            Type = MessageType.Info;
                            break;
                        }
                }
            }

            public override string Text { get; protected private set; }
            public override MessageType Type { get; protected private set; }
        }

        public sealed class PasswordRecoveryMessage : Message
        {
            public override string Text { get; protected private set; } = "Неверный код генерации пароля";
            public override MessageType Type { get; protected private set; } = MessageType.Danger;
        }
    }
}
