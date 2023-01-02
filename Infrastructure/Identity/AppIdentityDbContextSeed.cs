using Core.Entities;
using Core.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GetGroup.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            if (roleManager.Roles.Count() == 0 && userManager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin",
                    Email = "system@integrated-ps.com",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    CreateUserId = 1,
                    UpdateUserId = 1,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(user, "IpsP@ssw0rd");
                if (result.Succeeded)
                {

                    foreach (var role in Enum.GetValues(typeof(Roles)))
                    {
                        var newRole = new IdentityRole<int>
                        {
                            Name = role.ToString()
                        };

                        await roleManager.CreateAsync(newRole);
                        await userManager.AddToRoleAsync(user, role.ToString());

                        if (role.ToString() == Roles.Users.ToString())
                        {
                            var claims = new List<Claim> {
                        new Claim(Roles.Users.ToString(), Permissions.AddUser.ToString()),
                        new Claim(Roles.Users.ToString(), Permissions.EditUser.ToString()),
                        new Claim(Roles.Users.ToString(), Permissions.ViewUser.ToString()),
                        new Claim(Roles.Users.ToString(), Permissions.ActivateUser.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Suppliers.ToString())
                        {
                            var claims = new List<Claim> {
                        new Claim(Roles.Suppliers.ToString(), Permissions.AddSupplier.ToString()),
                        new Claim(Roles.Suppliers.ToString(), Permissions.EditSupplier.ToString()),
                        new Claim(Roles.Suppliers.ToString(), Permissions.ViewSupplier.ToString()),
                        new Claim(Roles.Suppliers.ToString(), Permissions.DeleteSupplier.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Clients.ToString())
                        {
                            var claims = new List<Claim> {
                        new Claim(Roles.Clients.ToString(), Permissions.AddClient.ToString()),
                        new Claim(Roles.Clients.ToString(), Permissions.EditClient.ToString()),
                        new Claim(Roles.Clients.ToString(), Permissions.ViewClient.ToString()),
                        new Claim(Roles.Clients.ToString(), Permissions.DeleteClient.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Inventory.ToString())
                        {
                            var claims = new List<Claim> {
                        new Claim( Roles.Inventory.ToString(), Permissions.AddProduct.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.EditProduct.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.ViewProduct.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.DeleteProduct.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.AddService.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.EditService.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.ViewService.ToString()),
                        new Claim( Roles.Inventory.ToString(), Permissions.DeleteService.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Purchasing.ToString())
                        {
                            var claims = new List<Claim> {
                        new Claim( Roles.Purchasing.ToString(), Permissions.AddPurchase.ToString()),
                        new Claim( Roles.Purchasing.ToString(), Permissions.EditPurchase.ToString()),
                        new Claim( Roles.Purchasing.ToString(), Permissions.ViewPurchase.ToString()),
                        new Claim( Roles.Purchasing.ToString(), Permissions.CancelPurchase.ToString()),
                        new Claim( Roles.Purchasing.ToString(), Permissions.AddPurchasePayment.ToString()),
                        new Claim( Roles.Purchasing.ToString(), Permissions.EditPurchasePayment.ToString()),
                        new Claim( Roles.Purchasing.ToString(), Permissions.DeletePurchasePayment.ToString()),

                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Sales.ToString())
                        {
                            var claims = new List<Claim>{
                        new Claim(Roles.Sales.ToString(), Permissions.AddSales.ToString()),
                        new Claim(Roles.Sales.ToString(), Permissions.EditSales.ToString()),
                        new Claim(Roles.Sales.ToString(), Permissions.ViewSales.ToString()),
                        new Claim(Roles.Sales.ToString(), Permissions.CancelSales.ToString()),
                        new Claim(Roles.Sales.ToString(), Permissions.AddSalesPayment.ToString()),
                        new Claim(Roles.Sales.ToString(), Permissions.EditSalesPayment.ToString()),
                        new Claim(Roles.Sales.ToString(), Permissions.DeleteSalesPayment.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Expenses.ToString())
                        {
                            var claims = new List<Claim>{
                        new Claim(Roles.Expenses.ToString(), Permissions.AddExpense.ToString()),
                        new Claim(Roles.Expenses.ToString(), Permissions.EditExpense.ToString()),
                        new Claim(Roles.Expenses.ToString(), Permissions.ViewExpense.ToString()),
                        new Claim(Roles.Expenses.ToString(), Permissions.DeleteExpense.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }
                        else if (role.ToString() == Roles.Transaction.ToString())
                        {
                            var claims = new List<Claim>{
                        new Claim(Roles.Transaction.ToString(), Permissions.ViewTransaction.ToString()),
                        };
                            foreach (var claim in claims)
                            {
                                await roleManager.AddClaimAsync(newRole, claim);
                            }
                            await userManager.AddClaimsAsync(user, claims);
                        }

                    }
                }

            }

        }
    }
}
