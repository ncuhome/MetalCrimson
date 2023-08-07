// Ignore Spelling: Json Deserialize

using Newtonsoft.Json;

namespace ER.Save
{
    public class JsonInterpreter : ISaveInterpreter
    {
        public bool Deserialize(string text, out SaveData saveData)
        {
            saveData = JsonConvert.DeserializeObject<SaveData>(text);
            return true;
        }

        public string Serialize(SaveData data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}