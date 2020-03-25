[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=signaturit_net-sdk&metric=alert_status&token=afc818be575d17db0a4170c1b4576967fa9e3798)](https://sonarcloud.io/dashboard?id=signaturit_net-sdk)

========================
DO NOT USE MASTER BRANCH
========================

Signaturit NET SDK
==================
This package is a NET wrapper around the Signaturit API. If you didn't read the documentation yet, maybe it's time to take a look [here](https://docs.signaturit.com/).

Test
----

In order to execute the test suite:

```
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat="opencover"
```

To upload the information to Sonarqube:

```
dotnet tool install --global dotnet-sonarscanner --version 4.7.1

dotnet sonarscanner begin /o:"signaturit" /k:signaturit_net-sdk /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="YOUR_TOKEN" /s:"$(PWD)/SonarQube.Analysis.xml"

dotnet build

dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat="opencover"

dotnet sonarscanner end /d:sonar.login="YOUR_TOKEN"
```

Configuration
-------------

The recommended way to install the SDK is through [project.json](https://docs.microsoft.com/es-es/dotnet/articles/core/tools/project-json).

```bash
"dependencies": {
    "signaturit": "1.0.0"
}
```

Then instantiate the Client class passing in your API access token.

```C#
string accessToken = "OTllYjUwM2NhYmNjNmJlYTZlNWEzNWYzYmZjNTRiZWI2YjU0ZjUxNzUwZDRjZjEwZTA0ZTFkZWQwZGExNDM3ZQ";

Signaturit.Client client = new Signaturit.Client(accessToken);
```

Please note that by default the client will use our sandbox API. When you are
ready to start using the production environment just get the correct access token and pass an additional argument to the constructor:

```C#
Signaturit.Client client = new Signaturit.Client(accessToken, true);
```

Examples
--------

## Signatures

### Count signature requests

Count your signature requests.

```C#
object response = client.countSignatures();
```

### Get all signature requests

Retrieve all data from your signature requests using different filters.

##### All signatures

```C#
object response = client.getSignatures();
```

##### Getting the last 50 signatures

```C#
object response = client.getSignatures(50);
```

##### Getting signatures with custom field "crm_id"

```C#
object response = client.getSignatures(100, 0, new { crm_id = "CUSTOM_ID" })
```

### Get signature request

Get the information regarding a single signature request passing its ID.

```C#
object response = client.getSignature("a066298d-2877-11e4-b641-080027ea3a6e");
```

### Signature request

Create a new signature request.

```C#
object files = new [] {
    "./documents/contracts/receipt250.pdf"
};

object recipients = new [] {
    new { name = "Mr John" , email = "john.doe@signaturit.com" }
};

object parameters = new {
    subject = "Receipt no. 250",
    body = "Please sign the receipt"
};

object response = client.createSignature(files, recipients, parameters);
```

You can add custom info in your requests

```C#
object files = new [] {
    "./documents/contracts/receipt250.pdf"
};

object recipients = new [] {
    new { name = "Mr John" , email = "john.doe@signaturit.com" }
};

object parameters = new {
    subject = "Receipt no. 250",
    body = "Please sign the receipt",
    data = new {
        crm_id = "45673"
    }
};

object response = client.createSignature(files, recipients, parameters);
```

You can send templates with the fields filled

```C#
object recipients = new [] {
    new { name = "Mr John" , email = "john.doe@signaturit.com" }
};

object parameters = new {
    subject = "Receipt no. 250",
    body = "Please sign the receipt",
    templates = "template_name",
    data = new {
        widget_id = "default value"
    }
};

object response = client.createSignature(files, recipients, parameters);
```

### Cancel signature request

Cancel a signature request.

```C#
object response = client.cancelSignature("a066298d-2877-11e4-b641-080027ea3a6e");
```

### Send reminder

Send a reminder email.

```C#
object response = client.sendReminder("a066298d-2877-11e4-b641-080027ea3a6e");
```

### Get audit trail

Get the audit trail of a signature request document

```C#
string response = client.downloadAuditTrail("a066298d-2877-11e4-b641-080027ea3a6e", "d474a1eb-2877-11e4-b641-080027ea3a6e");
```

### Get signed document

Get the signed document of a signature request document

```C#
string response = client.downloadSignedDocument("a066298d-2877-11e4-b641-080027ea3a6e", "d474a1eb-2877-11e4-b641-080027ea3a6e");
```

## Branding

### Get brandings

Get all account brandings.

```C#
object response = client.getBrandings();
```

### Get branding

Get a single branding.

```C#
object response = client.getBranding("6472aad7-2877-11e4-b641-080027ea3a6e");
```

### Create branding

Create a new branding.

```C#
object parameters = new {
    layout_color      = "#FFBF00",
    text_color        = "#2A1B0A",
    application_texts = new { sign_button = "Sign!" }
};

object response = client.createBranding(parameters);
```

### Update branding

Update a single branding.

```C#
object parameters = new {
    application_texts = new { send_button = "Send!" }
};

object response = client.updateBranding("6472aad7-2877-11e4-b641-080027ea3a6e", parameters);
```

## Template

### Get all templates

Retrieve all data from your templates.

```C#
object response = client.getTemplates();
```

## Email

### Get emails

####Get all certified emails

```C#
object response = client.getEmails()
```

####Get last 50 emails

```C#
object response = client.getEmails(50)
```

####Navigate through all emails in blocks of 50 results

```C#
object response = client.getEmails(50, 50)
```

### Count emails

Count all certified emails

```C#
object response = client.countEmails()
```

### Get email

Get a single email

```C#
object response = client.getEmail("EMAIL_ID")
```

### Create email

Create a new certified email.

```C#
object files = new [] {
    "./demo.pdf",
    "./receipt.pdf"
};

object recipients = new [] {
    new { name = "Mr John" , email = "john.doe@signaturit.com" }
};

object parameters = new {
    subject = "NET subject",
    body = "NET body"
};

object response = client.createEmail(files, recipients, "NET subject", "NET body")
```

### Get audit trail document

Get the audit trail document of an email request.

```C#
string response = client.downloadEmailAuditTrail("EMAIL_ID", "CERTIFICATE_ID")
```