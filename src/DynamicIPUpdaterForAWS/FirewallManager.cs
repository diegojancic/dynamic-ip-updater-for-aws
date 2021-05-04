using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace DynamicIPUpdaterForAWS
{
    public class FirewallManager
    {
        private readonly string _ipServer;
        private readonly Configs _configs;

        public FirewallManager(string ipServer, Configs configs)
        {
            _ipServer = ipServer;
            _configs = configs;
        }

        public void LoadPublicIp()
        {
            var req = WebRequest.Create(_ipServer);
            var response = req.GetResponse();

            if (response == null) throw new Exception("Response returned is null =(.  No internet connection?");

            var responseStream = response.GetResponseStream();
            if (responseStream == null) throw new Exception("Response stream is null =(.  No internet connection?");


            string returnedText;
            using (responseStream)
            using (var reader = new StreamReader(responseStream))
            {
                returnedText = reader.ReadToEnd().Trim();
            }

            _configs.PublicIp = returnedText;
        }

        public List<PortChangeResult> OpenPorts()
        {
            var results = new List<PortChangeResult>();
            foreach (var rule in _configs.Rules)
            {
                results.Add(OpenPort(rule));
            }

            return results;
        }

        private PortChangeResult OpenPort(Configs.Rule rule)
        {
            var client = new AmazonEC2Client();

            AuthorizeSecurityGroupIngressResponse res = null;

            try
            {
                res = client.AuthorizeSecurityGroupIngress(new AuthorizeSecurityGroupIngressRequest()
                {
                    IpPermissions = new List<IpPermission>() { GetIpPermissionRule(rule) },
                    GroupId = rule.SecurityGroupId
                });

            }
            catch (AmazonEC2Exception ex)
            {
                if (ex.Message.Contains("the specified rule") && ex.Message.Contains("already exists"))
                {
                    return new PortChangeResult
                    {
                        Message = $"Success: Connection to port {rule.Port} is (already) OPEN",
                        Color = Color.DarkGreen
                    };
                }
                throw;
            }

            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                return new PortChangeResult
                {
                    Message = $"Success: Connection to port {rule.Port} is OPEN",
                    Color = Color.DarkGreen
                };
            }

            return new PortChangeResult
            {
                Message = $"Error: couldn't open port {rule.Port}. Code: " + res.HttpStatusCode,
                Color = Color.DarkRed
            };
        }

        public List<PortChangeResult> ClosePorts()
        {
            var results = new List<PortChangeResult>();
            foreach (var rule in _configs.Rules)
            {
                results.Add(ClosePort(rule));
            }

            return results;
        }

        private PortChangeResult ClosePort(Configs.Rule rule)
        {
            var client = new AmazonEC2Client();

            RevokeSecurityGroupIngressResponse res = null;
            try
            {
                res = client.RevokeSecurityGroupIngress(new RevokeSecurityGroupIngressRequest()
                {
                    IpPermissions = new List<IpPermission>() { GetIpPermissionRule(rule) },
                    GroupId = rule.SecurityGroupId
                });
            }
            catch (AmazonEC2Exception ex)
            {
                if (ex.Message.Contains("The specified rule does not exist"))
                {
                    return new PortChangeResult
                    {
                        Message = $"Success: Connection to port {rule.Port} (already) CLOSED",
                        Color = Color.DarkGreen
                    };
                }
                throw;
            }

            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                return new PortChangeResult
                {
                    Message = $"Success: Connection to port {rule.Port} CLOSED",
                    Color = Color.DarkGreen
                };
            }

            return new PortChangeResult
            {
                Message = $"Error: couldn't close port {rule.Port}. Code: " + res.HttpStatusCode,
                Color = Color.DarkRed
            };
        }

        private IpPermission GetIpPermissionRule(Configs.Rule rule)
        {
            return new IpPermission()
            {
                FromPort = rule.Port,
                ToPort = rule.Port,
                IpProtocol = "tcp",
                Ipv4Ranges = new List<IpRange>
                {
                    new IpRange()
                    {
                        CidrIp = _configs.PublicIp + "/32",
                        Description = _configs.DeviceName
                    }
                }
            };
        }

        public class PortChangeResult
        {
            public string Message { get; set; }
            public Color Color { get; set; }
        }
    }
}
