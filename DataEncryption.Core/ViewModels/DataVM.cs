using System;
using System.Collections.Generic;
using System.Text;
using DataEncryption.Models;
using Core.MVVM;
using System.ComponentModel;
using DataEncryption.Core.BL;

namespace DataEncryption.Core.ViewModels
{
    class DataVM : ViewModelWithBL<DataBL>
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

        private string mensaje;
        public string Mensaje { get => mensaje; set => Set(ref mensaje, value); }

        private bool verMensaje;
        public bool VerMensaje { get => verMensaje; set => Set(ref verMensaje, value); }

        private string mostrarEncryptacion;
        public string MostrarEncryptacion { get => mostrarEncryptacion; set => Set(ref mostrarEncryptacion, value); }

        private string mostrarDesencryptacion;
        public string MostrarDesencryptacion { get => mostrarDesencryptacion; set => Set(ref mostrarDesencryptacion, value); }
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
        RelayCommand encryptionCommand = null;
        public RelayCommand EncryptionCommand
        {
            get => encryptionCommand ?? (encryptionCommand = new RelayCommand(async () =>
            {

               var res = await bl.GetEncriptarAsync(DataSelected.UrlServidorVM);
                MostrarEncryptacion = res.Mensaje;
            }, () => { return true; }));
        }

        RelayCommand desencryptionCommand = null;
        public RelayCommand DesencryptionCommand
        {
            get => desencryptionCommand ?? (desencryptionCommand = new RelayCommand(async () =>
            {

                var result = await bl.GetDesencriptarAsync(DataSelected.DesencryptionKey);
                MostrarDesencryptacion = result.Mensaje;

            }, () => { return true; }));
        }


        #endregion
    }
}
