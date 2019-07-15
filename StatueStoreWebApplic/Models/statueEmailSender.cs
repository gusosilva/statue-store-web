using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Windows;

namespace StatueStoreWebApplic.Models {
    public class statueEmailSender {

        public string Nome { get; set; }
        private string Body { get; set; }
        public string Assunto { get; set; }


        MailMessage mm = new MailMessage();

        public void AdicionarEmail(string adr) {
            mm.To.Add(new MailAddress(adr));
        }

        public void Enviar() {

            //Servidor smtp
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("statuestoreoficial@gmail.com", "!statue456");
            //Mensagem
            mm.From = new MailAddress("StatueStore@gmail.com", "StatueStore", Encoding.UTF8);
            mm.Body = Body;
            mm.IsBodyHtml = true;
            mm.Subject = Assunto;
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm); //envia os emails

        }

        public void setBodyEsqueciSenha(string codigo) {

            string corpo = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width' />
    <!-- For development, pass document through inliner -->
    <style type='text/css'>
        @import url('https://fonts.googleapis.com/css?family=Roboto:300');

        * {
            margin: 0;
            padding: 0;
            font-size: 100%;
            font-family: Roboto, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
            line-height: 1.65;
        }

        img {
            max-width: 100%;
            margin: 0 auto;
            display: block;
        }

        body, .body-wrap {
            width: 100% !important;
            height: 100%;
            background: #f8f8f8;
        }

        a {
            color: #890072;
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }

        .text-center {
            text-align: center;
        }

        .text-right {
            text-align: right;
        }

        .text-left {
            text-align: left;
        }

        .button {
            display: inline-block;
            color: white;
            background: #470054;
            border: solid #470054;
            border-width: 10px 20px 8px;
            font-weight: bold;
            border-radius: 4px;
        }

            .button:hover {
                text-decoration: none;
            }

        h1, h2, h3, h4, h5, h6 {
            margin-bottom: 20px;
            line-height: 1.25;
        }

        h1 {
            font-size: 32px;
        }

        h2 {
            font-size: 28px;
        }

        h3 {
            font-size: 24px;
        }

        h4 {
            font-size: 20px;
        }

        h5 {
            font-size: 16px;
        }

        p, ul, ol {
            font-size: 16px;
            font-weight: normal;
            margin-bottom: 20px;
        }

        .container {
            display: block !important;
            clear: both !important;
            margin: 0 auto !important;
            max-width: 580px !important;
        }

            .container table {
                width: 100% !important;
                border-collapse: collapse;
            }

            .container .masthead {
                padding: 80px 0;
                background: #9c008b;
                color: white;
            }

                .container .masthead h1 {
                    margin: 0 auto !important;
                    max-width: 90%;
                    text-transform: uppercase;
                }

            .container .content {
                background: white;
                padding: 30px 35px;
            }

                .container .content.footer {
                    background: none;
                }

                    .container .content.footer p {
                        margin-bottom: 0;
                        color: #888;
                        text-align: center;
                        font-size: 14px;
                    }

                    .container .content.footer a {
                        color: #888;
                        text-decoration: none;
                        font-weight: bold;
                    }

                        .container .content.footer a:hover {
                            text-decoration: underline;
                        }
    </style>
</head>
<body>
    <table class='body-wrap'>
        <tr>
            <td class='container'>
                <table>
                    <tr>
                        <td align='center' class='masthead'>
                            <h1>Statue Store</h1>
                            <h4 style='padding-top: 5%;'>Seu guarda roupa de possibilidades</h4>
                        </td>
                    </tr>
                    <tr>
                        <td class='content'>
                            <h2>Olá @#NOME#@,</h2>
                            <p>Parece que você esqueceu sua senha e solicitou sua troca em nosso website. Clique no botão abaixo para redefini-la. E não se esqueça dela, guarde-a com cuidado ! my precious</p>  
                            <table>
                                <tr>
                                    <td align='center'>
                                        <p>
                                            <a href='http://localhost:61017/RedefinindoSenha?codigo=@#CODIGO#@' class='button'>Redefinir minha senha</a>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                            <p>Caso essa requisição não tenha sido por você, outra pessoa possa estar tentando acessar a sua conta. Basta ignorar este email ou, caso se sentir inseguro com a sua senha atual, muda-la acessando nosso site: <a href='http://localhost:6071/Home'>Statue Store</a></p>
                            <p><em>Equipe Statue Store</em></p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class='container'>
                <!-- Message start -->
                <table>
                    <tr>
                        <td class='content footer' align='center'>
                            <p>Enviado por <a href='http://localhost:61017/Home'>Statue Store</a>, Av. Paulista, 666</p>
                            <p><a href='statuestoreoficial@gmail.com'>statuestoreoficial@gmail.com</a> | Equipe Gestora</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";


            corpo = corpo.Replace("@#CODIGO#@", codigo);
            corpo = corpo.Replace("@#NOME#@", Nome);
            Body = corpo;
        }


        public void setBodyAlertProduto(int idProduto, string NomeProduto, int QuantidadeAtual, int QuantidadeMinima, string tamanho) {

            string corpo = @"<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width' />
    <!-- For development, pass document through inliner -->
    <style type='text/css'>
        @import url('https://fonts.googleapis.com/css?family=Roboto:300');

        * {
            margin: 0;
            padding: 0;
            font-size: 100%;
            font-family: Roboto, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
            line-height: 1.65;
        }

        img {
            max-width: 100%;
            margin: 0 auto;
            display: block;
        }

        body, .body-wrap {
            width: 100% !important;
            height: 100%;
            background: #f8f8f8;
        }

        a {
            color: #890072;
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }

        .text-center {
            text-align: center;
        }

        .text-right {
            text-align: right;
        }

        .text-left {
            text-align: left;
        }

        .button {
            display: inline-block;
            color: white;
            background: #470054;
            border: solid #470054;
            border-width: 10px 20px 8px;
            font-weight: bold;
            border-radius: 4px;
        }

            .button:hover {
                text-decoration: none;
            }

        h1, h2, h3, h4, h5, h6 {
            margin-bottom: 20px;
            line-height: 1.25;
        }

        h1 {
            font-size: 32px;
        }

        h2 {
            font-size: 28px;
        }

        h3 {
            font-size: 24px;
        }

        h4 {
            font-size: 20px;
        }

        h5 {
            font-size: 16px;
        }

        p, ul, ol {
            font-size: 16px;
            font-weight: normal;
            margin-bottom: 20px;
        }

        .container {
            display: block !important;
            clear: both !important;
            margin: 0 auto !important;
            max-width: 580px !important;
        }

            .container table {
                width: 100% !important;
                border-collapse: collapse;
            }

            .container .masthead {
                padding: 80px 0;
                background: #9c008b;
                color: white;
            }

                .container .masthead h1 {
                    margin: 0 auto !important;
                    max-width: 90%;
                    text-transform: uppercase;
                }

            .container .content {
                background: white;
                padding: 30px 35px;
            }

                .container .content.footer {
                    background: none;
                }

                    .container .content.footer p {
                        margin-bottom: 0;
                        color: #888;
                        text-align: center;
                        font-size: 14px;
                    }

                    .container .content.footer a {
                        color: #888;
                        text-decoration: none;
                        font-weight: bold;
                    }

                        .container .content.footer a:hover {
                            text-decoration: underline;
                        }
    </style>
</head>
<body>
    <table class='body-wrap'>
        <tr>
            <td class='container'>
                <table>
                    <tr>
                        <td align='center' class='masthead'>
                            <h1>Statue Store</h1>
                            <h4 style='padding-top: 5%;'>Seu guarda roupa de possibilidades</h4>
                        </td>
                    </tr>
                    <tr>
                        <td class='content'>
                            <h2>Olá gerente</h2>
                            <p>Aparentemente um de nossos produtos chegou na quantidade mínima. E nosso protocolo é avisa-los para que possam realizar uma reposição de estoque, caso necessária.</p>  
                            <p>Identificação do produto:<b>@#IDPRODUTO#@</b><br>
                            Nome do Produto: <b>@#NOMEPRODUTO#@</b><BR>
                            Quantidade atual: <b>@#QUANTIDADEATUAL#@</b> <Br>
                            Quantidade mínima: <b>@#QUANTIDADEMINIMA#@</b><br>
                            Tamanho: <b>@#TAMANHO#@</b></p>
                            <table>
                                <tr>
                                    <td align='center'>
                                        <p>
                                            <a href='http://localhost:61017/ProdutoDetalhes?id=@#IDPRODUTO#@' class='button'>Visitar Pagina do produto</a>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class='container'>
                <!-- Message start -->
                <table>
                    <tr>
                        <td class='content footer' align='center'>
                            <p>Enviado por <a href='http://localhost:61017/Home'>Statue Store</a>, Av. Paulista, 666</p>
                            <p><a href='statuestoreoficial@gmail.com'>statuestoreoficial@gmail.com</a> | Equipe Gestora</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";


            corpo = corpo.Replace("@#IDPRODUTO#@", idProduto.ToString());
            corpo = corpo.Replace("@#NOMEPRODUTO#@", NomeProduto.ToString());
            corpo = corpo.Replace("@#QUANTIDADEATUAL#@", QuantidadeAtual.ToString());
            corpo = corpo.Replace("@#QUANTIDADEMINIMA#@", QuantidadeMinima.ToString());
            corpo = corpo.Replace("@#TAMANHO#@", tamanho.ToString());
            Body = corpo;
        }

        public void SetBodyNovoCliente(string nome) {
            string corpo = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width' />
    <!-- For development, pass document through inliner -->
    <style type='text/css'>
        @import url('https://fonts.googleapis.com/css?family=Roboto:300');

        * {
            margin: 0;
            padding: 0;
            font-size: 100%;
            font-family: Roboto, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
            line-height: 1.65;
        }

        img {
            max-width: 100%;
            margin: 0 auto;
            display: block;
        }

        body, .body-wrap {
            width: 100% !important;
            height: 100%;
            background: #f8f8f8;
        }

        a {
            color: #890072;
            text-decoration: none;
        }

            a:hover {
                text-decoration: underline;
            }

        .text-center {
            text-align: center;
        }

        .text-right {
            text-align: right;
        }

        .text-left {
            text-align: left;
        }

        .button {
            display: inline-block;
            color: white;
            background: #470054;
            border: solid #470054;
            border-width: 10px 20px 8px;
            font-weight: bold;
            border-radius: 4px;
        }

            .button:hover {
                text-decoration: none;
            }

        h1, h2, h3, h4, h5, h6 {
            margin-bottom: 20px;
            line-height: 1.25;
        }

        h1 {
            font-size: 32px;
        }

        h2 {
            font-size: 28px;
        }

        h3 {
            font-size: 24px;
        }

        h4 {
            font-size: 20px;
        }

        h5 {
            font-size: 16px;
        }

        p, ul, ol {
            font-size: 16px;
            font-weight: normal;
            margin-bottom: 20px;
        }

        .container {
            display: block !important;
            clear: both !important;
            margin: 0 auto !important;
            max-width: 580px !important;
        }

            .container table {
                width: 100% !important;
                border-collapse: collapse;
            }

            .container .masthead {
                padding: 80px 0;
                background: #9c008b;
                color: white;
            }

                .container .masthead h1 {
                    margin: 0 auto !important;
                    max-width: 90%;
                    text-transform: uppercase;
                }

            .container .content {
                background: white;
                padding: 30px 35px;
            }

                .container .content.footer {
                    background: none;
                }

                    .container .content.footer p {
                        margin-bottom: 0;
                        color: #888;
                        text-align: center;
                        font-size: 14px;
                    }

                    .container .content.footer a {
                        color: #888;
                        text-decoration: none;
                        font-weight: bold;
                    }

                        .container .content.footer a:hover {
                            text-decoration: underline;
                        }
    </style>
</head>
<body>
    <table class='body-wrap'>
        <tr>
            <td class='container'>
                <!-- Message start -->
                <table>
                    <tr>
                        <td align='center' class='masthead'>
                            <h1>Statue Store</h1>
                            <h4 style='padding-top: 5%;'>Seu guarda roupa de possibilidades</h4>
                        </td>
                    </tr>
                    <tr>
                        <td class='content'>
                            <h2>Ola @#NOME#@</h2>
                            <p>Parabéns, você acaba de se inscrever na Statue Store! Agora você pode efetuar compras e criar estampas personalizadas para as suas roupas!</p>
                            <p>Efetuar compras online nunca foi tão fácil, efetue o login para ir ao nosso site e aproveite todas as vantagens :)</p>
                            <table>
                                <tr>
                                    <td align='center'>
                                        <p>
                                            <a href='http://localhost:61017/Entrar' class='button'>Ir à Loja!</a>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                            <p> A equipe Statue Store agradece.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class='container'>
                <!-- Message start -->
                <table>
                    <tr>
                        <td class='content footer' align='center'>
                            <p>Enviado por <a href='http://localhost:61017/Home'>Statue Store</a>, Av. Paulista, 666</p>
                            <p><a href='statuestoreoficial@gmail.com'>statuestoreoficial@gmail.com</a> | Equipe Gestora</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
            corpo = corpo.Replace("@#NOME#@", nome);
            Body = corpo;
        }
    }
}

