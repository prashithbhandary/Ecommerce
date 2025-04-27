using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.domain.models
{
    public class AzureBlobSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }

}
