using LanchesMac.Areas.Admin.Services;
using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace LanchesMac;
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
        //Configuração do servoço de Banco de Dados.
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Incluindo serviço do Identity
        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>() // Para recuperar as informações do contexto
                .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options => options.AccessDeniedPath = "/Home/AccessDenied");
        services.Configure<ConfigurationImagens>(Configuration.GetSection("ConfigurationPastaImagens"));

        //Sobreescrevendo as políticas de senha 
        services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        // Registrando Repositories
        services.AddTransient<ILancheRepository, LancheRepository>(); 
        services.AddTransient<ICategoriaRepository, CategoriaRepository>();
        services.AddTransient<IPedidoRepository, PedidoRepository>();

        // Adicionando o servico ao Projeto
        services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

        //Adicionando servico de Relatorio
        services.AddScoped<RelatorioVendasServices>();
        services.AddScoped < GraficoVendasService>();

        //Implementando política
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin",
                   politica =>
                   {
                       politica.RequireRole("Admin");
                   });
        });

        // Registrando o servico da classe de Carrinho de Compras
        services.AddScoped(sp => CarrinhoCompra.GetCarrinho(sp));

        // Serviço para acessar os recursos do Http Context
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddControllersWithViews();

        //Incluindo o serviço de Paginação
        services.AddPaging(options =>
        {
            options.ViewName = "Bootstrap4";
            options.PageParameterName = "pageindex";
        });
    
        //Registrando o Middlewares
        services.AddMemoryCache();
        services.AddSession();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeedUserRoleInitial seedUserRoleInitial)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles(); // Estão em wwwroot

        app.UseRouting();

        // Criando as roles
        seedUserRoleInitial.SeedRoles();
        //Criando os usuários e atribuindo ao perfil
        seedUserRoleInitial.SeedUsers();

        app.UseSession();

        app.UseAuthentication(); //  Adicionando autenticação
        app.UseAuthorization();

        //Mapeamento de Rotas Padrão
        app.UseEndpoints(endpoints =>
        {
            // Testando roteamento
            /*  endpoints.MapControllerRoute(
                  name: "teste",
                  pattern: "testeme",
                  defaults: new { controller = "teste", Action = "index" });

              endpoints.MapControllerRoute(
                  name: "admin",
                  pattern: "admin/{action=Index}/{id?}",
                  defaults: new { controller = "admin" }
                  ); 
            // ###############################################

            endpoints.MapControllerRoute(
                name: "home",
                pattern: "{home}",
                defaults: new { Controller = "Home", Action = "Index" });

            endpoints.MapControllerRoute(
                name: "admin",
                pattern: "admin",
                defaults: new { Controller = "Admin", Action = "Index" });
            */
            // Criando rota para Listagem de Lanches Por Categoria
            endpoints.MapControllerRoute(
             name: "areas",
             pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
            );

            endpoints.MapControllerRoute(
                name: "categoriaFiltro",
                pattern: "Lanche/{action}/{categoria?}",
                defaults: new {Controller = "Lanche", action="List"});
             
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}