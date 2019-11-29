using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Stumpf_MVVM.View;
using Xamarin.Forms;
using Stumpf_MVVM.Model;
using System.Collections.ObjectModel;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using static Stumpf_MVVM.Model.ConsultaServicoModel;

namespace Stumpf_MVVM.ViewModel
{

    public class ConsultaServicoViewModel : BaseViewModel
    {
        List<ConsultaServicoModel> listateste;
        
        public ConsultaServicoViewModel()
        {
            ListItems = new ObservableCollection<ConsultaServicoModel>();
            PickerItems = new ObservableCollection<ConsultaServicoModel>();

            Database database = new Database();
            ConsultaServicoModel servico = new ConsultaServicoModel();
            listateste = database.Consultar();

            try
            {

                CheckConnection();
                NpgsqlConnection connection = ConsultaServicoModel.Conn();

               
                
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT funcionarios.codigo, funcionarios.nome, stk_operadores.codigo AS cod_operador, stk_operadores.cod_turno FROM funcionarios LEFT JOIN stk_operadores ON stk_operadores.codfunc = funcionarios.codigo WHERE NOT stk_operadores.codigo IS NULL", connection))
                {
                    var data = listateste.Select(a => a.NomeFunc).Distinct();
                    NpgsqlDataReader dr = command.ExecuteReader();
                    int contagem = 0;
                    if (data.Any())
                    {
                        while (dr.Read())
                        {
                            
                            foreach (var emp in data)
                            {
                                if (dr.GetValue(1).ToString() == emp)
                                {
                                    contagem++;
                                    break;
                                }
                            }
                            if (contagem == 0)
                            {
                                servico.Cod_funcionario = dr.GetValue(0).ToString();
                                servico.NomeFunc = dr.GetValue(1).ToString();
                                servico.Cod_operador = dr.GetValue(2).ToString();
                                servico.Cod_turno = dr.GetValue(3).ToString();
                                database.Cadastro(servico);

                            }
                        }
                    }
                    else
                    {
                        while (dr.Read())
                        {
                            servico.Cod_funcionario = dr.GetValue(0).ToString();
                            servico.NomeFunc = dr.GetValue(1).ToString();
                            servico.Cod_operador = dr.GetValue(2).ToString();
                            servico.Cod_turno = dr.GetValue(3).ToString();
                            database.Cadastro(servico);

                        }
                    }
                }
                connection.Close();

                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT DISTINCT stk_op_itens.codpro, produtosc.nomepro, produtosc.codmolde, stk_op.status FROM stk_op_plano LEFT JOIN stk_op_itens ON stk_op_itens.codigo = stk_op_plano.opitem_seq LEFT JOIN stk_op ON stk_op.codigo = stk_op_itens.cod_op LEFT JOIN produtosc ON produtosc.codigo = stk_op_itens.codpro WHERE NOT stk_op.cod_status IN (0, 3, 4, 5)", connection))
                {
                    NpgsqlDataReader dr = command.ExecuteReader();
                    var data = listateste.Select(a => a.Nomepro).Distinct();
                    if (data.Any())
                    {
                        while (dr.Read())
                        {
                            int contagem = 0;
                            foreach (var emp in data)
                            {

                                if (dr.GetValue(1).ToString() == emp)
                                {
                                    contagem++;
                                    break;
                                }

                            }
                            if (contagem == 0)
                            {
                                servico.Codpro = dr.GetValue(0).ToString();
                                servico.Nomepro = dr.GetValue(1).ToString();
                                servico.Cod_molde = dr.GetValue(2).ToString();
                                database.Cadastro(servico);
                            }

                        }
                        connection.Close();

                    }
                    else
                    {
                        while (dr.Read())
                        {
                            servico.Codpro = dr.GetValue(0).ToString();
                            servico.Nomepro = dr.GetValue(1).ToString();
                            servico.Cod_molde = dr.GetValue(2).ToString();
                            database.Cadastro(servico);
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => { await Application.Current.MainPage.DisplayAlert("Alert", "No internet connection", "Ok"); });
            }

            
            listateste = database.Consultar();
            PickerItems = new ObservableCollection<ConsultaServicoModel>();
            PickerProdItems = new ObservableCollection<ConsultaServicoModel>();
            var filtrafunc = new ObservableCollection<ConsultaServicoModel>(listateste.Where(a => !string.IsNullOrEmpty(a.NomeFunc)).Distinct());
            var filtraprod = new ObservableCollection<ConsultaServicoModel>(listateste.Where(a => !string.IsNullOrEmpty(a.Nomepro)).Distinct());
            string ant = string.Empty;
            int count = 0;
            foreach (var emp in filtrafunc)
            {
                if (ant == emp.NomeFunc && count == 0) {
                    count++;  
                }
                if (count == 0)
                {
                    PickerItems.Add(new ConsultaServicoModel() { NomeFunc = emp.NomeFunc, Cod_operador = emp.Cod_operador, Cod_turno = emp.Cod_turno });
                }
                ant = emp.NomeFunc;
            }
            count = 0;
            ant = string.Empty;
            foreach (var emp in filtraprod)
            {
                
                if (ant == emp.Nomepro)
                {
                    count++;
                }
                if (count == 0)
                {
                    PickerProdItems.Add(new ConsultaServicoModel() { Nomepro = emp.Nomepro, Codpro = emp.Codpro, Cod_molde = emp.Cod_molde });
                }
                ant = emp.Nomepro;
            }

            OnPropertyChanged();




        }
       
       public async Task CheckConnection()
        {
            try { NpgsqlConnection conn = ConsultaServicoModel.Conn();
                conn.Open();
                conn.Close();
            }
            catch (Exception ex)
            {

                string exception = ex.Message;
            }
        }
public Command EnviaDadosCommand
        {
            get
            {
                return new Command(EnviaCommand);
            }
        }
        public Command AtualizaDadosCommand
        {
            get
            {
                return new Command(AtualizaCommand);
            }
        }

        void AtualizaCommand(object parameter)
        {
            try
            {
                NpgsqlConnection connd = ConsultaServicoModel.Conn();
                connd.Open();


                // NpgsqlCommand command = new NpgsqlCommand("select nomefunc, etapa, subetapa, iniciavisible, paravisible, idserv, observacoes from servicos WHERE nomefunc=" + "'" + nome + "'" + ";", connd);
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT DISTINCT stk_op_itens.cod_op, stk_op_itens.item_op, stk_op_itens.codpro, stk_op_plano.codservico, stk_servicos.nome AS servico, stk_servicos.codgrupo AS servico_codgrupo, stk_servicos_grupos.descricao AS servico_grupo FROM stk_op_plano LEFT JOIN stk_op_itens ON stk_op_itens.codigo = stk_op_plano.opitem_seq LEFT JOIN stk_servicos ON stk_servicos.codigo = stk_op_plano.codservico LEFT JOIN stk_servicos_grupos ON stk_servicos_grupos.codigo = stk_servicos.codgrupo WHERE stk_op_itens.codpro = " + PickerProdSelected.Codpro + " ORDER BY stk_op_itens.cod_op, stk_op_itens.item_op, stk_op_plano.codservico;", connd))
                {
                    {
                        NpgsqlDataReader dr = command.ExecuteReader();

                        while (dr.Read())
                        {
                            ConsultaServicoModel servico = new ConsultaServicoModel();
                            servico.Cod_operador = dr.GetValue(0).ToString();
                            servico.NomeFunc = PickerSelected.NomeFunc;
                            servico.Nomepro = PickerProdSelected.Nomepro;
                            servico.Cod_turno = PickerSelected.Cod_turno;
                            servico.Cod_plano_producao = dr.GetValue(0).ToString();
                            servico.Item_op = dr.GetValue(1).ToString();
                            servico.Codpro = dr.GetValue(2).ToString();
                            servico.Cod_servico = dr.GetValue(3).ToString();
                            servico.SubEtapa = dr.GetValue(4).ToString();
                            servico.Etapa = dr.GetValue(6).ToString();

                            Database database = new Database();
                            database.Cadastro(servico);

                        }
                        dr.Close();
                    }
                }

                ListItems.Clear();
                ListItems = RetornaLista();
                ListItems = new ObservableCollection<ConsultaServicoModel>(ListItems.Where(a => a.NomeFunc == PickerSelected.NomeFunc && a.Nomepro == PickerProdSelected.Nomepro));

                OnPropertyChanged();
                connd.Close();
            }
            catch (Exception ex)
            {
                string exception = ex.Message;

            }
            
        }

        void EnviaCommand (object parameter)
        {
            try
            {
             
                Database database = new Database();

                //EXPORTAR BANCO DE DADOS LOCAL PARA EXTERNO, PEGAR DADOS DO SQLITE DO DIA E ENVIAR PARA O POSTGRESQL
                
                listateste = database.Consultar();
                var data = listateste.Where(a => a.NomeFunc == PickerSelected.NomeFunc && a.Nomepro == PickerProdSelected.Nomepro);
                var queryturno = listateste.Where(a => a.NomeFunc == PickerSelected.NomeFunc).Select(a => a.Cod_turno).Distinct();
               
                
                using (NpgsqlConnection connd = ConsultaServicoModel.Conn())
                {
                    using (NpgsqlCommand command1 = new NpgsqlCommand("SELECT * FROM producao_grupolancamento WHERE data = current_date", connd))
                    {
                        connd.Open();
                        var es = command1.ExecuteScalar();
                        string codturno = string.Empty;
                        if (es == null)
                        {

                            foreach (var emp in queryturno)
                            {
                                if (emp != null)
                                {
                                    codturno = emp;
                                }
                            }

                            command1.CommandText = "INSERT INTO producao_grupolancamento(codigo, data,codmaq,codturno) VALUES (COALESCE((SELECT MAX(codigo) FROM producao_grupolancamento), 0) +1, current_date, '1'," + codturno + ")";
                            command1.ExecuteNonQuery();
                            connd.Close();
                            connd.Open();
                        }



                        foreach (var emp in data)
                        {

                            if (!(emp.Etapa == null) && (!(emp.HoraFinal == null)))
                            {

                                emp.Grupo_lancamento = es.ToString();
                                database.Atualizacao(emp);

                                connd.Close();
                                connd.Open();
                                NpgsqlCommand command2 = new NpgsqlCommand("INSERT INTO producao (codigo, data_registro, hora_registro, data_producao, hora, tempo_char, tempo_dbl, produzido, codmaq, codope, codmol, codser, codturno, grupolancamento, plano_producao_codigo) VALUES (COALESCE((SELECT MAX(codigo) FROM producao), 0) + 1, current_date, current_time ,current_date,'" + emp.InicioGeral + "','" + emp.HoraFinal + "','" + emp.Duracao_double + "' ," + "0, '1'," + PickerSelected.Cod_operador + "," + PickerProdSelected.Cod_molde + "," + emp.Cod_servico + "," + PickerSelected.Cod_turno + "," + emp.Grupo_lancamento + "," + emp.Cod_plano_producao + ")", connd);


                                command2.ExecuteNonQuery();
                                command2.Dispose();

                            }
                            database.Delete(emp);
                        }

                    }
                    ListItems.Clear();
                    
                    connd.Close();
                    
                    OnPropertyChanged();
                   
                }
            }

            catch (Exception ex)
            {
                string exception = ex.Message;
            }
        }
        public ObservableCollection<ConsultaServicoModel> RetornaLista()
        {
            ObservableCollection<ConsultaServicoModel> ListaItens = new ObservableCollection<ConsultaServicoModel>();
            Database database = new Database();
            var listateste = database.Consultar();
            
           
            
                foreach (var item in listateste)
                {
                    ListaItens.Add(item);
                }
                        
            return ListaItens;
        }



       
        public int IndexSelected {get;set;}
        ConsultaServicoModel itemsPicker;
       

        public ConsultaServicoModel PickerSelected
        {
            get { return itemsPicker; }
            set
            {
                itemsPicker = value;
               
                OnPropertyChanged();
                if (pickerprod == null) {
                    ListItems.Clear();
                    ListItems = RetornaLista();
                    ListItems = new ObservableCollection<ConsultaServicoModel>(ListItems.Where(a=> a.NomeFunc == itemsPicker.NomeFunc && !string.IsNullOrEmpty(a.SubEtapa)));
                    IndexSelected = 0;

                } else
                {
                ListItems = new ObservableCollection<ConsultaServicoModel>(listateste.Where(a => a.Nomepro == pickerprod.Nomepro && a.NomeFunc == itemsPicker.NomeFunc && !string.IsNullOrEmpty(a.SubEtapa)));
                }
                
            }
        }

        ConsultaServicoModel pickerprod;
        public ConsultaServicoModel PickerProdSelected
        {
            get { return pickerprod; }
            set
            {
                pickerprod = value;
                OnPropertyChanged();
                if (pickerprod != null) {
                    ListItems.Clear();

                    ListItems = new ObservableCollection<ConsultaServicoModel>(listateste.Where(a => a.Nomepro == pickerprod.Nomepro && a.NomeFunc == itemsPicker.NomeFunc && !string.IsNullOrEmpty(a.SubEtapa)));
                }
                
              
                
            }
        }
        ObservableCollection<ConsultaServicoModel> listitems;
        public ObservableCollection<ConsultaServicoModel> ListItems 
        {

            get
            {
                return listitems;
            }
             
            set 
            {
                listitems = value;
                OnPropertyChanged(); 
            }  
        }
        public ObservableCollection<ConsultaServicoModel> PickerItems { get; set; }
        public ObservableCollection<ConsultaServicoModel> PickerProdItems { get; set; }
        
        
        public bool atualizalist;
     

  

        




    }

    
}
