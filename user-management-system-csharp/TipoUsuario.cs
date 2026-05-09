// Define os perfis de acesso disponíveis no sistema.
// O uso de enum foi usado para garantir maior segurança e legibilidade,
// evitando valores inválidos para o tipo de usuário.

namespace user_management_system_csharp
{
    enum TipoUsuario
    {
        Administrador,
        Usuario
    }
}
