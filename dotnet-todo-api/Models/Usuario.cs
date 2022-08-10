namespace dotnet_todo_api.Models
{
    public class Usuario
    {
        public Usuario()
        {
        }

        public string Id { get; set; }

        public string Nome { get; set; }

        public string NomeUsuario { get; set; }

        public string Senha { get; set; }

        public string Email { get; set; }
    }
}
