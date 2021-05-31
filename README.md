# Primeiro projeto em C#


## Criando uma API usando ASP.NET Core e Entity Framework Core



Durante uma das fases do VAIVOA, um projeto de aceleração da carreira de Desenvolvedores Backend e Cientistas de Dados, recebemos o desafio de desenvolver uma API REST  com com .Net Core e Entity Framework Core que retorna 2 endpoints, Um receberá o email da pessoa e retornará um objeto de resposta com o número do cartão de crédito. E o outro endpoint deverá listar, em ordem de criação, todos os cartões de crédito virtuais de um solicitante (passando seu email como parâmetro), um desafio enorma para aqueles que como eu, estão migrando de uma outra tecnologia.

Para a criação da API, tive que me aprofundar o máximo possível na linguagem C# para distinguir a sintaxe e aprender o necessário para utilizar o Framework então vamos para o resultado de 3 dias de imersão!
    

## Processo de criação da API
    

Desenvolvi o projeto utilizamos o Visual Studio 2019. Para começar, criei um projeto utilizando o template ASP.NET Core Web App sem configurações para HTTP :

![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/image-20210528145604137.png)

Uma vez com o projeto criado, criei a classe Cliente com as propriedades e-mail e uma lista de Cartões e a classe Cartao com as propriedades numero do cartão e cliente, ambas as classe com um ID. O Entity que realiza o ORM(Mapeamento Objeto Relacional), automaticamente reconhece as props Id como Chave Primária quando criado o banco de dados.




```c#
namespace Project.Domain
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<Cartao> Cartoes { get; set; }
    }
}

________________________



namespace Project.Domain
{
    public class Cartao
    {
        public int Id { get; set; }
        public string NumeroDoCartao { get; set; }

        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
    }
}

```

Após criadas as classes, criei uma classe, por convensão chamada ClienteContext herdando a classe DbContext para servir como repositório onde colocamos nos DbSet as classes que seriam mapeadas pelo Entity:




```c#
namespace Project.Repo
{
    public class ClienteContext : DbContext
    {
        public ClienteContext(DbContextOptions<ClienteContext> options): base(options){}

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public object Cliente { get; set; }
    }
}
```

 Após criada a classe Context, precisamos configurar o projeto para trabalhar com um banco de dados, neste caso, usei o Sql Server para criar a tabela. Para configurar o banco de dados, precisamos passar uma String de Conexão para o arquivo appsettings.json e depois criar uma referência na classe Startup.cs, que é a classe onde fazemos as injeções de dependência, essa String de conexão vem com as configurações do seu banco de dados local, no meu caso ficou assim:

 


```c#
 "ConnectionStrings": {
    "defaltConnection": "Password=postcore1324;Persist Security Info=True;User ID=sa;Initial Catalog=Project;Data Source=DESKTOP-TIGCP3C"

________________________

 public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
    
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ClienteContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaltConnection"));
            });
    
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
```        
        
Depois de feitas as configurações, podemos criar uma Migration com o comando Add-Migration no Package Manager Console, o comando cria uma classe com o script da criação do banco de dados. Depois é só usar o comando Update-Database para que o seu banco de dados seja criado no seu Servidor já com todas as tabelas e relações criadas pelo Entity.

![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/image-20210528152934961.png)



Uma vez criado o banco de dados, podemos criar os Controllers para fazer as requisições HTTP. Na Pasta Controllers, criamos uma nova classe controller chamada ClienteController para configurar as Rotas e criar as requisições com a seguinte configuração:
    
```c#
namespace Project.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteContext _context;
        public ClienteController(ClienteContext context)
        {
            _context = context;
        }

```

Em seguida, criamos as requisições para receber como parâmetro um e-mail e retornar um número de cartão de crédito aleatório que fica registrado no e-mail da requisição. Utilizei a classe Random do C# para gerar 4 instancias de números aleatórios, depois converti em uma String para concatena-los e salvar o número do cartão em um objeto do tipo Cartao, depois salvar este objeto na lista de cartões do Cliente da requisição e retornar o objeto com o número gerado. A requisição e o retorno ficaram da seguinte forma:

![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/image-20210528154830893.png)



![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/image-20210528155009952.png)

(Infelizmente por passar uma lógica dentro da requisição, não consegui retornar um objeto JSON no Postman)







Depois fiz a requisição para passar como parâmetro o e-mail e receber a lista de cartões já gerados para aquele e-mail em ordem de criação, do mais antigo para o mais recente. A Requisição e a resposta ficaram da seguinte forma:  

![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/image-20210528155425711.png)



![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/image-20210528155609141.png)





E assim finalizei meu desafio técnico da melhor forma que consegui. Poderiam ter sido usados classes DTO ao invés das entidades mapeadas, criar classes Services para criar as requisições e requisições mais bem estruturadas, porém devido ao prazo disponível para aprender a linguagem e os Frameworks não foi possível estruturar a API da forma que eu queria, mas entrego este desafio com a consciência tranquila e com a certeza que como sempre, dei meu melhor.
Meus agradecimentos ao VAIVOA pela oportunidade.


_________________________________


# Atualização

Mesmo que já não seja levado em conta este commit por ter sido realizado fora da data de entrega do desafio, decidi melhorar algumas partes do código que a principio não deram resultado satisfatório.
Para começar criei a interface IProjectRepository e a classe ProjectRepository para fazer as Querys:

```C#
namespace Project.Repo
{
    public interface IProjectRepository
    {
        //Metodo Genérico que recebe qualquer Tipo como parametro.
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<bool> SaveChangeAsync();


        Task<Cliente[]> GetAllClientes();
        Task<Cliente> GetClientesById(int id);
        Task<Cliente[]> GetClientesByNome(string nome);
        Task<Cliente> GetClienteByEmail(string email);


    }
}

```

____________________

```C#
namespace Project.Repo
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ClienteContext _context;

        public ProjectRepository(ClienteContext context) // cria uma referência do Context.
        {
            _context = context;
        }


        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }





        public async Task<Cliente[]> GetAllClientes()
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Cliente> GetClientesById(int id)
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Id);

            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Cliente[]> GetClientesByNome(string nome)
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Email.Contains(nome));

            return await query.ToArrayAsync();
        }


        public async Task<Cliente> GetClienteByEmail(string email)
        {
            IQueryable<Cliente> query = _context.Clientes
                .Include(c => c.Cartoes);

            query = query.AsNoTracking().OrderBy(c => c.Email.Equals(email));

            return await query.FirstOrDefaultAsync(c => c.Email == email);

        }
    }
}
```
Em seguida adicionei ao escopo do Context na classe Startup.

```C#
public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ClienteContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaltConnection"));
            });

            services.AddScoped<IProjectRepository, ProjectRepository>();
```
Depois na classe ClienteController realizei as seguintes requisições, desta vez retornando corretamente seus JSONs no Postman.

## Para retornar a lista de cartões referente a um e-mail:
```C#
[HttpGet("email/{email}")]
        public async Task<IActionResult> GetEmailCards(string email)
        {
            try
            {
                var clientes = await _repo.GetClienteByEmail(email);

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex}");
            }


        }
```
![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/PostmanClienteGet.png)

## Para criar um cartão aleatório e salva-lo no email da requisição:

```C#
[HttpGet("cartao/{email}")]
        public async Task<IActionResult> GetCard(string email)
        {
            try
            {
                var cliente = await _repo.GetClienteByEmail(email);

                if (cliente != null)
                {
                    var randomNumber = new Random();
                    int numUm = randomNumber.Next(1000, 10000);
                    int numDois = randomNumber.Next(1000, 10000);
                    int numTres = randomNumber.Next(1000, 10000);
                    int numQuatro = randomNumber.Next(1000, 10000);

                    var rUm = Convert.ToString(numUm);
                    var rDois = Convert.ToString(numDois);
                    var rTres = Convert.ToString(numTres);
                    var rQuatro = Convert.ToString(numQuatro);

                    string result = rUm + " " + rDois + " " + rTres + " " + rQuatro;


                    cliente.Cartoes = new List<Cartao>
                                        {
                                        new Cartao { NumeroDoCartao = result}
                                        };



                    _repo.Update(cliente);
                    if (await _repo.SaveChangeAsync())
                    {
                        return Ok(cliente);
                    }

                }

            }
            catch (Exception ex)
            {

                return BadRequest($"erro: {ex}");
            }
            return BadRequest("Não Criado");
        }
```
![image](https://github.com/Higor-Buiatti/projeto-vaivoa/blob/master/assets/PostmanCriarCartao.png)

Verifiquei que o Postman não estava recebendo a resposta devido também ao loop infinito criado quando uma entidade referenciava a outra sem parar. Ignorei o loop passando a seguinte configuração de Serializable na Classe Startup:

```C#
public void ConfigureServices(IServiceCollection services)
        {
           //...(Context)

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
                ;
        }
```

Assim consegui a resposta das requisições de forma correta.
