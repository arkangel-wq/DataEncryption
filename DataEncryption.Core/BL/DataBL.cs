﻿using DataEncryption.Core.ApiClient;
using DataEncryption.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEncryption.Core.BL
{
    public class DataBL
    {
        private DataServicesEncryption dataServices;

        public DataServicesEncryption DataServicesEncryption => dataServices ?? (dataServices = new DataServicesEncryption());

        public async Task<(bool isValid, string Mensaje)> GetEncriptarAsync(Data data)
        {
            var res = await DataServicesEncryption.GetEncriptar(data);
            if (res.statusCode == System.Net.HttpStatusCode.Created) return (true, res.Mensaje);
            return (false, "No se pudo realizar la peticion");
        }
        public async Task<(bool isValid, string Mensaje)> GetDesencriptarAsync(Data data)
        {
            var res = await DataServicesEncryption.GetDesencriptar(data);
            if (res.statusCode == System.Net.HttpStatusCode.Created) return (true, res.Mensaje);
            return (false, "No se pudo realizar la peticion");
        }
    }
}
