using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectOrderFood.Services
{
    public class Notification
    {
        //лист для хранения уведомлений для официанта
        private static List<string> NotificationMessage = new List<string>();
        //добавление сообщений (из кухни официанту)
        public static void AddNotification(string str)
        {
            NotificationMessage.Add(str);
        }
        //получение сообщений (уведомление для официанта)

        public static List<string> GetListMessage()
        {
            return NotificationMessage.ToList();
        }
    }
}