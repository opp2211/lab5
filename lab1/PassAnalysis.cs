using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class PassAnalysis
    {
        int[] passPowerGr = { 0, 10, 26, 26, 33 }; // Мощность символов по группам

        int alphPower = 0;

        string password = "";
        int s = 1000; // Скорость перебора паролей в секунду
        int m = 0;    // Неправв. попытки
        int v = 0;    // Пауза (сек) после m

        public string Password // Установка пароля
        {
            set
            {
                password = value;
            }
        }
        public int S // Установка s
        {
            set
            {
                s = value;
            }
        }
        public int M // Установка m
        {
            set
            {
                m = value;
            }
        }
        public int V // Установка v
        {
            set
            {
                v = value;
            }
        }

        public double NumOp // Рассчет и выдача кол-ва возможных операций
        {
            get
            {
                if (password.Length == 0)
                    return 0;
                else
                    return Math.Pow(alphPower, password.Length);
            }
        }

        public string TimeOp // Рассчет и выдача времени перебора
        {
            get
            {
                double t;
                double delayTimes = 0;
                if (s == 0)
                    t = 0;
                else if (m == 0 || v == 0)
                    t = NumOp / s;
                else
                    delayTimes = NumOp % m == 0 ? (NumOp / m) - 1 : Math.Floor(NumOp / m);
                    t = (NumOp / s) + delayTimes * v;

                double sec = t % 60;
                t = Math.Floor(t / 60);

                double min = t % 60;
                t = Math.Floor(t / 60);

                double hours = t % 24;
                t = Math.Floor(t / 24);

                double days = t % 30;
                t = Math.Floor(t / 30);

                double months = t % 12;
                double years = Math.Floor(t / 12);

                return string.Format($"{years} лет {months} месяцев {days} дней {hours} часов {min} минут {sec} секунд");
            }
        }
        public int AlphPower // Рассчет и выдача мощности алфавита
        {
            get
            {
                PowerAnalysis();
                return alphPower;
            }
        }
        int PowerAnalysis() // Рассчет мощности алфавита
        {
            bool[] isbelongToGr = new bool[5];
            for (int i = 0; i < password.Length; i++)
            {
                int sw = PowerChar(password[i]);
                switch (sw)
                {
                    case 0:
                        alphPower = 0;
                        return 0;
                    default:
                        isbelongToGr[sw] = true;
                        break;
                }
            }
            alphPower = 0;
            for (int i = 1; i < passPowerGr.Length; i++)
            {
                if (isbelongToGr[i])
                    alphPower += passPowerGr[i];
            }
            return alphPower;
        }

        int PowerChar(char ch) // Рассчет мощности символа
        {
            if (ch >= 48 && ch <= 57)
            {
                // Цифры +10
                return 1;
            }
            if (ch >= 65 && ch <= 90)
            {
                // Пропись +26
                return 2;
            }
            if (ch >= 97 && ch <= 122)
            {
                // Строчные +26
                return 3;
            }
            if (ch >= 32 && ch <= 126)
            {
                // Спец. символы +33
                return 4;
            }
            else
            {
                // Неверный ввод
                return 0;
            }
        }
    }
}
