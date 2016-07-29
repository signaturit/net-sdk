========================
DO NOT USE MASTER BRANCH
========================

Signaturit NET SDK
==================
This package is a NET wrapper around the Signaturit API. If you didn't read the documentation yet, maybe it's time to take a look [here](https://docs.signaturit.com/).

Configuration
-------------

The recommended way to install the SDK is through [project.json](https://docs.microsoft.com/es-es/dotnet/articles/core/tools/project-json).

```bash
"dependencies": {
    "signaturit": "1.0.0"
}
```

Then instantiate the Client class passing in your API access token.

```net
string accessToken = "OTllYjUwM2NhYmNjNmJlYTZlNWEzNWYzYmZjNTRiZWI2YjU0ZjUxNzUwZDRjZjEwZTA0ZTFkZWQwZGExNDM3ZQ";

Signaturit.Client client = new Signaturit.Client(accessToken);
```

Please note that by default the client will use our sandbox API. When you are
ready to start using the production environment just get the correct access token and pass an additional argument to the constructor:

```net
Signaturit.Client client = new Signaturit.Client(accessToken, true);
```

Examples
--------

## Signatures

### Count signature requests

Count your signature requests.

```net
object response = client.countSignatures();
```

### Get all signature requests

Retrieve all data from your signature requests using different filters.

##### All signatures

```net
object response = client.getSignatures();
```

##### Getting the last 50 signatures

```net
object response = client.getSignatures(50);
```

##### Getting signatures with custom field "crm_id"

```net
object response = client.getSignatures(100, 0, new { crm_id = "CUSTOM_ID" })
```

### Get signature request

Get the information regarding a single signature request passing its ID.

```net
object response = client.getSignature("a066298d-2877-11e4-b641-080027ea3a6e");
```

### Signature request

Create a new signature request.

```net
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

```net
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

```net
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

```net
object response = client.cancelSignature("a066298d-2877-11e4-b641-080027ea3a6e");
```

### Send reminder

Send a reminder email.

```net
object response = client.sendReminder("a066298d-2877-11e4-b641-080027ea3a6e");
```

### Get audit trail

Get the audit trail of a signature request document

```net
string response = client.downloadAuditTrail("a066298d-2877-11e4-b641-080027ea3a6e", "d474a1eb-2877-11e4-b641-080027ea3a6e");
```

### Get signed document

Get the signed document of a signature request document

```net
string response = client.downloadSignedDocument("a066298d-2877-11e4-b641-080027ea3a6e", "d474a1eb-2877-11e4-b641-080027ea3a6e");
```

## Branding

### Get brandings

Get all account brandings.

```net
object response = client.getBrandings();
```

### Get branding

Get a single branding.

```net
object response = client.getBranding("6472aad7-2877-11e4-b641-080027ea3a6e");
```

### Create branding

Create a new branding.

```net
object parameters = new {
    layout_color      = "#FFBF00",
    text_color        = "#2A1B0A",
    application_texts = new { sign_button = "Sign!" }
};

object response = client.createBranding(parameters);
```

### Update branding

Update a single branding.

```net
object parameters = new {
    application_texts = new { send_button = "Send!" }
};

object response = client.updateBranding("6472aad7-2877-11e4-b641-080027ea3a6e", parameters);
```

## Template

### Get all templates

Retrieve all data from your templates.

```net
object response = client.getTemplates();
```

## Email

### Get emails

####Get all certified emails

```net
object response = client.getEmails()
```

####Get last 50 emails

```net
object response = client.getEmails(50)
```

####Navigate through all emails in blocks of 50 results

```net
object response = client.getEmails(50, 50)
```

### Count emails

Count all certified emails

```net
object response = client.countEmails()
```

### Get email

Get a single email

```net
object response = client.getEmail("EMAIL_ID")
```

### Create email

Create a new certified email.

```net
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

```net
string response = client.downloadEmailAuditTrail("EMAIL_ID", "CERTIFICATE_ID")
```