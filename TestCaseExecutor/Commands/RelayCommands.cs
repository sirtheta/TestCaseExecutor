/*
 * Copyright (C) 2024 Michael Neuhaus
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TestCaseExecutor.Commands
{
    internal class RelayCommand<T> : ICommand
    {

        #region Members
        private readonly Action<T>? _Execute = null;
        private readonly Predicate<T>? _CanExecute = null;
        #endregion


        #region Constructors
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public RelayCommand(Action<T> Execute) : this(Execute, CanExecute: null) { }

        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="Execute">Execution logic</param>
        /// <param name="CanExecute">Execution status logic</param>
        public RelayCommand(Action<T>? Execute, Predicate<T>? CanExecute)
        {
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _CanExecute = CanExecute;
        }
        #endregion


        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object Parameter)
        {
            return _CanExecute == null || _CanExecute((T)Parameter);
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object Parameter)
        {
            _Execute?.Invoke((T)Parameter);
        }
        #endregion
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    }
}