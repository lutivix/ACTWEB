using System;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public partial class Macro200re1 : System.Web.UI.UserControl, IMacro
    {
        string macro;
        public string Texto { get; set; }
        public string Mascara { get; set; }
        public string TamanhoMascara { get; set; }
        public Entities.Macro EntidadeMacro { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //converteMacroHexadecimal(TextBox6.Text);
            //macro = "";
            converteMacroHexadecimal(EntidadeMacro.Texto);
            macro200OBC();
            if (temInicializacao())
            {
                //CheckBoxObcReiniciado.ForeColor = System.Drawing.SystemColors.WindowText;
                CheckBoxObcReiniciado.Checked = true;

            }
            else
            {
                //CheckBoxObcReiniciado.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                CheckBoxObcReiniciado.Checked = false;
            }

            if (temErroDeInicializacao())
            {
                //CheckBoxErro.ForeColor = System.Drawing.SystemColors.WindowText;
                CheckBoxErro.Checked = true;
                TextBoxCodigoErro.Text = Convert.ToString((Byte)(macro[1]));
            }
            else
            {
                //CheckBoxErro.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                CheckBoxErro.Checked = false;
                TextBoxCodigoErro.Text = " ";
            }

            if (temAplicacaoDeFreio())
            {
                determinaMotivoDeAplicacaoDeFreio();
            }
            else
            {
                //CheckBoxEmergenciaEOT.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxExessoVelocidade.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxInvasaoLimiteLicensa.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxDescarrilamento.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                TextBoxDescarrilamento.Text = "";
            }

            if (temAcionamentoDeModoPermissivo())
            {
                //CheckBoxModoPermissivo.ForeColor = System.Drawing.SystemColors.WindowText;
                CheckBoxModoPermissivo.Checked = true;

            }
            else
            {
                //CheckBoxModoPermissivo.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                CheckBoxModoPermissivo.Checked = false;

            }

            if (temInformacaoParaSincronismo())
            {
                determinaMotivoSincronismo();
            }
            else
            {
                //CheckBoxAtualizacaoNormal.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxRespSolicitacaoCco.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxUltrapassouNumeroPontosTroca.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxDescarteUltimoPontoTroca.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                //CheckBoxPenalExecessoVel.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                TextBoxLatitude.Text = "";
                TextBoxLongitude.Text = "";
                //determinaMotivoSincronismo();

            }
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

        //---------------------------------------------------------------------------
        public bool temInicializacao()
        {
            return (((Byte)(macro[0]) & 0x01) != 0x00);
        }
        //---------------------------------------------------------------------------
        public bool temErroDeInicializacao()
        {
            return (((Byte)(macro[0]) & 0x02) != 0x00);
        }
        //---------------------------------------------------------------------------
        public bool temAplicacaoDeFreio()
        {
            return (((Byte)(macro[0]) & 0x08) != 0x00);
        }
        //---------------------------------------------------------------------------
        public bool temAcionamentoDeModoPermissivo()
        {
            return (((Byte)(macro[0]) & 0x40) != 0x00);
        }
        //---------------------------------------------------------------------------
        public bool temInformacaoParaSincronismo()
        {
            return (((Byte)(macro[0]) & 0x80) != 0x00);
        }
        //---------------------------------------------------------------------------
        public void determinaMotivoDeAplicacaoDeFreio()
        {
            int offset;
            string enderecodesc;
            offset = 1;
            if (temErroDeInicializacao())
            {
                offset = offset + 1;
            }
            if ((Byte)(macro[offset] & 0x01) != 0)
            {
                CheckBoxEmergenciaEOT.ForeColor = System.Drawing.SystemColors.WindowText;
                CheckBoxEmergenciaEOT.Checked = true;
            }
            if ((Byte)(macro[offset] & 0x02) != 0)
            {
                CheckBoxExessoVelocidade.ForeColor = System.Drawing.SystemColors.WindowText;
                CheckBoxExessoVelocidade.Checked = true;
            }
            if ((Byte)(macro[offset] & 0x04) != 0)
            {
                CheckBoxInvasaoLimiteLicensa.ForeColor = System.Drawing.SystemColors.WindowText;
                CheckBoxInvasaoLimiteLicensa.Checked = true;
            }
            if ((Byte)(macro[offset] & 0x08) != 0)
            {
                CheckBoxDescarrilamento.Checked = true;
                CheckBoxDescarrilamento.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            if (CheckBoxDescarrilamento.Checked == true)
            {
                enderecodesc = "";
                enderecodesc = enderecodesc + IntToHex((Byte)(macro[offset + 2]));
                enderecodesc = enderecodesc + IntToHex((Byte)(macro[offset + 1]));
                TextBoxDescarrilamento.Text = enderecodesc;
            }
            else
            {
                TextBoxDescarrilamento.Text = "";
            }
        }

        public void determinaMotivoSincronismo()
        {
            int offset;
            uint SYNC;
            offset = 1;
            if (temErroDeInicializacao())
            {
                offset = offset + 1;
            }
            if (temAplicacaoDeFreio())
            {
                offset = offset + 1;
            }
            if (CheckBoxDescarrilamento.Checked)
            {
                offset = offset + 2;
            }
            SYNC = (Byte)(macro[offset]);
            if ((SYNC & 0xC0) == 0x00)
            {
                //Atualização Normal
                //CheckBoRadioGroupSincronismo->ItemIndex = 0;
                CheckBoxAtualizacaoNormal.Checked = true;
                CheckBoxAtualizacaoNormal.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            if ((SYNC & 0xC0) == 0x40)
            {
                //Resposta CCO
                //RadioGroupSincronismo->ItemIndex = 1;
                CheckBoxRespSolicitacaoCco.Checked = true;
                CheckBoxRespSolicitacaoCco.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            if ((SYNC & 0xC0) == 0x80)
            {
                //Ultrapassagem Ponto de Troca
                //RadioGroupSincronismo->ItemIndex = 2;
                CheckBoxUltrapassouNumeroPontosTroca.Checked = true;
                CheckBoxUltrapassouNumeroPontosTroca.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            if ((SYNC & 0xC0) == 0xC0)
            {
                //Descarte último ponto
                //RadioGroupSincronismo->ItemIndex = 3;
                CheckBoxDescarteUltimoPontoTroca.Checked = true;
                CheckBoxDescarteUltimoPontoTroca.ForeColor = System.Drawing.SystemColors.WindowText;
            }
            offset = offset + 1;
            TextBoxLatitude.Text = IntToCoord((((Byte)(macro[offset + 2]) << 16) + ((Byte)(macro[offset + 1]) << 8) + (Byte)(macro[offset])));
            offset = offset + 3;
            TextBoxLongitude.Text = IntToCoord((((Byte)(macro[offset + 2]) << 16) + ((Byte)(macro[offset + 1]) << 8) + (Byte)(macro[offset])));
        }

        public void macro200OBC()
        {
            string aux = EntidadeMacro.Texto.Length.ToString();

            if (aux == "6")
            {
                aux = EntidadeMacro.Texto.ToString();
                string posicao = aux.Substring(5);

                if (posicao == "2")
                {
                    CheckBoxTerraTracao.Checked = true;
                    LabelVelocidadeRecebida.Visible = false;
                    TextBoxVelocidadeRecebida.Visible = false;
                }
                if (posicao == "3")
                {
                    CheckBoxPenalidadeSisvem.Checked = true;
                    LabelVelocidadeRecebida.Visible = false;
                    TextBoxVelocidadeRecebida.Visible = false;
                }
                if( posicao == "4")
                {
                    CheckBoxIndicacaoDivergenciaVelocidade.Checked = true;
                    LabelVelocidadeRecebida.Visible = false;
                    TextBoxVelocidadeRecebida.Visible = false;
                }
                if (posicao == "5")
                {
                    CheckBoxTacoDesabilitado.Checked = true;
                    LabelVelocidadeRecebida.Visible = false;
                    TextBoxVelocidadeRecebida.Visible = false;
                }
            }
            if (aux == "8")
            {
                aux = EntidadeMacro.Texto.ToString();
                string posicao = aux.Substring(6);

                string myInt = posicao;
                int velocidade = Convert.ToInt32(myInt, 16); // converte hexadecimal para decimal

                if (velocidade.ToString() == "0" || velocidade.ToString() == "1" || velocidade.ToString() == "2" || velocidade.ToString() == "3" || velocidade.ToString() == "4"
                    || velocidade.ToString() == "5" || velocidade.ToString() == "6" || velocidade.ToString() == "7" || velocidade.ToString() == "8" || velocidade.ToString() == "9")
                {
                    CheckBoxExcessoCorrente.Checked = true;
                    TextBoxVelocidadeRecebida.Text = velocidade.ToString();
                    LabelVelocidadeRecebida.Visible = true;
                    TextBoxVelocidadeRecebida.Visible = true;
                }
                else
                {
                    CheckBoxExcessoCorrente.Checked = true;
                    TextBoxVelocidadeRecebida.Text = velocidade.ToString();
                    LabelVelocidadeRecebida.Visible = true;
                    TextBoxVelocidadeRecebida.Visible = true;
                }

            }
            if (aux == "10")
            {
                aux = EntidadeMacro.Texto.ToString();
                string posicao = aux.Substring(6);
                string myInt = posicao.Substring(2);
                string myInt2 = posicao.Substring(0,2);
                int velocidadeTACO = Convert.ToInt32(myInt, 16); // converte hexadecimal para decimal
                int velocidadeGPS = Convert.ToInt32(myInt2, 16);
                CheckBoxDivergenciaVelocidade.Checked = true;
                LabelVelocidadeGPS.Visible = true;
                LabelVelocidadeTACO.Visible = true;
                TextBoxVelocidadeGPS.Text = velocidadeGPS.ToString();
                TextBoxVelocidadeTACO.Text = velocidadeTACO.ToString();
                TextBoxVelocidadeGPS.Visible = true;
                TextBoxVelocidadeTACO.Visible = true;

            }

        }

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

        public string IntToHex(int valor)
        {
            return valor.ToString("X");
        }
    }
}