using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FileWorker.Controllers;
using FileWorker.Interfaces;
using FileWorker.Tools;

namespace FileWorker.Installer
{
    public class FileInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<FileWorkerController>());

            container.Register(Component.For<IXmlTools>().ImplementedBy<XmlTransformer>().LifestyleTransient())
                .Register(Component.For<IPdfTools>().ImplementedBy<PdfTools>().LifestyleTransient());
        }
    }
}
