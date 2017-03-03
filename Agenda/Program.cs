using Agenda.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Agenda
{
    class Program
    {
        static void Main(string[] args)
        {

            //var settings = new MongoClientSettings
            //{
            //    ServerSelectionTimeout = new TimeSpan(0, 0, 5),
            //    Server = new MongoServerAddress("localhost", 27017),
            //    Credentials = new[]{
            //        MongoCredential.CreateCredential("loja", "joel", "xyz123")
            //    }
            //};


            Console.WriteLine("Conectando no servidor.");
            //Conectar no servidor
            var client = new MongoClient("mongodb://localhost:27017");

            Console.WriteLine("Conectando no banco de dados.");
            //Conectar no banco de dados
            var database = client.GetDatabase("Agenda");

            Console.WriteLine("Obtendo a colecao de contatos.");
            //Colecao onde vai armazenar nosso objeto pessoa
            IMongoCollection<Pessoa> colecao = database.GetCollection<Pessoa>("contatos");


            //InserirPessoa(colecao);

            //AtualizarPessoa(colecao);

            //ExcluirPessoa(colecao);

            ListarPessoas(colecao);

            Console.ReadKey();
        }

        private static void ListarPessoas(IMongoCollection<Pessoa> colecao)
        {
            Console.WriteLine("Listando nossos contatos");
            //Filtro
            var filtro = Builders<Pessoa>.Filter.Empty;
            var pessoas = colecao.Find<Pessoa>(filtro).ToList();

            pessoas.ForEach(x => {
                Console.WriteLine(string.Concat("Nome: ", x.Nome, " Telefone: ", x.Telefone, " Email:" , x.Email));
            });
        }

        private static void ExcluirPessoa(IMongoCollection<Pessoa> colecao)
        {
            Console.WriteLine("Exlcuindo contatos");
            //Filtro
            var filtro = Builders<Pessoa>.Filter.Where(x => x.Nome == "Fernanda");
            colecao.DeleteMany(filtro);
        }

        private static void AtualizarPessoa(IMongoCollection<Pessoa> colecao)
        {
            Console.WriteLine("Atualizando pessoa");

            //Filtro
            var filtro = Builders<Pessoa>.Filter.Empty;

            //Alteracao
            var alteracao = Builders<Pessoa>.Update.Unset(p => p.Email);

            colecao.UpdateMany(filtro, alteracao);
        }

        private static void InserirPessoa(IMongoCollection<Pessoa> colecao)
        {
            //Console.WriteLine("Inserindo pessoa");

            Endereco end1 = new Endereco() { Cidade = "Rio de Janeiro", Pais = "Brasil" };
            Endereco end2 = new Endereco() { Cidade = "São Paulo", Pais = "Brasil" };

            List<Endereco> enderecos = new List<Endereco>();
            enderecos.Add(end1);
            enderecos.Add(end2);

            Pessoa p = new Pessoa() { Nome = "Paulo Rogerio", Telefone = "(21) 9714-5356", Email = "paulo.analista@outlook.com", Ativo=true, Enderecos = enderecos };

            colecao.InsertOne(p);
        }
    }
}
