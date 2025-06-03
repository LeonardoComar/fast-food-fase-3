using CasosDeUso.Bases.Interfaces.Gateway;
using NHibernate;

namespace InterfaceAdapters.Bases.Gateway
{
    public class BaseGateway<T>(ISession session) : IBaseGateway<T> where T : class
    {
        protected readonly ISession repositorioDados = session;

        public void Alterar(T entidade)
        {
            repositorioDados.Update(entidade);
        }

        public void Excluir(T entidade)
        {
            repositorioDados.Delete(entidade);
        }

        public void Inserir(T entidade)
        {
            repositorioDados.Save(entidade);
        }

        public void Refresh(T entidade)
        {
            repositorioDados.Refresh(entidade);
        }

        public IList<T> Consultar()
        {
            return (from c in repositorioDados.Query<T>() select c).ToList();
        }
        public T RetornarPorCodigo(int Codigo)
        {
            return repositorioDados.Get<T>(Codigo);
        }
        public IQueryable<T> Query()
        {
            return repositorioDados.Query<T>();
        }
    }
}
