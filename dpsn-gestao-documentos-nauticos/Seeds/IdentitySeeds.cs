using dpsn_gestao_documentos_nauticos.Models;
using Microsoft.AspNetCore.Identity;

namespace dpsn_gestao_documentos_nauticos.Seeds
{
    public class IdentitySeeds
    {
        public static async Task SeedRolesAndUser(IServiceProvider serviceProvider,
            string defaultPassword)
        {
            // Craindo as roles Admin e User
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roleNames = { "Admin", "Estaleiro", "Tecnologo" };
            foreach (var roleName in roleNames)
            {
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    var result = await roleManager.CreateAsync(
                            new ApplicationRole { Name = roleName }
                        );
                    if (result.Succeeded)
                    {
                        Console.WriteLine($"SEED: Role {roleName} foi criada");
                    }
                    else { return; }
                }
            } // Fim do foreach

            // Criando um usuário admin
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByEmailAsync("jpderussi@gmail.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "jpderussi@gmail.com",
                    Email ="jpderussi@gmail.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, defaultPassword);
                if (result.Succeeded)
                {
                    Console.WriteLine($"SEED: Usuário admin {adminUser.UserName} foi criado");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                } else { return; }
            }
        }
    }
}
