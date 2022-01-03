using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Drawing;
using Pen = System.Drawing.Pen;
using System.Configuration;
using System.Threading;
using RestSharp.Serialization.Json;
using Rectangle = System.Windows.Shapes.Rectangle;
using Brushes = System.Windows.Media.Brushes;

namespace ReservationTable_PK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        RestClient client = null;
        List<Reservation> reservations;
        List<Reservation> deleteList = new List<Reservation>();
        private List<Reservation> pending = new List<Reservation>();
        List<Reservation> editList = new List<Reservation>();
        private Reservation ClickedSeat;
        private const int size = 21;
        private const int space = 4;
        private bool[,] seats = new bool[20, 20];
        static Random rnd = new Random();
        public MainWindow()
        {
            string server = ConfigurationSettings.AppSettings["server"];
            int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
            client = new RestClient(string.Format($"http://{server}:{port}/php_pk/reservation/index.php"));
            InitializeComponent();
            ListReservations();
            ResetInputs();
            DrawTable(can_seats,null);
        }
        public void ResetInputs()
        {
            if (LoginForm.LoggedInUser == null)
            {
                btn_Login.IsEnabled = true;
                btn_LogOut.Visibility = Visibility.Hidden;
                
                btn_DeleteByName.Visibility = Visibility.Hidden;
                btn_DeleteByName.IsEnabled = false;
                btn_DeleteSelected.Visibility = Visibility.Hidden;
                btn_DeleteSelected.IsEnabled = false;

                btn_EditSelected.Visibility = Visibility.Hidden;
                btn_EditSelected.IsEnabled = false;
                btn_EditName.Visibility = Visibility.Hidden;
                btn_EditName.IsEnabled = false;

                btn_Search.Visibility = Visibility.Hidden;
                btn_Search.IsEnabled = false;

                tb_SeatColumn.IsEnabled = false;

                tb_SeatRow.IsEnabled = false;
            }
            else
            {
                btn_Login.IsEnabled = false;
                btn_LogOut.Visibility = Visibility.Visible;

                btn_DeleteByName.Visibility = Visibility.Visible;
                btn_DeleteByName.IsEnabled = true;
                btn_DeleteSelected.Visibility = Visibility.Visible;
                btn_DeleteSelected.IsEnabled = true;

                btn_EditSelected.Visibility = Visibility.Visible;
                btn_EditSelected.IsEnabled = true;
                btn_EditName.Visibility = Visibility.Visible;
                btn_EditName.IsEnabled = true;

                btn_Search.Visibility = Visibility.Visible;
                btn_Search.IsEnabled = true;

                tb_SeatColumn.IsEnabled = true;

                tb_SeatRow.IsEnabled = true;
            }
            tb_ReservationName.Clear();
            tb_SeatColumn.Clear();
            tb_SeatRow.Clear();
            ClickedSeat = null;
            ButtonCheck();
        }
        
        public void DrawTable(Canvas Canvas, object sender)
        {
            Canvas.Children.Clear();
            for (int j = 0; j < seats.GetLength(1); j++)
            {
                for (int i = 0; i < seats.GetLength(0); i++)
                {
                    Rectangle rectangle = new Rectangle
                    {
                        Height = size,
                        Width = size,
                    };
                    if (LoginForm.LoggedInUser == null)
                    {
                        if (!seats[i, j])
                        {
                            rectangle.MouseDown += Reserve;
                        }
                        rectangle.Fill = seats[i, j] ? Brushes.Red : Brushes.Green;
                    }
                    else
                    {
                        Button clickedButton = (Button)sender;
                        if (seats[i,j])
                        {
                            if (clickedButton == null) 
                            {
                                rectangle.Fill = Brushes.Red;
                            }
                            else if (clickedButton.Name == "btn_Search")
                            {
                                if (reservations.Find(x => x.ReservedBy == tb_ReservationName.Text && x.SeatColumn == j && x.SeatRow == i) != null)
                                {
                                    rectangle.Fill = Brushes.Pink;
                                }
                                else
                                {
                                    rectangle.Fill = Brushes.Red;
                                }
                            }
                            rectangle.MouseDown += ReservationViewing;
                        }
                        else
                        {
                            rectangle.Fill = Brushes.Green;
                            rectangle.MouseDown += Reserve;
                        }
                    }
                    
                    Canvas.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, i * (size + space));
                    Canvas.SetTop(rectangle, j * (size + space));
                }
            }
        }
        private void ReservationViewing(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            
            int px = Convert.ToInt32(Canvas.GetLeft(rectangle)) / (size + space);
            int py = Convert.ToInt32(Canvas.GetTop(rectangle)) / (size + space);

            ClickedSeat = reservations.Find(x => x.SeatRow == px && x.SeatColumn == py);

            tb_ReservationName.Text = ClickedSeat.ReservedBy;
            tb_SeatColumn.Text = (ClickedSeat.SeatColumn+1).ToString();
            tb_SeatRow.Text = (ClickedSeat.SeatRow+1).ToString();
        }
        private void Reserve(object sender, MouseButtonEventArgs e)
        {
            ClickedSeat = null;
            Rectangle rectangle = sender as Rectangle;
            Reservation temp;
            rectangle.Fill = (rectangle.Fill == Brushes.Green) ? Brushes.LightBlue : Brushes.Green;

            int px = Convert.ToInt32(Canvas.GetLeft(rectangle)) / (size + space);
            int py = Convert.ToInt32(Canvas.GetTop(rectangle)) / (size + space);

            if (px < seats.GetLength(0) && py < seats.GetLength(1))
            {
                if (!seats[px, py])
                {
                    temp = new Reservation(px, py);
                    pending.Add(temp);
                    tb_SeatColumn.Text = (temp.SeatColumn+1).ToString();
                    tb_SeatRow.Text = (temp.SeatRow+1).ToString();
                }
                if (seats[px, py])
                {
                    temp = pending.Find(x => x.SeatRow == px && x.SeatColumn == py);
                    pending.Remove(temp);
                    tb_SeatRow.Clear();
                    tb_SeatColumn.Clear();
                }
                SwitchState(px, py);
            }
        }
        public void ButtonCheck()
        {
            if (pending.Count == 0)
            {
                btn_reserve.IsEnabled = false;
            }
            else
            {
                btn_reserve.IsEnabled = true;
            }
        }
        public void SwitchState(int x, int y)
        {
            ButtonCheck();
            seats[x, y] = !seats[x, y];
        }
        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            LoginForm.LoggedInUser = null;
            pending.Clear();
            MessageBox.Show($"Successfully signed out!");
            ListReservations();
            ResetInputs();
            DrawTable(can_seats, null);
        }
        private void ListReservations()
        {
            seats = new bool[20, 20];
            var request = new RestRequest(Method.GET);
            request.RequestFormat = RestSharp.DataFormat.Json;

            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }

            reservations = new JsonSerializer().Deserialize<List<Reservation>>(response);

            foreach (Reservation r in reservations)
            {
                seats[r.SeatRow, r.SeatColumn] = true;
            }
        }
        private void btn_reserve_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ReservationName.Text == "")
            {
                MessageBox.Show("Reservation name is required, please try again!");
                return;
            }
            if (tb_ReservationName.Text.Length < 3)
            {
                MessageBox.Show("Reservation name is too short, please try again!");
                return;
            }
            if (tb_ReservationName.Text.Length > 255)
            {
                MessageBox.Show("Reservation name is too long, please try again!");
                return;
            }
            if (pending.Count == 0)
            {
                MessageBox.Show("Please select seat(s) to reserve");
                return;
            }
            bool successful = false;
            string message = "Failed, please try again";
            
            foreach (Reservation r in pending)
            {
                var request = new RestRequest(Method.POST);
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddJsonBody(new
                {
                    reservator = tb_ReservationName.Text,
                    rownum = r.SeatRow,
                    columnnum = r.SeatColumn
                });
                var response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    successful = true;
                }
                else
                {
                    message = response.StatusDescription;
                    pending.Clear();
                    break;
                }
            }
            if (!successful)
            {
                MessageBox.Show(message);
                pending.Clear();
                DrawTable(can_seats, null);
                return;
            }
            else
            {
                MessageBox.Show($"Reservation for \"{tb_ReservationName.Text}\" was successful!");
                pending.Clear();
                ListReservations();
                ResetInputs();
                DrawTable(can_seats, null);
            }
        }
        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            ListReservations();
            pending.Clear();
            ButtonCheck();
            DrawTable(can_seats, null);
        }
        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            pending.Clear();
            ListReservations();
            DrawTable(can_seats, null);
            LoginForm loginform = new LoginForm(this);
            loginform.ShowDialog();
            ResetInputs();
            DrawTable(can_seats, null);
        }
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            if (tb_ReservationName.Text == "")
            {
                MessageBox.Show("Please input a name");
                return;
            }
            if (reservations.Find(x=> x.ReservedBy == tb_ReservationName.Text) == null)
            {
                MessageBox.Show("The given reservation with this name does not exist!");
                return;
            }
            pending.Clear();
            ListReservations();
            DrawTable(can_seats, sender);
        }

        private void btn_DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            editList.Clear();
            deleteList.Clear();
            pending.Clear();
            if (LoginForm.LoggedInUser == null)
            {
                MessageBox.Show("You are not authorised to use this function!");
                DrawTable(can_seats,null);
                return;
            }
            else
            {
                if (tb_SeatColumn.Text == "" || tb_SeatRow.Text == "")
                {
                    MessageBox.Show("Input is empty, please try again");
                    return;
                }
                int column;
                int row;

                if (!int.TryParse(tb_SeatColumn.Text,out column) || !int.TryParse(tb_SeatRow.Text, out row))
                {
                    MessageBox.Show("Selected input is not a number, please try again!");
                    return;
                }
                if (row > 20 || column > 20)
                {
                    MessageBox.Show("Input too large! (>20)");
                    return;
                }
                if (row <= 0 || column <= 0)
                {
                    MessageBox.Show("Input too small! (<0)");
                    return;
                }
                ListReservations();
                Reservation selected = reservations.Find(x => x.SeatRow == row-1 && x.SeatColumn == column-1);
                if (selected == null)
                {
                    MessageBox.Show("The selected seat does not exist, please try again");
                    return;
                }
                else
                {
                    Delete(selected);
                    MessageBox.Show("Successul deletion!");
                    ResetInputs();
                    ListReservations();
                    DrawTable(can_seats, null);
                }

            }
        }

        private void btn_DeleteByName_Click(object sender, RoutedEventArgs e)
        {
            editList.Clear();
            deleteList.Clear();
            pending.Clear();
            if (LoginForm.LoggedInUser == null)
            {
                MessageBox.Show("You are not authorised to use this function!");
                DrawTable(can_seats, null);
                return;
            }
            else
            {
                if (tb_ReservationName.Text == "")
                {
                    MessageBox.Show("Name is empty, please try again");
                    return;
                }
                else
                {
                    ListReservations();
                    Reservation selected = reservations.Find(x => x.ReservedBy == tb_ReservationName.Text);
                    if (selected == null)
                    {
                        MessageBox.Show("There is no reservation with this name");
                        pending.Clear();
                        return;
                    }
                    else
                    {
                        foreach (Reservation reservation in reservations)
                        {
                            if (selected.ReservedBy == reservation.ReservedBy)
                            {
                                deleteList.Add(reservation);
                            }
                        }
                        foreach (Reservation r in deleteList)
                        {
                            if (r == reservations.Find(x => x.ID == r.ID))
                            {
                                Delete(r);
                            }
                        }
                        MessageBox.Show("Successul deletion!");
                        deleteList.Clear();
                    }
                }
                ListReservations();
                ResetInputs();
                DrawTable(can_seats, null);
                deleteList.Clear();
            }
        }
        private void Delete(Reservation selected)
        {
            var request = new RestRequest(Method.DELETE);
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.AddJsonBody(new
            {
                id = selected.ID,
                username = LoginForm.LoggedInUser.Name,
                password = LoginForm.LoggedInUser.Password
            });

            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                DrawTable(can_seats, null);
                return;
            }
            reservations.Remove(selected);
        }

        private void btn_ResetInputs_Click(object sender, RoutedEventArgs e)
        {
            ResetInputs();
            ClickedSeat = null;
        }

        private void Edit(Reservation reservation, string newName, int newColumn, int newRow)
        {
            var request = new RestRequest(Method.PUT);
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.AddJsonBody(new
            {
                id = reservation.ID,
                reservedBy = newName,
                seatRow = newRow,
                seatColumn = newColumn,
                username = LoginForm.LoggedInUser.Name,
                password = LoginForm.LoggedInUser.Password
            });

            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.StatusDescription);
                return;
            }
        }
        private void btn_EditSelected_Click(object sender, RoutedEventArgs e)
        {
            deleteList.Clear();
            editList.Clear();
            pending.Clear();
            if (LoginForm.LoggedInUser == null)
            {
                MessageBox.Show("You are not authorised to use this function!");
                DrawTable(can_seats, null);
                return;
            }
            else
            {
                if (tb_SeatColumn.Text == "" || tb_SeatRow.Text == "")
                {
                    MessageBox.Show("Input is empty, please try again");
                    return;
                }
                int column;
                int row;

                if (!int.TryParse(tb_SeatColumn.Text, out column) || !int.TryParse(tb_SeatRow.Text, out row))
                {
                    MessageBox.Show("Selected input is not a number, please try again!");
                    return;
                }
                if (row > 20 || column > 20)
                {
                    MessageBox.Show("Input too large! (>20)");
                    return;
                }
                if (row <= 0 || column <= 0)
                {
                    MessageBox.Show("Input too small! (<0)");
                    return;
                }
                ListReservations();
                if (ClickedSeat == null)
                {
                    MessageBox.Show("This seat does not exist / no input added!");
                    return;
                }
                Reservation selected = reservations.Find(x => x.ID == ClickedSeat.ID && x.ID == ClickedSeat.ID);
                if (selected == null)
                {
                    MessageBox.Show("There is no reservation with these given inputs");
                    return;
                }
                if (reservations.Find(x => x.SeatRow == row-1 && x.SeatColumn == column-1) != null)
                {
                    MessageBox.Show("The selected seat is already reserved!");
                    return;
                }
                else
                {
                    Edit(selected,selected.ReservedBy,column-1,row-1);
                    MessageBox.Show("Successul edit!");
                    ResetInputs();
                    ListReservations();
                    DrawTable(can_seats, null);
                }
            }

        }
        private void btn_EditName_Click(object sender, RoutedEventArgs e)
        {
            deleteList.Clear();
            pending.Clear();
            if (LoginForm.LoggedInUser == null)
            {
                MessageBox.Show("You are not authorised to use this function!");
                DrawTable(can_seats, null);
                return;
            }
            else
            {
                int column;
                int row;
                if (tb_ReservationName.Text == "")
                {
                    MessageBox.Show("Name is empty, please try again");
                    return;
                }
                
                if (!int.TryParse(tb_SeatColumn.Text, out column) || !int.TryParse(tb_SeatRow.Text, out row))
                {
                    MessageBox.Show("Selected input is not a number, please try again!");
                    return;
                }
                if (row > 20 || column > 20)
                {
                    MessageBox.Show("Input too large! (>20)");
                    return;
                }
                else
                {
                    ListReservations();
                    if (ClickedSeat == null)
                    {
                        MessageBox.Show("This seat does not exist / no input added!");
                        return;
                    }
                    Reservation selected = reservations.Find(x => x.ReservedBy == ClickedSeat.ReservedBy && x.ReservedBy == ClickedSeat.ReservedBy);
                    
                    if (selected == null)
                    {
                        MessageBox.Show("There is no such reservation");
                        pending.Clear();
                        return;
                    }
                    else
                    {
                        foreach (Reservation reservation in reservations)
                        {
                            if (selected.ReservedBy == reservation.ReservedBy)
                            {
                                editList.Add(reservation);
                            }
                        }
                        foreach (Reservation r in editList)
                        {
                            if (r == reservations.Find(x => x.ID == r.ID))
                            {
                                Edit(r, tb_ReservationName.Text, r.SeatColumn,r.SeatRow);
                            }
                        }
                        MessageBox.Show("Successul edit!");
                    }
                    
                }
                ListReservations();
                ResetInputs();
                DrawTable(can_seats, null);
                editList.Clear();
            }
        }
    }
}
