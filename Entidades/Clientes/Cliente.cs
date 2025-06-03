using Entidades.Util;

namespace Entidades.Clientes
{
    public class Cliente
    {
        public virtual int Codigo { get; protected set; }
        public virtual string Cpf { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Nome { get; protected set; }

        protected Cliente() { }

        public Cliente(int codigo)
        {
            SetCodigo(codigo);
        }

        public Cliente(string cpf, string email, string nome)
        {
            SetCpf(cpf);
            SetEmail(email);
            SetNome(nome);
        }

        public virtual void SetCodigo(int codigo)
        {
            Codigo = codigo;
        }

        public virtual void SetCpf(string cpf)
        {
            if (!StringExtensions.CpfValido(cpf.ApenasNumeros()))
                throw new RegraNegocioException("Cpf inválido");

            Cpf = cpf.ApenasNumeros();
        }
        public virtual void SetEmail(string email)
        {
            if (!StringExtensions.EmailValido(email) || email.Length > 200)
                throw new RegraNegocioException("E-mail inválido");

            Email = email;
        }

        public virtual void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome) || nome.Length > 150)
                throw new RegraNegocioException("Tamanho do Nome inválido");

            Nome = nome;
        }
    }
}
