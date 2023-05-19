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
                var inicio = "https://";
                if (DataSelected.UrlApis == null)
                {
                    Mensaje = "Ingresa Url Apis publicas 😒";
                    VerMensaje = true;
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    verMensaje = false;
                }
                else
                {
                    if (DataSelected.UrlApis.StartsWith(inicio))
                    {
                        Helpers.ApiHelper.Instance.UrlApisPublicas = DataSelected.UrlApis;
                        Console.WriteLine(Helpers.ApiHelper.Instance.UrlApisPublicas);
                        Mensaje = "Se establecio correctamente la url de las Apis publicas ❤️";
                        VerMensaje = true;
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        verMensaje = false;
                    }
                    else
                    {
                        Mensaje = "La Url de las Apis Publicas no es valida";
                        VerMensaje = true;
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        verMensaje = false;
                    }
                    
                }
            }));
        }

        RelayCommand encryptionCommand = null;
        public RelayCommand EncryptionCommand
        {
            get => encryptionCommand ?? (encryptionCommand = new RelayCommand(async () =>
            {
                var inicio = "rtmp";

                if (Helpers.ApiHelper.Instance.UrlApisPublicas == null)
                {
                    Mensaje = "Ingresa Url Apis publicas 😒";
                    VerMensaje = true;
                    await Task.Delay(TimeSpan.FromSeconds(3));
                    verMensaje = false;
                }
                else
                {
                    if (DataSelected.UrlServidorVM == null)
                    {
                        Mensaje = "Ingresa la Url del servidor de streaming";
                        VerMensaje = true;
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        verMensaje = false;
                    }
                    else
                    {
                        if (DataSelected.UrlServidorVM.StartsWith(inicio))
                        {
                            var res = await bl.GetEncriptarAsync(DataSelected.UrlServidorVM);
                            MostrarEncryptacion = res.Mensaje;
                            Mensaje = "Encryptando la Url del servidor de streaming";
                            VerMensaje = true;
                            await Task.Delay(TimeSpan.FromSeconds(3));
                            verMensaje = false;
                        }
                        else
                        {
                            Mensaje = "La Url del servidor de streaming no es valida";
                            VerMensaje = true;
                            await Task.Delay(TimeSpan.FromSeconds(3));
                            verMensaje = false;
                        }
                    }
                }

            }, () => { return true; }));
        }

        RelayCommand desencryptionCommand = null;
        public RelayCommand DesencryptionCommand
        {
            get => desencryptionCommand ?? (desencryptionCommand = new RelayCommand(async () =>
            {
                try
                {
                    if (Helpers.ApiHelper.Instance.UrlApisPublicas == null)
                    {
                        Mensaje = "Ingresa Url Apis publicas 😒";
                        VerMensaje = true;
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        verMensaje = false;
                    }
                    else
                    {
                        if (DataSelected.DesencryptionKey == null)
                        {
                            Mensaje = "Ingresa cadena encryptada 😒";
                            VerMensaje = true;
                            await Task.Delay(TimeSpan.FromSeconds(3));
                            verMensaje = false;
                        }
                        else
                        {
                            int longitud = DataSelected.DesencryptionKey.Length;
                            int cadenavalida = 72;

                            if (longitud == cadenavalida)
                            {
                                var result = await bl.GetDesencriptarAsync(DataSelected.DesencryptionKey);
                                MostrarDesencryptacion = result.Mensaje;
                                Mensaje = "Desencryptando la Url del servidor de streaming 😎";
                                VerMensaje = true;
                                await Task.Delay(TimeSpan.FromSeconds(3));
                                verMensaje = false;
                            }
                            else
                            {

                                Mensaje = "La Cadena de encrytacion no es valida 🤷‍♂️";
                                VerMensaje = true;
                                await Task.Delay(TimeSpan.FromSeconds(3));
                                verMensaje = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }, () => { return true; }));
        }


        #endregion
    }
}
