using System.Text.Json.Serialization;

namespace LogViewerApi.Models;

public record LogEntry
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("OrgId")]
    public string OrgId { get; init; } = string.Empty;

    [JsonPropertyName("OrgName")]
    public string OrgName { get; init; } = string.Empty;

    [JsonPropertyName("UserId")]
    public string UserId { get; init; } = string.Empty;

    [JsonPropertyName("generationStatus")]
    public string GenerationStatus { get; init; } = string.Empty;

    [JsonPropertyName("generationStartsAt")]
    public DateTime GenerationStartsAt { get; init; }

    [JsonPropertyName("generationEndsAt")]
    public DateTime GenerationEndsAt { get; init; }

    [JsonPropertyName("generationDuration")]
    public double GenerationDuration { get; init; }

    [JsonPropertyName("recordId")]
    public string RecordId { get; init; } = string.Empty;

    [JsonPropertyName("templateId")]
    public string TemplateId { get; init; } = string.Empty;

    [JsonPropertyName("sfdcDocumentTemplateId")]
    public string SfdcDocumentTemplateId { get; init; } = string.Empty;

    [JsonPropertyName("sfdcAssociateTemplateIds")]
    public string SfdcAssociateTemplateIds { get; init; } = string.Empty;

    [JsonPropertyName("fileName")]
    public string FileName { get; init; } = string.Empty;

    [JsonPropertyName("outputWanted")]
    public string OutputWanted { get; init; } = string.Empty;

    [JsonPropertyName("entryFormat")]
    public string EntryFormat { get; init; } = string.Empty;

    [JsonPropertyName("outputFormat")]
    public string OutputFormat { get; init; } = string.Empty;

    [JsonPropertyName("encryptSignature")]
    public bool EncryptSignature { get; init; }

    [JsonPropertyName("checkSignature")]
    public bool CheckSignature { get; init; }

    [JsonPropertyName("numberMainItems")]
    public int NumberMainItems { get; init; }

    [JsonPropertyName("numberChildItems")]
    public int NumberChildItems { get; init; }

    [JsonPropertyName("numberRecords")]
    public int NumberRecords { get; init; }

    [JsonPropertyName("numberImages")]
    public int NumberImages { get; init; }

    [JsonPropertyName("numberTags")]
    public int NumberTags { get; init; }

    [JsonPropertyName("generateForSignature")]
    public bool GenerateForSignature { get; init; }

    [JsonPropertyName("useTextToReplaceTable")]
    public bool UseTextToReplaceTable { get; init; }

    [JsonPropertyName("storageLocation")]
    public string StorageLocation { get; init; } = string.Empty;

    [JsonPropertyName("isAnonymous")]
    public bool IsAnonymous { get; init; }

    [JsonPropertyName("salesforceDocTemplateOptions")]
    public SalesforceDocTemplateOptions SalesforceDocTemplateOptions { get; init; } = new();

    [JsonPropertyName("generateFrom")]
    public string GenerateFrom { get; init; } = string.Empty;

    [JsonPropertyName("isGeneratedAsync")]
    public bool IsGeneratedAsync { get; init; }

    [JsonPropertyName("_rid")]
    public string Rid { get; init; } = string.Empty;

    [JsonPropertyName("_self")]
    public string Self { get; init; } = string.Empty;

    [JsonPropertyName("_etag")]
    public string Etag { get; init; } = string.Empty;

    [JsonPropertyName("_attachments")]
    public string Attachments { get; init; } = string.Empty;

    [JsonPropertyName("_ts")]
    public long Ts { get; init; }
}

public record SalesforceDocTemplateOptions
{
    [JsonPropertyName("versionningPolicy")]
    public string VersionningPolicy { get; init; } = string.Empty;

    [JsonPropertyName("overrideSalesforceFileSharing")]
    public bool OverrideSalesforceFileSharing { get; init; }

    [JsonPropertyName("userCanChangeLanguage")]
    public bool UserCanChangeLanguage { get; init; }
}