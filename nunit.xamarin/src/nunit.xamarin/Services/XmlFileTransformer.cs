using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using NUnit.Runner.Helpers;
using NUnit.Runner.Messages;
using NUnit.Runner.Services;
using Xamarin.Forms;

namespace NUnit.Runner.Services
{
    internal class XmlFileTransformer : TestResultProcessor
    {
        public XmlFileTransformer(TestOptions options)
            : base(options) { }

        public override async Task Process(ResultSummary testResult)
        {
            if (!string.IsNullOrEmpty(Options.XmlTransformFile))
            {
                try
                {
                    var xpathDocument = new XPathDocument(Options.ResultFilePath);
                    var transform = new XslCompiledTransform();
                    transform.Load(Options.XmlTransformFile);
                    var writer = new XmlTextWriter(Options.ResultFilePath, null);
                    transform.Transform(xpathDocument, null, writer);
                }
                catch (Exception ex)
                {
                    var message = $"Fatal error while trying to transform xml result: {ex.Message}";
                    MessagingCenter.Send(new ErrorMessage(message), ErrorMessage.Name);
                }
            }

            if (Successor != null)
            {
                await Successor.Process(testResult);
            }
        }
    }
}
