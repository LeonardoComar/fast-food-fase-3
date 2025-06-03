namespace CasosDeUso.Bases.Interfaces.Gateway
{
    public interface IBaseGateway<T>
    {
        void Inserir(T entidade);
        void Alterar(T entidade);
        void Excluir(T entidade);
        void Refresh(T entidade);
        T RetornarPorCodigo(int Codigo);
        IList<T> Consultar();
        IQueryable<T> Query();


    }
}
