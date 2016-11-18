using System;
using System.Data;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public partial class Macro200en : System.Web.UI.UserControl, IMacro
    {
        string macro;
        public string Texto { get; set; }
        public string Mascara { get; set; }
        public string TamanhoMascara { get; set; }
        public Entities.Macro EntidadeMacro { get; set; }
        int numCoordenadas;

        protected void Page_Load(object sender, EventArgs e)
        {

            converteMacroHexadecimal(EntidadeMacro.Texto);
            numCoordenadas = getNumeroDeCoordenadas();
        
            var b = EntidadeMacro.Texto;

            if (temInformacaoDeLicenciamento() || temCancelamentoTotal())
            {
                //string latitude = "";
                //string longitude = "";
                //string velocidade = "";

                DataTable dt = new DataTable();

                //dt.Columns.Add(latitude, System.Type.GetType("System.String"));
                //dt.Columns.Add(longitude, System.Type.GetType("System.String"));
                //dt.Columns.Add(velocidade, System.Type.GetType("System.String"));
                dt.Columns.Add("Latitude", typeof(string));
                dt.Columns.Add("Longitude", typeof(string));
                dt.Columns.Add("Velocidade", typeof(string));
                determinaFuncaoLicenciamento();

                for (int i = 1; i <= numCoordenadas; i++)
                {
                    var latitude = getLatitude(i).ToString();
                    var longitude = getLongitude(i).ToString();
                    var velocidade = getVelocidade(i).ToString();

                    dt.Rows.Add(new String[] { latitude, longitude, velocidade });
                }
                GridViewPontosTroca.DataSource = dt;
                GridViewPontosTroca.DataBind();

            }
            if (temParametroDeAproximacaoMaxima())
            {
                var a = Convert.ToString(getParametroDeAproximacaoMaxima());
                TextBoxAproximacaoMaxima.Text = Convert.ToString(getParametroDeAproximacaoMaxima());
            }
            if (temPalavraDeComando())
            {
                traduzPalavraComando();
            }
            if (temConfiguracaoDasMensagensDeRetorno())
            {
                traduzConfiguracaoDeRetorno();
            }


            //   t.Text = Convert.ToString(numCoordenadas);
            //var a = Convert.ToString(ParametroDeAproximacaoMaxima());
            //TextBoxAproximacaoMaxima.Text = Convert.ToString(ParametroDeAproximacaoMaxima());


            //if (temParametroDeAproximacaoMaxima())
            //{
            //    GroupBoxAproxMax->Font->Color = clWindowText;
            //    CheckBoxAproxMax->Checked = true;
            //    EditAproxMax->Text = getParametroDeAproximacaoMaxima();
            //}
            //else
            //{
            //    GroupBoxAproxMax->Font->Color = clInactiveCaption;
            //    CheckBoxAproxMax->Checked = false;
            //}
            //if (temPalavraDeComando())
            //{
            //    traduzPalavraComando();
            //    GroupBoxPalavraComando->Font->Color = clWindowText;
            //    CheckBoxPalavraComando->Checked = true;
            //}
            //else
            //{
            //    GroupBoxPalavraComando->Font->Color = clInactiveCaption;
            //    CheckBoxPalavraComando->Checked = false;
            //}
            //if (temConfiguracaoDasMensagensDeRetorno())
            //{
            //    traduzConfiguracaoDeRetorno();
            //    GroupBoxConfRetorno->Font->Color = clWindowText;
            //    CheckBoxConfRetorno->Checked = true;
            //}
            //else
            //{
            //    GroupBoxConfRetorno->Font->Color = clInactiveCaption;
            //    CheckBoxConfRetorno->Checked = false;
            //}


        }
        
        public void converteMacroHexadecimal(string texto)
        {
            macro = "";
            char teste;
            int j = 0;
            string aux = "";
            string convert = "";
            while (j < texto.Length)
            {
                aux = texto.Substring(j, 2);
                var o = HexToInt(aux);
                teste = Convert.ToChar(HexToInt(aux));
                convert = Convert.ToString(teste);
                macro = macro + convert;
                j = j + 2;
            }
            //           MessageBox.Show(Convert.ToString(teste));
        }

        public bool temInformacaoDeLicenciamento()
        {
            return ((Byte)macro[0] & 0x01) != 0x00;
        }
        public bool temParametroDeAproximacaoMaxima()
        {
            return ((Byte)(macro[0]) & 0x08) != 0x00;
        }
        public bool temPalavraDeComando()
        {
            return ((Byte)(macro[0]) & 0x10) != 0x00;
        }
        public bool temConfiguracaoDasMensagensDeRetorno()
        {
            return ((Byte)(macro[0]) & 0x20) != 0x00;
        }
        public string getCoordenada(int posicao)
        {
            var a = ((Byte)(macro[posicao + 1]) << 16);
            var b = ((Byte)(macro[posicao]) << 8);
            var c = (Byte)(macro[posicao - 1]);
            return IntToCoord(a + b + c);
        }
        public string getLatitude(int indice)
        {
            return getCoordenada(2 + 7 * (indice - 1)) + " S";
        }
        public string getLongitude(int indice)
        {
            return getCoordenada(5 + 7 * (indice - 1)) + " W";
        }
        public string getVelocidade(int indice)
        {
            return Convert.ToString(((Byte)(macro[7 + 7 * (indice - 1)])) + " km/h");
        }
        public string getParametroDeAproximacaoMaxima()
        {
            return Convert.ToString((Byte)(macro[2 + 7 * numCoordenadas]) * 100);
        }
        public int getNumeroDeCoordenadas()
        {
            int quant;
            quant = macro.Length - 1;
            if (quant > 1)
            {
                if (temParametroDeAproximacaoMaxima())
                {
                    quant = quant - 1;
                }
                if (temPalavraDeComando())
                {
                    quant = quant - 1;
                }
                if (temConfiguracaoDasMensagensDeRetorno())
                {
                    quant = quant - 1;
                }
                if (quant > 0)
                {
                    return (quant / 7);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public void determinaFuncaoLicenciamento()
        {
            uint CTR;
            //(macro[1]) alterado para 0, Confirmar
            CTR = (Byte)(macro[0]);
            if ((CTR & 0x40) == 0x40)
            {
                CheckBoxAdicaoPontosTroca.Checked = true;

            }
            if ((CTR & 0x80) == 0x80)
            {
                CheckBoxCancelParcialBuffer.Checked = true;
            }
            if ((CTR & 0xC0) == 0x00)

                CheckBoxSubstituicaoBufferTroca.Checked = true;

            if (temCancelamentoTotal())

                CheckBoxCancelTotalBufferTroca.Checked = true;
        }
        public bool temCancelamentoTotal()
        {
            return (((Byte)(macro[0]) & 0xC0) == 0xC0) & (((Byte)(macro[0]) & 0x03) == 0x00);
        }
        public void traduzPalavraComando()
        {
            uint COM;

            if (temParametroDeAproximacaoMaxima())
            {
                COM = (Byte)(macro[2 + 6 * numCoordenadas]);
                //alterado de 3 + 7 para 2 + 6
            }
            else
            {
                COM = (Byte)(macro[1 + 6 * numCoordenadas]);
                //alterado de 2 + 7 para 1 + 6
            }
            if ((COM & 0x01) == 0x01)
            {
                CheckBoxAplicacaoFreio.Checked = true;
            }
            else
            {
                CheckBoxAplicacaoFreio.Checked = false;
            }
            if ((COM & 0x02) == 0x02)
            {
                CheckBoxLiberacaoFreio.Checked = true;
            }
            else
            {
                CheckBoxLiberacaoFreio.Checked = false;
            }
            if ((COM & 0x04) == 0x04)
            {
                CheckBoxDesabilitaFuncaoSuper.Checked = true;
            }
            else
            {
                CheckBoxDesabilitaFuncaoSuper.Checked = false;
            }
            if ((COM & 0x08) == 0x08)
            {
                CheckBoxHabilitaFuncaoSuper.Checked = true;
            }
            else
            {
                CheckBoxHabilitaFuncaoSuper.Checked = false;
            }
            if ((COM & 0x10) == 0x10)
            {
                CheckBoxSolicitaNumeroPontoTroca.Checked = true;
            }
            else
            {
                CheckBoxSolicitaNumeroPontoTroca.Checked = false;
            }
        }
        public void traduzConfiguracaoDeRetorno()
        {
            uint CONF;
            int offset;

            offset = 2;
            if (temParametroDeAproximacaoMaxima())
            {
                offset = offset + 1;
            }
            if (temPalavraDeComando())
            {
                offset = offset + 1;
            }
            CONF = (Byte)(macro[offset + 7 * numCoordenadas]);
            if ((CONF & 0x01) == 0x01)
            {
                CheckBoxCodigoInicializacao.Checked = true;
            }
            else
            {
                CheckBoxCodigoInicializacao.Checked = false;
            }
            if ((CONF & 0x02) == 0x02)
            {
                CheckBoxCodigoErroInicializacao.Checked = true;
            }
            else
            {
                CheckBoxCodigoErroInicializacao.Checked = false;
            }
            if ((CONF & 0x04) == 0x04)
            {
                CheckBoxComutacaoPermissivo.Checked = true;
            }
            else
            {
                CheckBoxComutacaoPermissivo.Checked = false;
            }
            if ((CONF & 0x08) == 0x08)
            {
                CheckBoxAplicouFreioVeloLimite.Checked = true;
            }
            else
            {
                CheckBoxAplicouFreioVeloLimite.Checked = false;
            }
            if ((CONF & 0x10) == 0x10)
            {
                CheckBoxAplicouFreioInvasao.Checked = true;
            }

            else
            {
                CheckBoxAplicouFreioInvasao.Checked = false;
            }
            if ((CONF & 0x20) == 0x20)
            {
                CheckBoxDescarrilhamento.Checked = true;
            }
            else
            {
                CheckBoxDescarrilhamento.Checked = false;
            }
        }

        public int HexToInt(string valor)
        {
            int i, multiplicador;
            int resultado = 0;
            i = valor.Length - 1;
            multiplicador = 1;
            valor = valor.ToUpper();
            while (i >= 0)
            {
                if (valor[i] == '0' || valor[i] == '1' || valor[i] == '2' || valor[i] == '3' || valor[i] == '4' || valor[i] == '5'
                        || valor[i] == '6' || valor[i] == '7' || valor[i] == '8' || valor[i] == '9')
                {
                    resultado = resultado + (int)Char.GetNumericValue(valor[i]) * multiplicador;
                }
                else
                {
                    Convert.ToSByte("B", 16);
                    resultado = resultado + ((int)(valor[i]) - (int)('A') + 10) * multiplicador;
                }
                multiplicador = multiplicador << 4;
                i--;
            }
            return resultado;
        }
        //public int ParametroDeAproximacaoMaxima()
        //{
        //    //offset de 2 para 1, resolve erro: valor fora do indice da matriz
        //    int offset;
        //    offset = 1;
        //    if (temPontoLimite())
        //    {
        //        offset = offset + 6;
        //    }
        //    if (temVelocidadeLimite())
        //    {
        //        offset = offset + 1;
        //    }
        //    if (temPontoDeTrocaDeVelocidade())
        //    {
        //        offset = offset + 6;
        //    }
        //    return (int)((Byte)(macro[offset]) * 100);
        //}
        public bool temPontoLimite()
        {
            return (((Byte)(macro[1]) & 0x01) != 0x00);
        }
        public bool temPontoDeTrocaDeVelocidade()
        {
            return (((Byte)(macro[1]) & 0x04) != 0x00);
        }
        //public bool temVelocidadeLimite()
        //{
        //    return (((Byte)(macro[1]) & 0x02) != 0x00);
        //}
        public string IntToCoord(int coordenada)
        {
            string resultado;
            double remCoordenada = 0.0;
            resultado = "";
            coordenada = (coordenada / 10);
            remCoordenada = (coordenada % 3600);
            resultado = resultado + string.Format("{0:000}", (coordenada / 3600));

            remCoordenada = (coordenada % 3600);
            coordenada = (coordenada / 3600);
            coordenada = (int)remCoordenada;
            resultado = resultado + string.Format("{0:00}", (coordenada / 60));

            remCoordenada = (coordenada % 60);
            coordenada = (int)remCoordenada;
            resultado = resultado + string.Format("{0:00}", (remCoordenada));

            return resultado;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}

