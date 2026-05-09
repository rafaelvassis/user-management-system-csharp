using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace user_management_system_csharp
{
    internal class Program
    {
        // Salva a lista de usuários no arquivo JSON local.
        // Responsável por persistência simples dos dados do sistema.
        static void SalvarArquivo(List<Usuario> cadastro)
        {
            string json = JsonConvert.SerializeObject(cadastro, Formatting.Indented, new StringEnumConverter());

            File.WriteAllText("usuarios.json", json);
        }

        // Carrega os usuários salvos no arquivo JSON.
        // Caso o arquivo não exista ou esteja vazio/corrompido, retorna uma lista vazia.
        static List<Usuario> CarregarArquivo()
        {
            if (File.Exists("usuarios.json"))
            {
                string json = File.ReadAllText("usuarios.json");

                List<Usuario>? lista = JsonConvert.DeserializeObject<List<Usuario>>(json);

                if (lista == null)
                {
                    return new List<Usuario>();
                }

                return lista;
            }

            return new List<Usuario>();
        }

        // Busca um usuário pela matrícula dentro da lista.
        // Retorna o usuário encontrado ou null caso não exista.
        static Usuario? BuscarPorMatricula(List<Usuario> cadastro, string matricula)
        {
            foreach (Usuario user in cadastro)
            {
                if (user.Matricula == matricula)
                {
                    return user;
                }
            }

            return null;
        }

        // Responsável por capturar entrada do usuário com validação básica.
        // Garante que o valor não seja vazio ou apenas espaços.
        static string LerEntrada(string mensagem)
        {
            string entrada;

            do
            {
                Console.Write(mensagem);
                entrada = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida!");
                    Console.ResetColor();
                }

            } while (string.IsNullOrWhiteSpace(entrada));

            return entrada;
        }

        // Centraliza mensagens do sistema com controle de cor no console.
        // Usado para padronizar feedback ao usuário.
        static void Msg(string texto, ConsoleColor cor)
        {
            Console.ForegroundColor = cor;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        // Exibe menu de seleção de tipo de usuário.
        // Loop segue até que uma opção válida seja escolhida.
        static TipoUsuario SelecionarTipo()
        {
            int op;

            while (true)
            {
                Console.WriteLine("Selecione o tipo de usuário:");
                Console.WriteLine("\n1 - Administrador");
                Console.WriteLine("2 - Usuário");
                Console.Write("\nOpção: ");

                if (int.TryParse(Console.ReadLine(), out op))
                {
                    if (op == 1)
                    {
                        return TipoUsuario.Administrador;
                    }
                    else if (op == 2)
                    {
                        return TipoUsuario.Usuario;
                    }
                }

                Msg("Opção inválida", ConsoleColor.Red);
            }
        }

        // Pausa a execução do console até interação do usuário.
        static void Pausar()
        {
            Console.Write("\nPressione qualquer tecla...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            // Carrega dados persistidos ao iniciar o sistema.
            List<Usuario> cadastro = CarregarArquivo();

            int op;

            do
            {
                Console.Clear();

                Console.WriteLine(":::::::::::::::: CADASTRO DE USUÁRIOS ::::::::::::::::");
                Console.WriteLine("-------------------------------------------------------");

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("                         MENU                          ");
                Console.ResetColor();

                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("1 - Cadastrar novo usuário");
                Console.WriteLine("2 - Listar usuários");
                Console.WriteLine("3 - Buscar usuário");
                Console.WriteLine("4 - Alterar usuário");
                Console.WriteLine("5 - Remover usuário");
                Console.WriteLine("6 - Sair");
                Console.WriteLine("-------------------------------------------------------");

                Console.Write("Escolha uma opção: ");

                // Validação da entrada do menu principal.
                if (!int.TryParse(Console.ReadLine(), out op))
                {
                    Msg("Entrada inválida", ConsoleColor.Red);
                    Pausar();
                    continue;
                }

                switch (op)
                {
                    // -------------------------
                    // CADASTRO DE USUÁRIO
                    // -------------------------
                    case 1:
                        string matricula = LerEntrada("Matrícula: ");

                        // Verifica duplicidade de matrícula antes de cadastrar
                        if (BuscarPorMatricula(cadastro, matricula) != null)
                        {
                            Msg("Já existe usuário com essa matrícula.", ConsoleColor.Red);
                            Pausar();
                            break;
                        }

                        string nome = LerEntrada("Nome: ");
                        string departamento = LerEntrada("Departamento: ");

                        TipoUsuario tipo = SelecionarTipo();

                        Usuario novoUsuario = new Usuario(matricula, nome, departamento, tipo);

                        cadastro.Add(novoUsuario);
                        SalvarArquivo(cadastro);

                        Msg("Usuário cadastrado com sucesso!", ConsoleColor.DarkGreen);
                        Pausar();
                        break;

                    // ---------------------------------
                    // LISTAGEM DE USUÁRIOS CADASTRADOS
                    // ---------------------------------
                    case 2:
                        if (cadastro.Count == 0)
                        {
                            Msg("Nenhum usuário cadastrado.", ConsoleColor.Red);
                        }
                        else
                        {
                            Console.WriteLine("\nLista de usuários:");
                            Console.WriteLine();

                            foreach (Usuario user in cadastro)
                            {
                                Console.WriteLine(user.ToString());
                            }

                            Console.WriteLine();
                        }

                        Pausar();
                        break;

                    // -------------------------
                    // BUSCA
                    // -------------------------
                    case 3:
                        matricula = LerEntrada("Matrícula: ");

                        Usuario? usuarioBusca = BuscarPorMatricula(cadastro, matricula);

                        if (usuarioBusca != null)
                        {
                            Console.Write("Usuário encontrado: ");
                            Console.WriteLine(usuarioBusca.ToString());
                        }
                        else
                        {
                            Msg("Usuário não encontrado.", ConsoleColor.Red);
                        }

                        Pausar();
                        break;

                    // -------------------------
                    // ALTERAÇÃO
                    // -------------------------
                    case 4:
                        matricula = LerEntrada("Matrícula: ");

                        Usuario? usuarioAlt = BuscarPorMatricula(cadastro, matricula);

                        if (usuarioAlt != null)
                        {
                            Console.WriteLine($"Usuário encontrado: {usuarioAlt.ToString()}");

                            usuarioAlt.Nome = LerEntrada("Novo nome: ");
                            usuarioAlt.Departamento = LerEntrada("Novo departamento: ");
                            usuarioAlt.Tipo = SelecionarTipo();

                            SalvarArquivo(cadastro);

                            Msg("Usuário atualizado com sucesso!", ConsoleColor.DarkGreen);
                        }
                        else
                        {
                            Msg("Usuário não encontrado.", ConsoleColor.Red);
                        }

                        Pausar();
                        break;

                    // -------------------------
                    // REMOÇÃO
                    // -------------------------
                    case 5:
                        matricula = LerEntrada("Matrícula: ");

                        Usuario? usuarioDel = BuscarPorMatricula(cadastro, matricula);

                        if (usuarioDel != null)
                        {
                            cadastro.Remove(usuarioDel);
                            SalvarArquivo(cadastro);

                            Msg("Usuário removido com sucesso!", ConsoleColor.DarkGreen);
                        }
                        else
                        {
                            Msg("Usuário não encontrado.", ConsoleColor.Red);
                        }

                        Pausar();
                        break;

                    // -------------------------
                    // SAÍDA DO PROGRAMA
                    // -------------------------
                    case 6:
                        Console.WriteLine("Encerrando programa...");
                        Pausar();
                        break;

                    default:
                        Msg("Opção inválida!", ConsoleColor.Red);
                        Pausar();
                        break;
                }

            } while (op != 6);
        }
    }
}