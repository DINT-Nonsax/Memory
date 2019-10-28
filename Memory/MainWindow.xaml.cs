using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Memory
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Border ParejaUno { get; set; }
        public Border ParejaDos { get; set; }
        DispatcherTimer timer = new DispatcherTimer();
        List<Border> listaBordes = new List<Border>();


        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += timer_Tick;
        }

        private void nivel_buttonClick(object sender, RoutedEventArgs e)
        {
            if (baja_radioButton.IsChecked == true)
                loadLevel(3);
            else if (media_radioButton.IsChecked == true)
                loadLevel(4);
            else if (alta_radioButton.IsChecked == true)
                loadLevel(5);
            progressBar.Value = 0;
        }

        private void loadLevel(int numero)
        {
            progressBar.Maximum = (double)numero * 4 / 2;
            contenedor.Children.Clear();
            contenedor.RowDefinitions.Clear();
            contenedor.ColumnDefinitions.Clear();
            contenedor.Background = Brushes.Aqua;
            char[] cartas = CrearCartas(numero); 
            for (int i = 0; i < numero; i++)
                contenedor.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 4; i++)
                contenedor.ColumnDefinitions.Add(new ColumnDefinition());
            int contadorCartas = 0;
            for (int i = 0; i < numero; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Viewbox viewBox = new Viewbox();                    
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "s";
                    textBlock.Tag = cartas[contadorCartas].ToString();
                    textBlock.Style = (Style)Resources["carta"];
                    viewBox.Child = textBlock;


                    Border borde = new Border();
                    borde.Style = (Style)Resources["bordeCarta"];
                    //borde.Margin = new Thickness(3);
                    //borde.BorderThickness = new Thickness(1);
                    //borde.BorderBrush = Brushes.Black;
                   // borde.Background = Brushes.Brown;
                    //borde.CornerRadius = new CornerRadius(5);
                    borde.Child = viewBox;
                    borde.MouseDown += new MouseButtonEventHandler(carta_click);
                    contenedor.Children.Add(borde);
                    Grid.SetRow(borde, i);
                    Grid.SetColumn(borde, j);
                    listaBordes.Add(borde);
                    contadorCartas++;
                }
            }
        }
        // L , N, O, V, X, Y
        private char[] CrearCartas(int filas)
        {
            char[] nuevasCartas = null;
            switch (filas)
            {
                case 3:
                    nuevasCartas = new char[] { 'L', 'N', 'O', 'V', 'X', 'Y', 'L', 'N', 'O', 'V', 'X', 'Y' };
                    break;
                case 4:
                    nuevasCartas = new char[] { 'L', 'N', 'O', 'V', 'X', 'Y', '-', '$', 'L', 'N', 'O', 'V', 'X', 'Y', '-', '$' };
                    break;
                case 5:
                    nuevasCartas = new char[] { 'L', 'N', 'O', 'V', 'X', 'Y', '-', '$', 'b', 'i', 'L', 'N', 'O', 'V', 'X', 'Y', '-', '$', 'b', 'i' };
                    break;
                default:
                    
                    break;
            }

            List<char> arr = new List<char>(nuevasCartas);
            List<char> arrDes = new List<char>();

            Random randNum = new Random();
            while (arr.Count > 0)
            {
                int val = randNum.Next(0, arr.Count - 1);
                arrDes.Add(arr[val]);
                arr.RemoveAt(val);
            }
            nuevasCartas = arrDes.ToArray<char>();
            return nuevasCartas;
        }

        private void carta_click(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled)
                return;
            

            if (ParejaUno == null)
            {
                ParejaUno = (Border)sender;
                TextBlock textoParejaUno = ((TextBlock)((Viewbox)ParejaUno.Child).Child);
                textoParejaUno.Text = textoParejaUno.Tag.ToString();
            }                
            else
            {
                
                ParejaDos = (Border)sender;
                if (ParejaDos == ParejaUno)
                    return;
                TextBlock textoParejaUno = ((TextBlock)((Viewbox)ParejaUno.Child).Child);
                TextBlock textoParejaDos = ((TextBlock)((Viewbox)ParejaDos.Child).Child);
                textoParejaDos.Text = textoParejaDos.Tag.ToString();
                if (textoParejaUno.Text == textoParejaDos.Text)
                {
                    ParejaUno.Background = Brushes.ForestGreen;
                    ParejaDos.Background = Brushes.ForestGreen;
                    ParejaUno.MouseDown -= carta_click;
                    ParejaDos.MouseDown -= carta_click;
                    ParejaUno = null;
                    progressBar.Value++;
                    if (progressBar.Maximum == progressBar.Value)
                        MessageBox.Show("Partida acabada!", "Mensaje", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                { 
                    timer.Start();
                }
                
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            TextBlock textoParejaUno = ((TextBlock)((Viewbox)ParejaUno.Child).Child);
            TextBlock textoParejaDos = ((TextBlock)((Viewbox)ParejaDos.Child).Child);
            textoParejaUno.Text = "s";
            textoParejaDos.Text = "s";
            ParejaUno = null;            
            timer.Stop();
        }

        private void mostrarButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Border item in listaBordes)
            {
                if (timer.IsEnabled)
                    timer.Stop();
                item.Background = Brushes.ForestGreen;
                item.MouseDown -= carta_click;
                TextBlock textoCarta = ((TextBlock)((Viewbox)item.Child).Child);
                textoCarta.Text = textoCarta.Tag.ToString();
                progressBar.Value = progressBar.Maximum;
            }
        }
    }
}
