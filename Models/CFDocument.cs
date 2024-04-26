using System;
using System.Collections.Generic;


public class TEKSResponse{
    public CFDocument CFDocument { get; set; }
    public List<CFItem> CFItems { get; set; }
    public List<CFAssociation> CFAssociations { get; set; }
    public CFDefinitions CFDefinitions { get; set; }

}
public class CFDocument
{
    public string adoptionStatus { get; set; }
    public string creator { get; set; }
    public string description { get; set; }
    public string identifier { get; set; }
    public string language { get; set; }
    public DateTime lastChangeDateTime { get; set; }
    public string notes { get; set; }
    public string officialSourceURL { get; set; }
    public string publisher { get; set; }
    public DateTime statusEndDate { get; set; }
    public DateTime statusStartDate { get; set; }
    public string subject { get; set; }
    public List<SubjectURI> subjectURI { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
    public string version { get; set; }
}

public class SubjectURI
{
    public string identifier { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
}

public class CFItem
{
    public string alternativeLabel { get; set; }
    public string identifier { get; set; }
    public string language { get; set; }
    public DateTime lastChangeDateTime { get; set; }
    public string listEnumeration { get; set; }
    public string uri { get; set; }
    public string CFItemType { get; set; }
    public CFItemTypeURI CFItemTypeURI { get; set; }
    public string abbreviatedStatement { get; set; }
    public string fullStatement { get; set; }
    public string humanCodingScheme { get; set; }
    public DateTime statusEndDate { get; set; }
    public DateTime statusStartDate { get; set; }
}

public class CFItemTypeURI
{
    public string identifier { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
}

public class CFAssociation
{
    public CFDocumentURI CFDocumentURI { get; set; }
    public string associationType { get; set; }
    public DestinationNodeURI destinationNodeURI { get; set; }
    public string identifier { get; set; }
    public DateTime lastChangeDateTime { get; set; }
    public OriginNodeURI originNodeURI { get; set; }
    public int sequenceNumber { get; set; }
    public string uri { get; set; }
}

public class CFDocumentURI
{
    public string identifier { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
}

public class DestinationNodeURI
{
    public string identifier { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
}

public class OriginNodeURI
{
    public string identifier { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
}


public class CFDefinitions
{
    public List<object> CFAssociationGroupings { get; set; }
    public List<object> CFConcepts { get; set; }
    public List<CFItemType> CFItemTypes { get; set; }
    public List<object> CFLicenses { get; set; }
    public List<CFSubject> CFSubjects { get; set; }
}

public class CFItemType
{
    public string description { get; set; }
    public string hierarchyCode { get; set; }
    public string identifier { get; set; }
    public DateTime lastChangeDateTime { get; set; }
    public string title { get; set; }
    public string typeCode { get; set; }
    public string uri { get; set; }
}

public class CFSubject
{
    public string hierarchyCode { get; set; }
    public string identifier { get; set; }
    public DateTime lastChangeDateTime { get; set; }
    public string title { get; set; }
    public string uri { get; set; }
}


