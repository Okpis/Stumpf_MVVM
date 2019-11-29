using Npgsql;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Stumpf_MVVM.Model
{
    [Table("stumpf_prod0")]
    public class ConsultaServicoModel : INotifyPropertyChanged
    {
        Database database = new Database();
        public ConsultaServicoModel()
        {
            ServicoCommand = new Command(ServicoCommandToggle);
           
        }

        public Command ServicoCommand { get; }
        public Command IniciaTimerCommand { get; }

        
        void ServicoCommandToggle()
        {   
            if (ToggleText == "Iniciar") 
            {
                if (InicioGeral == null) 
                {
                    InicioGeral = DateTime.Now.ToString("hh:mm:ss");
                };

                HoraInicial = DateTime.Now.ToString();
                ToggleText = "Pausar";
                ToggleTextColor = "Red";
                database.Atualizacao(this);
            } else if (ToggleText == "Pausar")
            {
                string Time;
                string DateToDouble;
                if (HoraFinal != null)
                {
                    string res = DateTime.Parse(HoraInicial).Subtract(DateTime.Parse(DateTime.Now.ToString())).Duration().ToString();
                    TimeSpan timeSpanres = TimeSpan.Parse(res);
                    TimeSpan ConvertFinalTime = TimeSpan.Parse(HoraFinal);
                    TimeSpan SumFinalTime = ConvertFinalTime.Add(timeSpanres);
                    Time = SumFinalTime.ToString();
                    DateToDouble = SumFinalTime.TotalHours.ToString("N2").Replace(",", ".");
                    Duracao_double = DateToDouble;
                    HoraFinal = Time;
                   
                } else if (HoraFinal == null)
                {
                    Time = DateTime.Parse(HoraInicial).Subtract(DateTime.Parse(DateTime.Now.ToString())).Duration().ToString();
                    TimeSpan SumFinalTime = DateTime.Parse(HoraInicial).Subtract(DateTime.Parse(DateTime.Now.ToString())).Duration();
                    DateToDouble = SumFinalTime.TotalHours.ToString("N2").Replace(",", ".");
                    Duracao_double = DateToDouble;
                    HoraFinal = Time;
                    
                }
                
                database.Atualizacao(this);
                ToggleText = "Iniciar";
                ToggleTextColor = "Green";
            }
        }
        string horainicial;
        string horafinal;

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string NomeFunc { get; set; }
        public string Etapa { get; set; }
        public string SubEtapa { get; set; }

     
        public string HoraInicial
        {
            get { return horainicial; }
            set
            {
                horainicial = value;
                OnPropertyChanged();
            }

        }

        public string HoraFinal
        {
            get { return horafinal; }
            set
            {
                horafinal = value;
                OnPropertyChanged();
            }
        }


        public bool ServicoVisible { get; set; }
        public string Cod_plano_producao { get; set; }
        public string Item_op { get; set; }
        public string Codpro { get; set; }
        public string Nomepro { get; set; }
        public string Cod_servico { get; set; }
        public string Cod_funcionario { get; set; }
        public string Cod_operador { get; set; }
        public string Cod_molde { get; set; }
        public string Cod_turno { get; set; }
        public string Grupo_lancamento { get; set; }
        public string Duracao_double { get; set; }
        public string InicioGeral { get; set; }

        private string toggletextcolor = "Green";
        public string ToggleTextColor
        {
            get
            {
                return toggletextcolor;
            }
            set
            {
                toggletextcolor = value;
                OnPropertyChanged();
            }
        }

        private string toggletext = "Iniciar";
        public string ToggleText
        {
            get { return toggletext; }
            set
            {
                toggletext = value;
                OnPropertyChanged();
            }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static NpgsqlConnection Conn()
        {
            string connectionString = string.Empty;
            connectionString += "Server=192.168.1.207;";
            connectionString += "Port=5432;";
            connectionString += "User Id=postgres;";
            connectionString += "Password=202230;";
            connectionString += "Database=stumpf_dados";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            return conn;
        }
    }

   
}
