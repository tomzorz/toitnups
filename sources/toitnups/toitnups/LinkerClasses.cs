using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace toitnups
{
    [XmlRoot(ElementName = "assembly")]
    public class LinkAssembly
    {
        [XmlAttribute(AttributeName = "fullname")]
        public string Fullname { get; set; }
        [XmlAttribute(AttributeName = "preserve")]
        public string Preserve { get; set; }
        [XmlElement(ElementName = "type")]
        public List<LinkAssemblyType> LinkAssemblyTypes { get; set; }
    }

    [XmlRoot(ElementName = "type")]
    public class LinkAssemblyType
    {
        [XmlAttribute(AttributeName = "fullname")]
        public string Fullname { get; set; }
        [XmlAttribute(AttributeName = "preserve")]
        public string Preserve { get; set; }
    }

    [XmlRoot(ElementName = "linker")]
    public class Linker
    {
        [XmlElement(ElementName = "assembly")]
        public List<LinkAssembly> LinkAssemblies { get; set; }
    }
}
