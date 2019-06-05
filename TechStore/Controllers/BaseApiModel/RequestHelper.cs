namespace TechStore.Controllers.BaseApiModel
{
    public class RequestHelper
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public string RequestValue { set => SetRequestKeyValue(value); }

        private void SetRequestKeyValue(string request)
        {
            if (request.Contains(':'))
            {
                var keyValuestring = request.Split(':');

                if (keyValuestring.Length == 2)
                {
                    Key = keyValuestring[0];
                    Value = keyValuestring[1];
                }
            }
        }
    }
}
