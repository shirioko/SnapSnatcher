using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fiddler;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Net;

namespace SnapSnatcher
{
    public partial class frmCapture : Form
    {
        public string username = string.Empty;
        public string authToken = string.Empty;
        public string reqToken = string.Empty;

        public frmCapture()
        {
            InitializeComponent();
        }

        protected byte[] GetCertificate()
        {
            X509Certificate2 cert = CertMaker.GetRootCertificate();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-----BEGIN CERTIFICATE-----");
            sb.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            sb.AppendLine("-----END CERTIFICATE-----");
            return System.Text.Encoding.ASCII.GetBytes(sb.ToString());
        }

        private void frmCapture_Load(object sender, EventArgs e)
        {
            try
            {
                //check if root cert exists
                if (!CertMaker.rootCertExists())
                {
                    //create root cert
                    if (!CertMaker.createRootCert())
                    {
                        //oh shit nigga!
                        throw new Exception("FiddlerCore error: could not create root certificate");
                    }
                }

                //start fiddlercore
                List<Session> oAllSessions = new List<Session>();

                FiddlerApplication.BeforeRequest += delegate(Session oS)
                {
                    oS.bBufferResponse = true;
                    Monitor.Enter(oAllSessions);
                    oAllSessions.Add(oS);
                    Monitor.Exit(oAllSessions);
                };

                FiddlerApplication.BeforeRequest += delegate(Session oS)
                {
                    this.AddMessage(string.Format("Captured request to {0}", oS.host));
                    if (oS.host.Contains("snapcert"))
                    {
                        oS.utilCreateResponseAndBypassServer();
                        this.AddMessage("Returning certificate");
                        //return certificate, act as http server
                        byte[] cert = this.GetCertificate();
                        oS.responseBodyBytes = cert;
                        oS.oResponse.headers["Content-Type"] = "application/x-x509-ca-cert";
                        oS.oResponse.headers["Content-Length"] = cert.Length.ToString();
                        oS.oResponse.headers["Connection"] = "Keep-Alive";
                        oS.oResponse.headers.Add("Content-Disposition", "attachment; filename='fcore.cer'");
                        oS.responseCode = 200;
                    }
                };

                FiddlerApplication.BeforeResponse += delegate(Session oS)
                {
                    if (oS.host.Contains("feelinsonice-hrd.appspot.com"))
                    {
                        //try to capture req token
                        this.AddMessage("Captured relevant request, processing...");
                        try
                        {
                            oS.utilDecodeRequest();
                            oS.utilDecodeResponse();

                            //try response body
                            string responseBody = oS.GetResponseBodyAsString();
                            if (!string.IsNullOrEmpty(responseBody))
                            {
                                JObject obj = JObject.Parse(responseBody);
                                JToken tok = obj["auth_token"];
                                JToken uname = obj["username"];
                                if (tok != null && uname != null)
                                {
                                    this.authToken = tok.ToObject<string>();
                                    this.username = uname.ToObject<string>();
                                    this.OnSuccess();
                                    return;
                                }
                            }

                            //try request body
                            string requestBody = oS.GetRequestBodyAsString();
                            if(!string.IsNullOrEmpty(requestBody))
                            {
                                string[] param = requestBody.Split(new char[] { '&' });
                                string uname = string.Empty;
                                string reqt = string.Empty;
                                foreach (string arg in param)
                                {
                                    string[] keyval = arg.Split(new char[] { '=' }, 2);
                                    if (keyval[0] == "username")
                                    {
                                        uname = keyval[1];
                                    }
                                    else if (keyval[0] == "req_token")
                                    {
                                        reqt = keyval[1];
                                    }
                                }
                                if (!string.IsNullOrEmpty(uname) && !string.IsNullOrEmpty(reqt))
                                {
                                    this.username = uname;
                                    this.reqToken = reqt;
                                    this.OnSuccess();
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.AddMessage(string.Format("Error in handler: {0}", ex.ToString()));
                        }
                    }
                };
                CONFIG.IgnoreServerCertErrors = true;
                FiddlerApplication.Startup(8877, false, true, true);
                this.AddMessage("Proxy created on port 8877");
                this.AddMessage(string.Format("Set the proxy on your mobile device to {0}:8877", this.GetIP()));
                this.AddMessage("Listening...");
            }
            catch (Exception ex)
            {
                this.AddMessage(ex.Message);
            }
        }

        protected string GetIP()
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            if (localIPs.Length > 0)
            {
                List<string> v4ips = new List<string>();
                foreach (IPAddress addr in localIPs)
                {
                    if (!addr.IsIPv6LinkLocal)
                    {
                        v4ips.Add(addr.ToString());
                    }
                }

                if (v4ips.Count > 0)
                {
                    if (v4ips.Count > 1)
                    {
                        this.AddMessage("Warning: found multiple addresses, you might need to try a different one to work");
                        foreach (string address in v4ips)
                        {
                            this.AddMessage(string.Format("\t{0}", address));
                        }
                    }
                    return v4ips.FirstOrDefault();
                }
            }
            throw new Exception("ERROR: No local IP v4 addresses found");
        }

        protected void OnSuccess()
        {
            //yay!
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        protected delegate void StringDelegate(string message);
        protected void AddMessage(string message)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    StringDelegate d = new StringDelegate(AddMessage);
                    this.Invoke(d, message);
                }
                else
                {
                    this.txtOutput.Text += string.Format("{0}\r\n", message);
                }
            }
            catch (Exception)
            { }
        }

        protected void DoExit()
        {
            //disable proxy
            this.AddMessage("Shutting down proxy...");
            FiddlerApplication.Shutdown();
            Thread.Sleep(1000);
        }

        private void frmCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DoExit();
        }
    }
}
