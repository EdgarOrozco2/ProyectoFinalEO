using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProyectoFinal
{
    public partial class _default : System.Web.UI.Page
    {
        private static string key = ConfigurationManager.AppSettings["AesKey"]; 
        private static string iv = ConfigurationManager.AppSettings["AesIV"];

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                Tarjeta tarjeta = new Tarjeta()
                {
                    nombre = tbxNombre.Text,
                    apellido = tbxApellido.Text,
                    numero_tarjeta = ViewState["ccNbr"].ToString(),
                    fecha_caducidad = tbxFC.Text,
                    cvc = ViewState["ccCvc"].ToString()
                };

                validarInformacion(tarjeta);

                //Se codifica la información usando SHA256 para convertir a hexadecimal
                // No se codifican campos nombre y apellido ya que no se considera como información critica
                Tarjeta tarjetaCodificada = encriptarTarjeta(tarjeta, TipoEncriptacion.SHA256);

                //Se encripta la información usando AES256 (Esta información debe ser subida a DB)
                Tarjeta tarjetaEncriptada = encriptarTarjeta(tarjeta, TipoEncriptacion.AES256);

                //Se desencripta información
                Tarjeta tarjetaDesencriptada = desencriptarTarjeta(tarjetaEncriptada);

                //Se codifica tarjeta desecriptada para comparar
                Tarjeta tarjetaDesSHA256 = encriptarTarjeta(tarjetaDesencriptada, TipoEncriptacion.SHA256);

                //Se compara tarjeta inicial con tarjeta final
                if (tarjetaCodificada.numero_tarjeta.Equals(tarjetaDesSHA256.numero_tarjeta) && tarjetaCodificada.fecha_caducidad.Equals(tarjetaDesSHA256.fecha_caducidad) && tarjetaCodificada.cvc.Equals(tarjetaDesSHA256.cvc))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Información procesada correctamente');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error al procesar la información');", true);
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
            }
        }

        private void validarInformacion(Tarjeta t)
        {
            try
            {
                // - Sanitización de información del lado del servidor -
                //Nombre
                string nombreRegex = @"^[a-zA-Z\s]+$";
                if (!(Regex.IsMatch(t.nombre + t.apellido, nombreRegex)))
                    throw new Exception("El nombre no tiene un formato correcto.");

                //Numero de tarjeta
                string cardRegex = @"^\d{4} \d{4} \d{4} \d{4}$";
                if (!(Regex.IsMatch(t.numero_tarjeta, cardRegex)))
                    throw new Exception("El número de tarjeta no tiene un formato correcto.");

                // Fecha de caducidad
                string fechaRegex = @"^(0[1-9]|1[0-2])\/\d{2}$";
                if (!(Regex.IsMatch(t.fecha_caducidad, fechaRegex)))
                    throw new Exception("La fecha de caducidad no tiene un formato correcto o la fecha esta fuera de rango.");

                // CVC
                string cvcReger = @"^\d{3}$";
                if (!(Regex.IsMatch(t.cvc, cvcReger)))
                    throw new Exception("El código de seguridad no tiene un formato correcto.");

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
            }
        }


        protected void tbxCardNbr_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (tbxCardNbr.Text.Length.Equals(19)) 
                {
                    ViewState["ccNbr"] = tbxCardNbr.Text;

                    string lastNbr = tbxCardNbr.Text.Substring(tbxCardNbr.Text.Length -4);

                    tbxCardNbr.Text = "**** **** **** " + lastNbr;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
            }
        }

        protected void tbxCVC_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (tbxCVC.Text.Length.Equals(3))
                {
                    ViewState["ccCvc"] = tbxCVC.Text;
                    tbxCVC.Text = "***";
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
            }
        }

        private Tarjeta encriptarTarjeta(Tarjeta t, TipoEncriptacion enc)
        {
            Tarjeta tmpTarjeta = new Tarjeta(){
                nombre = t.nombre,
                apellido = t.apellido,
                numero_tarjeta = enc == TipoEncriptacion.SHA256 ? encSha256(t.numero_tarjeta) : encAes(t.numero_tarjeta),
                fecha_caducidad = enc == TipoEncriptacion.SHA256 ? encSha256(t.fecha_caducidad) : encAes(t.fecha_caducidad),
                cvc = enc == TipoEncriptacion.SHA256 ? encSha256(t.cvc) : encAes(t.cvc),
            };

            return tmpTarjeta;
        }

        private Tarjeta desencriptarTarjeta(Tarjeta t)
        {
            Tarjeta tmpTarjeta = new Tarjeta()
            {
                nombre = t.nombre,
                apellido = t.apellido,
                numero_tarjeta = desAes(t.numero_tarjeta),
                fecha_caducidad = desAes(t.fecha_caducidad),
                cvc = desAes(t.cvc)
            };

            return tmpTarjeta;
        }

        private string encAes(string txt)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(txt);
                            }
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
                return null;
            }
        }

        private string desAes(string txt)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(key);
                    aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(txt)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
                return null;
            }
        }

        private string encSha256(string txt)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] txtBytes = Encoding.UTF8.GetBytes(txt); //Se convierte texto a bytes

                    byte[] sha256Bytes = sha256.ComputeHash(txtBytes); //Se codifica en bytes

                    StringBuilder sb = new StringBuilder(); //Se convierte a string
                    for (int i = 0; i < sha256Bytes.Length; i++)
                    {
                        sb.Append(sha256Bytes[i].ToString("x2"));
                    }

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{ex.Message}');", true);
                return null;
            }
        }

    }
}