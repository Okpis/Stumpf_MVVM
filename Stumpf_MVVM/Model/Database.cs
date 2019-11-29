using SQLite;
using Stumpf_MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Stumpf_MVVM.Model
{
    class Database
    {
        public readonly SQLiteConnection _conexao;

        public Database()
        {
            var dep = DependencyService.Get<ICaminho>();
            string caminho = dep.ObterCaminho("database.sqlite");
            _conexao = new SQLiteConnection(caminho);
            _conexao.CreateTable<ConsultaServicoModel>();
        }

        public List<ConsultaServicoModel> Consultar()
        {
            return _conexao.Table<ConsultaServicoModel>().ToList();

        }


        public void Cadastro(ConsultaServicoModel consultaServicoModel)
        {
            _conexao.Insert(consultaServicoModel);
        }

        public void Atualizacao(ConsultaServicoModel consultaServicoModel)
        {
            _conexao.Update(consultaServicoModel);
        }

        public void Delete(ConsultaServicoModel consultaServicoModel)
        {
            _conexao.Delete(consultaServicoModel);
        }


    }

}

