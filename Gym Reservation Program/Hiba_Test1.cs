using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiba_Test1
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            TestReservation();
        }
        static void TestReservation()
        {
            Console.WriteLine($"{new string('-', 26)}Reservation Application{new string('-', 26)}");
            ReservationManager branch1 = new ReservationManager(729);
            branch1.ReservationList.Add(new Reservation(546, "Alex Du", PassType.DayPass));
            branch1.ShowAll();
            branch1.AddReservation(921, "Dolly Lively", PassType.SeasonPass);
            branch1.ReservationList[0].UpdatePassStatus(false);
            branch1.ShowAll();
            branch1.ShowAll(PassType.SeasonPass);
            ReservationManager branch2 = new ReservationManager(498);
            branch2.ReservationList.Add(new Reservation(576, "Dale", PassType.SeasonPass));
            Console.WriteLine();
            branch2.ShowAll();
            branch2.AddReservation(847, "Jack Gibbs", PassType.DayPass);
            branch2.ReservationList[1].UpdatePassStatus(false);
            branch2.ShowAll();
            branch2.ShowAll(PassType.DayPass);
            Console.WriteLine($"{new string('-', 75)}");
            Console.ReadKey();
        }

        enum PassType
        {
            LifetimePass, DayPass, SeasonPass
        }

        class Reservation
        {
            public int DriverID { get; }
            public string Name { get; }
            public PassType passType { get; }
            public bool PassStatus { get; private set; }

            public double PassPrice
            {
                get
                {
                    switch (passType)
                    {
                        case PassType.LifetimePass:
                            return 750.0;
                        case PassType.DayPass:
                            return 50.0;
                        case PassType.SeasonPass:
                            return 200.0;
                        default:
                            throw new InvalidOperationException("Invalid pass type.");
                    }
                }
            }

            public Reservation(int DriverID, string Name, PassType passType)
            {
                this.DriverID = DriverID;
                this.Name = Name;
                this.passType = passType;
                PassStatus = true;
            }

            public void UpdatePassStatus(bool activationStatus)
            {
                PassStatus = activationStatus;
                Console.WriteLine(this);
            }

            public override string ToString()
            {
                string status = PassStatus ? "Activated" : "Not Activated";
                return $"Reservation ID: {this.DriverID}    Name: {this.Name}    Type: {this.passType}       Pass Status: {status}    Pass Price: {this.PassPrice:C}";
            }

        }

        class ReservationManager
        {
            private static string FILENAME = "Manager_Reservation.txt";
            public int ID { get; }
            public List<Reservation> ReservationList { get; }

            public ReservationManager(int ID)
            {
                this.ID = ID;
                ReservationList = new List<Reservation>(); //empty list
                    
                    using (StreamReader reader = new StreamReader(FILENAME))
                    {
                        string recordLine;
                        while ((recordLine = reader.ReadLine()) != null)
                        {
                            string[] fields = recordLine.Split(',');
                            if (int.Parse(fields[0]) == this.ID)
                            {
                                int DriverID = int.Parse(fields[1]);
                                string Name = fields[2];
                                PassType passType = (PassType)Enum.Parse(typeof(PassType), fields[3], true);
                                Reservation reservation = new Reservation(DriverID, Name, passType);
                                ReservationList.Add(reservation);
                            }
                        }
                    reader.Close();
                    }
                
            }

            public void AddReservation(int ID, string Name, PassType passType)
            {
                Reservation reservation = new Reservation(ID, Name, passType);
                ReservationList.Add(reservation);
                Console.WriteLine(reservation);
            }

            public void ShowAll()
            {
                foreach (Reservation reservation in ReservationList)
                {
                    Console.WriteLine(reservation);
                    Console.WriteLine();
                }
            }

            public void ShowAll(PassType passType)
            {
                foreach (Reservation reservation in ReservationList)
                {
                    if (reservation.passType == passType)
                    {
                        Console.WriteLine(reservation);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
