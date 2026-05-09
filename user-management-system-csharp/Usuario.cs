namespace user_management_system_csharp
{
    // Representa a entidade de usuário do sistema.
    // A classe concentra os dados e comportamentos relacionados
    // ao cadastro e controle de acesso de usuários.
    internal class Usuario
    {
        // Identificador único do usuário no sistema.
        public string Matricula { get; set; }

        // Nome completo do usuário.
        public string Nome { get; set; }

        // Departamento ao qual o usuário pertence.
        public string Departamento { get; set; }

        // Perfil de acesso do usuário no sistema.
        public TipoUsuario Tipo { get; set; }

        // Inicializa um novo objeto Usuario com os dados informados.
        public Usuario(string matricula, string nome, string departamento, TipoUsuario tipo)
        {
            Matricula = matricula;
            Nome = nome;
            Departamento = departamento;
            Tipo = tipo;
        }

        // Verifica se o usuário possui perfil administrador.
        // Pode ser utilizado futuramente para controle de permissões.
        public bool EhAdministrador()
        {
            return Tipo == TipoUsuario.Administrador;
        }

        // Retorna uma representação textual amigável do objeto,
        // utilizada para exibição dos dados no console.
        public override string ToString()
        {
            return $"Matrícula: {Matricula} | Nome: {Nome} | Departamento: {Departamento} | Tipo: {Tipo}";
        }
    }

}


