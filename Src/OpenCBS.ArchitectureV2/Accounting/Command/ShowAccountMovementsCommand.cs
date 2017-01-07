﻿using System;
using OpenCBS.ArchitectureV2.Interface;
using OpenCBS.Extension.Accounting.CommandData;
using OpenCBS.Extension.Accounting.Interface.Presenter;

namespace OpenCBS.Extension.Accounting.Command
{
    public class ShowAccountMovementsCommand : ArchitectureV2.Command.Command, ICommand<ShowAccountMovementsCommandData>
    {
        private readonly Lazy<IAccountMovementsPresenter> _presenter;

        public ShowAccountMovementsCommand(Lazy<IAccountMovementsPresenter> presenter,
            IApplicationController applicationController)
            : base(applicationController)
        {
            _presenter = presenter;
        }

        public void Execute(ShowAccountMovementsCommandData commandData)
        {
            if (ActivateViewIfExists("AccountMovementsView"))
            {
                return;
            }
            if (string.IsNullOrEmpty(commandData.Account))
            {
                _presenter.Value.Run();
            }
            else
            {
                _presenter.Value.Run(commandData.Account, commandData.From, commandData.To);
            }
        }
    }
}