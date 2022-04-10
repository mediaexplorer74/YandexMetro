// MainPage

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;

namespace YandexMetro
{
    //public partial class MainWindow : Window
    public sealed partial class MainPage : Page
    {
        Dijkstra g = new Dijkstra();

        List<Line> _line = new List<Line>();
        List<Button> _but = new List<Button>();
        List<TextBlock> _label = new List<TextBlock>();
        List<Polyline> _polylines = new List<Polyline>();

        private ScaleTransform st = new ScaleTransform();

        public MainPage()
        {
            InitializeComponent();
            //Mage.LayoutTransform = st;
            st.ScaleX = 0.7;
            st.ScaleY = 0.7;
            //GridRoad.Background = Brushes.White;

            Debug.WriteLine("Debug Console v0.01\nДобро пожаловать в консоль отслеживания программы: МПТ.Метро");
            Debug.WriteLine(new string('#', 60) + "\n");

            //Синяя ветка
            {
                g.add_vertex("Парнас", new Dictionary<string, int>() { { "Проспект_Просвещения", 3 } });
                g.add_vertex("Проспект_Просвещения", new Dictionary<string, int>() { { "Парнас", 3 }, { "Озерки", 2 } });
                g.add_vertex("Озерки", new Dictionary<string, int>() { { "Проспект_Просвещения", 2 }, { "Удельная", 3 } });
                g.add_vertex("Удельная", new Dictionary<string, int>() { { "Озерки", 3 }, { "Пионерская", 3 } });
                g.add_vertex("Пионерская", new Dictionary<string, int>() { { "Удельная", 3 }, { "Черная_речка", 3 } });
                g.add_vertex("Черная_речка", new Dictionary<string, int>() { { "Пионерская", 3 }, { "Петроградская", 4 } });
                g.add_vertex("Петроградская", new Dictionary<string, int>() { { "Черная_речка", 4 }, { "Горьковская", 2 } });
                g.add_vertex("Горьковская", new Dictionary<string, int>() { { "Петроградская", 2 }, { "Невский_проспект", 4 } });
                g.add_vertex("Невский_проспект", new Dictionary<string, int>() { { "Горьковская", 4 }, { "Сенная_площадь", 4 }, { "Гостиный_двор", 3 } });
                g.add_vertex("Сенная_площадь", new Dictionary<string, int>() { { "Невский_проспект", 4 }, { "Технологический_институт_2", 3 }, { "Садовая", 2 }, { "Спасская", 1 } });
                g.add_vertex("Технологический_институт_2", new Dictionary<string, int>() { { "Сенная_площадь", 3 }, { "Фрунзенская", 2 }, { "Технологический_институт_1", 2 } });
                g.add_vertex("Фрунзенская", new Dictionary<string, int>() { { "Технологический_институт_2", 2 }, { "Московские_ворота", 2 } });
                g.add_vertex("Московские_ворота", new Dictionary<string, int>() { { "Фрунзенская", 2 }, { "Электросила", 2 } });
                g.add_vertex("Электросила", new Dictionary<string, int>() { { "Московские_ворота", 2 }, { "Парк_Победы", 2 } });
                g.add_vertex("Парк_Победы", new Dictionary<string, int>() { { "Электросила", 2 }, { "Московская", 2 } });
                g.add_vertex("Московская", new Dictionary<string, int>() { { "Парк_Победы", 3 }, { "Звёздная", 4 } });
                g.add_vertex("Звёздная", new Dictionary<string, int>() { { "Московская", 4 }, { "Купчино", 3 } });
                g.add_vertex("Купчино", new Dictionary<string, int>() { { "Звёздная", 3 } });

                _line.Add(Парнас_Проспект_Просвещения_Line);
                _line.Add(Проспект_Просвещения_Озерки_Line);
                _line.Add(Озерки_Удельная_Line);
                _line.Add(Удельная_Пионерская_Line);
                _line.Add(Пионерская_Черная_речка_Line);
                _line.Add(Черная_речка_Петроградская_Line);
                _line.Add(Петроградская_Горьковская_Line);
                _line.Add(Горьковская_Невский_проспект_Line);
                _line.Add(Невский_проспект_Сенная_площадь_Line);
                _line.Add(Сенная_площадь_Технологический_институт_2_Line);
                _line.Add(Технологический_институт_2_Фрунзенская_Line);
                _line.Add(Фрунзенская_Московские_ворота_Line);
                _line.Add(Московские_ворота_Электросила_Line);
                _line.Add(Электросила_Парк_Победы_Line);
                _line.Add(Парк_Победы_Московская_Line);
                _line.Add(Московская_Звёздная_Line);
                _line.Add(Звёздная_Купчино_Line);


                _but.Add(Парнас);
                _but.Add(Проспект_Просвещения);
                _but.Add(Озерки);
                _but.Add(Удельная);
                _but.Add(Пионерская);
                _but.Add(Черная_речка);
                _but.Add(Петроградская);
                _but.Add(Горьковская);
                _but.Add(Невский_проспект);
                _but.Add(Сенная_площадь);
                _but.Add(Технологический_институт_2);
                _but.Add(Фрунзенская);
                _but.Add(Московские_ворота);
                _but.Add(Электросила);
                _but.Add(Парк_Победы);
                _but.Add(Московская);
                _but.Add(Звёздная);
                _but.Add(Купчино);

                _label.Add(l_Парнас);
                _label.Add(l_Проспект_Просвещения);
                _label.Add(l_Озерки);
                _label.Add(l_Удельная);
                _label.Add(l_Пионерская);
                _label.Add(l_Черная_речка);
                _label.Add(l_Петроградская);
                _label.Add(l_Горьковская);
                _label.Add(l_Невский_проспект);
                _label.Add(l_Сенная_площадь);
                _label.Add(l_Технологический_институт_2);
                _label.Add(l_Фрунзенская);
                _label.Add(l_Московские_ворота);
                _label.Add(l_Электросила);
                _label.Add(l_Парк_Победы);
                _label.Add(l_Московская);
                _label.Add(l_Звёздная);
                _label.Add(l_Купчино);
            }
            //Зеленая ветка
            {
                g.add_vertex("Маяковская", new Dictionary<string, int>() { { "Гостиный_двор", 3 }, { "Площадь_Восстания", 1 }, { "Площадь_Александра_Невского_1", 4 } });
                g.add_vertex("Гостиный_двор", new Dictionary<string, int>() { { "Невский_проспект", 3 }, { "Василеостровская", 4 }, { "Маяковская", 3 } });
                g.add_vertex("Василеостровская", new Dictionary<string, int>() { { "Гостиный_двор", 3 }, { "Приморская", 4 } });
                g.add_vertex("Приморская", new Dictionary<string, int>() { { "Василеостровская", 4 }, { "Новокрестовская", 4 } });
                g.add_vertex("Новокрестовская", new Dictionary<string, int>() { { "Приморская", 4 }, { "Беговая", 4 } });
                g.add_vertex("Беговая", new Dictionary<string, int>() { { "Новокрестовская", 4 } });
                g.add_vertex("Площадь_Александра_Невского_1", new Dictionary<string, int>() { { "Маяковская", 4 }, { "Елизаровская", 5 }, { "Площадь_Александра_Невского_2", 1 } });
                g.add_vertex("Елизаровская", new Dictionary<string, int>() { { "Площадь_Александра_Невского_1", 5 }, { "Ломоносовская", 3 } });
                g.add_vertex("Ломоносовская", new Dictionary<string, int>() { { "Елизаровская", 3 }, { "Пролетарская", 3 } });
                g.add_vertex("Пролетарская", new Dictionary<string, int>() { { "Ломоносовская", 3 }, { "Обухово", 3 } });
                g.add_vertex("Обухово", new Dictionary<string, int>() { { "Пролетарская", 3 }, { "Рыбацкое", 4 } });
                g.add_vertex("Рыбацкое", new Dictionary<string, int>() { { "Обухово", 4 } });

                _polylines.Add(Гостиный_двор_Василеостровская_Line);
                _polylines.Add(Маяковская_Площадь_Александра_Невского_1_Line);

                _line.Add(Василеостровская_Приморская_Line);
                _line.Add(Приморская_Новокрестовская_Line);
                _line.Add(Новокрестовская_Беговая_Line);
                _line.Add(Гостиный_двор_Маяковская_Line);
                _line.Add(Площадь_Александра_Невского_1_Елизаровская_Line);
                _line.Add(Елизаровская_Ломоносовская_Line);
                _line.Add(Ломоносовская_Пролетарская_Line);
                _line.Add(Пролетарская_Обухово_Line);
                _line.Add(Обухово_Рыбацкое_Line);

                _but.Add(Василеостровская);
                _but.Add(Гостиный_двор);
                _but.Add(Приморская);
                _but.Add(Новокрестовская);
                _but.Add(Беговая);
                _but.Add(Площадь_Александра_Невского_1);
                _but.Add(Маяковская);
                _but.Add(Елизаровская);
                _but.Add(Ломоносовская);
                _but.Add(Пролетарская);
                _but.Add(Обухово);
                _but.Add(Рыбацкое);

                _label.Add(l_Василеостровская);
                _label.Add(l_Гостиный_двор);
                _label.Add(l_Приморская);
                _label.Add(l_Новокрестовская);
                _label.Add(l_Беговая);
                _label.Add(l_Площадь_Александра_Невского_1);
                _label.Add(l_Елизаровская);
                _label.Add(l_Ломоносовская);
                _label.Add(l_Пролетарская);
                _label.Add(l_Рыбацкое);
                _label.Add(l_Обухово);
                _label.Add(l_Маяковская);
            }
            //Красная ветка
            {
                g.add_vertex("Девяткино", new Dictionary<string, int>() { { "Гражданский_проспект", 3 } });
                g.add_vertex("Гражданский_проспект", new Dictionary<string, int>() { { "Девяткино", 3 }, { "Академическая", 3 } });
                g.add_vertex("Академическая", new Dictionary<string, int>() { { "Политехническая", 2 }, { "Гражданский_проспект", 3 } });
                g.add_vertex("Политехническая", new Dictionary<string, int>() { { "Академическая", 2 }, { "Площадь_Мужества", 3 } });
                g.add_vertex("Площадь_Мужества", new Dictionary<string, int>() { { "Политехническая", 3 }, { "Лесная", 3 } });
                g.add_vertex("Лесная", new Dictionary<string, int>() { { "Выборгская", 3 }, { "Площадь_Мужества", 3 } });
                g.add_vertex("Выборгская", new Dictionary<string, int>() { { "Лесная", 3 }, { "Площадь_Ленина", 2 } });
                g.add_vertex("Площадь_Ленина", new Dictionary<string, int>() { { "Чернышевская", 3 }, { "Выборгская", 2 } });
                g.add_vertex("Чернышевская", new Dictionary<string, int>() { { "Площадь_Ленина", 3 }, { "Площадь_Восстания", 2 } });
                g.add_vertex("Площадь_Восстания", new Dictionary<string, int>() { { "Маяковская", 1 }, { "Владимирская", 2 }, { "Чернышевская", 2 } });
                g.add_vertex("Владимирская", new Dictionary<string, int>() { { "Достоевская", 1 }, { "Площадь_Восстания", 2 }, { "Пушкинская", 2 } });
                g.add_vertex("Пушкинская", new Dictionary<string, int>() { { "Звенигородская", 3 }, { "Владимирская", 2 }, { "Технологический_институт_1", 2 } });
                g.add_vertex("Технологический_институт_1", new Dictionary<string, int>() { { "Технологический_институт_2", 2 }, { "Пушкинская", 2 }, { "Балтийская", 2 } });
                g.add_vertex("Балтийская", new Dictionary<string, int>() { { "Технологический_институт_1", 2 }, { "Нарвская", 3 } });

                g.add_vertex("Нарвская", new Dictionary<string, int>() { { "Балтийская", 3 }, { "Кировский_завод", 4 } });
                g.add_vertex("Кировский_завод", new Dictionary<string, int>() { { "Автово", 2 }, { "Нарвская", 4 } });
                g.add_vertex("Автово", new Dictionary<string, int>() { { "Ленинский_проспект", 3 }, { "Кировский_завод", 2 } });
                g.add_vertex("Ленинский_проспект", new Dictionary<string, int>() { { "Проспект_Ветеранов", 2 }, { "Автово", 3 } });
                g.add_vertex("Проспект_Ветеранов", new Dictionary<string, int>() { { "Ленинский_проспект", 2 } });



                _polylines.Add(Технологический_институт_1_Балтийская_Line);

                _line.Add(Девяткино_Гражданский_проспект_Line);
                _line.Add(Гражданский_проспект_Академическая_Line);
                _line.Add(Академическая_Политехническая_Line);
                _line.Add(Политехническая_Площадь_Мужества_Line);
                _line.Add(Площадь_Мужества_Лесная_Line);
                _line.Add(Лесная_Выборгская_Line);
                _line.Add(Выборгская_Площадь_Ленина_Line);
                _line.Add(Площадь_Ленина_Чернышевская_Line);
                _line.Add(Чернышевская_Площадь_Восстания_Line);
                _line.Add(Площадь_Восстания_Владимирская_Line);
                _line.Add(Владимирская_Пушкинская_Line);
                _line.Add(Пушкинская_Технологический_институт_1_Line);
                _line.Add(Балтийская_Нарвская_Line);
                _line.Add(Нарвская_Кировский_завод_Line);
                _line.Add(Кировский_завод_Автово_Line);
                _line.Add(Автово_Ленинский_проспект_Line);
                _line.Add(Ленинский_проспект_Проспект_Ветеранов_Line);


                _but.Add(Девяткино);
                _but.Add(Гражданский_проспект);
                _but.Add(Академическая);
                _but.Add(Политехническая);
                _but.Add(Площадь_Мужества);
                _but.Add(Лесная);
                _but.Add(Выборгская);
                _but.Add(Площадь_Ленина);
                _but.Add(Чернышевская);
                _but.Add(Площадь_Восстания);
                _but.Add(Владимирская);
                _but.Add(Пушкинская);
                _but.Add(Технологический_институт_1);
                _but.Add(Балтийская);
                _but.Add(Нарвская);
                _but.Add(Кировский_завод);
                _but.Add(Автово);
                _but.Add(Ленинский_проспект);
                _but.Add(Проспект_Ветеранов);


                _label.Add(l_Девяткино);
                _label.Add(l_Гражданский_проспект);
                _label.Add(l_Академическая);
                _label.Add(l_Политехническая);
                _label.Add(l_Площадь_Мужества);
                _label.Add(l_Лесная);
                _label.Add(l_Выборгская);
                _label.Add(l_Площадь_Ленина);
                _label.Add(l_Чернышевская);
                _label.Add(l_Площадь_Восстания);
                _label.Add(l_Владимирская);
                _label.Add(l_Пушкинская);
                _label.Add(l_Технологический_институт_1);
                _label.Add(l_Балтийская);
                _label.Add(l_Нарвская);
                _label.Add(l_Кировский_завод);
                _label.Add(l_Автово);
                _label.Add(l_Ленинский_проспект);
                _label.Add(l_Проспект_Ветеранов);
            }
            //Фиолетовая ветка
            {
                g.add_vertex("Комендантский_проспект", new Dictionary<string, int>() { { "Старая_Деревня", 3 } });
                g.add_vertex("Старая_Деревня", new Dictionary<string, int>() { { "Комендантский_проспект", 3 }, { "Крестовский_остров", 3 } });
                g.add_vertex("Крестовский_остров", new Dictionary<string, int>() { { "Старая_Деревня", 3 }, { "Чкаловская", 4 } });
                g.add_vertex("Чкаловская", new Dictionary<string, int>() { { "Крестовский_остров", 4 }, { "Спортивная", 2 } });
                g.add_vertex("Спортивная", new Dictionary<string, int>() { { "Чкаловская", 2 }, { "Адмиралтейская", 3 } });
                g.add_vertex("Адмиралтейская", new Dictionary<string, int>() { { "Спортивная", 3 }, { "Садовая", 3 } });
                g.add_vertex("Садовая", new Dictionary<string, int>() { { "Адмиралтейская", 3 }, { "Сенная_площадь", 2 }, { "Звенигородская", 4 }, { "Спасская", 1 } });
                g.add_vertex("Звенигородская", new Dictionary<string, int>() { { "Садовая", 4 }, { "Обводный_канал", 3 }, { "Пушкинская", 3 } });
                g.add_vertex("Обводный_канал", new Dictionary<string, int>() { { "Звенигородская", 3 }, { "Волковская", 3 } });
                g.add_vertex("Волковская", new Dictionary<string, int>() { { "Обводный_канал", 3 }, { "Бухарестская", 3 } });
                g.add_vertex("Бухарестская", new Dictionary<string, int>() { { "Волковская", 3 }, { "Международная", 3 } });
                g.add_vertex("Международная", new Dictionary<string, int>() { { "Бухарестская", 3 }, { "Проспект_Славы", 2 } });
                g.add_vertex("Проспект_Славы", new Dictionary<string, int>() { { "Международная", 2 }, { "Дунайская", 3 } });
                g.add_vertex("Дунайская", new Dictionary<string, int>() { { "Проспект_Славы", 3 }, { "Шушары", 3 } });
                g.add_vertex("Шушары", new Dictionary<string, int>() { { "Дунайская", 3 } });


                _line.Add(Комендантский_проспект_Старая_Деревня_Line);
                _line.Add(Старая_Деревня_Крестовский_остров_Line);
                _line.Add(Крестовский_остров_Чкаловская_Line);
                _line.Add(Чкаловская_Спортивная_Line);
                _line.Add(Спортивная_Адмиралтейская_Line);
                _line.Add(Адмиралтейская_Садовая_Line);
                _line.Add(Садовая_Звенигородская_Line);
                _line.Add(Звенигородская_Обводный_канал_Line);
                _line.Add(Обводный_канал_Волковская_Line);
                _line.Add(Волковская_Бухарестская_Line);
                _line.Add(Бухарестская_Международная_Line);
                _line.Add(Международная_Проспект_Славы_Line);
                _line.Add(Проспект_Славы_Дунайская_Line);
                _line.Add(Дунайская_Шушары_Line);

                _but.Add(Комендантский_проспект);
                _but.Add(Старая_Деревня);
                _but.Add(Крестовский_остров);
                _but.Add(Чкаловская);
                _but.Add(Спортивная);
                _but.Add(Адмиралтейская);
                _but.Add(Садовая);
                _but.Add(Звенигородская);
                _but.Add(Обводный_канал);
                _but.Add(Волковская);
                _but.Add(Бухарестская);
                _but.Add(Международная);
                _but.Add(Проспект_Славы);
                _but.Add(Дунайская);
                _but.Add(Шушары);

                _label.Add(l_Комендантский_проспект);
                _label.Add(l_Старая_Деревня);
                _label.Add(l_Крестовский_остров);
                _label.Add(l_Чкаловская);
                _label.Add(l_Спортивная);
                _label.Add(l_Адмиралтейская);
                _label.Add(l_Садовая);
                _label.Add(l_Звенигородская);
                _label.Add(l_Обводный_канал);
                _label.Add(l_Волковская);
                _label.Add(l_Бухарестская);
                _label.Add(l_Международная);
                _label.Add(l_Проспект_Славы);
                _label.Add(l_Дунайская);
                _label.Add(l_Шушары);
            }
            //Жёлтая ветка
            {
                g.add_vertex("Спасская", new Dictionary<string, int>() { { "Сенная_площадь", 1 }, { "Садовая", 2 }, { "Достоевская", 4 } });
                g.add_vertex("Достоевская", new Dictionary<string, int>() { { "Спасская", 4 }, { "Лиговский_проспект", 2 }, { "Владимирская", 1 } });
                g.add_vertex("Лиговский_проспект", new Dictionary<string, int>() { { "Достоевская", 2 }, { "Площадь_Александра_Невского_2", 2 } });
                g.add_vertex("Площадь_Александра_Невского_2", new Dictionary<string, int>() { { "Лиговский_проспект", 2 }, { "Площадь_Александра_Невского_1", 1 }, { "Новочеркасская", 3 } });
                g.add_vertex("Новочеркасская", new Dictionary<string, int>() { { "Площадь_Александра_Невского_2", 3 }, { "Ладожская", 3 } });
                g.add_vertex("Ладожская", new Dictionary<string, int>() { { "Новочеркасская", 3 }, { "Проспект_большевиков", 3 } });
                g.add_vertex("Проспект_большевиков", new Dictionary<string, int>() { { "Ладожская", 3 }, { "Улица_Дыбенко", 2 } });
                g.add_vertex("Улица_Дыбенко", new Dictionary<string, int>() { { "Проспект_большевиков", 2 } });


                _but.Add(Спасская);
                _but.Add(Достоевская);
                _but.Add(Лиговский_проспект);
                _but.Add(Площадь_Александра_Невского_2);
                _but.Add(Новочеркасская);
                _but.Add(Ладожская);
                _but.Add(Проспект_большевиков);
                _but.Add(Улица_Дыбенко);

                _label.Add(l_Спасская);
                _label.Add(l_Достоевская);
                _label.Add(l_Лиговский_проспект);
                _label.Add(l_Площадь_Александра_Невского_2);
                _label.Add(l_Новочеркасская);
                _label.Add(l_Ладожская);
                _label.Add(l_Проспект_большевиков);
                _label.Add(l_Улица_Дыбенко);

                _line.Add(Спасская_Достоевская_Line);
                _line.Add(Достоевская_Лиговский_проспект_Line);
                _polylines.Add(Лиговский_проспект_Площадь_Александра_Невского_2_Line);
                _polylines.Add(Площадь_Александра_Невского_2_Новочеркасская_Line);
                _line.Add(Новочеркасская_Ладожская_Line);
                _line.Add(Ладожская_Проспект_большевиков_Line);
                _line.Add(Проспект_большевиков_Улица_Дыбенко_Line);
            }

            foreach (var item in _but)
            {
                if (item.Name.Contains("_"))
                {
                    combox1.Items.Add(item.Name.Replace('_', ' '));
                    combox2.Items.Add(item.Name.Replace('_', ' '));
                }
                else
                {
                    combox1.Items.Add(item.Name);
                    combox2.Items.Add(item.Name);
                }
            }
        }

        private void Line_Change(object sender, RoutedEventArgs e)
        {

            foreach (var item in _polylines)
            {
                for (int i = 1; i < Dijkstra._nameStation.Length; i++)
                {
                    if (item.Name.Remove(item.Name.LastIndexOf('_'), 5) == Dijkstra._nameStation[i] + "_" + Dijkstra._nameStation[i - 1])
                    {
                        PanelGrid.Children.Add(new Polyline() { Name = item.Name, Points = item.Points, StrokeThickness = 9, Stroke = item.Stroke, Margin = item.Margin });
                    }
                }
            }

            foreach (var item in _line)
            {
                for (int i = 1; i < Dijkstra._nameStation.Length; i++)
                {
                    if (item.Name.Remove(item.Name.LastIndexOf('_'), 5) == Dijkstra._nameStation[i] + "_" + Dijkstra._nameStation[i - 1])
                    {
                        PanelGrid.Children.Add(new Line() { Name = item.Name, X1 = item.X1, X2 = item.X2, Y1 = item.Y1, Y2 = item.Y2, StrokeThickness = 9, Stroke = item.Stroke, Margin = item.Margin });
                    }
                }
            }

            foreach (var item in _but)
            {
                for (int i = 1; i < Dijkstra._nameStation.Length; i++)
                {
                    if (item.Name == Dijkstra._nameStation[i])
                    {
                        PanelGrid.Children.Add(new Button() { Name = item.Name, Margin = item.Margin, Height = item.Height, Width = item.Width, Template = item.Template });
                    }
                }
            }

            foreach (var item in _label)
            {
                for (int i = 1; i < Dijkstra._nameStation.Length; i++)
                {
                    if (item.Name.Remove(0, 2) == Dijkstra._nameStation[i])
                    {
                        PanelGrid.Children.Add(new Label() { Name = item.Name, Margin = item.Margin, Content = item.Content, Height = item.Height, Width = item.Width, HorizontalAlignment = item.HorizontalAlignment, VerticalAlignment = item.VerticalAlignment });
                    }
                }
            }
        }

        string _begin = null, _end = null;

        private void But_Click(object sender, RoutedEventArgs e)
        {
            if (combox1.Text.Contains(" "))
            {
                _begin = combox1.Text.Replace(' ', '_');
            }
            else
            {
                _begin = combox1.Text;
            }
            if (combox2.Text.Contains(" "))
            {
                _end = combox2.Text.Replace(' ', '_');
            }
            else
            {
                _end = combox2.Text;
            }

            PanelGrid.Children.RemoveRange(0, PanelGrid.Children.Count);
            for (int i = 0; i < Dijkstra._nameStation.Length - 1; i++)
            {
                Dijkstra._nameStation[i] = "";
            }
            bool a = false;

            if (combox1.Text != combox2.Text)
            {
                try
                {
                    time.Text = "";
                    g.shortest_path(_begin, _end).ForEach(x => Console.Write(x + " -> "));
                    Console.WriteLine(combox1.Text + " = " + Dijkstra._value + "мин.");
                    Line_Change(this, null);
                    GoTo.Opacity = 0.3;

                    time.Text = "≈" + Dijkstra._value + "мин.";
                    g.shortest_path(_end, _begin).ForEach(x => Console.Write(x + " -> "));
                    Console.WriteLine(combox1.Text + " = " + Dijkstra._value + "мин.");
                    Line_Change(this, null);



                    if (label0.Content.ToString() != "" && label0.Content.ToString() != label1.Content.ToString() && label0.Content.ToString() != label2.Content.ToString() && label0.Content.ToString() != label3.Content.ToString() && label0.Content.ToString() != label4.Content.ToString() && label0.Content.ToString() != combox1.Text + " - " + combox2.Text)
                    {
                        if (label1.Content.ToString() != "" && label1.Content.ToString() != label2.Content.ToString() && label1.Content.ToString() != label3.Content.ToString() && label1.Content.ToString() != label4.Content.ToString() && label1.Content.ToString() != label0.Content.ToString() && label1.Content.ToString() != combox1.Text + " - " + combox2.Text)
                        {
                            if (label2.Content.ToString() != "" && label2.Content.ToString() != label3.Content.ToString() && label2.Content.ToString() != label4.Content.ToString() && label2.Content.ToString() != label0.Content.ToString() && label2.Content.ToString() != label1.Content.ToString() && label2.Content.ToString() != combox1.Text + " - " + combox2.Text)
                            {
                                if (label3.Content.ToString() != "" && label3.Content.ToString() != label4.Content.ToString() && label3.Content.ToString() != label0.Content.ToString() && label3.Content.ToString() != label1.Content.ToString() && label3.Content.ToString() != label2.Content.ToString() && label3.Content.ToString() != combox1.Text + " - " + combox2.Text)
                                {
                                    if (label4.Content.ToString() != "" && label4.Content.ToString() != label0.Content.ToString() && label4.Content.ToString() != label1.Content.ToString() && label4.Content.ToString() != label2.Content.ToString() && label4.Content.ToString() != label3.Content.ToString() && label4.Content.ToString() != combox1.Text + " - " + combox2.Text)
                                    {
                                        label4.Content = label3.Content;
                                        label3.Content = label2.Content;
                                        label2.Content = label1.Content;
                                        label1.Content = label0.Content;
                                        label0.Content = combox1.Text + " - " + combox2.Text;
                                    }
                                    else if (label4.Content.ToString() == "")
                                    {
                                        label4.Content = combox1.Text + " - " + combox2.Text;
                                    }
                                }
                                else if (label3.Content.ToString() == "")
                                {
                                    label3.Content = combox1.Text + " - " + combox2.Text;
                                }
                            }
                            else if (label2.Content.ToString() == "")
                            {
                                label2.Content = combox1.Text + " - " + combox2.Text;
                            }
                        }
                        else if (label1.Content.ToString() == "")
                        {
                            label1.Content = combox1.Text + " - " + combox2.Text;
                        }
                    }
                    else if (label0.Content.ToString() == "")
                    {
                        label0.Content = combox1.Text + " - " + combox2.Text;
                    }
                }
                catch
                {
                    MessageBox.Show("Введите корректно данные!", "Ошибка поиска!", MessageBoxButton.OK, MessageBoxImage.Information);
                    combox1.Text = "";
                    combox2.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Введите корректно данные!", "Ошибка поиска!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            label0.Content = "";
            label1.Content = "";
            label2.Content = "";
            label3.Content = "";
            label4.Content = "";
        }

        string _stationForHistory;
        private void label0_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            _stationForHistory = label.Content.ToString();
            _stationForHistory.Trim();
            combox1.Text = _stationForHistory.Remove(_stationForHistory.IndexOf('-')).Trim();
            _stationForHistory = _stationForHistory.Remove(0, _stationForHistory.LastIndexOf('-')).Trim();
            combox2.Text = _stationForHistory.Remove(0, 2);
            But_Click(this, null);
        }

        bool But_Zn = false;
        private void Button_Move_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            if (But_Zn == false)
            {
                doubleAnimation.From = Panel.Width;
                doubleAnimation.Duration = TimeSpan.FromSeconds(0.25);
                doubleAnimation.To = 200;
                Panel.BeginAnimation(WidthProperty, doubleAnimation);
                doubleAnimation.FillBehavior = FillBehavior.HoldEnd;
            }
            else
            {
                doubleAnimation.From = Panel.Width;
                doubleAnimation.Duration = TimeSpan.FromSeconds(0.25);
                doubleAnimation.To = 1;
                Panel.BeginAnimation(WidthProperty, doubleAnimation);
            }
            But_Zn = !But_Zn;
        }

        private void CheckBox__Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_.IsChecked == true)
            {
                GridRoad.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#26272b"));
                label0.Foreground = Brushes.Black;
                label1.Foreground = Brushes.Black;
                label2.Foreground = Brushes.Black;
                label3.Foreground = Brushes.Black;
                label4.Foreground = Brushes.Black;
                l_Гостиный_двор.Foreground = Brushes.Black;
                l_Спасская.Foreground = Brushes.Black;
                l_Достоевская.Foreground = Brushes.Black;
            }
            else
            {
                GridRoad.Background = Brushes.White;
            }
        }

        private Point? _movePoint;

        private void Btn_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _movePoint = e.GetPosition(Btn);
            Btn.CaptureMouse();
        }

        private void Btn_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _movePoint = null;
            Btn.ReleaseMouseCapture();
        }

        int si = 0;
        private void Btn_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_movePoint == null)
                return;
            var p = e.GetPosition(this) - (Vector)_movePoint.Value;
            if (p.X + 400 < si + this.Width && p.X > -180)
                Canvas.SetLeft(Btn, p.X);
            if (p.Y < si - 340 + this.Height && p.Y > -400)
                Canvas.SetTop(Btn, p.Y);
        }

        private void Combox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ComboBox txtbox = (ComboBox)sender;
            if (txtbox.Text == "")
            {
                foreach (var item in _line)
                {
                    PanelGrid.Children.RemoveRange(0, PanelGrid.Children.Count);
                    GoTo.Opacity = 1;
                }
                time.Text = "";
            }
        }

        private void DeleteComboBox1_Click(object sender, RoutedEventArgs e)
        {
            combox1.Text = "";
            foreach (var item in _line)
            {
                PanelGrid.Children.RemoveRange(0, PanelGrid.Children.Count);
                GoTo.Opacity = 1;
            }
            time.Text = "";
        }

        private void DeleteComboBox2_Click(object sender, RoutedEventArgs e)
        {
            combox2.Text = "";
            foreach (var item in _line)
            {
                PanelGrid.Children.RemoveRange(0, PanelGrid.Children.Count);
                GoTo.Opacity = 1;
            }
            time.Text = "";
        }

        private void GridRoad_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point point = e.GetPosition(Mage);
            var position = GridRoad.Margin;
            if (e.Delta > 0 && st.ScaleX <= 1.4)
            {
                st.ScaleX *= 1.1;
                st.ScaleY *= 1.1;
                position.Left -= point.X * 0.1 * st.ScaleX;
                position.Top -= point.Y * 0.1 * st.ScaleY;
            }
            if (e.Delta < 0 && st.ScaleX >= 0.8)
            {
                st.ScaleX /= 1.1;
                st.ScaleY /= 1.1;
                position.Left += point.X * 0.1 * st.ScaleX;
                position.Top += point.Y * 0.1 * st.ScaleY;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                si = 800;
                this.Width = 800;
                this.Height = 500;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                si = 0;
                Canvas.SetLeft(Btn, 150);
                Canvas.SetTop(Btn, 10);
            }
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            Button but = (Button)sender;
            if (combox1.Text == "")
            {
                if (but.Name.Contains("_"))
                {
                    _begin = but.Name;
                    combox1.Text = but.Name.Replace('_', ' ');
                }
                else
                {
                    _begin = but.Name;
                    combox1.Text = but.Name;
                }
            }
            else if (combox2.Text == "")
            {
                if (but.Name.Contains("_"))
                {
                    _end = but.Name;
                    combox2.Text = but.Name.Replace('_', ' ');
                }
                else
                {
                    _end = but.Name;
                    combox2.Text = but.Name;
                }
            }
            if (combox1.Text != "" && combox2.Text != "")
            {
                But_Click(this, null);
                But_Zn = false;
                Button_Move_Click(this, null);
            }
        }
    }
}
