﻿// LICENSE PLACEHOLDER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenCBS.Enums;
using OpenCBS.MultiLanguageRessources;
using OpenCBS.CoreDomain.Accounting;
using OpenCBS.Services;

namespace OpenCBS.GUI.Accounting
{
    public partial class FrmAccount : Form
    {
        public FrmAccount()
        {
           Mode = 0;
           InitializeComponent();
           Initialize();
        }

        public FrmAccount(bool showBothTabs)
        {
            Mode = 0;
            ShowBothTabs = showBothTabs;
            InitializeComponent();
            Initialize();
        }

        public bool ShowBothTabs { get; set; }       

        public Account Account
        {
            get { return GetAccount(); }
            set { SetAccount(value); }
        }

        public AccountCategory AccountCategory
        {
            get { return GetAccountCategory(); }
            set { SetAccountCategory(value); }
        }

        // 0 - Account adding mode
        // 1 - Account category adding mode
        public int Mode { get; set; }

        private AccountCategory GetAccountCategory()
        {
            return new AccountCategory { Name = tbAccountCategory.Text};
        }

        private void SetAccountCategory(AccountCategory accountCategory)
        {
            tbAccountCategory.Text = accountCategory.Name;
        }

        private void SetAccount(Account pAccount)
        {
            if (!pAccount.ParentAccountId.HasValue)
            {
                string name = "";
                List<AccountCategory> accountCategories =
                    ServicesProvider.GetInstance().GetChartOfAccountsServices().SelectAccountCategories();

                foreach (var accountCategory in
                    accountCategories.Where(accountCategory => accountCategory.Id == (int) pAccount.AccountCategory))
                {
                    name = accountCategory.Name;
                }
                treeViewAccounts.SelectedNode = treeViewAccounts.Nodes.Find(name, false)[0];
            }
            else
            {
                treeViewAccounts.SelectedNode = treeViewAccounts.Nodes.Find(pAccount.ParentAccountId.ToString(), true)[0];
            }

            textNumericNumber.Text = pAccount.Number;
            textBoxLabel.Text = pAccount.Label;

            if (pAccount.Type)
            {
                textNumericNumber.Enabled = false;
            }

            rbCredit.Checked = !pAccount.DebitPlus;
            rbDebit.Checked = pAccount.DebitPlus;
        }

        private Account GetAccount()
        {
            var account = new Account
            {
                Number = textNumericNumber.Text,
                Label = textBoxLabel.Text,
                Balance = 0,
                StockBalance = 0
            };

            if (treeViewAccounts.SelectedNode.Tag is OAccountCategories)
            {
                account.AccountCategory = (OAccountCategories)treeViewAccounts.SelectedNode.Tag;
            }
            else
            {
                account.ParentAccountId = ((Account)treeViewAccounts.SelectedNode.Tag).Id;
                account.ParentAccountId = account.ParentAccountId.Value == -1 ? null : account.ParentAccountId;
                account.AccountCategory = ((Account)treeViewAccounts.SelectedNode.Tag).AccountCategory;
            }
            account.TypeCode = account.Label.ToUpper();
            account.DebitPlus = rbDebit.Checked;

            return account;
        }

        private void Initialize()
        {
            List<Account> accounts = ServicesProvider.GetInstance().GetChartOfAccountsServices().FindAllAccounts();
            List<AccountCategory> accountCategories = ServicesProvider.GetInstance().GetChartOfAccountsServices().SelectAccountCategories();

            foreach (AccountCategory accountCategory in accountCategories)
            {
                string name = MultiLanguageStrings.GetString(
                    Ressource.ChartOfAccountsForm, accountCategory.Name + ".Text");
                name = name ?? accountCategory.Name;

                Account accountCat = new Account
                {
                    Label = name,
                    AccountCategory = (OAccountCategories)accountCategory.Id,
                    Id = -1
                };

                TreeNode nodeAssets =
                new TreeNode(name)
                {
                    Tag = accountCat,
                    Name = accountCategory.Name
                };
                treeViewAccounts.Nodes.Add(nodeAssets);

                foreach (Account account in accounts.Where(item => item.AccountCategory == accountCat.AccountCategory && !item.ParentAccountId.HasValue))
                    AddAccountInTree(nodeAssets, account, accounts);
                
            }
            
            if(!ShowBothTabs)
                tabControlData.TabPages.Remove(tabPageCategory);
            if (accountCategories.Count == 0)
            {
                tabControlData.TabPages.Remove(tabPageAccount);
                Mode = 1;
            }
            else
                treeViewAccounts.SelectedNode = treeViewAccounts.Nodes[0];
        }

        private static void AddAccountInTree(TreeNode pParent, Account pAccount, List<Account> pListAccount)
        {
            TreeNode node = new TreeNode(pAccount.ToString()) { Tag = pAccount, Name = pAccount.Id.ToString() };
            pParent.Nodes.Add(node);
            foreach (Account account in pListAccount.Where(item => item.ParentAccountId == pAccount.Id))
                AddAccountInTree(node, account, pListAccount);
        }

        private void treeViewAccounts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            labelParentValue.Text = treeViewAccounts.SelectedNode.FullPath;
        }

        private void tabControlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            Mode = tabControlData.SelectedIndex;
        }
    }
}
