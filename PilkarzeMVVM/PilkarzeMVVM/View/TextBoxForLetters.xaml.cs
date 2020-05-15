using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PilkarzeMVVM.View
{
    /// <summary>
    /// Interaction logic for TextBoxForLetters.xaml
    /// </summary>
    public partial class TextBoxForLetters : UserControl
    {
        public TextBoxForLetters()
        {
            InitializeComponent();
        }

        #region Zdarznie własne

        //rejestracja zdarzenia tak, żeby możliwe było jego bindowanie
        public static readonly RoutedEvent TextChangedEvent =
        EventManager.RegisterRoutedEvent("TabItemSelected",
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(TextBoxForLetters));

        //definicja zdarzenia NumberChanged
        public event RoutedEventHandler NumberChanged
        {
            add { AddHandler(TextChangedEvent, value); }
            remove { RemoveHandler(TextChangedEvent, value); }
        }

        //Metoda pomocnicza wywołująca zdarzenie
        //przy okazji metoda ta tworzy obiekt argument przekazywany przez to zdarzenie
        private void RaiseTextChanged()
        {
            //argument zdarzenia
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(TextBoxForLetters.TextChangedEvent);
            //wywołanie zdarzenia
            RaiseEvent(newEventArgs);
        }

        #endregion Zdarznie własne

        #region Własność zależna

        //zarejestrowanie własności zależenej - taki mechanizm konieczny jest
        // aby możliwe było Bindowanie tej właśności z innnymi obiektami
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(TextBoxForLetters),
                new FrameworkPropertyMetadata(null)
            );

        //"czysta" właściwość powiązania z właściwości zależną
        //do niej będziemy się odnosić w XAMLU
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion Własność zależna

        #region Metody obsługujące wewnętrzne zdarzenia kontrolki

        //zdarzenie wywoływane zanim zmianie ulegnie tekst textBox-a
        //e.Text  - string zawierający ostatnio dopisany znakm jeszcze niedodany do
        //własności Text obiektu textBox
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.All(char.IsLetter)) e.Handled = true;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //przy każdej zmianie tekstu w polu textBox
            //wyrzucamy zdarzenie, które informuje o tym,
            //że zmieniła się liczba
            RaiseTextChanged();
        }

        #endregion Metody obsługujące wewnętrzne zdarzenia kontrolki

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }
    }
}