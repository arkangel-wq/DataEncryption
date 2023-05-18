using System;
using System.Collections.Generic;
using System.Text;
using DataEncryption.Models;
using Core.MVVM;
using System.ComponentModel;

namespace DataEncryption.Core.ViewModels
{
    class DataVM : ViewModelWithBL<Data>
    {
        #region Builde
        public DataVM()
        {
            DataSelected = new();
        }
        #endregion

        #region Properties
        private Data dataSelected;
        public Data DataSelected { get => dataSelected; set => Set(ref dataSelected, value); }
        #endregion

        #region Commands


        RelayCommand readUrlCommand = null;
        public RelayCommand ReadUrlCommand
        {
            get => readUrlCommand ?? (readUrlCommand = new RelayCommand(async () =>
            {
                Helpers.ApiHelper.Instance.UrlApisPublicas = DataSelected.UrlApis;
                Console.WriteLine(Helpers.ApiHelper.Instance.UrlApisPublicas) ;
            }));
        }
        #endregion
    }
}
