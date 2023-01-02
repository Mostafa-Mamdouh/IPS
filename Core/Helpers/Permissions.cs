using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public enum Permissions
    {
        [Description("Add User")]
        AddUser = 1,
        [Description("Edit User")]
        EditUser,
        [Description("List/View Users")]
        ViewUser,
        [Description("Activate/Deactivate User")]
        ActivateUser,

        [Description("Add Client")]
        AddClient,
        [Description("Edit Client")]
        EditClient,
        [Description("List/View Clients")]
        ViewClient,
        [Description("Delete Client")]
        DeleteClient,

        [Description("Add Supplier")]
        AddSupplier,
        [Description("Edit Supplier")]
        EditSupplier,
        [Description("List/View Suppliers")]
        ViewSupplier,
        [Description("Delete Supplier")]
        DeleteSupplier,

        [Description("Add Product")]
        AddProduct,
        [Description("Edit Product")]
        EditProduct,
        [Description("List/View Products")]
        ViewProduct,
        [Description("Delete Product")]
        DeleteProduct,

        [Description("Add Service")]
        AddService,
        [Description("Edit Service")]
        EditService,
        [Description("List/View Services")]
        ViewService,
        [Description("Delete Service")]
        DeleteService,

        [Description("Add Purchasing Invoice")]
        AddPurchase,
        [Description("Edit Purchasing Invoice")]
        EditPurchase,
        [Description("List/View Purchasing Invoices")]
        ViewPurchase,
        [Description("Cancel Purchasing Invoice")]
        CancelPurchase,
        [Description("Add Purchasing Invoice Payment")]
        AddPurchasePayment,
        [Description("Edit Purchasing Invoice Payment")]
        EditPurchasePayment,
        [Description("Delete Purchasing Invoice Payment")]
        DeletePurchasePayment,

        [Description("Add Sales Invoice")]
        AddSales,
        [Description("Edit Sales Invoice")]
        EditSales,
        [Description("List/View Sales Invoices")]
        ViewSales,
        [Description("Cancel Sales Invoice")]
        CancelSales,
        [Description("Add Sales Invoice Payment")]
        AddSalesPayment,
        [Description("Edit Sales Invoice Payment")]
        EditSalesPayment,
        [Description("Delete Sales Invoice Payment")]
        DeleteSalesPayment,

        [Description("Add Expense")]
        AddExpense,
        [Description("Edit Expense")]
        EditExpense,
        [Description("List/View Expenses")]
        ViewExpense,
        [Description("Delete Expense")]
        DeleteExpense,

        [Description("List/View Transactions")]
        ViewTransaction,
    }
    public enum Roles
    {
        Users = 1,
        Clients,
        Suppliers,
        Inventory,
        Purchasing,
        Sales,
        Expenses,
        Transaction
    }


}
