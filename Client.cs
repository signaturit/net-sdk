using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Signaturit
{
    public class Client
    {
        /**
         * Signaturit's production API URL
         */
        private const string PROD_BASE_URL = "https://api.signaturit.com";

        /**
         * Signaturit's sandbox API URL
         */
        private const string SANDBOX_BASE_URL = "https://api.sandbox.signaturit.com";

        /**
         * @var string
         */
        private string accessToken;

        /**
         * @var string
         */
        private string url;

        /**
         * @param string $accessToken
         * @param bool   $production
         */
        public Client(string accessToken, bool production = false)
        {
            this.accessToken = accessToken;
            this.url         = production ? PROD_BASE_URL : SANDBOX_BASE_URL;
        }

        /**
         * @param object $conditions
         *
         * @return int
         */
        public int countSignatures(object conditions = null)
        {
            dynamic json = jsonRequest("get", "signatures/count.json", conditions, null, null);

            return json.count;
        }

        /**
         * @param int     $limit
         * @param int     $offset
         * @param dynamic $conditions
         *
         * @return dynamic
         */
        public object getSignatures(int limit = 100, int offset = 0, dynamic conditions = null)
        {
            conditions = conditions == null ? new ExpandoObject() : dynamicToExpandoObject(conditions);

            conditions.limit  = limit;
            conditions.offset = offset;

            dynamic json = jsonRequest("get", "signatures.json", conditions, null, null);

            return json;
        }

        /**
         * @param string $signatureId
         *
         * @return dynamic
         */
        public object getSignature(string signatureId)
        {
            dynamic json = jsonRequest("get", $"signatures/{signatureId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $signatureId
         * @param string $documentId
         *
         * @return string
         */
        public Stream downloadAuditTrail(string signatureId, string documentId)
        {
            Stream response = streamRequest("get", $"signatures/{signatureId}/documents/{documentId}/download/audit_trail", null, null, null);

            return response;
        }

        /**
         * @param string $signatureId
         * @param string $documentId
         *
         * @return string
         */
        public Stream downloadSignedDocument(string signatureId, string documentId)
        {
            Stream response = streamRequest("get", $"signatures/{signatureId}/documents/{documentId}/download/signed", null, null, null);

            return response;
        }

        /**
         * @param dynamic $files
         * @param dynamic $recipients
         * @param dynamic $parameters
         *
         * @return dynamic
         */
        public object createSignature(object files, object recipients, dynamic parameters = null)
        {
            parameters = parameters == null ? new ExpandoObject() : dynamicToExpandoObject(parameters);

            parameters.recipients = recipients;

            dynamic json = jsonRequest("post", "signatures.json", null, parameters, files);

            return json;
        }

        /**
         * @param string $signatureId
         *
         * @return dynamic
         */
        public object cancelSignature(string signatureId)
        {
            dynamic json = jsonRequest("patch", $"signatures/{signatureId}/cancel.json", null, null, null);

            return json;
        }

        /**
         * @param string $signatureId
         *
         * @return dynamic
         */
        public object sendReminder(string signatureId)
        {
            dynamic json = jsonRequest("post", $"signatures/{signatureId}/reminder.json", null, null, null);

            return json;
        }

        /**
         * @param string $brandingId
         *
         * @return dynamic
         */
        public object getBranding(string brandingId)
        {
            dynamic json = jsonRequest("get", $"brandings/{brandingId}.json", null, null, null);

            return json;
        }

        /**
         * @return dynamic
         */
        public object getBrandings()
        {
            dynamic json = jsonRequest("get", "brandings.json", null, null, null);

            return json;
        }

        /**
         * @param object $parameters
         *
         * @return dynamic
         */
        public object createBranding(object parameters = null)
        {
            dynamic json = jsonRequest("post", "brandings.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $brandingId
         * @param object $parameters
         *
         * @return dynamic
         */
        public object updateBranding(string brandingId, object parameters = null)
        {
            dynamic json = jsonRequest("patch", $"brandings/{brandingId}.json", null, parameters, null);

            return json;
        }

        /**
         * @param int $limit
         * @param int $offset
         *
         * @return dynamic
         */
        public object getTemplates(int limit = 100, int offset = 0)
        {
            object conditions = new { limit = limit, offset = offset };

            dynamic json = jsonRequest("get", "templates.json", conditions, null, null);

            return json;
        }

        /**
         * @param object $conditions
         *
         * @return dynamic
         */
        public int countEmails(object conditions = null)
        {
            dynamic json = jsonRequest("get", "emails/count.json", conditions, null, null);

            return json.count;
        }

        /**
         * @param int     $limit
         * @param int     $offset
         * @param dynamic $conditions
         *
         * @return dynamic
         */
        public object getEmails(int limit = 100, int offset = 0, dynamic conditions = null)
        {
            conditions = conditions == null ? new ExpandoObject() : dynamicToExpandoObject(conditions);

            conditions.limit  = limit;
            conditions.offset = offset;

            dynamic json = jsonRequest("get", "emails.json", conditions, null, null);

            return json;
        }
        /**
         * @param string $emailId
         *
         * @return dynamic
         */
        public object getEmail(string emailId)
        {
            dynamic json = jsonRequest("get", $"emails/{emailId}.json", null, null, null);

            return json;
        }

        /**
         * @param dynamic $files
         * @param dynamic $recipients
         * @param string  $subject
         * @param string  $body
         * @param dynamic $parameters
         *
         * @return dynamic
         */
        public object createEmail(object files, dynamic recipients, string subject, string body, dynamic parameters = null)
        {
            parameters = parameters == null ? new ExpandoObject() : dynamicToExpandoObject(parameters);

            parameters.subject    = subject;
            parameters.body       = body;
            parameters.recipients = recipients;

            dynamic json = jsonRequest("post", "emails.json", null, parameters, files);

            return json;
        }

        /**
         * @param string $emailId
         * @param string $certificateId
         *
         * @return string
         */
        public Stream downloadEmailAuditTrail(string emailId, string certificateId)
        {
            Stream response = streamRequest("get", $"emails/{emailId}/certificates/{certificateId}/download/audit_trail", null, null, null);

            return response;
        }

        /**
         * @param object $conditions
         *
         * @return dynamic
         */
        public int countSMS(object conditions = null)
        {
            dynamic json = jsonRequest("get", "sms/count.json", conditions, null, null);

            return json.count;
        }

        /**
         * @param int     $limit
         * @param int     $offset
         * @param dynamic $conditions
         *
         * @return dynamic
         */
        public object getSMS(int limit = 100, int offset = 0, dynamic conditions = null)
        {
            conditions = conditions == null ? new ExpandoObject() : dynamicToExpandoObject(conditions);

            conditions.limit  = limit;
            conditions.offset = offset;

            dynamic json = jsonRequest("get", "sms.json", conditions, null, null);

            return json;
        }
        /**
         * @param string $smsId
         *
         * @return dynamic
         */
        public object getSMS(string smsId)
        {
            dynamic json = jsonRequest("get", $"sms/{smsId}.json", null, null, null);

            return json;
        }

        /**
         * @param dynamic $attachments
         * @param dynamic $recipients
         * @param string  $body
         * @param dynamic $parameters
         *
         * @return dynamic
         */
        public object createSMS(object attachments, dynamic recipients, string body, dynamic parameters = null)
        {
            parameters = parameters == null ? new ExpandoObject() : dynamicToExpandoObject(parameters);

            parameters.body       = body;
            parameters.recipients = recipients;

            dynamic json = jsonRequest("post", "sms.json", null, parameters, attachments);

            return json;
        }

        /**
         * @param string $smsId
         * @param string $certificateId
         *
         * @return string
         */
        public Stream downloadSMSAuditTrail(string smsId, string certificateId)
        {
            Stream response = streamRequest("get", $"sms/{smsId}/certificates/{certificateId}/download/audit_trail", null, null, null);

            return response;
        }

        /**
         * @param object $conditions
         *
         * @return dynamic
         */
        public int countSubscriptions(object conditions = null)
        {
            dynamic json = jsonRequest("get", "subscriptions/count.json", conditions, null, null);

            return json.count;
        }

        /**
         * @param int     $limit
         * @param int     $offset
         * @param dynamic $conditions
         *
         * @return dynamic
         */
        public object getSubscriptions(int limit = 100, int offset = 0, dynamic conditions = null)
        {
            conditions = conditions == null ? new ExpandoObject() : dynamicToExpandoObject(conditions);

            conditions.limit  = limit;
            conditions.offset = offset;

            dynamic json = jsonRequest("get", "subscriptions.json", conditions, null, null);

            return json;
        }
        /**
         * @param string $subscriptionId
         *
         * @return dynamic
         */
        public object getSubscription(string subscriptionId)
        {
            dynamic json = jsonRequest("get", $"subscriptions/{subscriptionId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $url
         * @param object $events
         *
         * @return dynamic
         */
        public object createSubscription(string url, object events)
        {
            object parameters = new { url = url, events = events };

            dynamic json = jsonRequest("post", "subscriptions.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $subscriptionId
         * @param object $parameters
         *
         * @return dynamic
         */
        public object updateSubscription(string subscriptionId, object parameters = null)
        {
            dynamic json = jsonRequest("patch", $"subscriptions/{subscriptionId}.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $subscriptionId
         *
         * @return dynamic
         */
        public object deleteSubscription(string subscriptionId)
        {
            dynamic json = jsonRequest("delete", $"subscriptions/{subscriptionId}.json", null, null, null);

            return json;
        }

        /**
         * @param object $conditions
         *
         * @return dynamic
         */
        public int countContacts(object conditions = null)
        {
            dynamic json = jsonRequest("get", "contacts/count.json", conditions, null, null);

            return json.count;
        }

        /**
         * @param int $limit
         * @param int $offset
         *
         * @return dynamic
         */
        public object getContacts(int limit = 100, int offset = 0)
        {
            object conditions = new { limit = limit, offset = offset };

            dynamic json = jsonRequest("get", "contacts.json", conditions, null, null);

            return json;
        }
        /**
         * @param string $contactId
         *
         * @return dynamic
         */
        public object getContact(string contactId)
        {
            dynamic json = jsonRequest("get", $"contacts/{contactId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $email
         * @param string $name
         *
         * @return dynamic
         */
        public object createContact(string email, string name)
        {
            object parameters = new { email = email, name = name };

            dynamic json = jsonRequest("post", "contacts.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $contactId
         * @param object $parameters
         *
         * @return dynamic
         */
        public object updateContact(string contactId, object parameters = null)
        {
            dynamic json = jsonRequest("patch", $"contacts/{contactId}.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $contactId
         *
         * @return dynamic
         */
        public object deleteContact(string contactId)
        {
            dynamic json = jsonRequest("delete", $"contacts/{contactId}.json", null, null, null);

            return json;
        }

        /**
         * @param int $limit
         * @param int $offset
         *
         * @return dynamic
         */
        public object getUsers(int limit = 100, int offset = 0)
        {
            object conditions = new { limit = limit, offset = offset };

            dynamic json = jsonRequest("get", "team/users.json", conditions, null, null);

            return json;
        }

        /**
         * @param string $userId
         *
         * @return dynamic
         */
        public object getUser(string userId)
        {
            dynamic json = jsonRequest("get", $"team/users/{userId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $email
         * @param string $role
         *
         * @return dynamic
         */
        public object inviteUser(string email, string role)
        {
            object parameters = new { email = email, role = role };

            dynamic json = jsonRequest("post", "team/users.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $userId
         * @param string $role
         *
         * @return dynamic
         */
        public object changeUserRole(string userId, string role)
        {
            object parameters = new { role = role };

            dynamic json = jsonRequest("patch", $"team/users/{userId}.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $userId
         *
         * @return dynamic
         */
        public object removeUser(string userId)
        {
            dynamic json = jsonRequest("delete", $"team/users/{userId}.json", null, null, null);

            return json;
        }

        /**
         * @param int $limit
         * @param int $offset
         *
         * @return dynamic
         */
        public object getGroups(int limit = 100, int offset = 0)
        {
            object conditions = new { limit = limit, offset = offset };

            dynamic json = jsonRequest("get", "team/groups.json", conditions, null, null);

            return json;
        }

        /**
         * @param string $groupId
         *
         * @return dynamic
         */
        public object getGroup(string groupId)
        {
            dynamic json = jsonRequest("get", $"team/groups/{groupId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $name
         *
         * @return dynamic
         */
        public object createGroup(string name)
        {
            object parameters = new { name = name };

            dynamic json = jsonRequest("post", "team/groups.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $groupId
         * @param string $name
         *
         * @return dynamic
         */
        public object updateGroup(string groupId, string name)
        {
            object parameters = new { name = name };

            dynamic json = jsonRequest("patch", $"team/groups/{groupId}.json", null, parameters, null);

            return json;
        }

        /**
         * @param string $groupId
         *
         * @return dynamic
         */
        public object deleteGroup(string groupId)
        {
            dynamic json = jsonRequest("delete", $"team/groups/{groupId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $groupId
         * @param string $userId
         *
         * @return dynamic
         */
        public object addManagerToGroup(string groupId, string userId)
        {
            dynamic json = jsonRequest("post", $"team/groups/{groupId}/managers/{userId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $groupId
         * @param string $userId
         *
         * @return dynamic
         */
        public object removeManagerFromGroup(string groupId, string userId)
        {
            dynamic json = jsonRequest("delete", $"team/groups/{groupId}/managers/{userId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $groupId
         * @param string $userId
         *
         * @return dynamic
         */
        public object addMemberToGroup(string groupId, string userId)
        {
            dynamic json = jsonRequest("post", $"team/groups/{groupId}/members/{userId}.json", null, null, null);

            return json;
        }

        /**
         * @param string $groupId
         * @param string $userId
         *
         * @return dynamic
         */
        public object removeMemberFromGroup(string groupId, string userId)
        {
            dynamic json = jsonRequest("delete", $"team/groups/{groupId}/members/{userId}.json", null, null, null);

            return json;
        }

        /**
         * @param int $limit
         * @param int $offset
         *
         * @return dynamic
         */
        public object getSeats(int limit = 100, int offset = 0)
        {
            object conditions = new { limit = limit, offset = offset };

            dynamic json = jsonRequest("get", "team/seats.json", conditions, null, null);

            return json;
        }

        /**
         * @param string $seatId
         *
         * @return dynamic
         */
        public object removeSeat(string seatId)
        {
            dynamic json = jsonRequest("delete", $"team/seats/{seatId}.json", null, null, null);

            return json;
        }

        /**
         * @param object input
         *
         * @return ExpandoObject
         */
        private ExpandoObject dynamicToExpandoObject(object input)
        {
            string json          = JsonConvert.SerializeObject(input);
            ExpandoObject output = JsonConvert.DeserializeObject<ExpandoObject>(json);

            return output;
        }

        /**
         * @param string $method
         * @param string $path
         * @param object $query
         * @param object $body
         * @param object files
         *
         * @return Stream
         */
        private Stream streamRequest(string method, string path, object query, object body, object files)
        {
            Stream response = Request(method, path, query, body, files).ReceiveStream().Result;

            return response;
        }

        /**
         * @param string $method
         * @param string $path
         * @param object $query
         * @param object $body
         * @param object files
         *
         * @return string
         */
        private string stringRequest(string method, string path, object query, object body, object files)
        {
            string response = Request(method, path, query, body, files).ReceiveString().Result;

            return response;
        }

        /**
         * @param string $method
         * @param string $path
         * @param object $query
         * @param object $body
         * @param object files
         *
         * @return dynamic
         */
        private object jsonRequest(string method, string path, object query, object body, object files)
        {
            string response = stringRequest(method, path, query, body, files);
            dynamic json    = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            return json;
        }

        /**
         * @param string $method
         * @param string $path
         * @param object $query
         * @param object $body
         * @param object files
         *
         * @return Task<string>
         */
        private async Task<HttpResponseMessage> Request(string method, string path, object query, object body, object files)
        {
            Url url = new Url($"{this.url}/v3/{path}");

            FlurlClient Request = url
                .SetQueryParams(query)
                .WithOAuthBearerToken(this.accessToken)
                .WithHeader("User-Agent", "signaturit-net-sdk 1.1.0");

            switch (method)
            {
                case "get":
                    return await Request.GetAsync();

                case "post":
                    if (files == null) {
                        body = body == null ? new {} : body;

                        return await Request.PostJsonAsync(body as object);
                    }

                    var content = new CapturedMultipartContent();

                    foreach (string file in files as IList<string>)
                    {
                        string name = convertToAscii(System.IO.Path.GetFileName(file));
                        string ext  = System.IO.Path.GetExtension(file);
                        string mime = ext == ".pdf" ? "application/pdf" : "application/msword";

                        content.AddFile($"files[{name}]", File.OpenRead(file), name, mime);
                    }

                    captureMultipartContentInObject(content, body, "");

                    return await Request.PostAsync(content);

                case "patch":
                    body = body == null ? new {} : body;

                    return await Request.PatchJsonAsync(body as object);

                case "delete":
                    return await Request.DeleteAsync();
            }

            return null;
        }

        private string convertToAscii(string text)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding ascii = Encoding.ASCII;
            return ascii.GetString(Encoding.Convert(utf8, ascii, utf8.GetBytes(text)));
        }

        /**
         * @param CapturedMultipartContent $content
         * @param object                   $body
         * @param string                   $key
         */
        private void captureMultipartContentInObject(CapturedMultipartContent content, object body, string key)
        {
            var data = JsonConvert.SerializeObject(body);
            var json = JObject.Parse(data);

            foreach (JToken child in json.Children()) {
                var property = child as JProperty;

                captureMultipartContentInJson(content, child, "");
            }
        }

        /**
         * @param CapturedMultipartContent $content
         * @param JToken                   $json
         * @param string                   $key
         */
        private void captureMultipartContentInJson(CapturedMultipartContent content, JToken json, string key)
        {
            var property = json as JProperty;

            if (property == null) {
                return;
            }

            if (property.Value.GetType().Name == "JArray") {
                int index = 0;

                foreach (JToken child in property.Values()) {
                    string valueKey = key == "" ? property.Name : $"{key}[{property.Name}]";
                    valueKey += $"[{index++}]";

                    if (child.GetType().Name == "JValue") {
                        content.AddString(valueKey, $"{child}");
                    } else {
                        foreach (JToken subChild in child) {
                            captureMultipartContentInJson(content, subChild, valueKey);
                        }
                    }
                }

                return;
            }

            if (property.Value.GetType().Name == "JObject") {
                string valueKey = key == "" ? property.Name : $"{key}[{property.Name}]";

                foreach (JToken child in property.Values()) {
                    captureMultipartContentInJson(content, child, valueKey);
                }

                return;
            }

            if (property.Value.GetType().Name == "JValue") {
                string valueKey = key == "" ? property.Name : $"{key}[{property.Name}]";

                content.AddString(valueKey, property.Value.ToString());

                return;
            }
        }
    }
}
