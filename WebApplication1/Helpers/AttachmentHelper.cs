using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using PluginExtractor.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.Helpers
{


    public class AttachmentHelper
    {
        private Guid _annotationId;
        private OrganizationServiceProxy _serviceProxy;
        private String _fileName;

        public void Run(ServerConnection.Configuration serverConfig, bool promptforDelete)
        {
            using (_serviceProxy = new OrganizationServiceProxy(serverConfig.OrganizationUri, serverConfig.HomeRealmUri, serverConfig.Credentials, serverConfig.DeviceCredentials))
            {
                _serviceProxy.EnableProxyTypes();
                Annotation setupAnnotation = new Annotation()
                {
                    Subject = "Example Annotation",
                    FileName = "ExampleAnnotationAttachment.txt",
                    DocumentBody = Convert.ToBase64String(
                            new UnicodeEncoding().GetBytes("Sample Annotation Text")),
                    MimeType = "text/plain"
                };

                // Create the Annotation object.
                _annotationId = _serviceProxy.Create(setupAnnotation);

                ColumnSet cols = new ColumnSet("filename", "documentbody");


                // Retrieve the annotation record.
                Annotation retrievedAnnotation =
                    (Annotation)_serviceProxy.Retrieve("annotation", _annotationId, cols);
                Console.WriteLine(", and retrieved.");
                _fileName = retrievedAnnotation.FileName;

                // Download the attachment in the current execution folder.
                using (FileStream fileStream = new FileStream(retrievedAnnotation.FileName, FileMode.OpenOrCreate))
                {
                    byte[] fileContent = Convert.FromBase64String(retrievedAnnotation.DocumentBody);
                    fileStream.Write(fileContent, 0, fileContent.Length);
                }
            }
        }
    }
}