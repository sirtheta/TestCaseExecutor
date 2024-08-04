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

using CsvHelper.Configuration.Attributes;
using System;

namespace TestCaseExecutor.MainClasses
{
    /// <summary>
    /// Mappingclass of the test suite CSV exported from azure devops
    /// </summary>
    internal class ImportCSVMapping
    {
        [Index(0)]
        public string? ID { get; set; }
        [Index(2)]
        public string? Title { get; set; }
        [Index(3)]
        public string? TestStep { get; set; }
        [Index(4)]
        public string? StepAction { get; set; }
        [Index(5)]
        public string? StepExpected { get; set; }
    }
}
